using System;

namespace Reliefie.API
{
    public class EventGridPublishMessage
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public string DataVersion { get; set; }
        public EventObject Data { get; set; }
        public DateTime EventTime { get; set; } = DateTime.UtcNow;
    }

    public class EventObject
    {
        public string EventType { get; set; }
        public object EventData { get; set; }
    }
}