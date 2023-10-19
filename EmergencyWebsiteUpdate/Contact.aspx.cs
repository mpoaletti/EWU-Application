using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EmergencyWebsiteUpdate
{
	public partial class Contact : Page
	{
		private static System.Web.UI.HtmlControls.HtmlGenericControl logOutControl;

		protected void Page_Load(object sender, EventArgs e)
		{
			//Hiding Logout Control until Okta Logout function is fixed - currently error response
			logOutControl = (System.Web.UI.HtmlControls.HtmlGenericControl)Master.FindControl("LogOut");
			if (logOutControl != null) logOutControl.Visible = false;

			if (!Request.IsAuthenticated) Response.Redirect("LoginPage.aspx", false);
		}
	}
}