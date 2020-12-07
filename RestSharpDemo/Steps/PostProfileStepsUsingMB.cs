using RestSharp;
using RestSharpDemo.Base;
using RestSharpDemo.Model;
using RestSharpDemo.Utilities;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using System;

namespace RestSharpDemo.Steps
{

    [Binding]
    public class PostProfileStepsUsingMB
    {
        private Settings _settings;
        public PostProfileStepsUsingMB(Settings settings) => _settings = settings;

        [Given(@"I Perform POST operation for ""(.*)"" with body")]
        public void GivenIPerformPOSTOperationForWithBody(string url, Table table)
        {

            dynamic data = table.CreateDynamicInstance();

            _settings.Request = new RestRequest(url, Method.POST);

            _settings.Request.AddJsonBody(new { name = data.name.ToString() });
            _settings.Request.AddUrlSegment("profileNo", ((int)data.profile).ToString());

            _settings.Response = _settings.RestClient.ExecuteAsyncRequest<Posts>(_settings.Request).GetAwaiter().GetResult();

        }

        [Given(@"I create service virutalization for profile")]
        public void GivenICreateServiceVirutalizationForProfile(Table table)
        {
            dynamic data = table.CreateDynamicInstance();

            //simple predicate
            _settings.Imposter.AddStub().
                OnPathAndMethodEqual($"/posts/{(int)data.profile}/profile", MbDotNet.Enums.Method.Post)
                .ReturnsJson(System.Net.HttpStatusCode.OK, new { name = (string)data.name, postId = ((int)data.profile).ToString()});

            _settings.MBClient.Submit(_settings.Imposter);
        }


    }
}
