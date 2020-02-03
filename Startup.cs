using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using GraphiQl;
using ChatSchema;
using GraphQL.Server;

#if !NETCOREAPP2_2
using Microsoft.AspNetCore.Server.Kestrel.Core;
#endif

namespace chatroom
{
    public class Startup
    {
#if NETCOREAPP2_2
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
#else
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
#endif
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

#if NETCOREAPP2_2
        public IHostingEnvironment Environment { get; }
#else
        public IWebHostEnvironment Environment { get; }
#endif

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
#if NETCOREAPP3_0
            // Workaround until GraphQL can swap off Newtonsoft.Json and onto the new MS one.
            // Depending on whether you're using IIS or Kestrel, the code required is different
            // See: https://github.com/graphql-dotnet/graphql-dotnet/issues/1116
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
#endif

            services
                .AddSingleton<IChat, ChatSchema.Chat>()
                .AddGraphQL(options =>
                {
                    options.EnableMetrics = true;
                    options.ExposeExceptions = Environment.IsDevelopment();
                })
                .AddWebSockets()
                .AddDataLoader()
                .AddGraphTypes(ServiceLifetime.Scoped);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseGraphQL<ChatSchema.ChatSchema>();
            app.UseGraphiQl("/graphql");
            app.UseMvc();
        }
    }
}
