using System;
using System.Collections.Generic;
using DotNetNuke.Services.Mail;

public class SendMail : ToSic.Sxc.Dnn.DynamicCode
{
  public void sendMails(Dictionary<string,object> contactFormRequest) {
    var settings = new {
      MailFrom = App.Settings.MailFrom,
      OwnerMail = App.Settings.OwnerMail,
      OwnerMailCC = "",
      OwnerMailTemplateFile = App.Settings.OwnerMailTemplateFile,
      CustomerMailCC = "",
      CustomerMailTemplateFile = App.Settings.CustomerMailTemplateFile
    };

    var customerMail = contactFormRequest["Mail"].ToString();

    try {
      Send(
        settings.OwnerMailTemplateFile, contactFormRequest, settings.MailFrom, settings.OwnerMail, settings.OwnerMailCC, customerMail
      );
    } catch(Exception ex) {
      throw new Exception("OwnwerSend mail failed: " + ex.Message);
    }

    try {
      Send(
        settings.CustomerMailTemplateFile, contactFormRequest, settings.MailFrom, customerMail, Content.CustomerMailCC, settings.OwnerMail
      );
    } catch(Exception ex) {
      throw new Exception("OwnwerSend mail failed: " + ex.Message);
    }
  }

  public bool Send(
    string emailTemplateFilename,
    Dictionary<string,object> contactFormRequest,
    string MailFrom,
    string MailTo,
    string MailCC,
    string MailReply)
  {
    var mailEngine = CreateInstance("../../live/email-templates/" + emailTemplateFilename);
    var mailSubj = mailEngine.Subject();
    var mailBody = mailEngine.Message(contactFormRequest).ToString();

    // Send Mail
    // uses the DNN command: http://www.dnnsoftware.com/dnn-api/html/886d0ac8-45e8-6472-455a-a7adced60ada.htm
    var sendMailResult = Mail.SendMail(MailFrom, MailTo,	MailCC,	"", MailReply, MailPriority.Normal,
      mailSubj, MailFormat.Html, System.Text.Encoding.UTF8, mailBody, new string[0], "", "", "", "", false);

    // Log to DNN - just as a last resort in case something is lost, to track down why
    var logInfo = new DotNetNuke.Services.Log.EventLog.LogInfo
    {
        LogTypeKey = DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT.ToString()
    };
    logInfo.AddProperty("MailFrom", MailFrom);
    logInfo.AddProperty("MailTo", MailTo);
    logInfo.AddProperty("MailCC", MailCC);
    logInfo.AddProperty("MailReply", MailReply);
    logInfo.AddProperty("MailSubject", mailSubj);
    logInfo.AddProperty("SSL", DotNetNuke.Entities.Host.Host.EnableSMTPSSL.ToString());
    logInfo.AddProperty("Result", sendMailResult);
    DotNetNuke.Services.Log.EventLog.EventLogController.Instance.AddLog(logInfo);

    return sendMailResult == "";
  }
}