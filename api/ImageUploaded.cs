using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Reliefie.API
{


    public  class ImageUploaded
    {
        private readonly ICosmosDBSQLService _cosmos;
        private readonly IEventGridService _eventgrid;

        public ImageUploaded(ICosmosDBSQLService cosmos, IEventGridService eventgrid)
        {
            _cosmos = cosmos;
            _eventgrid = eventgrid;
        }

        [FunctionName("ImageUploaded")]
        public async void Run([BlobTrigger("reliefie-images/{postId}", Connection = "AzureWebJobsStorage")]Stream myBlob, string postId, ILogger log)
        {
            log.LogInformation($"Blob trigger function Processed blob\n Name:{postId} \n Size: {myBlob.Length} Bytes");
            var container = await _cosmos.GetOrCreateContainerAsync("UserPost", "/id");
            var userPost = await _cosmos.ReadItemAsync<UserPost>(container,postId.Split('.')[0]);
            if (userPost == null)
            {
                log.LogError("Invalid image. No matching post. Deleting..");
                //Delete image from storage
            }
            else {
                log.LogInformation($"Item {userPost.Id} retrieved. Updating image url");
                userPost.SetImageUrl($"reliefie-images/{postId}");     
                await _cosmos.UpsertItemAsync(container,userPost);                
                await _eventgrid.PublishEventsAsync( new List<EventGridPublishMessage>() { new EventGridPublishMessage() {
                       Id = Guid.NewGuid().ToString(),                                
                        DataVersion = "1.0",
                        Data = new EventObject() { EventType = "UserPostVerified", EventData = userPost.Id },
                        Subject = "UserPostVerified"                    
                }});
                log.LogInformation($"Item {userPost.Id} retrieved. Update Completed");                
            }
        }
    }
}
