using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace EmergencyWebsiteUpdate {
	public class MessageBoxClass {
		private Page CallingPage = null;
		public MessageBoxClass(Page CallingPage) {
			this.CallingPage = CallingPage;
			}
		public void Show(String Message) {
			CallingPage.Response.Write("<script>alert('" + Message + "')</script>");
			}
		}
	}
