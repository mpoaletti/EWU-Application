<%@ Page Title="Login Page" validateRequest="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="EmergencyWebsiteUpdate.LoginPage" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
		<h2>
            &nbsp;</h2>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
			<ContentTemplate>
				<table style="width: 100%;">
					<tr>
						<td class="text-left" style="width: 268px; " rowspan="3">
							<img alt="University of Wisconsin Logo" src="swooshLOGOrevSmall.jpg" style="width: 175px; height: 110px" />
						</td>
						<td class="text-right" style="width: 268px; height: 22px">
							<asp:Label ID="lblUsername" runat="server" Font-Names="Montserrat" Text="Username:" Font-Bold="True"></asp:Label>
							&nbsp;</td>
						<td style="height: 22px; width: 265px">
							<asp:TextBox ID="txtboxUsername" runat="server" Width="200px" CausesValidation="True" ValidateRequestMode="Enabled"></asp:TextBox>
						</td>
						<td style="height: 22px"></td>
					</tr>
					<tr>
						<td class="text-right" style="width: 268px">
							<asp:Label ID="lblPassword" runat="server" Font-Names="Montserrat" Text="Password:" Font-Bold="True"></asp:Label>
						</td>
						<td style="width: 265px">
							<asp:TextBox ID="txtboxPassword" runat="server" TextMode="Password" Width="200px" CausesValidation="True" ValidateRequestMode="Enabled"></asp:TextBox>
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td>
						<td>
							<asp:Label ID="lblErrorMessage" runat="server" Visible="False"></asp:Label>
							<br />
							<asp:Button ID="bttnResendVerify" runat="server" Font-Names="Montserrat" Text="ReSend Verif Code" OnClick="bttnResendVerify_Click" Visible="False" Width="144px" />
							<asp:Button ID="bttnCancel" runat="server" Font-Names="Montserrat" OnClick="bttnCancel_Click" Text="Cancel" Visible="False" />
						</td>
					</tr>
					<tr>
						<td style="width: 268px; height: 35px;" class="text-right">
							<asp:Label ID="lblVerification" runat="server" Font-Names="Montserrat" Text="Enter Verification Code Sent to Phone Ending :" Font-Bold="True" Visible="False"></asp:Label>
							&nbsp;&nbsp;<asp:TextBox ID="txtboxVerificationCode" runat="server" Width="200px" CausesValidation="True" ValidateRequestMode="Enabled" Visible="False"></asp:TextBox>
							&nbsp;&nbsp; </td>
						<td style="width: 265px; height: 35px;" class="text-justify">
							<asp:Button ID="bttnLogin" runat="server" Font-Names="Montserrat" Text="Login" OnClick="bttnLogin_Click" />
							<br />
							<asp:Button ID="bttnVerify" runat="server" Font-Names="Montserrat" Text="Verify" OnClick="bttnVerify_Click" Visible="False" />
						</td>
						<td style="height: 35px"></td>
					</tr>
				</table>
			</ContentTemplate>
		</asp:UpdatePanel>
</asp:Content>