using Microsoft.VisualStudio.TestTools.UnitTesting;
using DJMAXPlus.Hook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJMAXPlus.Hook.Tests
{
    [TestClass()]
    public class InvokeHelperTests
    {
        [TestInitialize()]
        public void Init()
        {
            InvokeHelper.GenerateMainThread();
        }

        [TestCleanup()]
        public void Cleanup()
        {
            InvokeHelper.TerminateMainThread();
        }

        [MTAThread]
        [TestMethod()]
        public void InvokeTest()
        {
            Console.WriteLine($"Message From Test Runner Thread: {Thread.CurrentThread.ManagedThreadId}");
            InvokeHelper.Invoke(() =>
            {
                Console.WriteLine($"Message From Main Thread: {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(1000);
                Console.WriteLine($"Message From Main Thread: {Thread.CurrentThread.ManagedThreadId}");
            });
            Console.WriteLine($"Message From Test Runner Thread: {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(3000);
        }

        [MTAThread]
        [TestMethod()]
        public void BeginInvokeTest()
        {
            Console.WriteLine($"Message From Test Runner Thread: {Thread.CurrentThread.ManagedThreadId}");
            InvokeHelper.BeginInvoke(() =>
            {
                Console.WriteLine($"Message From Main Thread: {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(1000);
                Console.WriteLine($"Message From Main Thread: {Thread.CurrentThread.ManagedThreadId}");
            });
            Console.WriteLine($"Message From Test Runner Thread: {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}