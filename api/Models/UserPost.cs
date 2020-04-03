using System;
using Microsoft.Azure.Cosmos.Spatial;
using Newtonsoft.Json;


namespace  Reliefie.API
{
    public class UserPost
    {
        [JsonProperty("id")]
        public string Id { get; private set; } = System.Guid.NewGuid().ToString();
        public DateTime PostDateTime { get; private set; }  = System.DateTime.UtcNow;           
        [JsonProperty("location")]
        public Point Location { get; set; } = new Point(0,0);
        public string ImageUrl { get; private set; }    

        public void SetImageUrl(string url)   
        {
            ImageUrl = url;
        }
    }
     
}