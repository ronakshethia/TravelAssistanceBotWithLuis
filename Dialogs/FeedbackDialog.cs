﻿using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TravelBot.Dialogs
{
    [Serializable]
    public class FeedbackDialog : IDialog<IMessageActivity>
    {
        private string qnaURL;
        private string userQuestion;

        public FeedbackDialog(string url, string question)
        {
            // keep track of data associated with feedback
            qnaURL = url;
            userQuestion = question;
        }
        public async Task StartAsync(IDialogContext context)
        {
            var feedback = ((Activity)context.Activity).CreateReply("Did you find what you need?");
            feedback.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
        {
            new CardAction(){ Title = "👍", Type=ActionTypes.PostBack, Value=$"yes-positive-feedback" },
            new CardAction(){ Title = "👎", Type=ActionTypes.PostBack, Value=$"no-negative-feedback" }
        }
            };
            await context.PostAsync(feedback);
            context.Wait(this.MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var userFeedback = await result;
            if (userFeedback.Text.Contains("yes-positive-feedback") || userFeedback.Text.Contains("no-negative-feedback"))
            {
                if (userFeedback.Text.Contains("yes-positive-feedback"))
                {
                    // post positive feedback to DB...
                }
                else if (userFeedback.Text.Contains("no-negative-feedback"))
                {
                    // post negative feedback to DB...
                }
                await context.PostAsync("Thanks for your feedback!");
                context.Done<IMessageActivity>(null);
            }
            else
            {
                // no feedback, return to QnA dialog
                context.Done<IMessageActivity>(userFeedback);
            }
        }
    }
}