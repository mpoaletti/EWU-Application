using IdentityModel.Client;
using Microsoft.AspNet.Identity;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

[assembly: OwinStartup(typeof(EmergencyWebsiteUpdate.Startup))]

namespace EmergencyWebsiteUpdate {
	public class Startup {
		// These values are stored in Web.config. Make sure you update them!
		//private readonly string _clientId = Decryption.DecryptString((String)ConfigurationManager.AppSettings["decryptKey"], (String)ConfigurationManager.AppSettings["okta:ClientId"]);
		//private readonly string _orgUri = Decryption.DecryptString((String)ConfigurationManager.AppSettings["decryptKey"], (String)ConfigurationManager.AppSettings["okta:OrgUri"]);
		//private readonly string _clientSecret = Decryption.DecryptString((String)ConfigurationManager.AppSettings["decryptKey"], (String)ConfigurationManager.AppSettings["okta:ClientSecret"]);

		//private readonly string _clientId = client.GetSecret("ClientID").Value.Value;
		//private readonly string _orgUri = client.GetSecret("OrgUri").Value.Value;
		//private readonly string _clientSecret = client.GetSecret("ClientSecret").Value.Value;

		//private readonly string _clientId = Decryption.DecryptString(client.GetSecret("EncryptKey").Value.Value, client.GetSecret("ClientID").Value.Value);
		//private readonly string _orgUri = Decryption.DecryptString(client.GetSecret("EncryptKey").Value.Value, client.GetSecret("OrgUri").Value.Value);
		//private readonly string _clientSecret = Decryption.DecryptString(client.GetSecret("EncryptKey").Value.Value, client.GetSecret("ClientSecret").Value.Value);


		private readonly string _redirectUri = ConfigurationManager.AppSettings["okta:RedirectUri"];
		private readonly string _postLogoutRedirectUri = ConfigurationManager.AppSettings["okta:PostLogoutRedirectUri"];



		public void Configuration(IAppBuilder app) {
			ConfigureAuth(app);
		}

		public void ConfigureAuth(IAppBuilder app) {
			//string keyVaultName = Environment.GetEnvironmentVariable("VaultUri");
			//string kvUri = "https://" + keyVaultName + ".vault.azure.net";
			//private string kvUri = "https://" + Environment.GetEnvironmentVariable("VaultUri") + ".vault.azure.net";
			String azureURI = Decryption.DecryptString((String)ConfigurationManager.AppSettings["decryptKey"], (String)System.Configuration.ConfigurationManager.AppSettings["azureURI"]);
			SecretClient client = new SecretClient(new Uri(azureURI), new DefaultAzureCredential());
			
			//SecretClient client = new SecretClient(new Uri("https://" + Environment.GetEnvironmentVariable("VaultUri") + ".vault.azure.net"), new DefaultAzureCredential());
			//private SecretClient client = new SecretClient(new Uri(Environment.GetEnvironmentVariable("VaultUri")), new DefaultAzureCredential());
			//SecretClient client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());


			//string _clientId = Decryption.DecryptString(client.GetSecret("EncryptKey").Value.Value, client.GetSecret("ClientID").Value.Value);
			//string _orgUri = Decryption.DecryptString(client.GetSecret("EncryptKey").Value.Value, client.GetSecret("OrgUri").Value.Value);
			//string _clientSecret = Decryption.DecryptString(client.GetSecret("EncryptKey").Value.Value, client.GetSecret("ClientSecret").Value.Value);

			string _clientId = client.GetSecret("ClientID").Value.Value;
			string _orgUri = client.GetSecret("OrgUri").Value.Value;
			string _clientSecret = client.GetSecret("ClientSecret").Value.Value;


			app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
			app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
			app.UseCookieAuthentication(new CookieAuthenticationOptions());
			app.UseOpenIdConnectAuthentication(
			new OpenIdConnectAuthenticationOptions {
				ClientId = _clientId,
				ClientSecret = _clientSecret,
				Authority = _orgUri,
				RedirectUri = _redirectUri,
				PostLogoutRedirectUri = _postLogoutRedirectUri,
				ResponseType = OpenIdConnectResponseType.IdToken,
				Scope = OpenIdConnectScope.OpenIdProfile,
				TokenValidationParameters = new TokenValidationParameters { NameClaimType = "name" },
				Notifications = new OpenIdConnectAuthenticationNotifications {
					AuthorizationCodeReceived = async n => {
						var tokenClient = new TokenClient($"{_orgUri}/v1/token", _clientId, _clientSecret);
						var tokenResponse = await tokenClient.RequestAuthorizationCodeAsync(n.Code, _redirectUri);
						if (tokenResponse.IsError) {
							throw new Exception(tokenResponse.Error);
							}
						var userInfoClient = new UserInfoClient($"{_orgUri}/v1/userinfo");
						var userInfoResponse = await userInfoClient.GetAsync(tokenResponse.AccessToken);
						var claims = new List<Claim>(userInfoResponse.Claims) {
							new Claim("id_token", tokenResponse.IdentityToken),
							new Claim("access_token", tokenResponse.AccessToken),
							};
						n.AuthenticationTicket.Identity.AddClaims(claims);
						},
					RedirectToIdentityProvider = n => {
						// If signing out, add the id_token_hint
						if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.Logout) {
							var idTokenClaim = n.OwinContext.Authentication.User.FindFirst("id_token");
							if (idTokenClaim != null) {
								n.ProtocolMessage.IdTokenHint = idTokenClaim.Value;
								}
							}
						return Task.CompletedTask;
						},
					},
				}
			);
		}
	}
}
