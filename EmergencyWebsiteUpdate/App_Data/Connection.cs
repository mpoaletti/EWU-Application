using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace EmergencyWebsiteUpdate
{
  public partial class Connection
  {
    //Declare Variables for SFTP connection to GoDaddy website
    private static int GDPort;
    private static string GDServer;
    private static string GDUsername;
    private static string GDPassword;
    //Remove assignment ---------------------------
    private static string GDFilePath = "/home/efurzlan";

    //SFTP Client for connecting to GoDaddy
    public static SftpClient godaddySFTPClient;

    //SSH Client for connecting to secrets manager (Password Manager Pro)
    private static SshClient pwmgrSSHClient;

    //connection status booleans
    private static bool sftpConnected = false;
    private static bool sshConnected = false;

    //Connect to PMP over SSH for retrieving secrets
    public static void ConnectSSH() {
      try
      {
        PrivateKeyFile pkFile = new PrivateKeyFile(@"C:\Users\efurzlan\.ssh\id_rsa");
        //Uncomment for Production and update for location of file on server-------------------------
        //PrivateKeyFile pkFile = new PrivateKeyFile(@"C:\Users\efurzlan\.ssh\id_rsa");

        pwmgrSSHClient = new SshClient("secpmgr.uwsuper.edu", 5522, "efurzlanAPI", pkFile);
        //Uncomment for Production-----------------------------------
        //pwmgrSSHClient = new SshClient("secpmgr.uwsuper.edu", 5522, "APPS1API", pkFile);

        pwmgrSSHClient.Connect();
        sshConnected = true;
      }
      catch (Exception ex) {
        MessageBox.Show("Error - " + ex);
      }
    }

    public static void ConnectSFTP() {
      ConnectSSH();
      GDServer = RetrieveSSHData("GDServer");
      int.TryParse(RetrieveSSHData("GDPort"), out GDPort);
      GDUsername = RetrieveSSHData("GDUsername");
      GDPassword = RetrieveSSHData("GDPassword");
      DisconnectSSH();

      try
      {
        godaddySFTPClient = new SftpClient(GDServer, GDPort, GDUsername, GDPassword);
        godaddySFTPClient.Connect();
        sftpConnected = true;
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error - " + ex);
      }
    }

    public static void DisconnectSSH() {
      try
      {
        pwmgrSSHClient.Disconnect();
        sshConnected = false;
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error - " + ex);
      }
    }

    public static void DisconnectSFTP() {
      try
      {
        godaddySFTPClient.Disconnect();
        sftpConnected = false;
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error - " + ex);
      }
    }

    public static bool IsSFTPConnected() {
      return sftpConnected;
    }

    public static bool IsSSHConnected()
    {
      return sshConnected;
    }

    public static string RetrieveSSHData(string acctInfo) {
      if(IsSSHConnected()) {
        SshCommand retData = pwmgrSSHClient.CreateCommand("RETRIEVE --resource=EWUAPP --account=" + acctInfo);
        retData.Execute();
        return retData.Result.Substring(0,retData.Result.Length-2); //remove last 2 characters of /r/n
      }
      else {
        return "";
      }
    }

    //Transfer files to the GoDaddy server, transfer all files in one go using sftp
    public static void TransferFiles(string tempASPLocation, string tempASPCacheLocation, string tempTextCacheLocation)
    {
      try
      {
        godaddySFTPClient.ChangeDirectory(GDFilePath);

        //transfer asp file to store for future updates
        using (FileStream fs = new FileStream(tempASPCacheLocation, FileMode.Open))
        {
          godaddySFTPClient.BufferSize = 4 * 1024;
          godaddySFTPClient.UploadFile(fs, GDFilePath + "/previousASP.txt");
        }

        //transfer asp file for updating website
        using (FileStream fs = new FileStream(tempASPLocation, FileMode.Open))
        {
          godaddySFTPClient.BufferSize = 4 * 1024;
          godaddySFTPClient.UploadFile(fs, GDFilePath + "/testing.asp");     //UPDATE WITH Correct file name-------------------
        }

        using (FileStream fs = new FileStream(tempTextCacheLocation, FileMode.Open))
        {
          godaddySFTPClient.BufferSize = 4 * 1024;
          godaddySFTPClient.UploadFile(fs, GDFilePath + "/previousText.txt");
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error - " + ex);
      }
    }

    //retrieve the existing ASP version of previous message using sftp and return
    public static StringBuilder RetrievePrevASPFile(string tempASPCacheLocation)
    {
      StringBuilder oldInfo = new StringBuilder();

      try
      {
        godaddySFTPClient.ChangeDirectory(GDFilePath);

        using (FileStream fs = File.Create(tempASPCacheLocation))
        {
          godaddySFTPClient.DownloadFile(GDFilePath + "/previousASP.txt", fs);
        }
        oldInfo.AppendLine(System.IO.File.ReadAllText(tempASPCacheLocation));
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error - " + ex);
      }

      return oldInfo;
    }

    //retrieve the existing text version of previous message using sftp and return
    public static StringBuilder RetrievePrevTextFile(string tempTextCacheLocation)
    {
      StringBuilder oldInfo = new StringBuilder();

      try
      {
        godaddySFTPClient.ChangeDirectory(GDFilePath);

        using (FileStream fs = File.Create(tempTextCacheLocation))
        {
          godaddySFTPClient.DownloadFile(GDFilePath + "/previousText.txt", fs);
        }
        oldInfo.AppendLine(System.IO.File.ReadAllText(tempTextCacheLocation));
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error - " + ex);
      }

      return oldInfo;
    }
  }
}