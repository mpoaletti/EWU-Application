<%@ Page Title="Update Website" validateRequest="false" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UpdateWebsite.aspx.cs" Inherits="EmergencyWebsiteUpdate.UpdateWebsite" %>



<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
		<div class="jumbotron">
        <p>
            <table style="width:100%;">
							<tr>
								<td class="modal-sm" style="width: 19%">
									<img alt="University of Wisconsin Logo" src="swooshLOGOrevSmall.jpg" style="width: 175px; height: 110px" /></td>
								<td class="text-center" style="width: 27%">
            			<asp:Button ID="bttnConnection" runat="server" BackColor="Black" ForeColor="Gold" OnClick="bttnConnection_Click" Text="Connect" Enabled="False" Font-Names="Montserrat" Font-Bold="True"  />
                        <br />
            			<asp:Label ID="lblConnection" runat="server" Text="Not Connected" Font-Bold="True" ForeColor="Red" Font-Names="Montserrat ExtraBold"></asp:Label>
            		</td>
								<td style="width: 18%">
            <asp:Label ID="lblLoggedIn" runat="server" Text="Not Logged In" Font-Names="Montserrat ExtraBold" Font-Bold="True"></asp:Label>
                        <br />
                        </td>
								<td class="text-center">
                        <asp:DropDownList ID="drpdwnAddOrReplace" runat="server" Font-Names="Montserrat">
                            <asp:ListItem Selected="True"></asp:ListItem>
                            <asp:ListItem>Add To Current Message</asp:ListItem>
                            <asp:ListItem>Replace Current Message</asp:ListItem>
                        </asp:DropDownList>
                    <br />
									<br />
                        <asp:Button ID="bttnSubmit" runat="server" OnClick="bttnSubmit_Click" Text="Submit" BackColor="Black" ForeColor="Gold" Visible="False" Font-Names="Montserrat" Font-Bold="True" />
                        <asp:Button ID="bttnClearWebsite" runat="server" OnClick="bttnClearWebsite_Click" Text="Clear Website" BackColor="Black" ForeColor="Gold" Visible="False" Font-Names="Montserrat" Font-Bold="True" />
                    </td>
							</tr>
						</table>
				</p>
        <div>
            <table style="width:100%; height: 576px;" id="tblMain" class="nav-justified">
                <tr>
                    <td style="width: 60%; height: 62px;">
                        <asp:Label ID="lblSubjLine" runat="server" Font-Names="Montserrat ExtraBold" Text="Subject Line:" Font-Bold="True"></asp:Label>
                        <br />
                        <asp:TextBox ID="txtSubject" runat="server" Font-Names="Montserrat" Width="100%" TextMode="SingleLine"></asp:TextBox>
                    </td>
                    <td style="width: 3%; height: 62px;">
                        &nbsp;</td>
                    <td style="width: 37%; height: 62px;">
                        <asp:Label ID="lblCurrentMsg" runat="server" Font-Names="Montserrat ExtraBold" Text="Current Message:" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 60%">
                        <asp:Label ID="lblMsgInfo" runat="server" Font-Names="Montserrat ExtraBold" Text="Message Info:" Font-Bold="True"></asp:Label>
                        <br />
                        <asp:TextBox ID="txtMessage" class="ckeditor" runat="server" Height="600px" TextMode="MultiLine" Width="100%" Font-Names="Montserrat" style="resize:vertical"></asp:TextBox>
                        &nbsp;</td>
                 
                    <td style="width: 3%" class="col-xs-3">
                        &nbsp;</td>
                 
                    <td style="width: 37%">
                        <asp:TextBox ID="txtCurrentMessage" runat="server" Height="600px" TextMode="MultiLine" Width="100%" Font-Names="Montserrat" Enabled="False" style="resize:vertical"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
    </div>
  

    </asp:Content>
