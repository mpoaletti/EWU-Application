using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows;
using System.Web.Security;
using System.DirectoryServices.Protocols;
using System.Net;

namespace EmergencyWebsiteUpdate
{
  public partial class LoginPage : Page
  {
    //declare ldap username and password variables
    public static string ldapUserName;
    private string ldapPW;
    private string last4Phone;

    private int minRandomNum = 100000;
    private int maxRandomNum = 999999;
    private static int randNum;

    //declare control for LogOut page location to send users back to if not logged in
    System.Web.UI.HtmlControls.HtmlGenericControl logOutControl;

    protected void Page_Load(object sender, EventArgs e)
    {
      logOutControl = (System.Web.UI.HtmlControls.HtmlGenericControl)Master.FindControl("LogOut");
      if(logOutControl != null) logOutControl.Visible = false;
      Session["logInStatus"] = false;
    }

    protected void bttnLogin_Click(object sender, EventArgs e)
    {
      if (txtboxUsername.Text == "" || txtboxPassword.Text == "")
      {
        MessageBox.Show("Please enter Username and Password, then try again");
      }
      else
      {
        ldapUserName = txtboxUsername.Text;
        ldapPW = txtboxPassword.Text;
        
        if (loginVerified(ldapUserName, ldapPW)) {

          randNum = GenerateRandom(minRandomNum, maxRandomNum);
          last4Phone = "";
          Messaging.MessageUser("Your 6 digit authorization code for EWUApp is: " + randNum, ldapUserName, out last4Phone);

          lblUsername.Visible = false;
          lblPassword.Visible = false;
          txtboxUsername.Visible = false;
          txtboxPassword.Visible = false;
          bttnLogin.Visible = false;
          txtboxVerificationCode.Visible = true;
          lblVerification.Text = lblVerification.Text + " " + last4Phone;
          lblVerification.Visible = true;
          bttnVerify.Visible = true;
        }
      }
    }

    protected void bttnVerify_Click(object sender, EventArgs e) {
      if (txtboxVerificationCode.Text == randNum.ToString())
      {
        login2FA();
      }
      else {
        lblErrorMessage.Text = "Incorrect code, please try again";
        lblErrorMessage.Visible = true;
        bttnResendVerify.Visible = true;
        bttnCancel.Visible = true;
      }
    }

    protected void bttnResendVerify_Click(object sender, EventArgs e) {
      last4Phone = "";
      Messaging.MessageUser("Your 6 digit authorization code for EWUApp is: " + randNum, ldapUserName, out last4Phone);
    }

    protected void bttnCancel_Click(object sender, EventArgs e)
    {
      bttnResendVerify.Visible = false;
      bttnVerify.Visible = false;
      lblVerification.Visible = false;
      lblErrorMessage.Visible = false;
      lblErrorMessage.Text = "";
      lblPassword.Visible = true;
      lblUsername.Visible = true;
      txtboxUsername.Visible = true;
      txtboxPassword.Visible = true;
    }


    private void login2FA() {
      lblErrorMessage.Text = "Running";
      lblErrorMessage.Visible = true;
      Session["Username"] = ldapUserName;
      Session["logInStatus"] = true;
      FormsAuthenticationTicket tkt;
      string cookiestr;
      HttpCookie cookie;
      tkt = new FormsAuthenticationTicket(1, ldapUserName, DateTime.Now, DateTime.Now.AddMinutes(30), false, "");
      cookiestr = FormsAuthentication.Encrypt(tkt);
      cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);
      cookie.Path = FormsAuthentication.FormsCookiePath;
      Response.Cookies.Add(cookie);
      if (logOutControl != null) logOutControl.Visible = true;
      Response.Redirect("UpdateWebsite.aspx", false);
    }

    private bool loginVerified(string un, string pw)
    {
      //Domain Context 
      PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
      //Finding user
      UserPrincipal ldapUser = UserPrincipal.FindByIdentity(ctx, un);

      //Find the group
      GroupPrincipal grp = GroupPrincipal.FindByIdentity(ctx, "emerappadmins");

      if (ldapUser != null)
      {
        //check user authentication to verify logged in
        if (ctx.ValidateCredentials(un, pw) != true)
        {
          lblErrorMessage.Text = "Invalid Credentials, Please Try Again";
          lblErrorMessage.Visible = true;
          //invalid credentials
          return false;
        }
        else if (ldapUser.IsAccountLockedOut() == true)
        {
            lblErrorMessage.Text = "Account is locked, please contact help desk";
            lblErrorMessage.Visible = true;
            //user's account is locked
            return false;
         }
         else if (ldapUser.AccountExpirationDate != null)
        {
          lblErrorMessage.Text = "Account is locked, please contact help desk";
          lblErrorMessage.Visible = true;
          //User's account is expired
          return false;
        }
        else
        {
          //check if user in group
          if (ldapUser.IsMemberOf(grp))
          {
            lblErrorMessage.Text = "";
            lblErrorMessage.Visible = false;

            return true;
          }
          else
          {
            string msg = "User " + ldapUser + " attempted to access emergency website app without appropriate group access";
            //Message Jordan access was attempted
            //UPDATE USER being sent to ---------------------------
            //Messaging.MessageUser(msg, "jmilan");

            //then show the user a message that they do not have access
            lblErrorMessage.Text = "Access restricted, please contact help desk to request access";
            lblErrorMessage.Visible = true;

            return false;
          }
        }
      }

      else
      {
        lblErrorMessage.Text = "Error - contact help desk";
        lblErrorMessage.Visible = true;
        //user is null
        return false;
      }
    }

    private int GenerateRandom(int min, int max)
    {
      Random rnd = new Random();
      int r = rnd.Next(min, max);
      return r;
    }
	}
}