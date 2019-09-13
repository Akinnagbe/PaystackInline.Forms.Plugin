using Newtonsoft.Json;
using SampleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SampleApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaystackPage : ContentPage
    {
        public PaystackPage()
        {
            InitializeComponent();
            LoadPlugin();
        }

        void LoadPlugin()
        {
            var product = new PaystackModel();
            product.key = "pk_test_6acfe19632a2b3005e9bfe05648311f71d7c87b0";
            product.email = "me@you.com";
            product.amount = 490000m;
            //  product.subaccount = "ACCT_jiOpeieli";
            // product.bearer = "subaccount";
            product.@ref = Guid.NewGuid().ToString("N");
            product.currency = "NGN";
            //  product.metadata = custom;

            hybridWebView.Data = JsonConvert.SerializeObject(product);// product.ToString();
        
            
            hybridWebView.RegisterCloseAction(() => DisplayAlert("WebView Closed", "Close button clicked", "ok"));
            hybridWebView.RegisterCallBackAction(data => DisplayAlert("Alert", "Hello " + data, "OK"));
        }
    }
}