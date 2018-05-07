using System;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;

namespace Classifier.ImageDownloader
{
    class Program
    {
        private static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            var folder = args[0];
            var images = JsonConvert.DeserializeObject<ImageMetadata[]>(File.ReadAllText($"{folder}/images.json"));

            foreach (var image in images)
            {
                using (var fs = new FileStream($"{folder}/{image.ImageId}.{image.EncodingFormat}", FileMode.Create))
                {
                    using (var stream = client.GetStreamAsync(image.ContentUrl).Result)
                    {
                        stream.CopyTo(fs);
                        fs.Close();
                    }
                }
            }
        }
    }

    public class ImageMetadata
    {
        public string ImageId { get; set; }
        public string EncodingFormat { get; set; }
        public string ContentUrl { get; set; }
    }
}
