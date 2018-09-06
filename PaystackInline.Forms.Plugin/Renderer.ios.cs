using System;
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
        const string PaymentJavaScriptFunction = "function invokePaymentAction(data){window.webkit.messageHandlers.invokePayAction.postMessage(data);}";
        const string ClosePaymentJavaScriptFunction = "function invokeClosePaymentAction(data){window.webkit.messageHandlers.invokeCloseAction.postMessage(data);}";
        WKUserContentController userController;

        protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                var webviewElement = (HybridWebView)Element;
                userController = new WKUserContentController();
                var paymentscript = new WKUserScript(new NSString(PaymentJavaScriptFunction), WKUserScriptInjectionTime.AtDocumentEnd, false);
                var closeScript = new WKUserScript(new NSString(ClosePaymentJavaScriptFunction), WKUserScriptInjectionTime.AtDocumentEnd, false);
                var paystackScript = new WKUserScript(new NSString($"payWithPaystack({webviewElement.Data})"), WKUserScriptInjectionTime.AtDocumentEnd, false);
                userController.AddUserScript(paymentscript);
                userController.AddUserScript(closeScript);
                userController.AddUserScript(paystackScript);
                userController.AddScriptMessageHandler(this, "invokePayAction");
                userController.AddScriptMessageHandler(this, "invokeCloseAction");

                var config = new WKWebViewConfiguration { UserContentController = userController };
                var webView = new WKWebView(Frame, config);
                SetNativeControl(webView);
            }
            if (e.OldElement != null)
            {
                userController.RemoveAllUserScripts();
                userController.RemoveScriptMessageHandler("invokePayAction");
                userController.RemoveScriptMessageHandler("invokeCloseAction");
                var hybridWebView = e.OldElement as HybridWebView;
                hybridWebView.CleanUp();
            }
            if (e.NewElement != null)
            {
              //  string fileName = Path.Combine(NSBundle.MainBundle.BundlePath, string.Format("Content/{0}", Element.Uri));
                string fileName = Path.Combine(NSBundle.MainBundle.BundlePath, "paystack.html");
                Control.LoadRequest(new NSUrlRequest(new NSUrl(fileName, false)));
            }
        }

        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            if (message.Name == "invokePayAction")
            {
                Element.InvokeCallbackAction(message.Body.ToString());
            }
            if (message.Name == "invokeCloseAction")
            {
                Element.InvokeCloseAction();
            }

        }

    }
}
