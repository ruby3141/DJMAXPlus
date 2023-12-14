using DJMAXPlus.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJMAXPlus.Hook
{
    /// <summary>
    /// Helper Class to invoke to the Thread with Message Loop (a.k.a. UI Thread in Winform)
    /// </summary>
    public static class InvokeHelper
    {
        private static object _lock = new object();
        private static Thread? mainThread;

        private static Control? invokerControl;


        public static object Invoke(Delegate method, params object?[]? args)
        {
            if (invokerControl is null)
                return null;

            return invokerControl.Invoke(method, args);
        }

        public static void Invoke(Action method) => Invoke(method, null);

        public static object Invoke(Delegate method) => Invoke(method, null);

        public static T Invoke<T>(Func<T> method) => (T)Invoke(method, null);

        public static IAsyncResult BeginInvoke(Delegate method, params object?[]? args)
        {
            if (invokerControl is null)
                return Task.FromException(new NullReferenceException("invokerControl is not generated."));

            return invokerControl.BeginInvoke(method, args);
        }
        public static IAsyncResult BeginInvoke(Action method) => BeginInvoke(method, null);

        public static IAsyncResult BeginInvoke(Delegate method) => BeginInvoke(method, null);

        /// <summary>
        /// 
        /// </summary>
        public static void UseInvokerHelper()
        {
            invokerControl = new Control();
            invokerControl.CreateControl();
        }

        /// <summary>
        /// Run blank Application to generate a Message Loop.
        /// [CAUTION] If there's another Message Loop on your application, it can cause unintended issues.
        /// </summary>
        public static void GenerateMainThread()
        {
            lock (_lock)
            {
                if (mainThread != null)
                    return;

                mainThread = new Thread(()=> {UseInvokerHelper(); Application.Run()});
                mainThread.Start();
            }
        }

        /// <summary>
        /// Terminate Application and clean up thread.
        /// [CAUTION] If there's another Message Loop on your application, it can cause unintended issues.
        /// </summary>
        public static void TerminateMainThread()
        {
            lock (_lock)
            {
                if (mainThread == null)
                    return;

                Application.Exit();
                mainThread.Join();
                mainThread = null;
            }
        }
    }
}
