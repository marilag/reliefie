using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Reliefie.API
{
    public interface IEventGridService
    {
        Task PublishEventsAsync(IList<EventGridPublishMessage> data);
    }
     public class EventGridService : IEventGridService
    {
        private readonly EventGridOptions _options;
        private readonly EventGridClient _eventGridClient;
        private readonly string _topicHostname;

        public EventGridService(IOptions<EventGridOptions> options)
        {
            _options = options.Value;
            _topicHostname = new Uri(_options.TopicEndpoint).Host;
            var topicCredentials = new TopicCredentials(_options.Key);
            _eventGridClient = new EventGridClient(topicCredentials);
        }

        public async Task PublishEventsAsync(IList<EventGridPublishMessage> data) =>
            await _eventGridClient.PublishEventsAsync(_topicHostname, GetEventGridEvents(data));

        private IList<EventGridEvent> GetEventGridEvents(IList<EventGridPublishMessage> data)
        {
            return data.Select(x => new EventGridEvent()
            {
                Id = x.Id,
                EventType = x.Data.EventType,
                Data = JsonConvert.SerializeObject(x.Data),
                EventTime = x.EventTime,
                Subject = x.Subject,
                DataVersion = x.DataVersion
            }).ToArray();
        }
    }
}