using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SampleApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            JArray jarray = new JArray();
            dynamic customFieldsArray = new JArray();

            dynamic displayname = new JObject();
            displayname.display_name = "Mobile Number";
            displayname.variable_name = "mobile_number";
            displayname.@value = "+2348038541905";

            customFieldsArray.Add(displayname);
            dynamic jobtitlefield = new JObject();
            jobtitlefield.display_name = "Job Title";
            jobtitlefield.variable_name = "job_title";
            jobtitlefield.@value = "Helper";

            customFieldsArray.Add(jobtitlefield);



            dynamic custom = new JObject();
            custom.custom_fields = customFieldsArray;

            dynamic product = new JObject();
            product.key = "pk_test_eb19f9d000457612f2ded14cfb0cb223e56f1d11";
            product.email = "olamidejames007@gmail.com";
            product.amount = 490000m;
            product.subaccount = "ACCT_xopi2ghxe20jmv1";
            product.bearer = "subaccount";
            product.@ref = Guid.NewGuid();
            product.metadata = custom;

            hybridWebView.Data = product.ToString();


            hybridWebView.RegisterCloseAction(() => DisplayAlert("WebView Closed", "Close button clicked", "ok"));
            hybridWebView.RegisterCallBackAction(data => DisplayAlert("Alert", "Hello " + data, "OK"));
        }
    }
}
