using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Shared.Controls
{
    public partial class WebViewControl : UserControl
    {
        public bool IsWebViewReady { get; set; } = false;

        public WebViewControl()
        {
            InitializeComponent();
        }
        private async void WebView_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                IsWebViewReady = true;
            }
            else
            {
                MessageBox.Show($"WebView2 initialization failed. Error: {e.InitializationException.Message}");
            }
        }

        public async Task LoadPdfAsync(byte[] pdfBytes)
        {
            try
            {
                await webView.EnsureCoreWebView2Async();

                // Convert PDF bytes to Base64 string
                string pdfBase64 = Convert.ToBase64String(pdfBytes);
                string pdfDataUri = $"data:application/pdf;base64,{pdfBase64}";

                // Navigate to the PDF data URI
                webView.CoreWebView2.Navigate(pdfDataUri);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load PDF: {ex.Message}");
            }
        }
    }
}
