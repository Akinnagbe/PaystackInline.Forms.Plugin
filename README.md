# PaystackInline.Forms.Plugin
Paystack Inline Payment Plugin for Xamarin Forms and Windows

## NuGet
[Xam.Plugin.PaystackInline](https://www.nuget.org/packages/Xam.Plugin.PaystackInline/)

## Platforms
| Platform          | Version       | 
| -------------     |:-------------:| 
| Xamarin.iOS       | iOS 8+        | 
| Xamarin.Android   | API 15+       |   
| Windows 10 UWP    | in progress   |  

## Android Requirements
1. Add a reference to Mono.Android.Export, or a compiler error will result.
2. On Android Oreo ensure that the Android manifest sets the Target Android version to Automatic. Otherwise, the iframe won't load.

## Usage
   ### XAML
   ```
   <?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:Plugin.PaystackInline.Forms.Plugin;assembly=Plugin.PaystackInline.Forms.Plugin"
             x:Class="SampleApp.MainPage">

    <StackLayout>
        <local:PaystackWebView x:Name="hybridWebView"  HorizontalOptions="FillAndExpand" VerticalOptions="FillAndExpand"></local:PaystackWebView>

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