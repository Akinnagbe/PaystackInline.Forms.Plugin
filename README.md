# PaystackInline.Forms.Plugin
Paystack Inline Payment Plugin for Xamarin Forms and Windows

![ios 1](https://github.com/Akinnagbe/PaystackInline.Forms.Plugin/blob/master/PaystackInline.Forms.Plugin/ScreenShots/ios%201.PNG) ![android 1](https://github.com/Akinnagbe/PaystackInline.Forms.Plugin/blob/master/PaystackInline.Forms.Plugin/ScreenShots/droid%201.png)
![android 4](https://github.com/Akinnagbe/PaystackInline.Forms.Plugin/blob/master/PaystackInline.Forms.Plugin/ScreenShots/droid%204.png)

## NuGet
[Xam.Plugin.PaystackInline](https://www.nuget.org/packages/Xam.Plugin.PaystackInline/)

## Installation
1. Right Click on your Visual Studio Solution and Select Manage Nuget Package for Solution
2. Search for Xam.Plugin.PaystackInline
3. Select your PCL/.NET Standard project, Android project and iOS project
4. Click Install

## Platforms
| Platform          | Version       | 
| -------------     |:-------------:| 
| Xamarin.iOS       | iOS 8+        | 
| Xamarin.Android   | API 15+       |   
| Windows 10 UWP    | in progress   |  

## Android Requirements
1. Add a reference to Mono.Android.Export, or a compiler error will result.

## Usage
   ### XAML
   ```
   <?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:Plugin.PaystackInline.Forms.Plugin;assembly=Plugin.PaystackInline.Forms.Plugin"
             x:Class="SampleApp.MainPage">

    <StackLayout>
        <local:PaystackWebView x:Name="hybridWebView"  
        HorizontalOptions="FillAndExpand" 
        VerticalOptions="FillAndExpand"
        PaymentClosed="HybridWebView_PaymentClosed"
        PaymentSuccessful="hybridWebView_PaymentSuccessful"
         WebViewHeight="1000">
        </local:PaystackWebView>

    </StackLayout>

</ContentPage>
```
#### In the constructor of the page, after `InitializeComponent();` add
```

 JArray jarray = new JArray();
            dynamic customFieldsArray = new JArray();

            dynamic displayname = new JObject();
            displayname.display_name = "Mobile Number";
            displayname.variable_name = "mobile_number";
            displayname.@value = "+23480000000";

            customFieldsArray.Add(displayname);
            dynamic jobtitlefield = new JObject();
            jobtitlefield.display_name = "Job Title";
            jobtitlefield.variable_name = "job_title";
            jobtitlefield.@value = "Software Developer";

            customFieldsArray.Add(jobtitlefield);



            dynamic custom = new JObject();
            custom.custom_fields = customFieldsArray;

            dynamic product = new JObject();
            product.key = "pk_test_aaaaaaaaaaaaaaaaa";
            product.email = "me@you.com";
            product.amount = 490000m;
            product.subaccount = "ACCT_jiOpeieli";
            product.bearer = "subaccount";
            product.@ref = Guid.NewGuid();
            product.metadata = custom;

            hybridWebView.Data = product.ToString();


            hybridWebView.RegisterCloseAction(() => DisplayAlert("WebView Closed", "Close button clicked", "ok"));
            hybridWebView.RegisterCallBackAction(data => DisplayAlert("Alert", "Hello " + data, "OK"));

```
#### add the following event listners
```
 private void HybridWebView_PaymentClosed(object sender, string e)
        {
            DisplayAlert("WebView Closed", "Close button clicked event", "ok");
        }

       

        private void hybridWebView_PaymentSuccessful(object sender, string e)
        {
            DisplayAlert("Alert", "Hello " + e, "OK");
        }

```
