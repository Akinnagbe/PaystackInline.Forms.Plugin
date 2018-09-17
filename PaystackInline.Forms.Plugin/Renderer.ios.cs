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
    [Preserve(AllMembers = true)]
    public class PaystackWebViewRenderer : ViewRenderer<PaystackWebView, WKWebView>, IWKScriptMessageHandler
    {
        const string PaymentJavaScriptFunction = "function invokeCSharpAction(data){window.webkit.messageHandlers.invokePayAction.postMessage(data);}";
        const string ClosePaymentJavaScriptFunction = "function invokeCSharpCloseAction(data){window.webkit.messageHandlers.invokeCloseAction.postMessage(data);}";

        WKUserContentController userController;

        protected override void OnElementChanged(ElementChangedEventArgs<PaystackWebView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                var webviewElement = (PaystackWebView)Element;
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
                var paystackWebView = e.OldElement as PaystackWebView;
                paystackWebView.CleanUp();
            }
            if (e.NewElement != null)
            {
              //  string content = LoadHtmlString();
              //  Control.LoadHtmlString(content, baseUrl: null);
                 string fileName = Path.Combine(NSBundle.MainBundle.BundlePath, "Content/paystack.html");
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

        internal string LoadHtmlString()
        {
            var html = new StringBuilder();
            html.Append("<html>");
            html.AppendLine();
            html.Append("<body>");
            html.AppendLine();
            html.Append("<form>");
            html.AppendLine();
            html.Append("<script src=\"http://code.jquery.com/jquery-2.1.4.min.js\"></script>");
            html.AppendLine();
            html.Append("<script src=\"https://js.paystack.co/v1/inline.js\"></script>");
            html.AppendLine();
            html.Append("</form>");
            html.AppendLine();
            html.Append("<script>");
            html.AppendLine();
            html.Append("function payWithPaystack(jobj) {");
            html.AppendLine();
            html.Append(" jobj.callback= function(resp) {");
            html.AppendLine();
            html.Append("invokeCSharpAction(resp.reference);");
            html.AppendLine();
            html.Append("}");
            html.AppendLine();
            html.Append("jobj.onClose = function () {");
            html.AppendLine();
            html.Append("invokeCSharpCloseAction('');");
            html.AppendLine();
            html.Append("}");
            html.AppendLine();
            html.Append(" var handler = PaystackPop.setup(jobj);");
            html.AppendLine();
            html.Append("handler.openIframe();");
            html.AppendLine();
            html.Append(" }");
            html.AppendLine();
            html.Append("</script>");
            html.AppendLine();
            html.Append("</body>");
            html.AppendLine();
            html.Append("</html>");


            return html.ToString();
        }

    }
}
