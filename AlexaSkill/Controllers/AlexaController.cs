using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.ModelBinding;
using AlexaSkill;

namespace AlexaSkill.Controllers
{
    public class AlexaController : ApiController
    {
        [HttpPost, Route("api/alexa/demo")]
        public dynamic Epicor(AlexaSkill.Data.AlexaRequest alexaRequest)
        {
            new Data.Requests().Create(new Data.Request
            {
                MemberId = alexaRequest.Session.Attributes?.MemberId ?? 0,
                Timestamp = alexaRequest.Request.Timestamp,
                Intent = (alexaRequest.Request.Intent == null) ? "" : alexaRequest.Request.Intent.Name,
                AppId = alexaRequest.Session.Application.ApplicationId,
                RequestId = alexaRequest.Request.RequestId,
                SessionId = alexaRequest.Session.SessionId,
                UserId = alexaRequest.Session.User.UserId,
                IsNew = alexaRequest.Session.New,
                Version = alexaRequest.Version,
                Type = alexaRequest.Request.Type,
                Reason = alexaRequest.Request.Reason,
                //SlotsList = alexaRequest.Request.Intent.GetSlots(),
                DateCreated = DateTime.UtcNow
            });

            return new
            {
                version = "1.0",
                SessionAttributes = new { },
                response = new 
                {
                    outputSpeech = new
                    {
                        type = "PlainText",
                        text = "Hello happy World!",
                    },
                    card = new
                    {
                        type = "Simple",
                        title = "Epicor",
                        content = "Hello funny world!"
                    },
                    shouldEndSession = true
                }
            };
        }
    }
}
