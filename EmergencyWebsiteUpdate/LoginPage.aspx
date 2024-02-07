<%@ Page Title="Login Page" validateRequest="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="EmergencyWebsiteUpdate.LoginPage" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
		<h2>&nbsp;</h2>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
			<ContentTemplate>
				<table style="width: 100%;">
					<tr>
						<td class="text-left" style="width: 268px; " rowspan="3">
							<img alt="University of Wisconsin Logo" src="swooshLOGOrevSmall.jpg" style="width: 175px; height: 110px" />
						</td>
						<td class="text-right" style="width: 268px; height: 22px">
							&nbsp;</td>
						<td style="height: 22px; width: 265px">
							&nbsp;</td>
						<td style="height: 22px"></td>
					</tr>
					<tr>
						<td class="text-right" style="width: 268px">
							&nbsp;</td>
						<td style="width: 265px">
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td>
						<td>
							<br />
						</td>
					</tr>
					<tr>
						<td style="width: 268px; height: 35px;" class="text-right">
							&nbsp;&nbsp;&nbsp;&nbsp; </td>
						<td style="width: 265px; height: 35px;" class="text-justify">
							<br />
						</td>
						<td style="height: 35px"></td>
					</tr>
				</table>
			</ContentTemplate>
		</asp:UpdatePanel>
</asp:Content>