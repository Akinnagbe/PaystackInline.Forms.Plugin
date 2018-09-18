using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SampleApp
{
    public class HybridWebView : View
    {

        Action<string> action;
        Action closeAction;
        public static readonly BindableProperty UriProperty = BindableProperty.Create(
            propertyName: "Uri",
            returnType: typeof(string),
            declaringType: typeof(HybridWebView),
            defaultValue: default(string));

        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }

        public void RegisterAction(Action<string> callback)
        {
            action = callback;
        }

        public void Cleanup()
        {
            action = null;
        }

        public void InvokeAction(string data)
        {
            if (action == null || data == null)
            {
                return;
            }
            action.Invoke(data);
        }

        public static readonly BindableProperty DataProperty = BindableProperty.Create(nameof(Data), typeof(string), typeof(HybridWebView), defaultValue: string.Empty, defaultBindingMode: BindingMode.TwoWay);

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
