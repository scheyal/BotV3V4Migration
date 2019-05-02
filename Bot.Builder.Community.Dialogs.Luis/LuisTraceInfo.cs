using Newtonsoft.Json;
using Bot.Builder.Community.Dialogs.Luis.Models;

namespace Bot.Builder.Community.Dialogs.Luis
{
    /// <summary>
    /// The schema for the LUIS trace info
    /// </summary>
    public class LuisTraceInfo
    {
        /// <summary>
        /// The raw response coming back from the LUIS service
        /// </summary>
        [JsonProperty("luisResult")]
        public LuisResult LuisResult { set; get; }

        /// <summary>
        /// The options passed to the LUIS service
        /// </summary>
        [JsonProperty("luisOptions")]
        public ILuisOptions LuisOptions { get; set; }

        /// <summary>
        /// The metadata about the LUIS app
        /// </summary>
        [JsonProperty("luisModel")]
        public ILuisModel LuisModel { get; set; }
    }
}
