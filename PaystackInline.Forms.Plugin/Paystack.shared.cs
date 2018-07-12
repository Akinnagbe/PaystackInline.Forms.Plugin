using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Plugin.PaystackInline.Forms.Plugin
{
    public class PaystackWebView : View
    {
        Action<string> action;
        Action closeAction;
        public static readonly BindableProperty UriProperty = BindableProperty.Create(nameof(Uri), typeof(string), typeof(PaystackWebView), defaultValue: string.Empty, defaultBindingMode: BindingMode.TwoWay);
        /// <summary>
        ///  specifies the address of the web page to be loaded
        /// </summary>
        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }

        public static readonly BindableProperty DataProperty = BindableProperty.Create(nameof(Data), typeof(string), typeof(PaystackWebView), defaultValue: string.Empty, defaultBindingMode: BindingMode.TwoWay);

        public string Data
        {
            get { return (string)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }


        /// <summary>
        /// registers an Action with the control. The registered action will be invoked from JavaScript contained in the HTML file referenced through the Uri property.
        /// </summary>
        /// <param name="callback"></param>
        public void RegisterCallBackAction(Action<string> callback)
        {
            action = callback;
        }
        public void RegisterCloseAction(Action close)
        {
            closeAction = close;
        }
        /// <summary>
        /// method that removes the reference to the registered Action
        /// </summary>
        public void CleanUp()
        {
            action = null;
            closeAction = null;
        }

        /// <summary>
        /// method that invokes the registered Action. This method will be called from a custom renderer in each platform-specific project.
        /// </summary>
        /// <param name="data"></param>
        public void InvokeCallbackAction(string data)
        {
            if (action == null || data == null)
            {
                return;
            }
            action.Invoke(data);
        }
        public void InvokeCloseAction()
        {
            if (closeAction == null)
                return;
            closeAction.Invoke();
        }
    }



}
