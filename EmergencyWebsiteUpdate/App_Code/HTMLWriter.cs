using System.Text;
using System.Security.Cryptography;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace EmergencyWebsiteUpdate {
  public partial class HTMLWriter {
    //generate and return HTML code for All Clear messaging
    public StringBuilder Get_Clearing_Info() {
      StringBuilder clearingInfo = new StringBuilder();
      clearingInfo.AppendLine("<div id=\"allOK\">");
      clearingInfo.AppendLine("<h2>The University of Wisconsin-Superior is currently operating under normal conditions</h2>");
      clearingInfo.Append("<div class=\"timestamp\">");
      clearingInfo.Append(DateTime.Now.ToString("MMMM d, yyyy h:mm:ss tt CDT"));
      clearingInfo.AppendLine("</div>");
      clearingInfo.AppendLine("<p>In the Event of an Emergency, the campus community will be notified through a variety of mechanisms including the web, email, voicemail, and text messages.");
      clearingInfo.AppendLine("The UW-Superior homepage will broadcast an alert and direct users to an emergency page outlining the University's response.</p>");
      clearingInfo.AppendLine("</div>");
      return clearingInfo;
	    }
    //Function to take subject and message from input parameters and encode in HTML to return
    public StringBuilder UpdatedInfoHelper(string txtSubject, string txtMessage) {
      StringBuilder info = new StringBuilder();
      info.AppendLine("<div class=\"message\">");
      info.Append("<h2>");
      info.Append(txtSubject);
      info.AppendLine("</h2>");
      info.Append("<div class=\"timestamp\">");
      info.Append(DateTime.Now.ToString("MMMM d, yyyy h:mm:ss tt CDT"));
      info.AppendLine("</div>");
      info.AppendLine("<p>");
      info.AppendLine(txtMessage);
      info.AppendLine("</p>");
      info.AppendLine("</div>");
      return info;
	    }

    //function to take subject and text from input parameters and return without html
    public StringBuilder UpdatedInfoHelperText(string txtSubject, string txtMessage) {
      StringBuilder info = new StringBuilder();
      info.Append(StripHTML(txtSubject) + "\r\n");
      info.Append(DateTime.Now.ToString("MMMM d, yyyy h:mm:ss tt CDT") + "\r\n");
      info.AppendLine(StripHTML(txtMessage));
      return info;
	    }

    //generate the header HTML needed and return
    public StringBuilder Get_HTML_Header() {
      StringBuilder strReturn = new StringBuilder();
      strReturn.AppendLine("<!doctype html>");
      strReturn.AppendLine("<html>");
      strReturn.AppendLine("<head>");
      strReturn.AppendLine("<script src=\"http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js\" type=\"text/javascript\"></script>");
      strReturn.AppendLine("<script>");
      strReturn.AppendLine("window.ga=window.ga||function(){(ga.q=ga.q||[]).push(arguments)};");
      strReturn.AppendLine("ga.l=+new Date;");
      strReturn.AppendLine("ga('create', 'UA-109911362-1', 'auto');");
      strReturn.AppendLine("ga('send', 'pageview');");
      strReturn.AppendLine("</script>");
      strReturn.AppendLine("<script async src='https://www.google-analytics.com/analytics.js'></script>");
      strReturn.AppendLine("<script>function kuwsRadio(actionID)");
      strReturn.AppendLine("{var obj = document.getElementById(\"kuwsPlayerObj\");");
      strReturn.AppendLine("//obj.play();");
      strReturn.AppendLine("//var button = $(\"##kuwsPlayerBtn\");");
      strReturn.AppendLine("var myDiv = document.getElementById(\"kuwsPlayerBtn\");");
      strReturn.AppendLine("if (actionID == true)");
      strReturn.AppendLine("{obj.play();");
      strReturn.AppendLine("myDiv.innerHTML='<a id=\"kuwsPlayerBtnTag\" href=\"javascript:kuwsRadio(false);\" title=\"Stop Listening to 91.3 KUWS\">STOP RADIO <span class=\"fa fa-play-circle\"></span></a>';}");
      strReturn.AppendLine("else{");
      strReturn.AppendLine("obj.pause();");
      strReturn.AppendLine("myDiv.innerHTML='<a id=\"kuwsPlayerBtnTag\" href=\"javascript:kuwsRadio(true);\" title=\"Start Listening to 91.3 KUWS\">LISTEN TO 91.3 KUWS <span class=\"fa fa-play-circle\"></span></a>';}");
      strReturn.AppendLine("}//$(document).ready(function () { kuwsRadio(false); });");
      strReturn.AppendLine("</script>");
      strReturn.AppendLine("<link rel=\"stylesheet\" type=\"text/css\" id=\"googleFonts\" href=\"https://fonts.googleapis.com/css?family=Open+Sans:400,400italic,600,600italic,700,700italic,800\" media=\"none\" onload=\"if(media!='all')media='all'\" />");
      strReturn.AppendLine("<link rel=\"stylesheet\" type=\"text/css\" href=\"emergency-min.css?rnd=#randrange(1,999)#\" />");
      strReturn.AppendLine("<link type=\"text/css\" rel=\"stylesheet\" href=\"https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css\" media=\"none\" onload=\"if(media!='all')media='all'\" />");
      strReturn.AppendLine("<meta charset=\"utf-8\" />");
      strReturn.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1, user-scalable=no\" />");
      strReturn.AppendLine("<title>Emergency Notification Website - UW-Superior</title>");
      strReturn.AppendLine("</head>");
      return strReturn;
	    }

    //generate HTML for body including the Subject and Text information compiled
    //in UpdatedInfoHelper sent to UpdateWebsite.Get_Updated_Info
    public StringBuilder Get_HTML_Body(StringBuilder informationUpdate) {
      StringBuilder strReturn = new StringBuilder();
      strReturn.AppendLine("<body>");
      strReturn.AppendLine("<h1 class=\"hide\">Emergency Notification Website - UW-Superior</h1>");
      strReturn.AppendLine("<div id=\"contentWrapper\">");
      strReturn.AppendLine("<div class=\"container\">");
      strReturn.AppendLine("<div class=\"sixteen columns\"><div id=\"logo\" class=\"sprite\"></div></div>");
      strReturn.AppendLine("<div class=\"twelve columns\" id=\"start\"><div class=\"mobilePadding\" id=\"content\">");
      //informationUpdate is updated in the Get_Updated_Info or Get_Clearing_Info function
      //and then inserted into body here.
      strReturn.AppendLine(informationUpdate.ToString());
      strReturn.AppendLine("</div></div>");
      strReturn.AppendLine("<!---");
      strReturn.AppendLine("<div id=\"kuwsPlayerBtn\" >");
      strReturn.AppendLine("<a href=\"javascript:kuwsRadio(true);\" title=\"Start Listening to 91.3 KUWS\" target=\"_top\">Listen to KUWS </a>");
      strReturn.AppendLine("</div>--->");
      strReturn.AppendLine("<div class=\"four columns\" id=\"important\">");
      strReturn.AppendLine("<div id=\"kuwsPlayerBtn\" class=\"kuwsBtn-yellow\">");
      strReturn.AppendLine("<a id=\"kuwsPlayerBtnTag\" href=\"javascript:kuwsRadio(true);\" title=\"Start Listening to 91.3 KUWS\" class=\"\">");
      strReturn.AppendLine("LISTEN TO 91.3 KUWS <span class=\"fa fa-play-circle\"></span>");
      strReturn.AppendLine("</a>");
      strReturn.AppendLine("</div>");
      strReturn.AppendLine("<audio controls id=\"kuwsPlayerObj\" class=\"hide\">");
      strReturn.AppendLine("<source src=\"https://kuws.streamguys1.com/live\" type=\"audio/mpeg\">Your browser does not support the audio tag.");
      strReturn.AppendLine("</audio>");
      strReturn.AppendLine("<h2>Emergency Assistance</h2>");
      strReturn.AppendLine("<ul>");
      //Phone Numbers for Emergency Assistance - Hardcoded may need updating
      strReturn.AppendLine("<li><a href=\"tel:715-394-8114\"><span class=\"sprite\"></span><strong>University Police:</strong> 715-394-8114</a></li>");
      strReturn.AppendLine("<li><a href=\"tel:715-394-8400\"><span class=\"sprite\"></span><strong>Weather Hotline:</strong> 715-394-8400</a></li>");
      strReturn.AppendLine("<li><a href=\"tel:715-394-8101\"><span class=\"sprite\"></span><strong>Information:</strong> 715-394-8101</a></li>");
      strReturn.AppendLine("<li><a href=\"tel:715-394-8400\"><span class=\"sprite\"></span><strong>Operating Status:</strong> 715-394-8400</a></li>");
      strReturn.AppendLine("<li><a href=\"tel:715-394-8300\"><span class=\"sprite\"></span><strong>IT Help Desk:</strong> 715-394-8300</a></li>");
      strReturn.AppendLine("</ul>");
      strReturn.AppendLine("<div class=\"cta\">");
      strReturn.AppendLine("<a href=\"https://www.getrave.com/login/uwsuper\">Sign Up for SAFE Alerts <div>Have alerts sent directly to your mobile device</div></a>");
      strReturn.AppendLine("</div>");
      strReturn.AppendLine("</div>");
      strReturn.AppendLine("<div id=\"links\" class=\"clear\">");
      strReturn.AppendLine("<div class=\"six columns row\">");
      strReturn.AppendLine("<h2><span>Street Address</span></h2>");
      strReturn.AppendLine("<address class=\"mobilePadding\">Belknap St. and Catlin Ave.<br/>Superior, WI 54880</address>");
      strReturn.AppendLine("<iframe src=\"https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d2735.391071751575!2d-92.09034088391748!3d46.717747579135676!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x52ae51949402867d%3A0x47462662b49e367e!2sUniversity+of+Wisconsin-Superior!5e0!3m2!1sen!2su\" style=\"height: 300px; width: 100%;\" scrolling=\"no\" frameborder=\"no\" allowfullscreen=\"true\" allowtransparency=\"true\"></iframe>");
      strReturn.AppendLine("</div>");
      strReturn.AppendLine("<div class=\"five columns row\">");
      strReturn.AppendLine("<h2><span>Campus Logins</span></h2>");
      strReturn.AppendLine("<div class=\"mobilePadding\">");
      strReturn.AppendLine("<ul>");
      strReturn.AppendLine("<li><a href=\"https://outlook.office365.com/uwsuper.onmicrosoft.com\">E-Mail</a></li>");
      strReturn.AppendLine("<li><a href=\"https://library.uwsuper.edu/library\">Jim Dan Hill Library</a></li>");
      strReturn.AppendLine("<li><a href=\"http://uws.instructure.com/\">Learn@UW-Superior/Canvas</a>");
      strReturn.AppendLine("</li>");
      strReturn.AppendLine("<li><a href=\"https://www.uwsuper.sis.wisconsin.edu/supprd-login\">E-Hive</a></li>");
      strReturn.AppendLine("<li><a href=\"https://wisdm2.doit.wisc.edu/wisdm2/Main.aspx\">WISDM</a></li>");
      strReturn.AppendLine("</ul>");
      strReturn.AppendLine("</div>");
      strReturn.AppendLine("</div>");
      strReturn.AppendLine("<div class=\"five columns row\">");
      strReturn.AppendLine("<h2><span>Useful Links</span></h2>");
      strReturn.AppendLine("<div class=\"mobilePadding\">");
      strReturn.AppendLine("<ul>");
      strReturn.AppendLine("<li><a href=\"https://apply.wisconsin.edu/\">Apply Online to UW-Superior</a></li>");
      strReturn.AppendLine("<li><a href=\"campusmap.pdf\">UW-Superior Campus Camp (PDF)</a></li>");
      strReturn.AppendLine("</ul>");
      strReturn.AppendLine("</div>");
      strReturn.AppendLine("</div>");
      strReturn.AppendLine("</div>");
      strReturn.AppendLine("</div>");
      strReturn.AppendLine("</div>");
      strReturn.AppendLine("</div>");
      return strReturn;
	    }

    //generate HTML footer to return
    public StringBuilder Get_HTML_Footer() {
      StringBuilder strReturn = new StringBuilder();
      strReturn.AppendLine("<footer>");
      strReturn.AppendLine("<div class=\"container\">");
      strReturn.AppendLine("<div class=\"eleven columns sprite\">");
      strReturn.AppendLine("<div id=\"footerContent\" class=\"mobilePadding\">");
      strReturn.AppendLine("<div id=\"schoolName\">University of Wisconsin-Superior</div>");
      strReturn.AppendLine("<div id=\"copyright\">Copyright &copy; The Board of Regents of the University of Wisconsin System<br/>University of Wisconsin-Superior is an equal opportunity educator and employer</div>");
      strReturn.AppendLine("</div>");
      strReturn.AppendLine("</div>");
      strReturn.AppendLine("<div class=\"five columns\"><ul id=\"social\"><li><a href=\"https://www.facebook.com/uwsuper\"><div class=\"min\">facebook</div><span class=\"fa fa-facebook-square\"></span></a></li><li><a href=\"https://twitter.com/uw_superior\"><div class=\"min\">twitter</div><span class=\"fa fa-twitter-square\"></span></a></li><li><a href=\"https://www.instagram.com/uw_superior/\"><div class=\"min\">instagram</div><span class=\"fa fa-instagram\"></span></a></li><li><a href=\"https://www.youtube.com/user/uwsuperior\"><div class=\"min\">youtube</div><span class=\"fa fa-youtube-square\"></span></a></li><li><a href=\"https://www.linkedin.com/school/50679/\"><div class=\"min\">linkedin</div><span class=\"fa fa-linkedin-square\"></span></a></li></ul>");
      strReturn.AppendLine("</div>");
      strReturn.AppendLine("</div>");
      strReturn.AppendLine("</footer>");
      strReturn.AppendLine("<script> links = document.getElementsByTagName(\"a\"); for(i=0;i<links.length;i++){ text = links[i].innerText.replace(/\\r?\\n|\\r/g,\"\"); if( text != \"\" && links[i].id != \"kuwsPlayerBtnTag\" ){ links[i].target = \"_blank\"; links[i].setAttribute(\"onclick\",\"ga('send','event','Link Click', '\" + text + \"','1');\"); } } </script>");
      strReturn.AppendLine("</body>");
      strReturn.AppendLine("</html>");
      return strReturn;
	    }

    //Strip HTML from subject and text to store in plain text mode
    public static string StripHTML(string input) {
      string updatedString = Regex.Replace(input, "<.*?>", String.Empty);
      updatedString = updatedString.Replace("&ndash;", "-");
      updatedString = updatedString.Replace("&nbsp;", " ");
      return updatedString;
			}
		}
	}
