using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Twilio;
using Twilio.Http;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML.Messaging;

namespace EmergencyWebsiteUpdate
{
  public class Messaging
  {
    public static void MessageUser(string message, string user, out string last4Phone)
    {
      Connection.ConnectSSH();
      string accountSid = Connection.RetrieveSSHData("TwilioSID");
      string authToken = Connection.RetrieveSSHData("TwilioAuthToken");
      string sendingPhoneNum = Connection.RetrieveSSHData("TwilioSendPhone");
      String phoneNum = Connection.RetrieveSSHData(user + "Phone");
      Connection.DisconnectSSH();

      last4Phone = phoneNum.Substring(phoneNum.Length - 4);

      TwilioClient.Init(accountSid, authToken);

      if (phoneNum != null) {
        // Send SMS Text
        var msge = MessageResource.Create(
            body: message,
            from: new Twilio.Types.PhoneNumber(sendingPhoneNum),
            to: new Twilio.Types.PhoneNumber(phoneNum));
        Console.WriteLine(msge.Sid);
      }
    }

    public static void MessageUser(string message, string user)
    {
      Connection.ConnectSSH();
      string accountSid = Connection.RetrieveSSHData("TwilioSID");
      string authToken = Connection.RetrieveSSHData("TwilioAuthToken");
      string sendingPhoneNum = Connection.RetrieveSSHData("TwilioSendPhone");
      String phoneNum = Connection.RetrieveSSHData(user + "Phone");
      Connection.DisconnectSSH();

      TwilioClient.Init(accountSid, authToken);

      if (phoneNum != null)
      {
        // Send SMS Text
        var msge = MessageResource.Create(
            body: message,
            from: new Twilio.Types.PhoneNumber(sendingPhoneNum),
            to: new Twilio.Types.PhoneNumber(phoneNum));
        Console.WriteLine(msge.Sid);
      }
    }
  }
}