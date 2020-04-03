using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Reliefie.API
{


    public  class ImageUploaded
    {
        private readonly ICosmosDBSQLService _cosmos;

        public ImageUploaded(ICosmosDBSQLService cosmos)
        {
            _cosmos = cosmos;
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
                log.LogInformation($"Item {userPost.Id} retrieved. Update Completed");
            }
        }
    }
}
