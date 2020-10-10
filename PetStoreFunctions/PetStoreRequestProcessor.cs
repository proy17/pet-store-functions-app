using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System.Linq;
using Newtonsoft.Json.Linq;
using PetStoreFunction.Models;

namespace PetStoreFunction
{
    public static class PetStoreRequestProcessor
    {
        [FunctionName("GetPetStoreData")]
        public static async Task<JObject> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string petType = req.Query["petType"];
                if (!string.IsNullOrEmpty(petType))
                {
                    string apiUrl = Environment.GetEnvironmentVariable("Serviceurl");
                    var client = new RestClient(apiUrl);
                    var request = new RestRequest(Method.GET);
                    var restResponse = await client.ExecuteGetAsync(request);
                    Owner[] owners = JsonConvert.DeserializeObject<Owner[]>(restResponse.Content);
                    if (owners.Count() > 0)
                    {
                        return await GetOwnerByGenderAndByPetType(owners, petType);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                log.LogError(ex, $" Error occured while processing the request - {ex.Message}");
                throw;
            }
        }
        public static async Task<JObject> GetOwnerByGenderAndByPetType(Owner[] owners, string petType)
        {
            JArray malePetOwners = new JArray();
            JArray femalePetOwners = new JArray();
            JObject viewDataModel = new JObject(
                               new JProperty("Male", malePetOwners),
                               new JProperty("Female", femalePetOwners));

            foreach (var owner in owners)
            {
                if (owner.Pets != null && owner.Pets.Count() > 0)
                {
                    foreach (var pet in owner.Pets)
                    {
                        if (pet.Type.ToLower() == petType.ToLower())
                        {
                            ((JArray)viewDataModel[owner.Gender]).Add(pet.Name);
                        }
                    }
                }
            }
            return viewDataModel;
        }
    }
}






