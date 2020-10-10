using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetStoreFunction.Models
{
    public class Owner
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("gender")]
        public string  Gender { get; set; }
        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonProperty("pets")]
        public Pet[] Pets { get; set; }

    }
}
