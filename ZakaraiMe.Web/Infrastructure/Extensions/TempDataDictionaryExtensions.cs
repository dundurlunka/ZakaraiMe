﻿namespace ZakaraiMe.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    /// <summary>
    /// Extension methods for errors, warnings and success messages. Uses tempData to store the message.
    /// </summary>
    public static class TempDataDictionaryExtensions
    {
        public static void AddSuccessMessage(this ITempDataDictionary tempData, string message, params string[] args)
        {
            if (args.Length > 0)
                message = string.Format(message, args);

            tempData[WebConstants.TempDataSuccessMessageKey] = message;

        }

        public static void AddErrorMessage(this ITempDataDictionary tempData, string message, params string[] args)
        {
            if (args.Length > 0)
                message = string.Format(message, args);

            tempData[WebConstants.TempDataErrorMessageKey] = message;
        }

        public static void AddWarningMessage(this ITempDataDictionary tempData, string message, params string[] args)
        {
            if (args.Length > 0)
                message = string.Format(message, args);

            tempData[WebConstants.TempDataWarningMessageKey] = message;
        }
    }
}
