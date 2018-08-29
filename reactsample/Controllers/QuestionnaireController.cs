using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using reactsample.Model;
using System.Diagnostics;

namespace reactsample.Controllers
{
    [Route("api/[controller]")]
    public class QuestionnaireController : Controller
    {
        public QuestionnaireController()
        {
            new FileStorageUtils().Setup().Wait();
        }

        [HttpGet("[action]")]
        public async Task<List<Questionnaire>> GetAll()
        {
            var fs = new FileStorageUtils();
            return await fs.LoadAllQuestionnaires(); 
        }

        [HttpGet("[action]/{questionnaireName}")]
        public async Task<Questionnaire> GetQuestionnaire(string questionnaireName)
        {
            var fs = new FileStorageUtils();
            var items =  await fs.LoadAllQuestionnaires();
            return items.FirstOrDefault(n => n.Name == questionnaireName);
        }

        [HttpPost("[action]")]
        public async Task<QuestionnaireResponse> PostResponse([FromBody]QuestionnaireResponse response)
        {
            var fs = new FileStorageUtils();
            await fs.WriteQuestionnaireResponse(response);
            return response;
        }
    }
}
