using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace LibraryManagementSystem.Helpers
{
    public enum ToastType
    {
        Success,
        Error,
        Warning,
        Info
    }

    public static class ToastMessageHelper
    {

        public static void SetToastMessage(ITempDataDictionary tempData, string message, string title, ToastType type)
        {
            tempData["ToastMessage"] = message;
            tempData["ToastTitle"] = title;
            tempData["ToastType"] = type.ToString().ToLower();
        }
    }
}
