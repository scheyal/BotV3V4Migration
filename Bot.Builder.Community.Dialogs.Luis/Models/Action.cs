namespace Bot.Builder.Community.Dialogs.Luis.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;

    public partial class Action
    {
        /// <summary>
        /// Initializes a new instance of the Action class.
        /// </summary>
        public Action() { }

        /// <summary>
        /// Initializes a new instance of the Action class.
        /// </summary>
        public Action(bool? triggered = default(bool?), string name = default(string), IList<ActionParameter> parameters = default(IList<ActionParameter>))
        {
            Triggered = triggered;
            Name = name;
            Parameters = parameters;
        }

        /// <summary>
        /// True if the Luis action is triggered, false otherwise.
        /// </summary>
        [JsonProperty(PropertyName = "triggered")]
        public bool? Triggered { get; set; }

        /// <summary>
        /// Name of the action.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// The parameters for the action.
        /// </summary>
        [JsonProperty(PropertyName = "parameters")]
        public IList<ActionParameter> Parameters { get; set; }

    }
}
