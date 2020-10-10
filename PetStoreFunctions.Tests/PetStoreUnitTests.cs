using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PetStoreFunction;
using PetStoreFunction.Models;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace PetStoreFunctions.Tests
{ 
    public class PetStoreUnitTests
    {
        private readonly ILogger logger = TestFactory.CreateLogger();
        private Owner[] LoadJson( string fileName)
        {
            Owner[] ownerData = null;
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                ownerData = JsonConvert.DeserializeObject<Owner[]>(json);
            }
            return ownerData;
        }

        [Fact]
        public async void Get_Pet_Owner_By_Gendder_By_Dog_Type_Should_Return_Cat_Owners()
        {
         var petOwnerData =  LoadJson(@"PetOwnerData.json");
         var response =   await PetStoreRequestProcessor.GetOwnerByGenderAndByPetType(petOwnerData,"Cat" );
            Assert.IsType<JArray>(response["Male"]);
            Assert.IsType<JArray>(response["Female"]);
            Assert.Equal(4,response["Male"].Count());
            Assert.Equal(3,response["Female"].Count());
            Assert.Equal("Garfield",response["Female"].First());
            Assert.Equal("Garfield",response["Male"].First());
        }

        [Fact]
        public async void Get_Pet_Owner_By_Gendder_By_Dog_Type_Should_Return_Dog_Owners()
        {
            var petOwnerData = LoadJson(@"PetOwnerData.json");
            var response = await PetStoreRequestProcessor.GetOwnerByGenderAndByPetType(petOwnerData, "Dog");
            Assert.IsType<JArray>(response["Male"]);
            Assert.IsType<JArray>(response["Female"]);
            Assert.Equal(2, response["Male"].Count());
            Assert.Equal("Fido", response["Male"].First());
        }
    }
}
