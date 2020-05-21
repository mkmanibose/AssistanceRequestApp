// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.BotBuilderSamples.Bots
{
    using AssistanceRequestApp.DL.Interface;
    using AssistanceRequestApp.Models.UserDefinedModels;
    using global::EchoBot.Models;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.AI.QnA;
    using Microsoft.Bot.Schema;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    /// <summary>
    /// Defines the <see cref="EchoBot" />.
    /// </summary>
    public class EchoBot : ActivityHandler
    {
        /// <summary>
        /// Gets the EchoBotQnA.
        /// </summary>
        public QnAMaker EchoBotQnA { get; private set; }

        /// <summary>
        /// Gets the Configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

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

        /// <summary>
        /// Defines the inProgressStatuses.
        /// </summary>
        private readonly List<string> inProgressStatuses = new List<string>() { "InProgress", "Open", "Submitted" };

        /// <summary>
        /// Defines the thanksRegards.
        /// </summary>
        private readonly string thanksRegards = Environment.NewLine + "Thanks & Regards!" + Environment.NewLine + "Frameworks & Services Team";

        /// <summary>
        /// Initializes a new instance of the <see cref="EchoBot"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint<see cref="QnAMakerEndpoint"/>.</param>
        /// <param name="configuration">The configuration<see cref="IConfiguration"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger{EchoBot}"/>.</param>
        /// <param name="context">The context<see cref="IRequestDLRepository"/>.</param>
        public EchoBot(QnAMakerEndpoint endpoint, IConfiguration configuration, ILogger<EchoBot> logger, IRequestDLRepository context)
        {
            EchoBotQnA = new QnAMaker(endpoint);
            Configuration = configuration;
            this.logger = logger;
            this.requestDLRepository = context;
            lstDetailRequestModels = requestDLRepository.GetAllRequests();
        }

        /// <summary>
        /// The OnMessageActivityAsync.
        /// </summary>
        /// <param name="turnContext">The turnContext<see cref="ITurnContext{IMessageActivity}"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            await AccessQnAMaker(turnContext, cancellationToken);
        }

        /// <summary>
        /// The OnMembersAddedAsync.
        /// </summary>
        /// <param name="membersAdded">The membersAdded<see cref="IList{ChannelAccount}"/>.</param>
        /// <param name="turnContext">The turnContext<see cref="ITurnContext{IConversationUpdateActivity}"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            try
            {
                string welcomeText = "Hi, How May Help you!";
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

        /// <summary>
        /// The GetAnswers.
        /// </summary>
        /// <param name="questionText">The questionText<see cref="string"/>.</param>
        /// <param name="relatedEnvironment">The relatedEnvironment<see cref="string"/>.</param>
        /// <param name="natureOfRequest">The natureOfRequest<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private string GetAnswers(string questionText, string relatedEnvironment, string natureOfRequest)
        {
            string searchAnswer = string.Empty;
            try
            {
                searchAnswer = Convert.ToString(lstDetailRequestModels.Where
                    (r => r.Status.Equals("Closed") && r.DateCompleted != null && r.ResolutionComments != null &&
                    r.RelatedEnvironment.ToString().ToLower().Equals(relatedEnvironment.ToLower()) && r.NatureofRequest.ToString().ToLower().Equals(natureOfRequest.ToLower()) && r.DescriptionofRequest.ToString().ToLower().Contains(questionText.ToLower()))
                    .OrderByDescending(o => o.CreatedDate).ThenByDescending(t => t.DateCompleted)
                    .Select(s => s.ResolutionComments).FirstOrDefault());
            }
            catch (Exception ex)
            {
                logger.LogError("Exception in GetAnswers " + ex.Message);
                logger.LogError(ex.StackTrace);
            }
            return searchAnswer;
        }

        /// <summary>
        /// The GetInProgressRequests.
        /// </summary>
        /// <param name="questionText">The questionText<see cref="string"/>.</param>
        /// <param name="relatedEnvironment">The relatedEnvironment<see cref="string"/>.</param>
        /// <param name="natureOfRequest">The natureOfRequest<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private string GetInProgressRequests(string questionText, string relatedEnvironment, string natureOfRequest)
        {
            string searchAnswer = string.Empty;
            try
            {
                searchAnswer = Convert.ToString(lstDetailRequestModels.Where
                   (r => inProgressStatuses.Contains(r.Status)
                   && r.DateCompleted == null && r.ResolutionComments == null
                    && r.RelatedEnvironment.ToString().ToLower().Equals(relatedEnvironment.ToLower()) && r.NatureofRequest.ToString().ToLower().Equals(natureOfRequest.ToLower()) && r.DescriptionofRequest.ToString().ToLower().Contains(questionText.ToLower()))
                    .OrderByDescending(o => o.CreatedDate)
                    .Select(s => s.DescriptionofRequest).FirstOrDefault());
            }
            catch (Exception ex)
            {
                logger.LogError("Exception in GetInProgressRequests " + ex.Message);
                logger.LogError(ex.StackTrace);
            }
            return searchAnswer;
        }

        /// <summary>
        /// The AccessQnAMaker.
        /// </summary>
        /// <param name="turnContext">The turnContext<see cref="ITurnContext{IMessageActivity}"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task AccessQnAMaker(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(turnContext.Activity.Text))
                {
                    // The actual call to the QnA Maker service.
                    var response = await EchoBotQnA.GetAnswersAsync(turnContext);
                    if (response.Length <= 0)
                    {
                        var applicationObject = Startup.QnAs.Last();
                        if (applicationObject.Answer.Equals("Please enter your application name"))
                        {
                            Startup.QnAs.Add(new QnA { Question = "ApplicationName", Answer = turnContext.Activity.Text, LoggedTime = DateTime.Now });
                        }
                    }
                    else
                    {
                        if (Startup.QnAs.Count > 0)
                        {
                            var applicationObject = Startup.QnAs.Last();
                            if (applicationObject.Answer.Equals("Please enter your application name"))
                            {
                                Startup.QnAs.Add(new QnA { Question = "ApplicationName", Answer = turnContext.Activity.Text, LoggedTime = DateTime.Now });
                                response = null;
                            }
                            if (applicationObject.Answer.Equals("Please enter your issue statement or request statement"))
                            {
                                response = null;
                            }
                        }
                    }

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
                                if (QnAForEnvironment.Question.Equals("Request Assistance") || QnAForEnvironment.Question.Equals("Request Information") || QnAForEnvironment.Question.Equals("Request Change") || QnAForEnvironment.Question.Equals("Suggest New Feature"))
                                {
                                    Startup.NatureOfRequest = QnAForEnvironment.Question;
                                }
                            }

                            if (QnAForEnvironment.Answer.Equals("Is your application New or Old?"))
                            {
                                if (QnAForEnvironment.Question.Equals("1) .NET") || QnAForEnvironment.Question.Equals("2) Java") || QnAForEnvironment.Question.Equals("3) Python") || QnAForEnvironment.Question.Equals("4) Mainframe"))
                                {
                                    Startup.KickStartRequest.DomainName = QnAForEnvironment.Question.Remove(0, 3);
                                }
                            }

                            if (QnAForEnvironment.Answer.Equals("Please enter your application name"))
                            {
                                if (QnAForEnvironment.Question.Equals("Old") || QnAForEnvironment.Question.Equals("New"))
                                {
                                    Startup.KickStartRequest.TypeofApplication = QnAForEnvironment.Question;
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
                        var hrefLink = XElement.Parse("<span><a href=\"https://kickstartassistancerequest.azurewebsites.net/Request/CreateEditRequest\">New Request</a></span>")
                                        .Descendants("a")
                                        .Select(x => x.Attribute("href").Value)
                                        .FirstOrDefault();
                        //In the below logic where sql db call happens when there is no match found in qna maker
                        if (Startup.QnAs.Count() > 1)
                        {
                            var questionAndAnswerObject = Startup.QnAs.Last();


                            if (questionAndAnswerObject.Answer.Equals("Please enter your issue statement or request statement"))
                            {
                                if (!string.IsNullOrWhiteSpace(Startup.Environment) && !string.IsNullOrWhiteSpace(Startup.NatureOfRequest))
                                {
                                    var answer = GetAnswers(turnContext.Activity.Text, Startup.Environment, Startup.NatureOfRequest);
                                    if (!string.IsNullOrWhiteSpace(answer))
                                    {
                                        string replyText = $"Here is resolution steps: {answer}" + thanksRegards;
                                        await turnContext.SendActivityAsync(MessageFactory.Text(replyText), cancellationToken);
                                        Startup.Environment = null;
                                        Startup.NatureOfRequest = null;
                                        Startup.QnAs = new List<QnA>();
                                    }
                                    else
                                    {
                                        var inProgressRequests = GetInProgressRequests(turnContext.Activity.Text, Startup.Environment, Startup.NatureOfRequest);
                                        if (!string.IsNullOrWhiteSpace(inProgressRequests))
                                        {
                                            string replyText = $"Already request has been raised with similar context : {inProgressRequests}" + thanksRegards;
                                            await turnContext.SendActivityAsync(MessageFactory.Text(replyText), cancellationToken);
                                            Startup.Environment = null;
                                            Startup.NatureOfRequest = null;
                                            Startup.QnAs = new List<QnA>();
                                        }
                                        else
                                        {
                                            string defaultText = $"We could not find the suitable resolution to you, Please raise new request." + Environment.NewLine + hrefLink + thanksRegards;
                                            await turnContext.SendActivityAsync(MessageFactory.Text(defaultText, defaultText), cancellationToken);
                                            Startup.Environment = null;
                                            Startup.NatureOfRequest = null;
                                            Startup.QnAs = new List<QnA>();
                                        }
                                    }
                                }
                                else
                                {
                                    string defaultText = $"We could not find the suitable resolution to you, Please raise new request." + Environment.NewLine + hrefLink + thanksRegards;
                                    await turnContext.SendActivityAsync(MessageFactory.Text(defaultText, defaultText), cancellationToken);
                                    Startup.Environment = null;
                                    Startup.NatureOfRequest = null;
                                    Startup.QnAs = new List<QnA>();
                                }
                            }
                            var applicationQnA = Startup.QnAs.Last();
                            if (applicationQnA.Question.Equals("ApplicationName"))
                            {
                                Startup.KickStartRequest.ApplicationName = string.Concat(applicationQnA.Answer.Where(c => !char.IsWhiteSpace(c)));
                                if (!string.IsNullOrWhiteSpace(Startup.KickStartRequest.DomainName) && !string.IsNullOrWhiteSpace(Startup.KickStartRequest.TypeofApplication))
                                {
                                    string kickstartURL = string.Format("https://kickstartassistancerequest.azurewebsites.net/Request/KickstartApplicationDetails?typeofapplication={0}&applicationname={1}&domain={2}", Startup.KickStartRequest.TypeofApplication.ToLower().Replace(" ", string.Empty), Startup.KickStartRequest.ApplicationName.ToLower().Replace(" ", string.Empty), Startup.KickStartRequest.DomainName.ToLower().Replace(" ", string.Empty));
                                    string replyText = $"Please use the below url to kickstart your application : " + Environment.NewLine + kickstartURL + thanksRegards;
                                    await turnContext.SendActivityAsync(MessageFactory.Text(replyText), cancellationToken);
                                    Startup.Environment = null;
                                    Startup.NatureOfRequest = null;
                                    Startup.QnAs = new List<QnA>();
                                }
                                else
                                {
                                    string defaultText = $"Some error occured, Please try after some time." + thanksRegards;
                                    await turnContext.SendActivityAsync(MessageFactory.Text(defaultText, defaultText), cancellationToken);
                                    Startup.Environment = null;
                                    Startup.NatureOfRequest = null;
                                    Startup.QnAs = new List<QnA>();
                                }
                            }
                        }
                        else
                        {
                            string defaultText = $"We could not find the suitable resolution to you, Please raise new request." + Environment.NewLine + hrefLink + Environment.NewLine + thanksRegards;
                            await turnContext.SendActivityAsync(MessageFactory.Text(defaultText, defaultText), cancellationToken);
                            Startup.Environment = null;
                            Startup.NatureOfRequest = null;
                            Startup.QnAs = new List<QnA>();
                        }
                    }
                }
                else
                {
                    string defaultText = $"Thanks for reaching out to us, We will get back to you shortly" + thanksRegards;
                    await turnContext.SendActivityAsync(MessageFactory.Text(defaultText, defaultText), cancellationToken);
                    Startup.Environment = null;
                    Startup.NatureOfRequest = null;
                    Startup.QnAs = new List<QnA>();
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Exception in AccessQnAMaker " + ex.Message);
                logger.LogError(ex.StackTrace);
                Startup.Environment = null;
                Startup.NatureOfRequest = null;
                Startup.QnAs = new List<QnA>();
            }
        }
    }
}
