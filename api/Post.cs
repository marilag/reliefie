using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace Reliefie.API
{
    public  class Post
    {
        private readonly ICosmosDBSQLService _cosmos;
        public Post(ICosmosDBSQLService cosmos)
        {
            _cosmos = cosmos;
        }
        [FunctionName("Post")]
        public  async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "userpost")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Post httptrigger started");           

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
           
            UserPost userPost = JsonConvert.DeserializeObject<UserPost>(requestBody);
            log.LogDebug("User post:", userPost);
            var container = await _cosmos.GetOrCreateContainerAsync("UserPost", "/id");
            var response = await container.CreateItemAsync(userPost,new Microsoft.Azure.Cosmos.PartitionKey(userPost.Id));          

            return new OkObjectResult(userPost);
        }
    }
}
