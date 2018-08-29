using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using reactsample;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System.Diagnostics;
using System.Collections.Generic;
using reactsample.Model;
using System.Linq;
using Newtonsoft.Json;
using System.Text;

namespace XUnitTestProject1
{

    public class UnitTest1
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        public UnitTest1()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task EnsureCanCallAnEndpoint()
        {
            var response = await _client.GetAsync("/api/sampledata/weatherforecasts");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CanGetQuestionnaires()
        {
            var response = await _client.GetAsync("/api/questionnaire/getall");
            List<Questionnaire> r = await response.Content.ReadAsAsync<List<Questionnaire>>();
            //Debug.WriteLine($"@@@ xx {r} ");
            Assert.True(r.Count() > 0);
            Questionnaire q = r.First();
            Assert.Equal("Questionnaire1", q.Name);
            Assert.True(q.Sections.Count() == 2);
            QuestionnaireSection section = q.Sections.First();
            Assert.Equal("Section1", section.SectionName);
            List<Question> sectionQuestions = section.Questions;
            Assert.True(sectionQuestions.Count() == 4);
            Question s1q1 = sectionQuestions.First();
            Assert.Equal("What is your name?", s1q1.QuestionText);
            Assert.Equal(QuestionType.Text, s1q1.QuestionType);
            Question s1q2 = sectionQuestions[1];
            Assert.Equal(QuestionType.Radio, s1q2.QuestionType);
        }

        [Fact]
        public async Task CanGetQuestionnaire()
        {
            var response = await _client.GetAsync("/api/questionnaire/getquestionnaire/Questionnaire1");
            Questionnaire r = await response.Content.ReadAsAsync<Questionnaire>();
            Assert.Equal("Questionnaire1", r.Name);
        }

        [Fact]
        public async Task CanSubmitQuestionnaireResponse() {

            QuestionnaireResponseItem CreateRI(int questionId, string resp)
            {
                return new QuestionnaireResponseItem { QuestionId = questionId, Response = resp };
            }

            var response = await _client.GetAsync("/api/questionnaire/getquestionnaire/Questionnaire1");
            Questionnaire r = await response.Content.ReadAsAsync<Questionnaire>();
            var qr = new QuestionnaireResponse
            {
                QuestionnaireName = r.Name,
                SubmitTimeStamp = 0L,
                Responses = new List<QuestionnaireResponseItem> {
                                                     CreateRI(1, "Tony"),
                                                     CreateRI(2, "1"),
                                                     CreateRI(3, "3"),
                                                     CreateRI(4,"Orange choclate"),
                                                     CreateRI(5,"Whaaat!")
                                                 }
            };
            var r1 = await _client.PostAsJsonAsync("/api/questionnaire/postresponse", qr);
            r1.EnsureSuccessStatusCode();
            var c1 = await r1.Content.ReadAsStringAsync();
            Debug.WriteLine($"@@@ {c1}");
        }

        


    }
}
