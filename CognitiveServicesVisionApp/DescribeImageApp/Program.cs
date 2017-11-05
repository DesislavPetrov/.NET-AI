using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
namespace DescribeImageApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Task<string> result = DescribeImage(@"D:\Downloads\dog-family-tree-feature.jpg");
            Console.WriteLine(result.Result);
        }
        public static async Task<string> DescribeImage(string imageFilePath)
        {
            // Create an HTTP client object to be able to make API calls
            using (HttpClient httpClient = new HttpClient())
            {
                // Build the HTTP request object with the required header (subscription key) and request parameters and API URL
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "ffab5d629f554b92ac4386ad986f1ebd");

                using (MultipartFormDataContent requestContent = new MultipartFormDataContent())
                {
                    var queryString = HttpUtility.ParseQueryString(string.Empty);
                    queryString["maxCandidates"] = "1";
                    var uri = "https://westeurope.api.cognitive.microsoft.com/vision/v1.0/describe?" + queryString;
                    // Read the image file as multipart form data, and add it to the HTTP request object
                    try
                    {
                        var imageContent = new ByteArrayContent(System.IO.File.ReadAllBytes(imageFilePath));
                        requestContent.Add(imageContent);
                        // Call the Computer Vision API by supplying it your HTTP request object
                        HttpResponseMessage responseMessage = await httpClient.PostAsync(uri, requestContent);
                        // Display the HTTP response from the API as a string
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
