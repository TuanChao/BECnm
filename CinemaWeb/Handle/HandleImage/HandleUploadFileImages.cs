using CloudinaryDotNet.Actions;
using CloudinaryDotNet;

namespace CinemaWeb.Handle.HandleImage
{
    public class HandleUploadFileImages
    {
        static string nameCloud = "dxy9xwjcz";
        static string apiKey = "659753988773243";
        static string apiSecret = "MHw92BFDAFk9jk9xQYDO3MSD9MI";
        static public Account acc = new Account(nameCloud, apiKey, apiSecret);
        static public Cloudinary cloudy = new Cloudinary(acc);
        public static async Task<string> UploadLoadFile(IFormFile f)
        {
            if (f == null || f.Length == 0)
            {
                throw new ArgumentException("Tập tin chưa được chọn");
            }
            using (var stream = f.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(f.FileName, stream),
                    PublicId = "images" + DateTime.UtcNow.Ticks,
                    Transformation = new Transformation().Width(400).Height(400).Crop("fit")
                };
                var uploadfileResult = await HandleUploadFileImages.cloudy.UploadAsync(uploadParams);
                if (uploadfileResult.Error != null)
                {
                    throw new Exception(uploadfileResult.Error.Message);
                }
                string imageUrl = uploadfileResult.SecureUrl.ToString();
                return imageUrl;
            }
        }
        public static async Task DeleteFile(string url)
        {
            Uri uri = new Uri(url);
            string path = uri.Segments[5];
            int dotIndex = path.LastIndexOf('.');
            if (dotIndex >= 0)
            {
                path = path.Substring(0, dotIndex);
            }
            var deleteParams = new DeletionParams(path)
            {
                ResourceType = ResourceType.Image
            };
            var deleteResult = await cloudy.DestroyAsync(deleteParams);
            if (deleteResult.Error != null)
            {
                throw new Exception(deleteResult.Error.Message);
            }
        }
        public static async Task<string> UpdateFile(string url, IFormFile file)
        {
            await HandleUploadFileImages.DeleteFile(url);
            string link = await HandleUploadFileImages.UploadLoadFile(file);
            return link;
        }
    }
}
