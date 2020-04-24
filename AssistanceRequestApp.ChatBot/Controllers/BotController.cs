// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace AssistanceRequestApp.ChatBot.Controllers
{
    using AssistanceRequestApp.DL.Interface;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Integration.AspNet.Core;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;
    // This ASP Controller is created to handle a request. Dependency Injection will provide the Adapter and IBot
    // implementation at runtime. Multiple different IBot implementations running at different endpoints can be
    // achieved by specifying a more specific type for the bot constructor argument.
    [Route("api/messages")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly IBotFrameworkHttpAdapter Adapter;
        private readonly IBot Bot;
        private readonly ILogger<BotController> logger;
        private readonly IRequestDLRepository requestDLRepository;
        public BotController(IBotFrameworkHttpAdapter adapter, IBot bot, ILogger<BotController> logger, IRequestDLRepository context)
        {
            Adapter = adapter;
            Bot = bot;
            this.logger = logger;
            this.requestDLRepository = context;
        }

        [HttpPost, HttpGet]
        public async Task PostAsync()
        {
            // Delegate the processing of the HTTP POST to the adapter.
            // The adapter will invoke the bot.
            await Adapter.ProcessAsync(Request, Response, Bot);
        }
    }
}
