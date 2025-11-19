using System.Reflection.Metadata;

namespace E_Commerce.Services
{
    public class ImageHandler
    {
        public string HandledURL(byte[] imageBytes, string ControllerName)
        {

            if (imageBytes != null)
            {
                //string mime = null;
                //string pureBase64 = base64Image.ToString();

                //// Extract MIME if exists
                //if (base64Image.Contains("base64,"))
                //{
                //    var parts = base64Image.Split(',');
                //    mime = parts[0];
                //    pureBase64 = parts[1];
                //}

                //// Detect extension
                //string extension = ".jpg"; // default
                //if (!string.IsNullOrEmpty(mime))
                //{
                //    if (mime.Contains("image/png")) extension = ".png";
                //    else if (mime.Contains("image/jpeg")) extension = ".jpg";
                //    else if (mime.Contains("image/jpg")) extension = ".jpg";
                //    else if (mime.Contains("image/webp")) extension = ".webp";
                //    else if (mime.Contains("image/gif")) extension = ".gif";
                //}

                //// Decode Base64
                //byte[] imageBytes = Convert.FromBase64String(pureBase64);

                // Save file
                string folderPath = Path.Combine("wwwroot", "images", $"{ControllerName}");
                Directory.CreateDirectory(folderPath);

                string fileName = $"{Guid.NewGuid()}.jpg";
                string filePath = Path.Combine(folderPath, fileName);

                File.WriteAllBytes(filePath, imageBytes);

                return $"/images/{ControllerName}/{fileName}";
            }
            else
            {
                return null;
            }
        }
    }
}
