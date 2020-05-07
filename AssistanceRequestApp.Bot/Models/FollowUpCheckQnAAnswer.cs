using Newtonsoft.Json;

namespace EchoBot.Models
{
    public class FollowUpCheckQnAAnswer
    {
        [JsonProperty("context")]
        public FollowUpCheckContext Context
        {
            get;
            set;
        }
    }
}
