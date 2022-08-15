using Newtonsoft.Json;
using System.Collections.Generic;

namespace Photos.Models
{
    public class PhotoUploadModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("tags")]
        public IEnumerable<string> Tags { get; set; }

        [JsonProperty("filepath")]
        public string FilePath { get; set; }
    }
}
