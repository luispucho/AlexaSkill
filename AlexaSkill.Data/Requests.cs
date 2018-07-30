using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

namespace AlexaSkill.Data
{
    public class Requests
    {
        public Request Create(Request request)
        {
            //var x = ConfigurationManager.ConnectionStrings["EpicorAlexa_dbEntities"].ToString();

            using (var db = new EpicorAlexa_dbEntity())
            {
                
                var member = db.Members.FirstOrDefault(m => m.AlexaUserId == request.UserId);

                if (member == null)
                {
                    request.Member = new Member() { AlexaUserId = request.UserId, CreatedDate = DateTime.UtcNow, LastRequestDate = DateTime.UtcNow, RequestCount = 1 };

                    db.Requests.Add(request);
                }
                else
                {
                    member.Requests.Add(request);
                }

                db.SaveChanges();
            }

            return request;
        }
    }
}
