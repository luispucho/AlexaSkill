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
            var locale = alexaRequest.request?.locale ?? ""; //language used in alexa device
            string requestIntent = alexaRequest.request?.intent?.name.ToString() ?? "none";
            var timeTotal = Convert.ToInt32(alexaRequest.request?.intent?.slots?.seconds ?? "3");

            FirebaseClientClient.Set("AlexaSessionId", alexaRequest.session.sessionId);         
            FirebaseClientClient.Set("AlexaIntent", requestIntent);

            var responseText = "got it!";

            switch (requestIntent.Replace(@"""",""))
            {
                case "MoveBackwardIntent":
                    FirebaseClientClient.Set("backward", timeTotal);
                    break;
                case "MoveForwardIntent":
                    FirebaseClientClient.Set("forward", timeTotal);
                    break; 
                case "PlayHornIntent":
                    FirebaseClientClient.Set("horn", 1);
                    break;
                case "PlayStarWarsMusicIntent":
                    FirebaseClientClient.Set("horn", 2);
                    responseText = "may the force be with you";
                    break;
                case "LightOnIntent":
                    FirebaseClientClient.Set("light", true);
                    break;
                case "LightOffIntent":
                    FirebaseClientClient.Set("light", false);
                    break;
                case "MoveRightIntent":
                    FirebaseClientClient.Set("forward", timeTotal);
                    FirebaseClientClient.Set("right", true);
                    FirebaseClientClient.Set("left", false);
                    break;
                case "MoveLeftIntent":
                    FirebaseClientClient.Set("forward", timeTotal);
                    FirebaseClientClient.Set("left", true);
                    FirebaseClientClient.Set("right", false);
                    break;
                    
                default:
                    responseText = (new Random()).Next(0, 11) % 2 == 0 ? "what up dude, let's do it!" : "they see me rolling";
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
                        content = "they see me rolling"
                    },
                    shouldEndSession = true
                }
            };
        }
    }
}
