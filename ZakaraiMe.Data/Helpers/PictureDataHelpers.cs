﻿namespace ZakaraiMe.Data.Helpers
{
    public static class PictureDataHelpers
    {
        private const string ImagesFolder = @"\images\database\";
        private const string FormatConstant = ".jpg";

        public static string GetPictureFilePath(string wwwRootPath, string imageName)
        {
            return wwwRootPath + ImagesFolder + imageName + FormatConstant;
        }
    }
}