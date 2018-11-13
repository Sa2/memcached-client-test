using Autofac;
using Autofac.Extensions.DependencyInjection;
using Enyim.Caching;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace memcached_console_app
{
    class Program
    {
        protected static Application App { get; private set; }
        private static bool isSignal;

        private static void ConsoleApplicationDispose(object obj, ConsoleCancelEventArgs args)
        {
            isSignal = true;
            Console.WriteLine("Finish");
        }

        public static int Main(string[] args)
        {
            try
            {
                var appContext = new AppContext();
                isSignal = false;

                Console.WriteLine("Run");
                //var memcachedTest = new MemcachedTest();

                Console.CancelKeyPress += new ConsoleCancelEventHandler(ConsoleApplicationDispose);

                while (!isSignal)
                {
                    System.Threading.Thread.Sleep(1000);
                }

                return 0;
            }
            catch (Exception e)
            {
                if (e is TargetInvocationException)
                {
                    e = e.InnerException ?? e;
                }
                //AppLog.Error("処理が異常終了しました", e);
                Console.WriteLine("処理が異常終了しました");
                return -1;
            }
            finally
            {
                Console.WriteLine("Application finish.");
                //AppLog.Debug("Application finish.");
            }
        }

    }
}
