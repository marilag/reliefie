using System;

namespace Reliefie.API.Models
{
    public class Post
    {
        public string Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public Location Location { get; set; }
        public string ImageUrl { get; set; }       

    }

     public class Location 
    {
        public string Type { get; set; } = "Point";
        public string[] Coordinates { get; set; }
    }
}