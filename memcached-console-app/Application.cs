using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Enyim.Caching;

namespace memcached_console_app
{
    public class Application : IDisposable
    {

        public delegate void ContextualInitializer(ContainerBuilder builder);

        public IServiceProvider Container { get; private set; }

        public IContainer Registry { get; private set; }
        public Application(
            IServiceCollection initialCollection,
            ContextualInitializer contextualInitializer = null)
        {
            Container = CreateContainer(initialCollection, contextualInitializer, out var reg);
 //           Log = Container.GetService<ILogger<Application>>();
            Registry = reg;
//            InitializeHumanizerDictionary();

        }

        private AutofacServiceProvider CreateContainer(
           IServiceCollection initialCollection,
           ContextualInitializer contextualInitializer,
           out IContainer registry)
        {
            var builder = new ContainerBuilder();


            initialCollection.AddEnyimMemcached();

            builder.RegisterType<IMemcachedClient>()
                .As<IMemcachedClient>()
                .AsSelf()
                .InstancePerLifetimeScope()
                .InstancePerDependency()
                .PropertiesAutowired();

            builder.RegisterInstance(this);

            contextualInitializer?.Invoke(builder);

            registry = builder.Build();

            var provider = new AutofacServiceProvider(registry);

            return provider;
        }

        public void Dispose()
        {
            // 循環してるのを破棄しておく
            Container = null;
        }
    }
}
