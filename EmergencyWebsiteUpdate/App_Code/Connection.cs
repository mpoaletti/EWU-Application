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

		public Connection(Page CallingPage) {
			this.CallingPage = CallingPage;
			this.MessageBox = new MessageBoxClass(CallingPage);
		}

		public void UploadFile(FileStream fs, String EndPointURL) {
			String azureURI = Decryption.DecryptString((String)ConfigurationManager.AppSettings["decryptKey"], (String)System.Configuration.ConfigurationManager.AppSettings["azureURI"]);
			SecretClient clientSC = new SecretClient(new Uri(azureURI), new DefaultAzureCredential());

			String Username = clientSC.GetSecret("GDUsername").Value.Value;
			String Password = clientSC.GetSecret("GDPassword").Value.Value;

			bool testUpload = false;
			String testUploadVal = (String)System.Configuration.ConfigurationManager.AppSettings["TestUpload"];
			if (testUploadVal != null) testUpload = (testUploadVal.ToLower() == "true");

			if (testUpload) {
				Username = clientSC.GetSecret("TestUsername").Value.Value;
				Password = clientSC.GetSecret("TestPassword").Value.Value;
			}

			StreamReader readStream = new StreamReader(fs);

			FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(EndPointURL);
			ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
			ftpRequest.Credentials = new NetworkCredential(Username, Password);

			using (Stream ftpRequestStream = ftpRequest.GetRequestStream())
			{
				fs.CopyTo(ftpRequestStream);
			}
		}

		public class RegularFTPConnector {	
			private Page CallingPage = null;
			SecretClient clientSC;
			bool IsTesting = false;

			public RegularFTPConnector(Page CallingPage) {
				this.CallingPage = CallingPage;
				String azureURI = Decryption.DecryptString((String)ConfigurationManager.AppSettings["decryptKey"], (String)System.Configuration.ConfigurationManager.AppSettings["azureURI"]);
				clientSC = new SecretClient(new Uri(azureURI), new DefaultAzureCredential());
				
				String testUploadVal = (String)System.Configuration.ConfigurationManager.AppSettings["TestUpload"];
				if (testUploadVal != null) IsTesting = (testUploadVal.ToLower() == "true");
			}

			public String GetEmergencyContentASP() {
				String TestingASPFile = clientSC.GetSecret("FTPPathTestingFileLocation").Value.Value;
				String ProductionASPFile = clientSC.GetSecret("FTPPathProductionASPFileLocation").Value.Value;
				String RemoteFtpPath = (IsTesting?TestingASPFile:ProductionASPFile);
				return GetContent(RemoteFtpPath);
			}

			public String GetEmergencyContentPreviousText() {
				String RemoteFtpPath = clientSC.GetSecret("FTPPathPreviousTXTMessageFileLocation").Value.Value;
				if(IsTesting) RemoteFtpPath = clientSC.GetSecret("FTPPathTestingPreviousTXTMessageFileLocation").Value.Value;
				return GetContent(RemoteFtpPath);
			}

			public String GetEmergencyContentPreviousASP() {
				String RemoteFtpPath = clientSC.GetSecret("FTPPathPreviousASPMessageFileLocation").Value.Value;
				if (IsTesting) RemoteFtpPath = clientSC.GetSecret("FTPPathTestingPreviousASPMessageFileLocation").Value.Value;
				return GetContent(RemoteFtpPath);
			}

			public String GetContent(String RemoteFtpPath) {
				String Username = clientSC.GetSecret("GDUsername").Value.Value;
				String Password = clientSC.GetSecret("GDPassword").Value.Value;

				if(IsTesting)
				{
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
			String azureURI = Decryption.DecryptString((String)ConfigurationManager.AppSettings["decryptKey"], (String)System.Configuration.ConfigurationManager.AppSettings["azureURI"]);
			SecretClient clientSC = new SecretClient(new Uri(azureURI), new DefaultAzureCredential());

			String GDFilePath = clientSC.GetSecret("GDFilePath").Value.Value;
			String GDTestUpload = clientSC.GetSecret("GDTestUpload").Value.Value;

			bool IsTesting = false;

			String testUploadVal = (String)System.Configuration.ConfigurationManager.AppSettings["TestUpload"];
			if (testUploadVal != null) IsTesting = (testUploadVal.ToLower() == "true");

			String FilePath = (IsTesting ? GDTestUpload : GDFilePath);

			using (FileStream fs = new FileStream(tempASPLocation, FileMode.Open))
			{
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
		}
	}
}
