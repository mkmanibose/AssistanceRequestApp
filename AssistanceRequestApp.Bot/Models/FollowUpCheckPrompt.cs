using Newtonsoft.Json;

namespace EchoBot.Models
{
    public class FollowUpCheckPrompt
    {
        [JsonProperty("displayText")]
        public string DisplayText
        {
            get;
            set;
        }
    }
}
