using Serilog;
using System;
using System.Windows;

namespace Shared.Services
{
    public class ExceptionService
    {
        public static void HandleError(Exception ex, string message)
        {
            Log.Error(ex, "An exception occurred");
            MessageBox.Show($"{Translations.GetValue("Error")} : {message}", "Good Grades", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
