using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;
using RestSharpDemo.Base;
using RestSharpDemo.Utilities;

namespace RestSharpDemo.Steps
{
    [Binding]
    class FileuploadSteps
    {

        private Settings _settings;
        public FileuploadSteps(Settings settings) => _settings = settings;


        [Given(@"I perform POST operation for ""(.*)""")]
        public void GivenIPerformPOSTOperationFor(string path)
        {
            _settings.Request = new RestRequest(path, Method.POST);
            _settings.Request.AddFile("file", @"C:\Users\karth\Downloads\YouTube series 2.jpg", "image/jpeg");

            _settings.Response = _settings.RestClient.ExecuteAsPost(_settings.Request, "POST");
        }

        [Then(@"I see the file is being uploaded with response as (.*)")]
        public void ThenISeeTheFileIsBeingUploadedWithResponseAs(string status)
        {
            Assert.That(_settings.Response.StatusCode.ToString(), Is.EqualTo(status));
        }
    }

}
