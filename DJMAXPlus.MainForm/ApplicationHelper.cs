namespace DJMAXPlus.MainForm
{
    public static class ApplicationHelper
    {
        private static object _applicationThreadLock = new object();
        private static Thread? applicationThread;
        private static Control? invokerControl;

        /// <summary>
        /// Make new Thread and run Application Main Loop on there.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when Application main loop is already running</exception>
        public static void Run(ApplicationContext applicationContext)
        {
            lock (_applicationThreadLock)
            {
                if (applicationThread != null)
                    throw new InvalidOperationException("Application already running");

                applicationThread = new Thread((ctx) => { invokerControl = new Control(); invokerControl.CreateControl(); Application.Run((ApplicationContext)ctx!); });
                applicationThread.Start(applicationContext);
            }
        }



        /// <summary>
        /// Make Application Exit() and join Main Loop Thread
        /// </summary>
        public static void Stop()
        {
            lock (_applicationThreadLock)
            {
                if (applicationThread != null)
                {
                    Application.Exit();
                    applicationThread.Join();

                    applicationThread = null;
                }
            }
        }

        public static object Invoke(Delegate method, params object?[]? args)
        {
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
    }
}
