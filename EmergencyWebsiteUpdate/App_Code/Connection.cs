using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web.UI;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace EmergencyWebsiteUpdate {
	public class Connection {
		public MessageBoxClass MessageBox = null;
		public Page CallingPage = null;
		//private String username;
		//private String password;

		public Connection(Page CallingPage) {
			this.CallingPage = CallingPage;
			this.MessageBox = new MessageBoxClass(CallingPage);
		}

		public void UploadFile(FileStream fs, String EndPointURL) {
			//String Username = Decryption.DecryptString((String)ConfigurationManager.AppSettings["decryptKey"], (String)System.Configuration.ConfigurationManager.AppSettings["MontyCheatGDUsername"]);
			//String Password = Decryption.DecryptString((String)ConfigurationManager.AppSettings["decryptKey"], (String)System.Configuration.ConfigurationManager.AppSettings["MontyCheatGDPassword"]);
			
			SecretClient clientSC = new SecretClient(new Uri("https://ewukv.vault.azure.net/"), new DefaultAzureCredential());

			//String Username = Decryption.DecryptString(clientSC.GetSecret("EncryptKey").Value.Value, clientSC.GetSecret("GDUsername").Value.Value);
			//String Password = Decryption.DecryptString(clientSC.GetSecret("EncryptKey").Value.Value, clientSC.GetSecret("GDPassword").Value.Value);
			String Username = clientSC.GetSecret("GDUsername").Value.Value;
			String Password = clientSC.GetSecret("GDPassword").Value.Value;


			bool testUpload = false;
			String testUploadVal = (String)System.Configuration.ConfigurationManager.AppSettings["TestUpload"];
			if (testUploadVal != null) testUpload = (testUploadVal.ToLower() == "true");

			if (testUpload) {
				//Username = Decryption.DecryptString(clientSC.GetSecret("EncryptKey").Value.Value, clientSC.GetSecret("TestUsername").Value.Value);
				//Password = Decryption.DecryptString(clientSC.GetSecret("EncryptKey").Value.Value, clientSC.GetSecret("TestPassword").Value.Value);
				Username = clientSC.GetSecret("TestUsername").Value.Value;
				Password = clientSC.GetSecret("TestPassword").Value.Value;

			}

			//String FileContent = null;
			StreamReader readStream = new StreamReader(fs);
			//FileStream newStream = readStream.ReadToEnd();
			/*using (StreamReader reader = new StreamReader(fs)) {
				FileContent = reader.ReadToEnd();
			}*/
			//if (FileContent.Length>0) {
				//byte[] fileContent = Encoding.ASCII.GetBytes(FileContent);

				FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(EndPointURL);
				ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
				//ftpRequest.KeepAlive = true;
				//ftpRequest.UsePassive = true;
				//ftpRequest.UseBinary = true;
				ftpRequest.Credentials = new NetworkCredential(Username, Password);

				//try{
				using (Stream ftpRequestStream = ftpRequest.GetRequestStream())
				{
					fs.CopyTo(ftpRequestStream);
				}
				//catch(Exception ex) {
				//	MessageBox.Show(ex.Message);
				//}
				//FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
				//MessageBox.Show(ftpResponse.ToString());
				
				/*using (var client = new WebClient())
				{
					client.Credentials = new NetworkCredential(Username, Password);

					
					try{
						client.UploadData(EndPointURL, fileContent);
					}
					catch(Exception ex) {
						MessageBox.Show(ex.Message);
					}


					//using (var postStream = client.OpenWrite(EndPointURL))
					//{
					//	postStream.Write(fileContent, 0, fileContent.Length);
					//}
				}*/
			//}
		}

		public class RegularFTPConnector {
			
			private Page CallingPage = null;
			//String TestingASPFile = Decryption.DecryptString((String)ConfigurationManager.AppSettings["decryptKey"], (String)System.Configuration.ConfigurationManager.AppSettings["FTPPathTestingFileLocation"]);
			//String ProductionASPFile = Decryption.DecryptString((String)ConfigurationManager.AppSettings["decryptKey"], (String)System.Configuration.ConfigurationManager.AppSettings["FTPPathProductionASPFileLocation"]);
			SecretClient clientSC;
			bool IsTesting = false;

			public RegularFTPConnector(Page CallingPage) {
				this.CallingPage = CallingPage;
				//SecretClient clientSC = new SecretClient(new Uri("https://ewukv.vault.azure.net/"), new DefaultAzureCredential());
				clientSC = new SecretClient(new Uri("https://ewukv.vault.azure.net/"), new DefaultAzureCredential());
				
				String testUploadVal = (String)System.Configuration.ConfigurationManager.AppSettings["TestUpload"];
				if (testUploadVal != null) IsTesting = (testUploadVal.ToLower() == "true");
			}


			//String TestingASPFile = Decryption.DecryptString((String)ConfigurationManager.AppSettings["decryptKey"], (String)System.Configuration.ConfigurationManager.AppSettings["FTPPathTestingFileLocation"]);
			//String ProductionASPFile = Decryption.DecryptString((String)ConfigurationManager.AppSettings["decryptKey"], (String)System.Configuration.ConfigurationManager.AppSettings["FTPPathProductionASPFileLocation"]);

			//readonly Boolean IsTesting = true;
			public String GetEmergencyContentASP() {
				//String TestingASPFile = Decryption.DecryptString(clientSC.GetSecret("EncryptKey").Value.Value, clientSC.GetSecret("FTPPathTestingFileLocation").Value.Value);
				//String ProductionASPFile = Decryption.DecryptString(clientSC.GetSecret("EncryptKey").Value.Value, clientSC.GetSecret("FTPPathProductionASPFileLocation").Value.Value);
				String TestingASPFile = clientSC.GetSecret("FTPPathTestingFileLocation").Value.Value;
				String ProductionASPFile = clientSC.GetSecret("FTPPathProductionASPFileLocation").Value.Value;

				String RemoteFtpPath = (IsTesting?TestingASPFile:ProductionASPFile);
				return GetContent(RemoteFtpPath);
			}

			public String GetEmergencyContentPreviousText() {
				//String RemoteFtpPath = Decryption.DecryptString((String)ConfigurationManager.AppSettings["decryptKey"], (String)System.Configuration.ConfigurationManager.AppSettings["FTPPathPreviousTXTMessageFileLocation"]);
				//String RemoteFtpPath = Decryption.DecryptString(clientSC.GetSecret("EncryptKey").Value.Value, clientSC.GetSecret("FTPPathPreviousTXTMessageFileLocation").Value.Value);
				String RemoteFtpPath = clientSC.GetSecret("FTPPathPreviousTXTMessageFileLocation").Value.Value;
				if(IsTesting) RemoteFtpPath = clientSC.GetSecret("FTPPathTestingPreviousTXTMessageFileLocation").Value.Value;

				return GetContent(RemoteFtpPath);
			}

			public String GetEmergencyContentPreviousASP() {
				//String RemoteFtpPath = Decryption.DecryptString((String)ConfigurationManager.AppSettings["decryptKey"], (String)System.Configuration.ConfigurationManager.AppSettings["FTPPathPreviousASPMessageFileLocation"]);
				//String RemoteFtpPath = Decryption.DecryptString(clientSC.GetSecret("EncryptKey").Value.Value, clientSC.GetSecret("FTPPathPreviousASPMessageFileLocation").Value.Value);
				String RemoteFtpPath = clientSC.GetSecret("FTPPathPreviousASPMessageFileLocation").Value.Value;
				if (IsTesting) RemoteFtpPath = clientSC.GetSecret("FTPPathTestingPreviousASPMessageFileLocation").Value.Value;

				return GetContent(RemoteFtpPath);
			}

			public String GetContent(String RemoteFtpPath) {
				//String Username = Decryption.DecryptString((String)ConfigurationManager.AppSettings["decryptKey"], (String)System.Configuration.ConfigurationManager.AppSettings["MontyCheatGDUsername"]);
				//String Password = Decryption.DecryptString((String)ConfigurationManager.AppSettings["decryptKey"], (String)System.Configuration.ConfigurationManager.AppSettings["MontyCheatGDPassword"]);

				//SecretClient clientSC = new SecretClient(new Uri("https://ewukv.vault.azure.net/"), new DefaultAzureCredential());

				//String Username = Decryption.DecryptString(clientSC.GetSecret("EncryptKey").Value.Value, clientSC.GetSecret("GDUsername").Value.Value);
				//String Password = Decryption.DecryptString(clientSC.GetSecret("EncryptKey").Value.Value, clientSC.GetSecret("GDPassword").Value.Value);

				String Username = clientSC.GetSecret("GDUsername").Value.Value;
				String Password = clientSC.GetSecret("GDPassword").Value.Value;


				//bool testUpload = false;
				//String testUploadVal = (String)System.Configuration.ConfigurationManager.AppSettings["TestUpload"];
				//if (testUploadVal != null) testUpload = (testUploadVal.ToLower() == "true");

				//if (testUpload)
				if(IsTesting)
				{
					//Username = Decryption.DecryptString(clientSC.GetSecret("EncryptKey").Value.Value, clientSC.GetSecret("TestUsername").Value.Value);
					//Password = Decryption.DecryptString(clientSC.GetSecret("EncryptKey").Value.Value, clientSC.GetSecret("TestPassword").Value.Value);
					Username = clientSC.GetSecret("TestUsername").Value.Value;
					Password = clientSC.GetSecret("TestPassword").Value.Value;

				}

				Boolean UseBinary = true; // use true for .zip file or false for a text file
				Boolean UsePassive = true;
				FtpWebRequest request = (FtpWebRequest)WebRequest.Create(RemoteFtpPath);
				request.Method = WebRequestMethods.Ftp.DownloadFile;
				request.KeepAlive = true;
				request.UsePassive = UsePassive;
				request.UseBinary = UseBinary;
				request.Credentials = new NetworkCredential(Username, Password);
				//UpdateWebsite uw = (UpdateWebsite)CallingPage;
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

		public void TransferFiles(String tempASPLocation, String tempASPCacheLocation, String tempTextCacheLocation) {
			//String GDFilePath = Decryption.DecryptString((String)ConfigurationManager.AppSettings["decryptKey"], (String)System.Configuration.ConfigurationManager.AppSettings["GoDaddyFilePath"]);

			//String GDTestUpload = (String)System.Configuration.ConfigurationManager.AppSettings["GoDaddyTestUpload"];

			SecretClient clientSC = new SecretClient(new Uri("https://ewukv.vault.azure.net/"), new DefaultAzureCredential());

			//String GDFilePath = Decryption.DecryptString(clientSC.GetSecret("EncryptKey").Value.Value, clientSC.GetSecret("GoDaddyFilePath").Value.Value);
			//String GDTestUpload = Decryption.DecryptString(clientSC.GetSecret("EncryptKey").Value.Value, clientSC.GetSecret("GoDaddyTestUpload").Value.Value);

			String GDFilePath = clientSC.GetSecret("GDFilePath").Value.Value;
			String GDTestUpload = clientSC.GetSecret("GDTestUpload").Value.Value;

			bool IsTesting = false;

			String testUploadVal = (String)System.Configuration.ConfigurationManager.AppSettings["TestUpload"];
			if (testUploadVal != null) IsTesting = (testUploadVal.ToLower() == "true");

			//Boolean TestUpload = false;

			//if (GDTestUpload!=null) TestUpload = (GDTestUpload.ToLower()=="true");

			//String FilePath = (TestUpload ? GDTestUpload : GDFilePath);
			String FilePath = (IsTesting ? GDTestUpload : GDFilePath);

			using (FileStream fs = new FileStream(tempASPLocation, FileMode.Open))
			{
				//UploadFile(fs, FilePath + (TestUpload ? "testing.asp" : "/emergency-content.asp"));
				UploadFile(fs, FilePath + (IsTesting ? "/testing.asp" : "/emergency-content.asp"));
			}

			using (FileStream fs = new FileStream(tempASPCacheLocation, FileMode.Open))
			{
				UploadFile(fs, FilePath + "/previousASP.txt");
			}

			using (FileStream fs = new FileStream(tempTextCacheLocation, FileMode.Open))
			{
				UploadFile(fs, FilePath + "/previousText.txt");
			}

			/*
			using (FileStream fs = new FileStream(tempASPLocation, FileMode.Open)) {
				UploadFile(fs, GDFilePath + (TestUpload?"testing.asp":"/emergency-content.asp"));
			}
			
			using (FileStream fs = new FileStream(tempASPCacheLocation, FileMode.Open)) {
				UploadFile(fs, GDFilePath + "/previousASP.txt");
			}

			using (FileStream fs = new FileStream(tempTextCacheLocation, FileMode.Open)) {
				UploadFile(fs, GDFilePath + "/previousText.txt");
			}*/
		}

	}
}
