using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Envoy.Sample.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<Dispatcher>()
                      .As<IDispatchCommands>()
                      .As<IDispatchEvents>()
                      .As<IDispatchRequests>();

            builder.RegisterType<Executor>()
                .As<IExecuteCommands>()
                .As<IExecuteEvents>()
                .As<IExecuteRequests>();

            builder.RegisterType<TraceLogger>()
                .As<ILogger>();

            builder.RegisterType<AutofacAdaptor>()
                .As<IResolver>();

            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
               .Where(t => t.GetTypeInfo().ImplementedInterfaces.Any(i => i.IsGenericType &&
                (
                    i.GetGenericTypeDefinition() == typeof(IHandleCommand<>) ||
                    i.GetGenericTypeDefinition() == typeof(IHandleRequest<,>) ||
                    i.GetGenericTypeDefinition() == typeof(IHandleEvent<>)
                )))
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
