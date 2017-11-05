using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
namespace OcrApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Task<string> result = ExtractText(@"D:\Downloads\seldom-seen-road-ocr.jpg");
            Console.WriteLine(result.Result);
        }
        public static async Task<string> ExtractText(string imageFilePath)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "ffab5d629f554b92ac4386ad986f1ebd");
                using (MultipartFormDataContent requestContent = new MultipartFormDataContent())
                {
                    var queryString = HttpUtility.ParseQueryString(string.Empty);
                    queryString["detectOrientation"] = "true";
                    var uri = "https://westeurope.api.cognitive.microsoft.com/vision/v1.0/ocr?" + queryString;
                    try
                    {
                        var imageContent = new ByteArrayContent(System.IO.File.ReadAllBytes(imageFilePath));
                        requestContent.Add(imageContent);
                        HttpResponseMessage responseMessage = await httpClient.PostAsync(uri, requestContent);
                        string responseJson = await responseMessage.Content.ReadAsStringAsync();
                        return responseJson;
                    }
                    catch (System.IO.FileNotFoundException ex)
                    {
                        return "The specified image is invalid";
                    }
                    catch (ArgumentException ex)
                    {
                        return "The HTTP erquest objectdoes not seem to be correctly formed";
                    }
                }
            }
        }
    }
}
