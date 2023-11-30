﻿using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Okta.AspNet;
using Owin;
using Microsoft.Owin.Security.Cookies;

#pragma warning disable SA1300 // Element should begin with upper-case letter
namespace EmergencyWebsiteUpdate
{
	public partial class SiteMaster : MasterPage
	{

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		protected void BtnLogin_Click(object sender, EventArgs e)
		{
			if (!Request.IsAuthenticated)
			{
				HttpContext.Current.GetOwinContext().Authentication.Challenge(
					new AuthenticationProperties { RedirectUri = "https://ewu.uwsuper.edu:4443/UpdateWebsite" },
					OpenIdConnectAuthenticationDefaults.AuthenticationType);
			}

		}

		protected void BtnLogout_Click(object sender, EventArgs e)
		{
			Response.Redirect("LogOut.aspx", false);
		}
	}
}