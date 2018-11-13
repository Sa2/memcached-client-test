using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace memcached_console_app
{
    class AppContext
    {
        internal static Application App { get; private set; }

        public AppContext()
        {
            try
            {
                App = new Application(new ServiceCollection(), builder =>
                {
                    //builder.RegisterServerDomainServiceWithAutowiring();
                });

            }
            catch (Exception err)
            {
                var msg = err.Message;
            }
        }

        internal static T GetService<T>()
        {
            Debug.Assert(App != null, "初期化前に実行することはできません");
            return App.Container.GetRequiredService<T>();
        }
    }
}
