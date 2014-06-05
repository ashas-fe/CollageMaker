using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace InstCollageMaker
{
    public class CollageMaker
    {
        private async Task<byte[]> LoadContactPhoto(string img_url)
        {
            using (var client = new HttpClient())
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, img_url);
                var responseMessage = await client.SendAsync((requestMessage));
                var responseData = await responseMessage.Content.ReadAsByteArrayAsync();
                return responseData;
            }
        }

        private async Task<BitmapImage> LoadImageAsync(byte[] img_data)
        {
            using (var ms = new MemoryStream(img_data))
            {
                var image = new BitmapImage();
                image.SetSource(ms);
                return image;
            }
        }

        public async Task<WriteableBitmap> MakeCollage(string[] image_URLs)
        {
            List<BitmapImage> images = new List<BitmapImage>(); // BitmapImage list.

            foreach (string image in image_URLs)
            {

                byte[] imgData = await LoadContactPhoto(image);
                BitmapImage img = await LoadImageAsync(imgData);
                images.Add(img);
            }

            // Create a bitmap to hold the combined image 
            BitmapImage finalImage = new BitmapImage();
            StreamResourceInfo sri = System.Windows.Application.GetResourceStream(new Uri("White.jpg",
                UriKind.Relative));
            finalImage.SetSource(sri.Stream);
            WriteableBitmap wbFinal = new WriteableBitmap(finalImage);

            const int deltax = 200;
            const int deltay = 200;

            for (int i = 0; i<images.Count; i++)
            {                
                Image image = new Image();
                image.Height = deltax;
                image.Width = deltay;
                image.Source = images[i];

                // TranslateTransform                      
                TranslateTransform tf = new TranslateTransform();
                tf.X = i / 2 *deltax;
                tf.Y = i % 2 * deltax;
                wbFinal.Render(image, tf);
            }

            wbFinal.Invalidate();
            return wbFinal;
        }
    }
}
