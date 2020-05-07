using Newtonsoft.Json;

namespace EchoBot.Models
{
    public class FollowUpCheckResult
    {
        [JsonProperty("answers")]
        public FollowUpCheckQnAAnswer[] Answers
        {
            get;
            set;
        }
    }
}
