﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using Plugin.PaystackInline.Forms.Plugin;
using Plugin.PaystackInline.Forms.Plugin.iOS;
using UIKit;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PaystackWebView), typeof(PaystackWebViewRenderer))]
namespace Plugin.PaystackInline.Forms.Plugin.iOS
{
   public class PaystackWebViewRenderer : ViewRenderer<PaystackWebView, WKWebView>, IWKScriptMessageHandler
    {
        const string JavaScriptFunction = "function invokeCSharpAction(data){window.webkit.messageHandlers.invokeAction.postMessage(data);}";
        WKUserContentController userController;

        protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                userController = new WKUserContentController();
                var script = new WKUserScript(new NSString(JavaScriptFunction), WKUserScriptInjectionTime.AtDocumentEnd, false);
                userController.AddUserScript(script);
                userController.AddScriptMessageHandler(this, "invokeAction");

                var config = new WKWebViewConfiguration { UserContentController = userController };
                var webView = new WKWebView(Frame, config);
                SetNativeControl(webView);
            }
            if (e.OldElement != null)
            {
                userController.RemoveAllUserScripts();
                userController.RemoveScriptMessageHandler("invokeAction");
                var hybridWebView = e.OldElement as HybridWebView;
                hybridWebView.CleanUp();
            }
            if (e.NewElement != null)
            {
                string fileName = Path.Combine(NSBundle.MainBundle.BundlePath, string.Format("Content/{0}", Element.Uri));
                Control.LoadRequest(new NSUrlRequest(new NSUrl(fileName, false)));
            }
        }

        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            Element.InvokeCallbackAction(message.Body.ToString());
        }
    }
}
