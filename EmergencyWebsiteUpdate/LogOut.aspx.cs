using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Windows;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;

namespace EmergencyWebsiteUpdate
{
	public partial class LogOut : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if(Connection.godaddySFTPClient != null) {
					if (!Connection.IsSFTPConnected()) {
						try
						{
						Connection.godaddySFTPClient.Disconnect();
					}
					catch(Exception ex) {
						MessageBox.Show("Error - " + ex);
					}
				}
			}

			Context.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType, OpenIdConnectAuthenticationDefaults.AuthenticationType);
		}
	}
}