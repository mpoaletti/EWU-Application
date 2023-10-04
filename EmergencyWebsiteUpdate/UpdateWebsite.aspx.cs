using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows;


namespace EmergencyWebsiteUpdate
{
  public partial class UpdateWebsite : Page
  {

    //StringBuilder infoCurrent = new StringBuilder();
    //String Builder for holding previous message in plain text
    private StringBuilder prevMessageNoHTML = new StringBuilder();

    //string builder for update to send to website
    private StringBuilder informationUpdate = new StringBuilder();

    private static bool loggedIn = false;
    
    //class instantiation for gathering html
    private HTMLWriter hwriter = new HTMLWriter();

    //class instantiation for connecting to server
    private static Connection connection = new Connection();

    //location paths of temp folder to store for SFTP transfer to GoDaddy server
    //file temporary location of webpage being replaced to transfer
    private string tempASPLocation = Path.GetTempPath() + "/testing.asp";
    
    //file temporary location for pulling current message to transfer
    private string tempASPCacheLocation = Path.GetTempPath() + "/previousASP.txt";

    //file location for previous message storage in plain text
    private string tempTextCacheLocation = Path.GetTempPath() + "/previousText.txt";

    protected void Page_Load(object sender, EventArgs e)
    {

      loggedIn = (bool)Session["logInStatus"];

      lblLoggedIn.Text = "Logged in as: " + Session["Username"];


      if (!loggedIn)
      {
        bttnConnection.Enabled = false;
        lblConnection.Text = "Not Logged In";
      }
      else
      {
        bttnConnection.Enabled = true;
        lblConnection.Text = "Not Connected";
      }

      if(!Connection.IsSFTPConnected())
      {
        bttnClearWebsite.Visible = false;
        bttnSubmit.Visible = false;
        drpdwnAddOrReplace.Visible = false;
      }
      else {
        lblConnection.Text = "CONNECTED";
        lblConnection.ForeColor = System.Drawing.Color.Black;
        bttnConnection.Text = "Disconnect";
      }
    }

    protected void bttnSubmit_Click(object sender, EventArgs e)
    {
      if(!Connection.IsSFTPConnected())
      {
        MessageBox.Show("Error - Not Connected to Server");
        return;
      }

      if (drpdwnAddOrReplace.SelectedValue == "")
      {
        {
          MessageBox.Show("Please Select to either Add or Overwrite current messages");
        }
      }

      string subLineText = txtSubject.Text;
      string msgLineText = txtMessage.Text;

      //Check for text box to be filled out or not
      if (subLineText != "" && msgLineText != "")
      {
        //Call Updated Info method to grab the form inputs
        //and turn into HTML for outputting to website
        informationUpdate = Get_Updated_Info();
        
        //call update info text method to get the inputs in plain text
        prevMessageNoHTML = Get_Updated_Info_Text();

        Update_Website();
      }
      else
      {
        MessageBox.Show("Subject or Information section left blank, please complete both Subject and Information");
      }
    }

    protected void bttnClearWebsite_Click(object sender, EventArgs e)
    {
      if (!Connection.IsSFTPConnected())
      {
        MessageBox.Show("Error - Not Connected to Server");
        return;
      }

      prevMessageNoHTML.Clear();
      prevMessageNoHTML.Append("The University of Wisconsin-Superior is currently operating under normal conditions\r\n");
      prevMessageNoHTML.Append(DateTime.Now.ToString("MMMM d, yyyy h:mm:ss tt CDT\r\n"));
      prevMessageNoHTML.Append("In the Event of an Emergency, the campus community will be notified through a variety of mechanisms including the web, email, voicemail, and text messages.\r\n");
      prevMessageNoHTML.Append("The UW-Superior homepage will broadcast an alert and direct users to an emergency page outlining the University's response.");

      //Call Clearing info to input standard normal operating message
      informationUpdate = hwriter.Get_Clearing_Info();

      Update_Website();
    }

    protected void bttnConnection_Click(object sender, EventArgs e)
    {
      if (!Connection.IsSFTPConnected())
      {
        try
        {
          Connection.ConnectSFTP();
        }
        catch (Exception ex)
        {
          MessageBox.Show("Error - " + ex);
        }

        if (Connection.IsSFTPConnected())
        {
          bttnClearWebsite.Visible = true;
          bttnSubmit.Visible = true;
          drpdwnAddOrReplace.Visible = true;

          lblConnection.Text = "CONNECTED";
          lblConnection.ForeColor = System.Drawing.Color.Black;
          bttnConnection.Text = "Disconnect";

          prevMessageNoHTML = Connection.RetrievePrevTextFile(tempTextCacheLocation);
          txtCurrentMessage.Text = prevMessageNoHTML.ToString();

        }
      }
      else
      {
        try
        {
          Connection.DisconnectSFTP();
        }
        catch (Exception ex)
        {
          MessageBox.Show("Error - " + ex);
        }

        if (!Connection.IsSFTPConnected())
        {
          bttnClearWebsite.Visible = false;
          bttnSubmit.Visible = false;
          drpdwnAddOrReplace.Visible = false;

          lblConnection.Text = "NOT CONNECTED";
          lblConnection.ForeColor = System.Drawing.Color.Red;
          bttnConnection.Text = "Connect";

          txtCurrentMessage.Text = "";
        }
      }
    }

    private void Update_Website()
    {
      StringBuilder updateInfo = new StringBuilder();
      updateInfo.AppendLine(hwriter.Get_HTML_Header().ToString());
      updateInfo.AppendLine(hwriter.Get_HTML_Body(informationUpdate).ToString());
      updateInfo.AppendLine(hwriter.Get_HTML_Footer().ToString());


      try
      {
        //Write HTML file to temp file to transfer to server    
        File.WriteAllText(tempASPLocation, updateInfo.ToString());

        //Write previous ASP message to cache file to retrieve on add to message next time
        File.WriteAllText(tempASPCacheLocation, informationUpdate.ToString());

        //write previous text version of message to cache file to retrieve on add to message
        File.WriteAllText(tempTextCacheLocation, prevMessageNoHTML.ToString());
      }
      catch(Exception ex){
        MessageBox.Show("Error - " + ex);
      }

      Connection.TransferFiles(tempASPLocation, tempASPCacheLocation, tempTextCacheLocation);

      MessageBox.Show("Website Update Completed");

      txtMessage.Text = "";
      txtSubject.Text = "";

      txtCurrentMessage.Text = prevMessageNoHTML.ToString();
    }


    private StringBuilder Get_Updated_Info()
    {
      StringBuilder info = new StringBuilder();

      info.Append(hwriter.UpdatedInfoHelper(txtSubject.Text, txtMessage.Text));
      
      //Retrieve Cache Info
      if (drpdwnAddOrReplace.SelectedValue == "Add To Current Message")
      {
        info.Append(Connection.RetrievePrevASPFile(tempASPCacheLocation));
      }

      return info;
    }

    private StringBuilder Get_Updated_Info_Text() {
      StringBuilder info = new StringBuilder();

      //Retrieve Cache Info
      if (drpdwnAddOrReplace.SelectedValue == "Add To Current Message")
      {
        info = Connection.RetrievePrevTextFile(tempTextCacheLocation);
      }

      info.Append(hwriter.UpdatedInfoHelperText(txtSubject.Text, txtMessage.Text));

      return info;
    }
  }


}