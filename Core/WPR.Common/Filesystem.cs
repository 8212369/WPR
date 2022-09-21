using System;
using System.Collections.Generic;
using System.IO;

#if __ANDROID__
using Android.Content.Res;
#endif

namespace WPR.Common
{
    public static class Filesystem
    {
        // https://stackoverflow.com/questions/58744/copy-the-entire-contents-of-a-directory-in-c-sharp
        public static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            Directory.CreateDirectory(targetPath);

            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }

#if __ANDROID__
        public static void CopyFolderFromAssets(AssetManager assets, string sourcePath, string targetPath)
        {
            string[]? assetFilenames = assets.List(sourcePath);
            if ((assetFilenames == null) || (assetFilenames.Length == 0))
            {
                return;
            }

            Directory.CreateDirectory(targetPath);

            foreach (string assetFilename in assetFilenames)
            {
                CopyFileFromAssets(assets, Path.Combine(sourcePath, assetFilename), Path.Combine(targetPath, assetFilename));
            }
        }

        public static void CopyFileFromAssets(AssetManager assets, string sourceFile, string destFile)
        {
            using (Stream assetStream = assets.Open(sourceFile))
            {
                using (FileStream destStream = File.Open(destFile, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    assetStream.CopyTo(destStream);
                }
            }
        }
#endif
    }
}
