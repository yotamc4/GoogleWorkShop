using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YOTY.Service.Core.Services.Mail;
using YOTY.Service.Data;
using YOTY.Service.Data.Entities;
using YOTY.Service.WebApi.PublicDataSchemas;

namespace YOTY.Service.Core.Managers.Notifications
{
    public class NotificationsManager : INotificationsManager
    {
        private const string TimeToVoteForSuppliersToParticipantsBody = "Great news! There are more than one optional proposals for the group-buy you are participating in! <br /><br />You have 48 hours to vote for your preferred proposal, <br /><b>please visit the groups page during this time and vote.</b>";
        private const string TimeToVoteForSuppliersToParticipantsSubject = "Time To Vote - UniBuy";
        private const string VoteStartedToSuppliersBody = "In the next 48 hours participants will vote between proposals,<br /><br />Your proposal might be selected, stay tuned!";
        private const string VoteStartedToSuppliersSubject = "Voting Started - UniBuy";
        private const string TimeToPayBody = "A supplier proposal for the group-buy you are participating in has been chosen! <br /><br /><b>please visit the groups page during this time and complete payment.</b>";
        private const string TimeToPaySubject = "Time To Pay - UniBuy";
        private const string SupplierCancellationBody = "We are sorry to inform you that the supplier has canceled the deal for now,  <br />as other participants canceled their participations.";
        private const string SupplierCancellationSubject = "Deal Cancellation - UniBuy";
        private const string SupplierNotFoundCancellationBody = "We are sorry to inform you that no supplier has made a proposal to a group-buy you are participating in. <br />Better luck next time.";
        private const string SupplierNotFoundCancellationSubject = "Supplier Not Found - UniBuy";
        private const string GroupCompletionBody = "The group-buy you participated in has come to completion <br /><br />We hope you had a satisfying experience, always at your service.";
        private const string GroupCompletionSubject = "Group-Buy Completed - UniBuy";
        private const string ProgressBarCompletionToParticipantsBody = "A group-buy you are participating in has fulfilled a new proposal requirements!";
        private const string ProgressBarCompletionToSuppliersBody = "A proposal to a group-buy you've also made a proposal to has just reached its requirements.";
        private const string ProgressBarCompletionSubject = "Group Met Proposal Terms - UniBuy";
        private const string FoundChosenSupplierToSuppliersBody = "A proposal to a group-buy you've also made a proposal to has been selected.";
        private const string FoundChosenSupplierToChosenSuppliersBody = "Congrats! your proposal has been selected by majority of the group-buy participants,  <br />They group is now entering the payment phase.";
        private const string FoundChosenSupplierSubject = "Proposal Selected - UniBuy";

        private const string domain = "https://localhost:3000";
        private readonly YotyContext _context;
        private readonly IMailService _mail;

        public NotificationsManager(YotyContext context, IMailService mail)
        {
            _context = context;
            _mail = mail;
        }

        // FOR TESTING canceled 
        public async static Task<Response> Ping(string bidId, IMailService mail, YotyContext context)
        {
            try
            {
                var bid = await context.Bids.FindAsync(bidId).ConfigureAwait(false);
                MailRequest request = new MailRequest() {
                    Body = personalizeBody(VoteStartedToSuppliersBody, $"Yotam Cohen {bid.Category}", "fakeBidId"),
                    Subject = VoteStartedToSuppliersSubject,
                    ToEmail = "yotamc4@gmail.com",
                };
                await mail.SendEmailAsync(request);
            }
            catch (Exception ex)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
            }
            return new Response() { IsOperationSucceeded = true, SuccessOrFailureMessage = "Ping Pong" };
        }

        public async Task<Response> NotifyBidSuppliersAsync(string bidId, string body, string subject)
        {
            IEnumerable<KeyValuePair<string, string>> emailNamePairs;
            try
            {
                emailNamePairs = await _context.Set<SupplierProposalEntity>()
                .Where(p => p.BidId == bidId).Include(p => p.Supplier).Select(p => new KeyValuePair<string, string>(p.Supplier.Email, p.Supplier.Name)).ToListAsync().ConfigureAwait(false);
                // Console.WriteLine("NotifyBidSuppliers");
                // Console.WriteLine(string.Join(", ", emailNamePairs.Select(p => p.ToString())));
            }
            catch (Exception ex)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
            }
            return await NotifyListAsync(body, subject, emailNamePairs, bidId).ConfigureAwait(false);
        }

        public async Task<Response> NotifyBidParticipantsAsync(string bidId, string body, string subject)
        {
            IEnumerable<KeyValuePair<string, string>> emailNamePairs;
            try
            {
                emailNamePairs = await _context.Set<ParticipancyEntity>()
                .Where(p => p.BidId == bidId).Include(p => p.Buyer).Select(p => new KeyValuePair<string, string>(p.Buyer.Email, p.Buyer.Name)).ToListAsync().ConfigureAwait(false);
                // Console.WriteLine("NotifyBidParticipants");
                // Console.WriteLine(string.Join(", ", emailNamePairs.Select(p => p.ToString())));
            }
            catch (Exception ex)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
            }
            return await NotifyListAsync(body, subject, emailNamePairs, bidId);
        }

        public async Task<Response> NotifyBidAllAsync(string bidId, string body, string subject)
        {
            var notifyBidParticipantsTask = NotifyBidParticipantsAsync(bidId, body, subject);
            var NotifyBidSuppliersTask = NotifyBidSuppliersAsync(bidId, body, subject);

            await Task.WhenAll(notifyBidParticipantsTask, NotifyBidSuppliersTask);

            Response notifyBidParticipantsResponse = await notifyBidParticipantsTask;
            Response NotifyBidSuppliersResponse = await NotifyBidSuppliersTask;

            if(!notifyBidParticipantsResponse.IsOperationSucceeded || !NotifyBidSuppliersResponse.IsOperationSucceeded)
            {
                return !notifyBidParticipantsResponse.IsOperationSucceeded ? notifyBidParticipantsResponse : NotifyBidSuppliersResponse;
            }
            return new Response() { IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };

        }

        private async Task<Response> NotifyListAsync(string body, string subject, IEnumerable<KeyValuePair<string, string>> emailNamePairs, string bidId)
        {
            MailRequest request = new MailRequest() {
                Body = body,
                Subject = subject
            };

            List<Task> sendEmailTasks = new List<Task>();

            foreach (var emailNamePair in emailNamePairs)
            {
                request.ToEmail = emailNamePair.Key;
                request.Body = personalizeBody(body, emailNamePair.Value, bidId);
                var sendEmailTask = _mail.SendEmailAsync(request);
                sendEmailTasks.Add(sendEmailTask);
            }
            try
            {
                await Task.WhenAll(sendEmailTasks).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
            }
            return new Response() { IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response> NotifyBidChosenSupplierAsync(string bidId)
        {
            return await this.NotifyBidChosenSupplierAsync(bidId, FoundChosenSupplierToChosenSuppliersBody, FoundChosenSupplierSubject).ConfigureAwait(false);
        }
        public async Task<Response> NotifyBidChosenSupplierAsync(string bidId, string body, string subject)
        {
            IEnumerable<KeyValuePair<string, string>> emailNamePairs;

            try
            { 
                emailNamePairs = await _context.Bids
                    .Where(bid => bid.Id == bidId)
                    .Include(bid => bid.ChosenProposal)
                    .ThenInclude(p => p.Supplier)
                    .Select(bid => bid.ChosenProposal.Supplier)
                    .Select(s => new KeyValuePair<string, string>(s.Email, s.Name))
                    .ToListAsync()
                    .ConfigureAwait(false); ;
            }
            catch (Exception ex)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
            }

            return await NotifyListAsync(body, subject, emailNamePairs, bidId);
        }

        public async Task<Response> NotifyBidAllCompletionAsync(string bidId)
        {
            return await NotifyBidAllAsync(bidId, GroupCompletionBody, GroupCompletionSubject).ConfigureAwait(false);
        }

        public async Task<Response> NotifyBidParticipantsTimeToVoteAsync(string bidId)
        {
            return await this.NotifyBidParticipantsAsync(bidId, TimeToVoteForSuppliersToParticipantsBody, TimeToVoteForSuppliersToParticipantsSubject).ConfigureAwait(false);
        }

        public async Task<Response> NotifyBidTimeToVoteAsync(string bidId)
        {
            var res1 = await this.NotifyBidParticipantsTimeToVoteAsync(bidId).ConfigureAwait(false);
            if (!res1.IsOperationSucceeded)
            {
                return res1;
            }
            return await this.NotifyBidSuppliersAsync(bidId, VoteStartedToSuppliersBody, VoteStartedToSuppliersSubject).ConfigureAwait(false);
        }

        public async Task<Response> NotifyBidParticipantsTimeToPayAsync(string bidId)
        {
            return await this.NotifyBidParticipantsAsync(bidId, TimeToPayBody, TimeToPaySubject).ConfigureAwait(false);
        }

        public async Task<Response> NotifyBidTimeToPayAsync(string bidId)
        {
            var res1 = await this.NotifyBidParticipantsTimeToPayAsync(bidId);
            if (!res1.IsOperationSucceeded)
            {
                return res1;
            }
            return await this.NotifyBidSuppliersAsync(bidId, FoundChosenSupplierToSuppliersBody, FoundChosenSupplierSubject).ConfigureAwait(false);
        }

        public async Task<Response> NotifyBidParticipantsSupplierNotFoundCancellationAsync(string bidId)
        {
            return await this.NotifyBidParticipantsAsync(bidId, SupplierNotFoundCancellationBody, SupplierNotFoundCancellationSubject).ConfigureAwait(false);
        }

        public async Task<Response> NotifyBidParticipantsSupplierCancellationAsync(string bidId)
        {
            return await this.NotifyBidParticipantsAsync(bidId, SupplierCancellationBody, SupplierCancellationSubject).ConfigureAwait(false); ;
        }

        public async Task<Response> NotifyBidAllProgressBarCompletionAsync(string bidId)
        {
            var res1 = await this.NotifyBidSuppliersAsync(bidId, ProgressBarCompletionToSuppliersBody, ProgressBarCompletionSubject).ConfigureAwait(false); ;
            if (!res1.IsOperationSucceeded)
            {
                return res1;
            }
            return await this.NotifyBidParticipantsAsync(bidId, ProgressBarCompletionToParticipantsBody, ProgressBarCompletionSubject).ConfigureAwait(false); ;
        }

        private string getSuccessMessage([CallerMemberName] string callerName = "")
        {
            return $"{callerName} success";
        }
        private static string personalizeBody(string body, string name, string bidId)
        {
            string bidUrl = getBidUrl(bidId);
            return $"<b>Hello {name}</b>,<br /><br /> {body} <br />visit <a href='{bidUrl}'>the group page</a> for more info <br /><br />  Thanks <br />The UniBuy Team!";
        }

        private static string getBidUrl(string bidId)
        {
            return $"{domain}/groups/{bidId}";
        }

        public Task<Response> NotifyOnFirstSupplierJoinedToBidAsync(string bidId)
        {
            throw new NotImplementedException();
        }

        public Task<Response> NotifyOnNewAttractivePropsalForBidAsync(string bidId)
        {
            throw new NotImplementedException();
        }

        public Task<Response> NotifyOnDemandedUnitFirstReachedProposalThresholdAsync(string bidId)
        {
            throw new NotImplementedException();
        }
    }
}
