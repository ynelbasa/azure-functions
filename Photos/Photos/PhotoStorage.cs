using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Photos.Models;
using System;
using Azure.Storage.Blobs;

namespace Photos
{
    public static class PhotoStorage
    {
        [FunctionName(nameof(PhotoStorage))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest request,
            [Blob("photos", FileAccess.ReadWrite, Connection = Literals.StorageConnectionString)] BlobContainerClient blobClient,
            ILogger logger)
        {
            var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            var photoUploadModel = JsonConvert.DeserializeObject<PhotoUploadModel>(requestBody);

            var newId = Guid.NewGuid();
            var blobName = $"{newId}.jpg";

            await blobClient.CreateIfNotExistsAsync();
            
            var blob = blobClient.GetBlobClient(blobName);
            await blob.UploadAsync(photoUploadModel.FilePath, overwrite: true);

            logger?.LogInformation($"Successfully upload {newId}.jpg file");
            return new OkObjectResult(newId);
        }
    }
}
