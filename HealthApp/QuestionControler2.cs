using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace HealthApp
{
    public class QuestionControler
    {
        public int Age { get; set; }
        public string Gender { get; set; }
        HttpHelper HttpHelper = new HttpHelper();
        public static string _Session;
        public static string _Cookies;
        WebClient webClient = new WebClient();
        public List<Simptomes> allSimptomes = new List<Simptomes>();
        public List<Simptomes> chooseSimptomes = new List<Simptomes>();

        public (string question, string desc, string[] answers, string questionnaireId, string linkId) StartQuestion(string i)
        {
            string url = "https://helzy.ru/api/v1/steps/" + i;
            string s = HttpHelper.GET(url, webClient.Headers);

            JObject json = JObject.Parse(s);

            string questionnaireId = json["questionnaire"]["id"].ToString();

            string linkId = json["questionnaire"]["items"][0]["id"].ToString();

            var answers = json["questionnaire"]["items"][0]["options"].ToArray().Select(x => x.ToString()).ToArray();
            var question = json["questionnaire"]["title"].ToString();

            string desc = "";

            try
            {
                desc = json["questionnaire"]["items"][0]["description"].ToString();
            }
            catch
            {

            }

            List<string> answersString = new List<string>();

            foreach (var answer in answers)
            {
                answersString.Add(answer.ToString().Replace("{{", "{").Replace("}}", "}"));
            }

            return (question, desc, answersString.ToArray(), questionnaireId, linkId);
        }

        public Disease FinalRequest()
        {
            string url = "https://helzy.ru/api/v1/reports";

            string s = HttpHelper.GET(url, webClient.Headers);

            JObject json = JObject.Parse(s);


            Disease disease = new Disease()
            {
                Title = json["diseases"][0]["title"].ToString(),
                Detail = json["diseases"][0]["detail"].ToString(),
                DetailWithAnswers = json["summary"]["detail"].ToString(),
                Probalility = json["diseases"][0]["url"].ToString()

            };

            return disease;
        }

        public string SendAnswer(string answer, string questionnaireId, string linkId)
        {
            string data = "{\"questionnaireId\":\"" + questionnaireId +
                 "\",\"items\":[{\"linkId\":\"" + linkId +
                 "\",\"answer\":" + answer + "}]}";

            WebHeaderCollection headers = new WebHeaderCollection();

            headers.Add(HttpRequestHeader.Accept, "application/json, text/plain, */*");
            headers.Add("AppSessionId", _Session);
            headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.212 YaBrowser/21.5.3.742 Yowser/2.5 Safari/537.36");
            headers.Add(HttpRequestHeader.ContentType, "application/json");

            var result = HttpHelper.POST("https://helzy.ru/api/v1/steps", data, headers);

            return result;
        }

        internal void SendSimptomes()
        {

            foreach (var item in chooseSimptomes)
            {
                Send(item);
            }


            void Send(Simptomes simptome)
            {
                string url = "https://helzy.ru/api/v1/observations";

                string json = JsonConvert.SerializeObject(simptome);

                //WebClient webClient1 = new WebClient();
                //webClient1.Headers.Add("appSessionId", _Session);

                //webClient1.Headers.Add("Accept-Language", " en-US");
                //webClient1.Headers.Add("Accept", " text/html, application/xhtml+xml, */*");
                //webClient1.Headers.Add("User-Agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)");

                ////string html = webClient1.UploadString(url, json);
                Console.WriteLine(json);
                Console.WriteLine(_Session);

                WebHeaderCollection headers = new WebHeaderCollection();

                headers.Add(HttpRequestHeader.Accept, "application/json, text/plain, */*");
                headers.Add("AppSessionId", _Session);
                headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.212 YaBrowser/21.5.3.742 Yowser/2.5 Safari/537.36");
                headers.Add(HttpRequestHeader.ContentType, "application/json");

                var result = HttpHelper.POST(url, json, headers);
            }
            string url1 = "https://helzy.ru/api/v1/steps?process=true";
            string requestJson = HttpHelper.GET(url1, webClient.Headers);

        }

        public static int MaxLenghtSimptomes { get; set; } = 50;

        public void newDiagnostic(string age, string gender)
        {
            webClient.Headers.Add("Accept-Language", " en-US");
            webClient.Headers.Add("Accept", " text/html, application/xhtml+xml, */*");
            webClient.Headers.Add("User-Agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)");

            string url = @"https://helzy.ru/api/v1/sessions";
            string json = "{\"age\":\"" + age + "\",\"gender\":\"" + gender + "\",\"text\":\" - \"}";

            string requestJson = HttpHelper.POST(url, json);
            _Session = System.Text.Json.JsonSerializer.Deserialize<appSessionIdClass>(requestJson).appSessionId;



        }



        public void AddSimptome(string nameSimptome)
        {
            if (allSimptomes.Where(x => x.display.ToLower() == nameSimptome.ToLower()).FirstOrDefault() == null)
            {
                Simptomes simptomes1 = allSimptomes.Where(x => x.display.ToLower().Contains(nameSimptome.ToLower())).FirstOrDefault();
                chooseSimptomes.Add(simptomes1);
                return;
            }
            Simptomes simptomes = allSimptomes.Where(x => x.display.ToLower() == nameSimptome.ToLower()).FirstOrDefault();
            chooseSimptomes.Add(simptomes);

        }

        /// <summary>
        /// Поиск симптомов, найденые симтромы вугружаются в allSimptomes, а выбранные добавляются в chooseSimptomes. 
        /// </summary>
        /// <param name="request">Строка симптома, например "темп"</param>
        public void Simptomes(string request)
        {
            allSimptomes.Clear();
            string url = "https://helzy.ru/api/v1/symptoms?name={" + request + "}";
            webClient.Headers.Add("appSessionId", _Session);
            string requestJson = HttpHelper.GET(url, webClient.Headers);
            var unKnownType = System.Text.Json.JsonSerializer.Deserialize<List<Simptomes>>(requestJson);
            allSimptomes.AddRange(unKnownType);
            for (int i = 0; i < allSimptomes.Count(); i++)
            {
                if (allSimptomes[i].display.Length > MaxLenghtSimptomes)
                {
                    allSimptomes[i].shortDisplay = allSimptomes[i].display.Substring(0, MaxLenghtSimptomes - 3) + "...";
                }
                else
                {
                    allSimptomes[i].shortDisplay = allSimptomes[i].display;
                }
            }

        }



    }

    /// <summary>
    /// Для десериализации 1 шага.
    /// </summary>
    public class appSessionIdClass
    {
        public string appSessionId { get; set; }
    }



    public class Simptomes
    {
        public string system { get; set; }
        public string code { get; set; }
        public string display { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string shortDisplay { get; set; }
    }

    public class Disease
    {
        public string Title { get; set; }
        public string DetailWithAnswers { get; set; }
        public string Detail { get; set; }
        public string Probalility { get; set; }
    }
}