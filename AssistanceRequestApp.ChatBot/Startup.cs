// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using AssistanceRequestApp.ChatBot.Bots;
using AssistanceRequestApp.ChatBot.EchoBot;
using AssistanceRequestApp.DL.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace AssistanceRequestApp.ChatBot
{
    public class Startup
    {
        private const string BotOpenIdMetadataKey = "BotOpenIdMetadata";
        internal static List<QnA> QnAs = new List<QnA>();
        internal static string Environment = null;
        internal static string NatureOfRequest = null;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        [System.Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Create the Bot Framework Adapter.
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            _ = services.AddTransient<IBot, Bots.EchoBot>();
            services.AddDbContext<AssistanceRequestAppDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("AzureDatabaseConnectionString")));
            services.AddTransient<DL.Interface.IRequestDLRepository, DL.RequestDLRepository>();
            // Create QnAMaker endpoint as a singleton
            services.AddSingleton(new QnAMakerEndpoint
            {
                KnowledgeBaseId = Configuration.GetValue<string>($"QnAKnowledgebaseId"),
                EndpointKey = Configuration.GetValue<string>($"QnAAuthKey"),
                Host = Configuration.GetValue<string>($"QnAEndpointHostName")
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [System.Obsolete]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/myapp-{Date}.txt");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseWebSockets();
        }
    }
}
