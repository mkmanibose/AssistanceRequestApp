using Newtonsoft.Json;

namespace EchoBot.Models
{
    public class FollowUpCheckContext
    {
        [JsonProperty("prompts")]
        public FollowUpCheckPrompt[] Prompts
        {
            get;
            set;
        }
    }
}
