using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Webkit;
using Java.Interop;
using Plugin.PaystackInline.Forms.Plugin;
using Plugin.PaystackInline.Forms.Plugin.Droid;
using System;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PaystackWebView), typeof(PaystackWebViewRenderer))]
namespace Plugin.PaystackInline.Forms.Plugin.Droid
{
    [Preserve(AllMembers = true)]
    public class PaystackWebViewRenderer : ViewRenderer<PaystackWebView, Android.Webkit.WebView>
    {
        private const string CallBackJavaScriptFunction = "function invokeCSharpAction(data){jsBridge.invokeCallbackAction(data);}";
        private const string CloseJavaScriptFunction = "function invokeCSharpCloseAction(){jsBridge.invokeCloseAction();}";
        private Context _context;

        public PaystackWebViewRenderer(Context context) : base(context)
        {
            _context = context;
        }
        protected override void OnElementChanged(ElementChangedEventArgs<PaystackWebView> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
            {
                var webview = new Android.Webkit.WebView(_context);
                webview.Settings.JavaScriptEnabled = true;
                SetNativeControl(webview);
            }
            if (e.OldElement != null)
            {
                //unsubscribe from events
                Control.RemoveJavascriptInterface("jsBridge");
                var hybridWebView = e.OldElement as PaystackWebView;
                hybridWebView.CleanUp();
            }
            if (e.NewElement != null)
            {
                //subscribe to Events
                var webviewElement = Element;
                Control.AddJavascriptInterface(new JSBridge(this, _context), "jsBridge");
                string content = LoadHtmlString();
                Control.SetWebViewClient(new CustomWebViewClient(webviewElement.Data));
                // Control.LoadUrl("file:///android_asset/paystack.html");


                Control.LoadDataWithBaseURL("", content, "text/html", "UTF-8", null);
                InjectJS(CallBackJavaScriptFunction);
                InjectJS(CloseJavaScriptFunction);
            }
        }

        private void InjectJS(string script)
        {
            if (Control != null)
            {
                Control.LoadUrl(string.Format("javascript: {0}", script));
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
            html.Append("invokeCSharpCloseAction();");
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

    internal class CustomWebViewClient : WebViewClient
    {
        private string Record = "";
        string logTag = "Plugin.PaystackInline.Forms";
        public CustomWebViewClient(string record)
        {
            Record = record;
        }
        public override void OnPageFinished(Android.Webkit.WebView view, string url)
        {
            base.OnPageFinished(view, url);

            view.LoadUrl(string.Format("javascript:payWithPaystack({0})", Record));
            Android.Util.Log.Info(logTag,$"OnPageFinished: {url}");
        }
        public override void OnPageStarted(Android.Webkit.WebView view, string url, Bitmap favicon)
        {
            base.OnPageStarted(view, url, favicon);
            Android.Util.Log.Info(logTag, $"OnPageStarted: {url}");
        }
        public override void OnReceivedError(Android.Webkit.WebView view, IWebResourceRequest request, WebResourceError error)
        {
            base.OnReceivedError(view, request, error);
            Android.Util.Log.Error(logTag, $"OnReceivedError: {error.Description}");
        }
        [Obsolete]
        public override void OnReceivedError(Android.Webkit.WebView view, [GeneratedEnum] ClientError errorCode, string description, string failingUrl)
        {
            base.OnReceivedError(view, errorCode, description, failingUrl);
        }

        public override void OnReceivedHttpError(Android.Webkit.WebView view, IWebResourceRequest request, WebResourceResponse errorResponse)
        {
            base.OnReceivedHttpError(view, request, errorResponse);
            Android.Util.Log.Error(logTag, $"OnReceivedError: {errorResponse.ReasonPhrase}");
        }
    }

    internal class JSBridge : Java.Lang.Object
    {
        private readonly WeakReference<PaystackWebViewRenderer> hybridWebViewRenderer;
        private Context _context;
        public JSBridge(PaystackWebViewRenderer hybridRenderer, Context context)
        {
            hybridWebViewRenderer = new WeakReference<PaystackWebViewRenderer>(hybridRenderer);
            _context = context;
        }

        [JavascriptInterface]
        [Export("invokeCallbackAction")]
        public void InvokeAction(string data)
        {
            PaystackWebViewRenderer hybridRenderer;

            if (hybridWebViewRenderer != null && hybridWebViewRenderer.TryGetTarget(out hybridRenderer))
            {
                hybridRenderer.Element.InvokeCallbackAction(data);
            }
        }
        [JavascriptInterface]
        [Export("invokeCloseAction")]
        public void InvokeCloseAction()
        {
            PaystackWebViewRenderer hybridRenderer;

            if (hybridWebViewRenderer != null && hybridWebViewRenderer.TryGetTarget(out hybridRenderer))
            {
                hybridRenderer.Element.InvokeCloseAction();
            }
        }
    }
}
