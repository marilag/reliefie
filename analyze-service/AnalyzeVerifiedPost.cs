// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Reliefie.Analyze
{
    public  class UserPostVerified
    {
        ICosmosDBSQLService _cosmos;
        public UserPostVerified(ICosmosDBSQLService cosmos)
        {
            _cosmos = cosmos;
        }
        [FunctionName("AnalyzeVerifiedPost")]
        public async void Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            log.LogInformation($"EventData: {eventGridEvent.Data.ToString()}");
            try {
                EventObject userPostId = JsonConvert.DeserializeObject<EventObject>((string)eventGridEvent.Data);
                log.LogInformation($"PostId: {userPostId.EventData.ToString()}");
                var container = await _cosmos.GetOrCreateContainerAsync("UserPost", "/id");
                UserPostViewModel userPost = await _cosmos.ReadItemAsync<UserPostViewModel>(container,userPostId.EventData.ToString());
                if (userPost == null)
                {
                    log.LogError("Invalid image. No matching post. Deleting..");                
                }
                else
                {
                    log.LogInformation($"Userpost retrieved. {userPost.Id}");
                }        
            }   
            catch (Exception ex)
            {
                log.LogError($"Error occured {ex.Message}");                
            } 
            
            
        }
    }
}
