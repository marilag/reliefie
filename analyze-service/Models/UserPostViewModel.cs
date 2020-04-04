using System;
using Microsoft.Azure.Cosmos.Spatial;
using Newtonsoft.Json;


namespace   Reliefie.Analyze
{
    public class UserPostViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; } 
        public DateTime PostDateTime { get;  set; } 
        [JsonProperty("location")]
        public Point Location { get; set; } 
        public string ImageUrl { get; set; }    

        
    }
     
}