using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Speech.V1;
using Grpc.Auth;
using MGedik.PronunciationTest.WebSite.Models;

namespace MGedik.PronunciationTest.WebSite.Controllers
{
    public class SoundController : Controller
    {
        [HttpPost]
        public async Task<ActionResult> PostFormData(HttpPostedFileBase file, string testType)
        {
            if (file.ContentLength < 1)
                return new HttpStatusCodeResult(HttpStatusCode.UnsupportedMediaType);

            TestItem selectedTestItem = Session["selectedTestItem"] as TestItem;
            SpeakTestItem selectedSpeakTestItem = Session["selectedSpeakTestItem"] as SpeakTestItem;

            if ((testType == "text" && selectedTestItem == null) || (testType == "speak" && selectedSpeakTestItem == null))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            string root = Server.MapPath("~/App_Data");
            string fileName = string.Format("BodyPart_{0}", Guid.NewGuid());
            string filePath = Path.Combine(root, fileName);
            file.SaveAs(filePath);

            string resultText = await Recognition(System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/spech text-d0a18cc9ed24.json"), filePath);
            resultText = resultText.ToLower(CultureInfo.GetCultureInfo("en-US")).Trim();
            if (testType == "text")
            {
                if (selectedTestItem != null && selectedTestItem.Pronunciation == resultText.ToLower().Trim())
                {
                    TestContainer.SetTestItemStatus(selectedTestItem.Id, true);
                    return Json(new { Status = "OK", SpeechToText = resultText });
                }
                if (selectedTestItem != null)
                    TestContainer.SetTestItemStatus(selectedTestItem.Id, false);
            }
            else if(testType == "speak")
            {
                if (selectedSpeakTestItem != null)
                {
                    if (selectedSpeakTestItem.Answer.Contains("|"))
                    {
                        var possibleAnswerList = selectedSpeakTestItem.Answer.Split('|').ToList();
                        foreach (var possibleAnswer in possibleAnswerList)
                        {
                            if (resultText.StartsWith(possibleAnswer))
                            {
                                SpeakTestContainer.SetSpeakTestItemStatus(selectedSpeakTestItem.Id, true);
                                return Json(new { Status = "OK", SpeechToText = resultText });
                            }
                        }

                    }
                    else if (resultText.StartsWith(selectedSpeakTestItem.Answer))
                    {
                        SpeakTestContainer.SetSpeakTestItemStatus(selectedSpeakTestItem.Id, true);
                        return Json(new {Status = "OK", SpeechToText = resultText});
                    }
                }
                if (selectedSpeakTestItem != null)
                    SpeakTestContainer.SetSpeakTestItemStatus(selectedSpeakTestItem.Id, false);
            }
            return Json(new { Status = "FAIL", SpeechToText = resultText });
        }

        private async Task<string> Recognition(string credentialsFilePath, string filePath)
        {
            GoogleCredential googleCredential;
            using (Stream m = new FileStream(credentialsFilePath, FileMode.Open))
            {
                googleCredential = GoogleCredential.FromStream(m);
            }
            var channel = new Grpc.Core.Channel(SpeechClient.DefaultEndpoint.Host, googleCredential.ToChannelCredentials());

            var speech = SpeechClient.Create(channel);
            var streamingCall = speech.StreamingRecognize();
            await streamingCall.WriteAsync(new StreamingRecognizeRequest
            {
                StreamingConfig = new StreamingRecognitionConfig()
                {
                    Config = new RecognitionConfig()
                    {
                        Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                        SampleRateHertz = 16000,
                        LanguageCode = "en-US"
                    },
                    InterimResults = true
                }
            });

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                var buffer = new byte[32 * 1024];
                int bytesRead;
                while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await streamingCall.WriteAsync(
                        new StreamingRecognizeRequest
                        {
                            AudioContent = Google.Protobuf.ByteString.CopyFrom(buffer, 0, bytesRead)
                        });
                    //await Task.Delay(500);
                };
            }

            string finalText = string.Empty;

            Task printResponses = Task.Run(async () =>
            {
                while (await streamingCall.ResponseStream.MoveNext(default(CancellationToken)))
                {
                    foreach (var result in streamingCall.ResponseStream.Current.Results)
                    {
                        if (!result.IsFinal)
                        {
                            foreach (var alternative in result.Alternatives)
                            {
                                Debug.WriteLine("alternative: " + alternative.Transcript);
                            }
                        }
                        else
                        {
                            foreach (var alternative in result.Alternatives)
                            {
                                Debug.WriteLine("final: " + alternative.Transcript);
                            }

                            var speechRecognitionAlternative = result.Alternatives.OrderByDescending(x => x.Confidence).FirstOrDefault();
                            if (speechRecognitionAlternative != null)
                                finalText = speechRecognitionAlternative.Transcript;
                        }
                    }
                }
            });

            await streamingCall.WriteCompleteAsync();
            await printResponses;
            return finalText;
        }
    }
}
