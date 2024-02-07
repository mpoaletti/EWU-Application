using System;
using System.Web;
using System.Web.UI;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;

namespace EmergencyWebsiteUpdate {
	public partial class LoginPage : Page {
		//declare control for LogOut page location to send users back to if not logged in
		private static System.Web.UI.HtmlControls.HtmlGenericControl logOutControl;
	
		protected void Page_Load(object sender, EventArgs e) {
			logOutControl = (System.Web.UI.HtmlControls.HtmlGenericControl)Master.FindControl("LogOut");
			if(logOutControl != null) logOutControl.Visible = false;
			if (Request.IsAuthenticated) {
				Session["Username"] = HttpContext.Current.GetOwinContext().Request.User.Identity.Name;
				Session["logInStatus"] = true;
				Response.Redirect("UpdateWebsite.aspx", false);
			}
			else Session["logInStatus"] = false;
		}

		protected void bttnLogin_Click(object sender, EventArgs e) {
			if (!Request.IsAuthenticated) {
				HttpContext.Current.GetOwinContext().Authentication.Challenge(
					new AuthenticationProperties { RedirectUri = "/" }, OpenIdConnectAuthenticationDefaults.AuthenticationType
					);
			}
		}
	}
}
