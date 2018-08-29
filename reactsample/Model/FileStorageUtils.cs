using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace reactsample.Model
{



    public class FileStorageUtils
    {
        public FileStorageUtils()
        {
            string fs = "";
            try
            {
                var config = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json")
                                .Build();
                fs = config["FileStorage"];
            }
            catch { }
            //default for testing framework
            if (String.IsNullOrWhiteSpace(fs))
                fs = @"c:\temp\";
            QuestionnaireFileLocation = $@"{fs}questionnaire";
            ResponseFileLocation = $@"{fs}responses";
        }

        public string QuestionnaireFileLocation { get; set; }
        public string ResponseFileLocation { get; set; }
        public string QuesionnaireFileName
        {
            get { return $@"{QuestionnaireFileLocation}\InitialData.csv"; }
        }

        public async Task Setup()
        {
            Debug.WriteLine("@@@ ppppppppppppppp");
            if (!Directory.Exists(QuestionnaireFileLocation))
                Directory.CreateDirectory(QuestionnaireFileLocation);
            if (!Directory.Exists(ResponseFileLocation))
                Directory.CreateDirectory(ResponseFileLocation);
            if (File.Exists(QuesionnaireFileName))
                File.Delete(QuesionnaireFileName);
            await CopyEmbeddedFileToQuestionnaireLocation();
        }

        async Task CopyEmbeddedFileToQuestionnaireLocation()
        {
            var assembly = Assembly.GetAssembly(typeof(FileStorageUtils));
            var resourceStream = assembly.GetManifestResourceStream("reactsample.Data.InitialData.csv");
            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                string fc = await reader.ReadToEndAsync();
                File.WriteAllText(QuesionnaireFileName, fc);
            }
        }

        public async Task<List<Questionnaire>> LoadAllQuestionnaires()
        {
            string[] splitLine(string line)
            {
                return line.Split(',');
            }

            int? ParseInt(string x)
            {
                if (string.IsNullOrWhiteSpace(x)) return null;
                return Int32.Parse(x);
            }

            string[] fl = await File.ReadAllLinesAsync(QuesionnaireFileName);
            var fl1 = fl.Skip(1)
                        .Select(splitLine)
                        .Select(n => new
                        {
                            QN = n[0],
                            Sec = n[1],
                            QId = Int32.Parse(n[2]),
                            Q = n[3],
                            QT = (QuestionType)(Int32.Parse(n[4])),
                            QOId = ParseInt(n[5]),
                            PQId = ParseInt(n[6]),
                            PQOptId = ParseInt(n[7])
                        })
                        .GroupBy(n => n.QN)
                        .Select(qe => new Questionnaire
                        {
                            Name = qe.Key,
                            Sections = qe.GroupBy(s => s.Sec)
                                        .Select(s => new QuestionnaireSection
                                        {
                                            SectionName = s.Key,
                                            Questions = s.GroupBy(q => q.QId)
                                                        .Select(q => new Question
                                                        {
                                                            QuestionId = q.Key,
                                                            QuestionType = q.First().QT,
                                                            QuestionText = q.First().Q,
                                                            ParentQuestionId = q.First().PQId,
                                                            ParentQuestionOptionId = q.First().PQOptId,
                                                            Options = q.Skip(1)
                                                                       .Select(o => new QuestionOption { OptionId = o.QOId.Value, OptionText = o.Q })
                                                                       .ToList()

                                                        })
                                                        .ToList()
                                        })
                                        .ToList()
                        })
                        .ToList();
            return fl1;
        }

        public async Task WriteQuestionnaireResponse(QuestionnaireResponse response) {
            string toCsvLine(QuestionnaireResponseItem item)
            {
                return $"{response.QuestionnaireName},{item.QuestionId},{item.Response}";
            }
            response.SubmitTimeStamp = DateTime.Now.Ticks;
            string fn = $"{ResponseFileLocation}\\{response.QuestionnaireName}_{response.SubmitTimeStamp}.csv";
            string[] lines = response.Responses.Select(toCsvLine).ToArray();
            await File.WriteAllLinesAsync(fn, lines);
        } 

    }
}
