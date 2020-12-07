using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using RestSharpDemo.Base;
using RestSharpDemo.Model;
using RestSharpDemo.Utilities;

namespace RestSharpDemo.Steps
{

    [Binding]
    class LocationSteps
    {

        private Settings _settings;
        public LocationSteps(Settings settings) => _settings = settings;


        [Given(@"I perform operation for location as ""(.*)""")]
        public void GivenIPerformOperationForLocationAs(int id)
        {
            _settings.Request.AddOrUpdateParameter("id", id.ToString());
            _settings.Response = _settings.RestClient.ExecuteAsyncRequest<List<LocationModel>>(_settings.Request).GetAwaiter().GetResult();
        }

        [Then(@"I should see the ""(.*)"" name as ""(.*)"" in response")]
        public void ThenIShouldSeeTheNameAsInResponse(string key, string value)
        {
            Assert.That(_settings.Response.GetResponseObjectArray(key), Is.EqualTo(value), $"The {key} is not matching");
        }

        [Given(@"I perform POST operation to create new location with following details")]
        public void GivenIPerformPOSTOperationToCreateNewLocationWithFollowingDetails(Table table)
        {
            dynamic data = table.CreateDynamicInstance();

            _settings.Request = new RestRequest("/location", Method.POST);

            //body
            var body = new LocationModel
            {
                city = (string)data.city,
                country = (string)data.country,
                address = new List<Address>()
                {
                    new Address()
                    {
                        street = (string)data.street,
                       flat_no = (string)data.flatNo,
                       pincode = (int)data.pincode,
                       type = (string)data.type
                    }
                }
            };

            _settings.Request.AddJsonBody(body);

            _settings.Response = _settings.RestClient.Execute(_settings.Request);
        }

        [Given(@"I perform PUT operation to update the address details")]
        public void GivenIPerformPUTOperationToUpdateTheAddressDetails(Table table)
        {
            dynamic data = table.CreateDynamicInstance();

            var dynamicId = _settings.Response.GetResponseObject("id");

            _settings.Request = new RestRequest($"/location/{dynamicId}", Method.PUT);

            //body
            var body = new LocationModel
            {
                city = (string)data.city,
                country = (string)data.country,
                address = new List<Address>()
                {
                    new Address()
                    {
                        street = (string)data.street,
                       flat_no = (string)data.flatNo,
                       pincode = (int)data.pincode,
                       type = (string)data.type
                    }
                }
            };

            _settings.Request.AddJsonBody(body);

            _settings.Response = _settings.RestClient.Execute(_settings.Request);

        }

        [Then(@"I should see the ""(.*)"" name as ""(.*)"" for address")]
        public void ThenIShouldSeeTheNameAsForAddress(string key, string value)
        {
            var locations = Libraries.DeserializeResponse(_settings.Response);

            foreach (var location in locations)
            {
                if (location.Key == key)
                {
                    var address = JsonConvert.DeserializeObject<List<Address>>(location.Value);

                    if (address != null)
                        Assert.That(address.FirstOrDefault().street, Is.EqualTo(value));
                }
            }
        }


        [Then(@"I perform DELETE operation of the newly created address")]
        public void ThenIPerformDELETEOperationOfTheNewlyCreatedAddress()
        {
            var dynamicId = _settings.Response.GetResponseObject("id");

            _settings.Request = new RestRequest($"/location/{dynamicId}", Method.DELETE);

            _settings.Response = _settings.RestClient.Execute(_settings.Request);
        }
    }
}