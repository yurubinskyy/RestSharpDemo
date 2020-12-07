using System;
using System.Configuration;
using RestSharp;
using RestSharpDemo.Base;
using RestSharpDemo.Model;
using RestSharpDemo.Utilities;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace RestSharpDemo.Steps
{

    [Binding]
    public class PostProfileSteps
    {
        private Settings _settings;
        public PostProfileSteps(Settings settings) => _settings = settings;

        [Given(@"I Perform POST operation for ""(.*)"" with body")]
        public void GivenIPerformPOSTOperationForWithBody(string url, Table table)
        {

            dynamic data = table.CreateDynamicInstance();

            _settings.Request = new RestRequest(url, Method.POST);

            _settings.Request.AddJsonBody(new { name = data.name.ToString() });
            _settings.Request.AddUrlSegment("profileNo", ((int)data.profile).ToString());

            _settings.Response = _settings.RestClient.ExecuteAsyncRequest<Posts>(_settings.Request).GetAwaiter().GetResult();

        }
    }
}
