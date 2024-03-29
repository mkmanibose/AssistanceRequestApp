﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace AssistanceRequestApp.ChatBot.Bots
{
    using AssistanceRequestApp.DL.Interface;
    using AssistanceRequestApp.Models.UserDefinedModels;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.AI.QnA;
    using Microsoft.Bot.Schema;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    public class EchoBot : ActivityHandler
    {
        public QnAMaker EchoBotQnA { get; private set; }
        public IConfiguration Configuration { get; }
        //private readonly List<QnA> QnAs;
        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<EchoBot> logger;

        /// <summary>
        /// Defines the requestDLRepository.
        /// </summary>
        private readonly IRequestDLRepository requestDLRepository;

        /// <summary>
        /// Defines the lstDetailRequestModels.
        /// </summary>
        private readonly List<DetailRequestModel> lstDetailRequestModels = new List<DetailRequestModel>();

        public EchoBot(QnAMakerEndpoint endpoint, IConfiguration configuration, ILogger<EchoBot> logger, IRequestDLRepository context)
        {
            EchoBotQnA = new QnAMaker(endpoint);
            Configuration = configuration;
            // QnAs = new List<QnA>();
            this.logger = logger;
            this.requestDLRepository = context;
            lstDetailRequestModels = requestDLRepository.GetAllRequests();
        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            await AccessQnAMaker(turnContext, cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            try
            {
                var welcomeText = "Hello and welcome!";
                foreach (var member in membersAdded)
                {
                    if (member.Id != turnContext.Activity.Recipient.Id)
                    {
                        await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Exception in OnMembersAddedAsync " + ex.Message);
                logger.LogError(ex.StackTrace);
            }
        }

        private string GetAnswers(string questionText, string relatedEnvironment, string natureOfRequest)
        {
            string searchAnswer = string.Empty;
            try
            {
                searchAnswer = Convert.ToString(lstDetailRequestModels.Where
                    (r => r.Status.Equals("Closed") && r.DateCompleted != null && r.ResolutionComments != null &&
                    r.RelatedEnvironment.Equals(relatedEnvironment) && r.NatureofRequest.Equals(natureOfRequest) &&
                    r.DescriptionofRequest.ToString().ToLower().Contains(questionText))
                    .OrderByDescending(o => o.CreatedDate).ThenByDescending(t => t.DateCompleted)
                    .Select(s => s.ResolutionComments).FirstOrDefault());
                //using (var sqlConnection = new SqlConnection(Configuration.GetValue<string>($"AzureDatabaseConnectionString")))
                //{
                //    sqlConnection.Open();
                //    var sqlQuery = @"SELECT TOP 1 ResolutionComments
                //                    FROM [dbo].Request
                //                    WHERE ResolutionComments IS NOT NULL
                //                    AND DateCompleted IS NOT NULL
                //                    AND [Status] = 'Closed'
                //                    AND RelatedEnvironment = @relatedEnvironment
                //                    AND NatureofRequest = @natureofRequest
                //                    AND DescriptionofRequest LIKE '%' + @filter + '%'
                //                    ORDER BY CreatedDate, DateCompleted DESC";
                //    var command = new SqlCommand(sqlQuery, sqlConnection);
                //    command.CommandType = CommandType.Text;
                //    command.Parameters.AddWithValue("@filter", questionText);
                //    command.Parameters.AddWithValue("@relatedEnvironment", relatedEnvironment);
                //    command.Parameters.AddWithValue("@natureofRequest", natureOfRequest);
                //    var result = command.ExecuteScalar();
                //    if (result != null && result != string.Empty)
                //    {
                //        return result.ToString();
                //    }
                //}
            }
            catch (Exception ex)
            {
                logger.LogError("Exception in GetAnswers " + ex.Message);
                logger.LogError(ex.StackTrace);
            }
            return searchAnswer;
        }

        private async Task AccessQnAMaker(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(turnContext.Activity.Text))
                {
                    // The actual call to the QnA Maker service.
                    var response = await EchoBotQnA.GetAnswersAsync(turnContext);
                    if (response != null && response.Length > 0)
                    {

                        //Below logic is to avoid keywords issue being searched in SQL for resolution
                        if (Startup.QnAs.Count() > 1)
                        {
                            var questionAndAnswerObject = Startup.QnAs[Startup.QnAs.Count - 1];
                            if (questionAndAnswerObject.Answer.Equals("Please enter your issue statement or request statement"))
                            {
                                var matches = Startup.QnAs.Where(x => x.Question.Equals(response.FirstOrDefault().Questions.First()));
                                if (matches.Count() >= 1)
                                {
                                    if (!string.IsNullOrWhiteSpace(matches.FirstOrDefault().Question))
                                    {
                                        if (!string.IsNullOrWhiteSpace(Startup.Environment) && !string.IsNullOrWhiteSpace(Startup.NatureOfRequest))
                                        {
                                            response = null;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (response != null && response.Length > 0)
                    {
                        //Adding the question, answer to model to maintain history
                        Startup.QnAs.Add(new QnA { Question = response.First().Questions.First(), Answer = response.First().Answer, LoggedTime = DateTime.Now });

                        if (Startup.QnAs.Count() > 1)
                        {
                            var QnAForEnvironment = Startup.QnAs.Last();
                            if (QnAForEnvironment.Answer.Equals("What is the nature of your request?"))
                            {
                                if (QnAForEnvironment.Question.Equals(".NET") || QnAForEnvironment.Question.Equals("Java") || QnAForEnvironment.Question.Equals("MainFrame") || QnAForEnvironment.Question.Equals("Python"))
                                {
                                    Startup.Environment = QnAForEnvironment.Question;
                                }
                            }

                            if (QnAForEnvironment.Answer.Equals("Please enter your issue statement or request statement"))
                            {
                                if (QnAForEnvironment.Question.Equals("Request Assistance") || QnAForEnvironment.Question.Equals("Reqest Information") || QnAForEnvironment.Question.Equals("Request Change") || QnAForEnvironment.Question.Equals("Suggest New Feature"))
                                {
                                    Startup.NatureOfRequest = QnAForEnvironment.Question;
                                }
                            }
                        }

                        //Below code is handling prompt type answers to display properly in chat window
                        // create http client to perform qna query
                        var followUpCheckHttpClient = new HttpClient();

                        // add QnAAuthKey to Authorization header
                        followUpCheckHttpClient.DefaultRequestHeaders.Add("Authorization", Configuration.GetValue<string>("QnAAuthKey"));

                        // construct the qna query url
                        var url = Configuration.GetValue<string>("QnAQueryUrl");

                        // post query
                        var checkFollowUpJsonResponse = await followUpCheckHttpClient.PostAsync(url, new StringContent("{\"question\":\"" + turnContext.Activity.Text + "\"}", Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();

                        // parse result
                        var followUpCheckResult = JsonConvert.DeserializeObject<FollowUpCheckResult>(checkFollowUpJsonResponse);

                        // initialize reply message containing the default answer
                        var reply = MessageFactory.Text(response[0].Answer);

                        if (followUpCheckResult.Answers.Length > 0 && followUpCheckResult.Answers[0].Context.Prompts.Length > 0)
                        {
                            // if follow-up check contains valid answer and at least one prompt, add prompt text to SuggestedActions using CardAction one by one
                            reply.SuggestedActions = new SuggestedActions();
                            reply.SuggestedActions.Actions = new List<CardAction>();
                            for (int i = 0; i < followUpCheckResult.Answers[0].Context.Prompts.Length; i++)
                            {
                                var promptText = followUpCheckResult.Answers[0].Context.Prompts[i].DisplayText;
                                reply.SuggestedActions.Actions.Add(new CardAction() { Title = promptText, Type = ActionTypes.ImBack, Value = promptText });
                            }
                        }
                        await turnContext.SendActivityAsync(reply, cancellationToken);
                    }
                    else
                    {
                        //In the below logic where sql db call happens when there is no match found in qna maker
                        if (Startup.QnAs.Count() > 1)
                        {
                            var questionAndAnswerObject = Startup.QnAs[Startup.QnAs.Count - 1];
                            if (questionAndAnswerObject.Answer.Equals("Please enter your issue statement or request statement"))
                            {
                                if (!string.IsNullOrWhiteSpace(Startup.Environment) && !string.IsNullOrWhiteSpace(Startup.NatureOfRequest))
                                {
                                    var answer = GetAnswers(turnContext.Activity.Text, Startup.Environment, Startup.NatureOfRequest);
                                    if (!string.IsNullOrWhiteSpace(answer))
                                    {
                                        var replyText = $"Here is resolution steps: {answer}";
                                        await turnContext.SendActivityAsync(MessageFactory.Text(replyText), cancellationToken);
                                        Startup.Environment = null;
                                        Startup.NatureOfRequest = null;
                                    }
                                    else
                                    {
                                        var defaultText = $"We could not find the suitable resolution to you, Please raise new request. Thanks";
                                        await turnContext.SendActivityAsync(MessageFactory.Text(defaultText, defaultText), cancellationToken);
                                    }
                                }
                                else
                                {
                                    var defaultText = $"We could not find the suitable resolution to you, Please raise new request. Thanks";
                                    await turnContext.SendActivityAsync(MessageFactory.Text(defaultText, defaultText), cancellationToken);
                                }
                            }
                            else
                            {
                                var defaultText = $"We could not find the suitable resolution to you, Please raise new request. Thanks";
                                await turnContext.SendActivityAsync(MessageFactory.Text(defaultText, defaultText), cancellationToken);
                            }
                        }
                        else
                        {
                            var defaultText = $"We could not find the suitable resolution to you, Please raise new request. Thanks";
                            await turnContext.SendActivityAsync(MessageFactory.Text(defaultText, defaultText), cancellationToken);
                        }
                    }
                }
                else
                {
                    var defaultText = $"Thanks for reaching out to us, We will get back to you shortly";
                    await turnContext.SendActivityAsync(MessageFactory.Text(defaultText, defaultText), cancellationToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Exception in AccessQnAMaker " + ex.Message);
                logger.LogError(ex.StackTrace);
            }
        }
    }

    class FollowUpCheckResult
    {
        [JsonProperty("answers")]
        public FollowUpCheckQnAAnswer[] Answers
        {
            get;
            set;
        }
    }

    class FollowUpCheckQnAAnswer
    {
        [JsonProperty("context")]
        public FollowUpCheckContext Context
        {
            get;
            set;
        }
    }

    class FollowUpCheckContext
    {
        [JsonProperty("prompts")]
        public FollowUpCheckPrompt[] Prompts
        {
            get;
            set;
        }
    }

    class FollowUpCheckPrompt
    {
        [JsonProperty("displayText")]
        public string DisplayText
        {
            get;
            set;
        }
    }

    class QnA
    {
        public string Question
        {
            get;
            set;
        }
        public string Answer
        {
            get;
            set;
        }
        public DateTime LoggedTime
        {
            get;
            set;
        }
    }
}
