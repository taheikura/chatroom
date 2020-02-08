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
using GraphQL.Server;
using GraphQL.Server.Ui.GraphiQL;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using ChatGraphQL;
using nsChatSchema;
using GraphQL;

namespace chatroom
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddSingleton<IChat, nsChatSchema.Chat>();
            services.AddScoped<ChatSchema>();
            services.AddScoped<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));
            services.AddGraphQL(options =>
            {
                options.EnableMetrics = false;
                options.ExposeExceptions = this.Environment.IsDevelopment();
            })
            .AddWebSockets()
            .AddDataLoader();
            services.AddLogging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // This will enable WebSockets in Asp.Net core
            app.UseWebSockets();
            // Enable endpoint for websockets (subscriptions)
            app.UseGraphQLWebSockets<ChatSchema>("/graphql");
            // Enable endpoint for querying
            app.UseGraphQL<ChatSchema>("/graphql");
            // This will enable GraphiQL UI for testing the queries
            app.UseGraphiQLServer(new GraphiQLOptions());
        }
    }
}
