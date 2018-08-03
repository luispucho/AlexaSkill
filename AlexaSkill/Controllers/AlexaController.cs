using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.ModelBinding;
using AlexaSkill;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using Microsoft.Ajax.Utilities;

namespace AlexaSkill.Controllers
{
    public class AlexaController : ApiController
    {
        private IFirebaseConfig config => new FirebaseConfig()
        {
            AuthSecret = System.Configuration.ConfigurationManager.ConnectionStrings["Firebase_Db"].ConnectionString + "i",
            BasePath = "https://iotclass-18t2.firebaseio.com/"
        };
        private static IFirebaseClient _client;

        public IFirebaseClient FirebaseClientClient => _client ?? (_client = new FirebaseClient(config));


        [HttpPost, Route("api/alexa/demo")]
        public dynamic Epicor(dynamic alexaRequest)
        {


            /*
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
            */

            string requestIntent = alexaRequest.request?.intent?.name ?? "none";

            FirebaseClientClient.Set("AlexaSessionId", alexaRequest.session.sessionId);
            
            FirebaseClientClient.Set("AlexaIntent", requestIntent);
            var responseText = "ya le vaaaa!";
            switch (requestIntent)
            {
                case "MoveBackwardIntent":

                    break;
                case "MoveForwardIntent":
                    break;
                case "PlayHornIntent":
                    break;
                default:
                    responseText = "bienvenido al colibri mandingo";
                    break;
            }
            //add a switch
            return new
            {
                version = "1.0",
                SessionAttributes = new { },
                response = new 
                {
                    outputSpeech = new
                    {
                        type = "PlainText",
                        text = responseText,
                    },
                    card = new
                    {
                        type = "Simple",
                        title = "Colibri Mandingo",
                        content = "Hola guardian"
                    },
                    shouldEndSession = true
                }
            };
        }
    }
}
