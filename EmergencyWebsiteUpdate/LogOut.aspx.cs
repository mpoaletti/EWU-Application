using System;
using System.Web;
using Microsoft.Owin.Security.Cookies;

namespace EmergencyWebsiteUpdate {
	public partial class LogOut : System.Web.UI.Page {
		public MessageBoxClass MessageBox = null;

		protected void Page_Load(object sender, EventArgs e) {
			HttpContext.Current.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
			Response.Redirect("LoginPage.aspx", false);
		}
	}

	public class SignOutController : System.Web.Mvc.Controller {
		[System.Web.Mvc.HttpPost]
		public System.Web.Mvc.ActionResult SignOut(HttpContext context){

			context.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
			return RedirectToAction("Index", "LoginPage.aspx");
		}
	}
}
