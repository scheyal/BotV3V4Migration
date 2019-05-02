namespace Bot.Builder.Community.Dialogs.Luis.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;

    /// <summary>
    /// Child entity in Luis composite entity.
    /// </summary>
    public partial class CompositeChild
    {
        /// <summary>
        /// Initializes a new instance of the CompositeChild class.
        /// </summary>
        public CompositeChild() { }

        /// <summary>
        /// Initializes a new instance of the CompositeChild class.
        /// </summary>
        public CompositeChild(string type, string value)
        {
            Type = type;
            Value = value;
        }

        /// <summary>
        /// Type of child entity.
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Value extracted by Luis.
        /// </summary>
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        /// <summary>
        /// Validate the object. Throws ValidationException if validation fails.
        /// </summary>
        public virtual void Validate()
        {
            if (Type == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Type");
            }
            if (Value == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Value");
            }
        }
    }
}
