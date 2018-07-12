using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Webkit;
using Java.Interop;
using Plugin.PaystackInline.Forms.Plugin;
using Plugin.PaystackInline.Forms.Plugin.Droid;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PaystackWebView), typeof(PaystackWebViewRenderer))]
namespace Plugin.PaystackInline.Forms.Plugin.Droid
{
    [Preserve(AllMembers = true)]
    public class PaystackWebViewRenderer : ViewRenderer<PaystackWebView, Android.Webkit.WebView>
    {
        const string CallBackJavaScriptFunction = "function invokeCSharpAction(data){jsBridge.invokeCallbackAction(data);}";
        const string CloseJavaScriptFunction = "function invokeCSharpCloseAction(){jsBridge.invokeCloseAction();}";

        Context _context;
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
                this.SetNativeControl(webview);
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
                var webviewElement = (PaystackWebView)Element;
                Control.AddJavascriptInterface(new JSBridge(this), "jsBridge");
                Control.SetWebViewClient(new CustomWebViewClient(webviewElement.Data));
                
                Control.LoadUrl(string.Format("file:///android_asset/Content/{0}", Element.Uri));
                InjectJS(CallBackJavaScriptFunction);
                InjectJS(CloseJavaScriptFunction);
            }
        }

        void InjectJS(string script)
        {
            if (Control != null)
            {
                Control.LoadUrl(string.Format("javascript: {0}", script));
            }
        }

    }

    class CustomWebViewClient : WebViewClient
    {
        string Record = "";
        public CustomWebViewClient(string record)
        {
            Record = record;
        }
        public override void OnPageFinished(Android.Webkit.WebView view, string url)
        {
            base.OnPageFinished(view, url);

            view.LoadUrl(string.Format("javascript:payWithPaystack({0})", Record));
        }
        public override void OnPageStarted(Android.Webkit.WebView view, string url, Bitmap favicon)
        {
            base.OnPageStarted(view, url, favicon);
        }
    }

     class JSBridge : Java.Lang.Object
    {
        readonly WeakReference<PaystackWebViewRenderer> hybridWebViewRenderer;

        public JSBridge(PaystackWebViewRenderer hybridRenderer)
        {
            hybridWebViewRenderer = new WeakReference<PaystackWebViewRenderer>(hybridRenderer);
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
