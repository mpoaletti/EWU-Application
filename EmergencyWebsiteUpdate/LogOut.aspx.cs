using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Renci.SshNet;

namespace EmergencyWebsiteUpdate {
	public partial class LogOut : System.Web.UI.Page {
		public MessageBoxClass MessageBox = null;
		public LogOut(Page CallingPage) {
			this.MessageBox = new MessageBoxClass(CallingPage);
			}
		protected void Page_Load(object sender, EventArgs e) {
			//Page CallingPage = this;
			//Connection Connection = new Connection(this);
			//SftpClient godaddySFTPClient = (SftpClient)CallingPage.Session["godaddySFTPClient"];
			//if (Connection.godaddySFTPClient != null) {
			//		if (!Connection.IsSFTPConnected()) {
			//			try {
			//				Connection.godaddySFTPClient.Disconnect();
			//				}
			//			catch(Exception ex) {
			//				MessageBox.Show("Error - " + ex.Message);
			//			}
			//		}
			//	}
			Context.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType, OpenIdConnectAuthenticationDefaults.AuthenticationType);
			}
		}
	}
