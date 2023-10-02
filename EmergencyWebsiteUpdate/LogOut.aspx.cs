using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Windows;

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

			FormsAuthentication.SignOut();
			Response.Redirect("LoginPage", true);
		}
	}
}