using System;
using System.Text.Json.Serialization;

namespace WeCheerImageApp.Api.Models
{
    public class ImageEvent
    {
        public required string ImageUrl { get; set; }
        public required string Description { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public DateTime ReceivedAt { get; set; }
    }
} 