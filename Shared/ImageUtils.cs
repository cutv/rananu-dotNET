using System.Drawing;
using System.Drawing.Imaging;

namespace Common
{
    public static class ImageUtils
    {
        public static void Image2Jpeg(string sourcePath, string destinationPath)
        {
            if (!File.Exists(sourcePath))
                throw new IOException("Source path not exist");
            if (!File.Exists(sourcePath))
                throw new IOException("Source path not exist");
            string fileName = Path.GetFileNameWithoutExtension(sourcePath);
            using (Image png = Image.FromFile(sourcePath))
            {
                png.Save(destinationPath, ImageFormat.Jpeg);
            }
        }
        public static void Image2Jpeg(Stream stream, string destinationFilePath)
        {
            using (Image png = Image.FromStream(stream))
            {
                png.Save(destinationFilePath, ImageFormat.Jpeg);
            }
        }
    }
}
