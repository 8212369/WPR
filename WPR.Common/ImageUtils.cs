using System;
using System.Collections.Generic;
using System.IO;

#if __ANDROID__
using Android.Graphics;
#else
using System.Drawing;
#endif

namespace WPR.Common
{
    public static class ImageUtils
    {
        public static List<String> SplitAndSave(string storePath, string filenameFormat, Stream originalImageStream,
            int rowCount, int columnCount)
        {
#if __ANDROID__
            Bitmap? originalBitmap = BitmapFactory.DecodeStream(originalImageStream);
            if (originalBitmap == null)
            {
                return new List<String>();
            }

            int tileSizeX = originalBitmap.Width / columnCount;
            int tileSizeY = originalBitmap.Height / rowCount;

            List<string> pathList = new List<string>();
            Directory.CreateDirectory(storePath);

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    string imagePath = System.IO.Path.Combine(storePath, String.Format(filenameFormat, i, j));

                    Bitmap? tempBitmap = Bitmap.CreateBitmap(originalBitmap, j * tileSizeX, i * tileSizeY, tileSizeX, tileSizeY);
                    if (tempBitmap == null)
                    {
                        Log.Error(LogCategory.Common, "Can't create splitted bitmap on Android!");
                        return pathList;
                    }

                    using (FileStream fs = new FileStream(imagePath, FileMode.OpenOrCreate))
                    {
                        tempBitmap.Compress(Bitmap.CompressFormat.Png, 100, fs);
                        pathList.Add(imagePath);
                    }
                }

            }

            return pathList;
#else
            Image originalImage = Image.FromStream(originalImageStream);
            int tileSizeX = originalImage.Width / columnCount;
            int tileSizeY = originalImage.Height / rowCount;

            Bitmap img = new Bitmap(tileSizeX, tileSizeY, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var graphicsDrawer = System.Drawing.Graphics.FromImage(img);

            Directory.CreateDirectory(storePath);

            List<String> pathList = new List<String>();

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    string imagePath = Path.Combine(storePath, String.Format(filenameFormat, i, j));

                    graphicsDrawer.Clear(System.Drawing.Color.Transparent);
                    graphicsDrawer.DrawImage(originalImage,
                        new System.Drawing.Rectangle(0, 0, tileSizeX, tileSizeY),
                        new System.Drawing.Rectangle(j * tileSizeX, i * tileSizeY, tileSizeX, tileSizeY),
                        GraphicsUnit.Pixel);

                    img.Save(imagePath, System.Drawing.Imaging.ImageFormat.Png);
                    pathList.Add(imagePath);
                }

            }

            return pathList;
#endif
        }
    }
}
