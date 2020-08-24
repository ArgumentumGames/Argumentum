using System;
using System.Web.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using DotNetNuke.Security;
using DotNetNuke.Web.Api;
public class FormController : ToSic.Sxc.Dnn.ApiController
{
    
  [HttpPost]
  [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Anonymous)]
  [ValidateAntiForgeryToken]
  public void ProcessForm([FromBody]Dictionary<string,object> contactFormRequest)
  {
    // 1. add IP / host, and save all fields
    // if you add fields to your content-type, just make sure they are 
    // in the request with the correct name, they will be added automatically
    contactFormRequest.Add("SubmitDate", DateTime.Now);
    contactFormRequest.Add("SenderIp", System.Web.HttpContext.Current.Request.UserHostAddress);
    contactFormRequest.Add("Status", "registered");
    App.Data.Create("Registration", contactFormRequest);

    // added feature to create a full-save of each request into a system-protocol content-type
    contactFormRequest.Add("Timestamp", DateTime.Now);
    contactFormRequest.Add("ModuleId", Dnn.Module.ModuleID);
    contactFormRequest.Add("Title", "Form " + DateTime.Now.ToString("s"));
    // add raw-data, in case the content-type has a "RawData" field
    contactFormRequest.Add("RawData", JsonConvert.SerializeObject(contactFormRequest));
    App.Data.Create("SystemProtocol", contactFormRequest);

    var sendMail = CreateInstance("parts/SendMail.cs");
    sendMail.sendMails(contactFormRequest);        
  }
}