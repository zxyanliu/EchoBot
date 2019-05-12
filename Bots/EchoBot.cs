// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Linq;
using Microsoft.Bot.Builder.AI.QnA;

namespace Microsoft.Bot.Builder.EchoBot
{
    public class EchoBot : ActivityHandler
    {

        public QnAMaker EchoBotQnA { get; private set; }
        public EchoBot(QnAMakerEndpoint endpoint)
        {
            // connects to QnA Maker endpoint for each turn
            EchoBotQnA = new QnAMaker(endpoint);
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {


            if (turnContext.Activity.Text.Equals("Ethnic"))
            {
                var reply = ((ITurnContext)turnContext).Activity.CreateReply("QnA Maker Returned: " + "https://www.mamnoonrestaurant.com");
                var attachment = new Attachment
                {
                    ContentUrl = "https://www.mamnoonrestaurant.com/assets/menu/_largeSlide/mamnoon_brooke_fitts001066.jpg",
                    ContentType = "image/jpg",
                    Name = "https://www.mamnoonrestaurant.com",
                };
                reply.Attachments = new List<Attachment>() { attachment };
                await turnContext.SendActivityAsync(reply, cancellationToken);
                return;
            }

            if (turnContext.Activity.Text.Equals("Fast food"))
            {
                var reply = ((ITurnContext)turnContext).Activity.CreateReply("QnA Maker Returned: " + "https://www.jackinthebox.com/");
                var attachment = new Attachment
                {
                    ContentUrl = "https://www.chewboom.com/wp-content/uploads/2017/01/Jack-In-The-Box-Adds-Jumbo-Jack-To-4-For-4-Combo-678x381.jpg",
                    ContentType = "image/jpg",
                    Name = "https://www.jackinthebox.com",
                };
                reply.Attachments = new List<Attachment>() { attachment };
                await turnContext.SendActivityAsync(reply, cancellationToken);
                return;
            }

            if (turnContext.Activity.Text.Equals("Fast casual"))
            {
                var reply = ((ITurnContext)turnContext).Activity.CreateReply("QnA Maker Returned: " + "https://www.subway.com/en-us/menunutrition");
                var attachment = new Attachment
                {
                    ContentUrl = "https://securecdn.pymnts.com/wp-content/uploads/2018/10/feature-image-RRI2.jpg",
                    ContentType = "image/jpg",
                    Name = "https://www.subway.com/en-us/menunutrition",
                };
                reply.Attachments = new List<Attachment>() { attachment };
                await turnContext.SendActivityAsync(reply, cancellationToken);
                return;
            }

            if (turnContext.Activity.Text.Equals("Casual dining"))
            {
                var reply = ((ITurnContext)turnContext).Activity.CreateReply("QnA Maker Returned: " + "http://www.terryskitchenbellevue.com");
                var attachment = new Attachment
                {
                    ContentUrl = "https://static1.squarespace.com/static/56b7b6cdf85082f877094fb1/t/5cc400ac9b747a242bacab55/1556349124802/Eggs+Benedict+with+Canadian+Bacon.jpg",
                    ContentType = "image/jpg",
                    Name = "http://www.terryskitchenbellevue.com",
                };
                reply.Attachments = new List<Attachment>() { attachment };
                await turnContext.SendActivityAsync(reply, cancellationToken);
                return;
            }

          






            // First send the user input to your QnA Maker knowledgebase
            await AccessQnAMaker(turnContext, cancellationToken);
            //await turnContext.SendActivityAsync(MessageFactory.Text($"Echo: {turnContext.Activity.Text}"), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Welcome to Echo Bot."), cancellationToken);
                }
            }
        }

        private async Task AccessQnAMaker(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var results = await EchoBotQnA.GetAnswersAsync(turnContext);
            if (results.Any())
            {
                string answer = results.First().Answer;

                if(answer.Equals("Are you looking for restaurant"))
                {
                    var reply = ((ITurnContext)turnContext).Activity.CreateReply("QnA Maker Returned: " + results.First().Answer);

                    reply.SuggestedActions = new SuggestedActions()
                    {
                        Actions = new List<CardAction>()
                        {
                            new CardAction(){ Title = $"Ethnic", Type = ActionTypes.ImBack, Value=$"Ethnic" },
                            new CardAction(){ Title = $"Fast food", Type = ActionTypes.ImBack, Value=$"Fast food" },
                            new CardAction(){ Title = $"Fast casual", Type = ActionTypes.ImBack, Value=$"Fast casual" },
                            new CardAction(){ Title = $"Casual dining", Type = ActionTypes.ImBack, Value=$"Casual dining" }
                           
                        }
                    };

                    await turnContext.SendActivityAsync(reply, cancellationToken);
                    return;
                }

                


                await turnContext.SendActivityAsync(MessageFactory.Text("QnA Maker Returned: " + results.First().Answer), cancellationToken);
            }
            else
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("Sorry, could not find an answer in the Q and A system."), cancellationToken);
            }
        }


    }
}
