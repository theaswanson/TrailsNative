using System;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace TrailsNative
{
    public class Core
    {
        
        public static async Task<Weather> GetWeather(string zipCode)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += (s, e) => {
                var text = e.Result; // get the downloaded text
                string localFilename = "siteContents.json";
                string localPath = Path.Combine(documentsPath, localFilename);
                File.WriteAllText(localPath, text); // writes to local storage
            };

            WebClient webClient2 = new WebClient();
            webClient2.DownloadStringCompleted += (s, e) => {
                var text = e.Result;
                string localFilename = "lastUpdate.json";
                string localPath = Path.Combine(documentsPath, localFilename);
                File.WriteAllText(localPath, text);
            };

            var url = new Uri("http://codechameleon.com/dev/fwt/siteContents.php"); // siteContents
            var url2 = new Uri("http://codechameleon.com/dev/fwt/lastUpdate.php"); // lastUpdate

            webClient.Encoding = Encoding.UTF8;
            webClient.DownloadStringAsync(url);

            webClient2.Encoding = Encoding.UTF8;
            webClient2.DownloadStringAsync(url2);

            var res = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(documentsPath + "/siteContents.json"));
            foreach (var page in res.contents.pages)
            {
                Console.WriteLine(page.name);
                Console.WriteLine(page.slug);
                Console.WriteLine(page.coverImage);
                Console.WriteLine(page.body);
            }
            foreach (var news in res.contents.news)
            {
                Console.WriteLine(news.name);
                Console.WriteLine(news.slug);
                Console.WriteLine(news.coverImage);
                Console.WriteLine(news.summary);
                Console.WriteLine(news.body);
            }
            foreach (var events in res.contents.events)
            {
                Console.WriteLine(events.name);
                Console.WriteLine(events.slug);
                Console.WriteLine(events.date);
                Console.WriteLine(events.coverImage);
                Console.WriteLine(events.summary);
                Console.WriteLine(events.body);
            }


            JObject o1 = JObject.Parse(File.ReadAllText(documentsPath + "/siteContents.json"));

            // read JSON directly from a file
            using (StreamReader file = File.OpenText(documentsPath + "/siteContents.json"))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject o2 = (JObject)JToken.ReadFrom(reader);
            }

            //webClient.DownloadFile("http://codechameleon.com/dev/fwt/siteContents.php", "siteContents.xls"); // Download Using HttpWebRequest
            //Toast.MakeText(this, "File Downloaded", ToastLength.Short).Show();

            //Sign up for a free API key at http://openweathermap.org/appid  
            string key = "2c35c629917f1ab7c2ead0a15f1c971e";
            string queryString = "http://api.openweathermap.org/data/2.5/weather?zip="
                + zipCode + ",us&appid=" + key + "&units=imperial";

            //Make sure developers running this sample replaced the API key
            if (key != "2c35c629917f1ab7c2ead0a15f1c971e")
            {
                throw new ArgumentException("You must obtain an API key from openweathermap.org/appid and save it in the 'key' variable.");
            }

            dynamic results = await DataService.getDataFromService(queryString).ConfigureAwait(false);

            if (results["weather"] != null)
            {
                Weather weather = new Weather();
                weather.Title = (string)results["name"];
                weather.Temperature = (string)results["main"]["temp"] + " F";
                weather.Wind = (string)results["wind"]["speed"] + " mph";
                weather.Humidity = (string)results["main"]["humidity"] + " %";
                weather.Visibility = (string)results["weather"][0]["main"];

                DateTime time = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
                DateTime sunrise = time.AddSeconds((double)results["sys"]["sunrise"]);
                DateTime sunset = time.AddSeconds((double)results["sys"]["sunset"]);
                weather.Sunrise = sunrise.ToString() + " UTC";
                weather.Sunset = sunset.ToString() + " UTC";
                return weather;
            }
            else
            {
                return null;
            }
        }
    }
}