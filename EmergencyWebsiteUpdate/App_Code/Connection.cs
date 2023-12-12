using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EmergencyWebsiteUpdate {
  public class Connection {
    //Declare Variables for SFTP connection to GoDaddy website
    private int GDPort;
    private string GDServer;
    private string GDUsername;
    private string GDPassword;
    private string GDFilePath = (String)System.Configuration.ConfigurationManager.AppSettings["GoDaddyFilePath"]; 
    //SFTP Client for connecting to GoDaddy
    public SftpClient godaddySFTPClient;
    //SSH Client for connecting to secrets manager (Password Manager Pro)
    private SshClient pwmgrSSHClient;
		public MessageBoxClass MessageBox = null;
		public Page CallingPage = null;

		public Connection(Page CallingPage) {
			this.CallingPage = CallingPage;
			this.MessageBox = new MessageBoxClass(CallingPage);
			}

    public void ConnectSSH() {
			String PrivateKeyFileName = Decryption.DecryptString((String)ConfigurationManager.AppSettings["decryptKey"], (String)System.Configuration.ConfigurationManager.AppSettings["PrivateKeyFileName"]);
			String APIUserName = Decryption.DecryptString((String)ConfigurationManager.AppSettings["decryptKey"], (String)System.Configuration.ConfigurationManager.AppSettings["APIUserName"]);
			String APIHostUrl = Decryption.DecryptString((String)ConfigurationManager.AppSettings["decryptKey"], (String)System.Configuration.ConfigurationManager.AppSettings["APIHostUrl"]);
			String APIHostPort = Decryption.DecryptString((String)ConfigurationManager.AppSettings["decryptKey"], (String)System.Configuration.ConfigurationManager.AppSettings["APIHostPort"]);
			

			if (PrivateKeyFileName==null || APIUserName==null || APIHostUrl==null || APIHostPort==null)
	      MessageBox.Show("Error - Application Settings PrivateKeyFileName, APIUserName, APIHostUrl, and APIHostPort Are Not All Set in web.config");
			else
				try {
					//PrivateKeyFile pkFile = new PrivateKeyFile(@"C:\Users\efurzlan\.ssh\id_rsa");
					//Uncomment for Production and update for location of file on server-------------------------
					//pwmgrSSHClient = new SshClient("secpmgr.uwsuper.edu", 5522, "efurzlanAPI", pkFile);
					//Uncomment for Production-----------------------------------
					//pwmgrSSHClient = new SshClient("secpmgr.uwsuper.edu", 5522, "APPS1API", pkFile);
					PrivateKeyFile pkFile = new PrivateKeyFile(PrivateKeyFileName);
					pwmgrSSHClient = new SshClient(APIHostUrl, Convert.ToInt32(APIHostPort), APIUserName, pkFile);
					pwmgrSSHClient.Connect();
					//sshConnected = true;
					CallingPage.Session["sshConnected"] = true;
					}
				catch (Exception ex) {
					MessageBox.Show("Error - " + ex.Message);
					}
		  }

		public void UploadFile(FileStream fs, String EndPointURL) {
			String Username = Decryption.DecryptString((String)ConfigurationManager.AppSettings["decryptKey"], (String)System.Configuration.ConfigurationManager.AppSettings["MontyCheatGDUsername"]);
			String Password = Decryption.DecryptString((String)ConfigurationManager.AppSettings["decryptKey"], (String)System.Configuration.ConfigurationManager.AppSettings["MontyCheatGDPassword"]);
			

			String FileContent = null;
			using (StreamReader reader = new StreamReader(fs)) {
				FileContent = reader.ReadToEnd();
				}
			if (FileContent.Length>0) {
				byte[] fileContent = Encoding.ASCII.GetBytes(FileContent);
				using (var client=new WebClient()) {
					client.Credentials = new NetworkCredential(Username, Password);
					using (var postStream = client.OpenWrite(EndPointURL)) {
						postStream.Write(fileContent, 0, fileContent.Length);
						}
					}
				}
			}

		public class RegularFTPConnector {
			private Page CallingPage = null;
			public RegularFTPConnector(Page CallingPage) {
				this.CallingPage = CallingPage;
				}
			String TestingASPFile = "ftp://107.180.50.87/httpdocs/testing.asp";
			String ProductionASPFile = "ftp://107.180.50.87/httpdocs/emergency-content.asp";
			readonly Boolean IsTesting = true;
			public String GetEmergencyContentASP() {
				String RemoteFtpPath = (IsTesting?TestingASPFile:ProductionASPFile);
				return GetContent(RemoteFtpPath);
				}
			public String GetEmergencyContentPreviousText() {
				String RemoteFtpPath = "ftp://107.180.50.87/httpdocs/previousText.txt";
				return GetContent(RemoteFtpPath);
				}
			public String GetEmergencyContentPreviousASP() {
				String RemoteFtpPath = "ftp://107.180.50.87/httpdocs/previousASP.txt";
				return GetContent(RemoteFtpPath);
				}
			public String GetContent(String RemoteFtpPath) {
				String Username="UWSWebmaster";
				String Password = "B_k92g3r";
				Boolean UseBinary = true; // use true for .zip file or false for a text file
				Boolean UsePassive = true;
				FtpWebRequest request = (FtpWebRequest)WebRequest.Create(RemoteFtpPath);
				request.Method = WebRequestMethods.Ftp.DownloadFile;
				request.KeepAlive = true;
				request.UsePassive = UsePassive;
				request.UseBinary = UseBinary;
				request.Credentials = new NetworkCredential(Username, Password);
				UpdateWebsite uw = (UpdateWebsite)CallingPage;
				FtpWebResponse response = (FtpWebResponse)request.GetResponse();
				Stream responseStream = response.GetResponseStream();
				StreamReader reader = new StreamReader(responseStream);
				MemoryStream writer = new MemoryStream();
				long length = response.ContentLength;
				int bufferSize = 2048;
				int readCount;
				byte[] buffer = new byte[2048];
				readCount = responseStream.Read(buffer, 0, bufferSize);
				while (readCount > 0) {
					writer.Write(buffer, 0, readCount);
					readCount = responseStream.Read(buffer, 0, bufferSize);
					}
				reader.Close();
				response.Close();
				writer.Position = 0;
				StreamReader r = new StreamReader(writer);
				return r.ReadToEnd();
				}
			}

		//public String MessageBufferASP = null;
		//public String MessageBufferPreviousText = null;
		//public String MessageBufferPreviousASP = null;

    public void ConnectSFTP() {
			//String MontySaysCheatOnPasswordStuff = (String)System.Configuration.ConfigurationManager.AppSettings["MontySaysCheatOnPasswordStuff"];
			//if (MontySaysCheatOnPasswordStuff!="0") {

			String MontyCheatGDServer = Decryption.DecryptString((String)ConfigurationManager.AppSettings["decryptKey"], (String)System.Configuration.ConfigurationManager.AppSettings["MontyCheatGDServer"]);
			String MontyCheatGDPort = Decryption.DecryptString((String)ConfigurationManager.AppSettings["decryptKey"], (String)System.Configuration.ConfigurationManager.AppSettings["MontyCheatGDPort"]);
			String MontyCheatGDUsername = Decryption.DecryptString((String)ConfigurationManager.AppSettings["decryptKey"], (String)System.Configuration.ConfigurationManager.AppSettings["MontyCheatGDUsername"]);
			String MontyCheatGDPassword = Decryption.DecryptString((String)ConfigurationManager.AppSettings["decryptKey"], (String)System.Configuration.ConfigurationManager.AppSettings["MontyCheatGDPassword"]);
		

			GDServer = MontyCheatGDServer; GDPort = Convert.ToInt32(MontyCheatGDPort); GDUsername = MontyCheatGDUsername; GDPassword = MontyCheatGDPassword;
			//	}
			//else {
				//ConnectSSH();
				//GDServer = RetrieveSSHData("GDServer");
				//int.TryParse(RetrieveSSHData("GDPort"), out GDPort); // How About GDPort = Convert.ToInt32(RetrieveSSHData("GDPort")) ?;
				//GDUsername = RetrieveSSHData("GDUsername");
				//GDPassword = RetrieveSSHData("GDPassword");
				//DisconnectSSH();
				//}
			try {
				RegularFTPConnector ftp = new RegularFTPConnector(CallingPage);
				//MessageBufferPreviousText = ftp.GetEmergencyContentPreviousText();
				CallingPage.Session["sftpConnected"] = true;
				}
			catch (Exception ex) {
				MessageBox.Show("Error - " + ex.Message);
				}
	    }
    public void DisconnectSSH() {
      try {
        pwmgrSSHClient.Disconnect();
        ////sshConnected = false;
				CallingPage.Session["sshConnected"] = false;
	      }
      catch (Exception ex) {
        MessageBox.Show("Error - " + ex.Message);
	      }
		  }

    public void DisconnectSFTP() {
      try {
				SftpClient godaddySFTPClient = (SftpClient)CallingPage.Session["godaddySFTPClient"];
				godaddySFTPClient.Disconnect();
				CallingPage.Session["sftpConnected"] = false;
	      }
      catch (Exception ex) {
        MessageBox.Show("Error - " + ex.Message);
	      }
		  }

    public bool IsSFTPConnected() { 
			if (CallingPage.Session["sftpConnected"]==null)
				return false;
			else
				return (Boolean)CallingPage.Session["sftpConnected"];
	    }

    public bool IsSSHConnected() {
			if (CallingPage.Session["sshConnected"]==null)
				return false;
			else
				return (Boolean)CallingPage.Session["sshConnected"];
	    }

    public string RetrieveSSHData(string acctInfo) {
      if(IsSSHConnected()) {
        SshCommand retData = pwmgrSSHClient.CreateCommand("RETRIEVE --resource=EWUAPP --account=" + acctInfo);
        retData.Execute();
        return retData.Result.Substring(0,retData.Result.Length-2); //remove last 2 characters of /r/n
	      }
      else {
        return "";
	      }
		  }

    public void TransferFiles(String tempASPLocation, String tempASPCacheLocation, String tempTextCacheLocation) {
			GDFilePath = Decryption.DecryptString((String)ConfigurationManager.AppSettings["decryptKey"], (String)System.Configuration.ConfigurationManager.AppSettings["GoDaddyFilePath"]);

			String GDTestUpload = (String)System.Configuration.ConfigurationManager.AppSettings["GoDaddyTestUpload"];
			
			Boolean TestUpload = false;
			if (GDTestUpload!=null)
				TestUpload = (GDTestUpload.ToLower()=="true");
      using (FileStream fs = new FileStream(tempASPLocation, FileMode.Open)) {
        UploadFile(fs, GDFilePath + (TestUpload?"testing.asp":"/emergency-content.asp"));
		    }
      using (FileStream fs = new FileStream(tempASPCacheLocation, FileMode.Open)) {
        UploadFile(fs, GDFilePath + "/previousASP.txt");
	      }
      using (FileStream fs = new FileStream(tempTextCacheLocation, FileMode.Open)) {
        UploadFile(fs, GDFilePath + "/previousText.txt");
	      }
			}

	}
}
