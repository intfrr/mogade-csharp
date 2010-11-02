using System.Collections.Generic;
using Mogade.Leaderboards;
using Newtonsoft.Json;

namespace Mogade
{
   public class Mogade : IMogade, IRequestContext
   {
      public const int VERSION = 1;
      
      public Mogade(string gameKey, string secret)
      {
         Key = gameKey;
         Secret = secret;
      }

      public int ApiVersion
      {
         get { return VERSION; }
      }
      public string Key { get; private set; }
      public string Secret { get; private set; }

      public Ranks SaveScore(string leaderboardId, Score score)
      {
         var payload = new Dictionary<string, object>
                       {
                          {"leaderboard_id", leaderboardId}, 
                          {"score", new {username = score.UserName, points = score.Points, data = score.Data}},
                       };
         var communicator = new Communicator(this);
         return JsonConvert.DeserializeObject<Ranks>(communicator.SendPayload(Communicator.PUT, "scores", payload));         
      }

      public Leaderboard GetLeaderboard(string leaderboardId, LeaderboardScope scope, int page)
      {
         var payload = new Dictionary<string, object> {{"leaderboard", new {id = leaderboardId, scope = (int) scope, page = page}}};
         var communicator = new Communicator(this);
         return JsonConvert.DeserializeObject<Leaderboard>(communicator.SendPayload(Communicator.POST, "scores", payload));       
      }
   }
}