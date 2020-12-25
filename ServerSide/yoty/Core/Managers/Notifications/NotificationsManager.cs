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
        private const string TimeToVoteForSuppliersBody = "Great news! There are more than one optional proposals for the group-buy you are participating in! <br /><br />You have 48 hours to vote for your preferred proposal, <br /><b>please visit the groups page during this time and vote.</b>";
        private const string TimeToVoteForSuppliersSubject = "Time To Vote - UniBuy";
        private const string TimeToPayBody = "A supplier proposal for the group-buy you are participating in has been chosen! <br /><br /><b>please visit the groups page during this time and complete payment.</b>";
        private const string TimeToPaySubject = "Time To Pay - UniBuy";
        private const string SupplierCancellationBody = "We are sorry to inform you that the supplier has canceled the deal for now, as other participants canceled their participations.";
        private const string SupplierCancellationSubject = "Deal Cancellation - UniBuy";
        private const string GroupCompletionBody = "The group-buy you participated in has come to completion <br /><br />We hope you had a satisfying experience, always at your service.";
        private const string GroupCompletionSubject = "Group-Buy Completed - UniBuy";
        private const string ProgressBarCompletionToParticipantsBody = "A group-buy you are participating in has fulfilled a new proposal requirements!";
        private const string ProgressBarCompletionToSuppliersBody = "A proposal to a group-buy you've made a proposal in has just reached its requirements.";
        private const string ProgressBarCompletionSubject = "Group Met Proposal Terms - UniBuy";

        private readonly YotyContext _context;
        private readonly IMapper _mapper;
        private readonly IMailService _mail;

        public NotificationsManager(IMapper mapper, YotyContext context, IMailService mail)
        {
            _mapper = mapper;
            _context = context;
            _mail = mail;
        }

        // FOR TESTING canceled 
        public async Task<Response> Ping()
        {
            MailRequest request = new MailRequest() {
                Body = this.personalizeBody(TimeToVoteForSuppliersBody, "Yotam Cohen"),
                Subject = TimeToVoteForSuppliersSubject,
                ToEmail = "yotamc4@gmail.com",
            };
            try
            {
                await _mail.SendEmailAsync(request);
            }
            catch (Exception ex)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
            }
            return new Response() { IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response> NotifyBidSuppliers(string bidId, string body, string subject)
        {
            IEnumerable<KeyValuePair<string, string>> emailNamePairs;
            try
            {
                emailNamePairs = await _context.Set<SupplierProposalEntity>()
                .Where(p => p.BidId == bidId).Include(p => p.Supplier).Select(p => new KeyValuePair<string, string>(p.Supplier.Email, p.Supplier.Name)).ToListAsync().ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
            }
            return await NotifyList(body, subject, emailNamePairs);
        }

        public async Task<Response> NotifyBidParticipants(string bidId, string body, string subject)
        {
            IEnumerable<KeyValuePair<string, string>> emailNamePairs;
            try
            {
                emailNamePairs = await _context.Set<ParticipancyEntity>()
                .Where(p => p.BidId == bidId).Include(p => p.Buyer).Select(p => new KeyValuePair<string, string>(p.Buyer.Email, p.Buyer.Name)).ToListAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
            }
            return await NotifyList(body, subject, emailNamePairs);
        }

        public async Task<Response> NotifyBidAll(string bidId, string body, string subject)
        {
            Response r1 = await NotifyBidParticipants(bidId, body, subject);
            Response r2 = await NotifyBidSuppliers(bidId, body, subject);
            if(!r1.IsOperationSucceeded || !r2.IsOperationSucceeded)
            {
                return !r1.IsOperationSucceeded ? r1 : r2;
            }
            return new Response() { IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };

        }

        private async Task<Response> NotifyList(string body, string subject, IEnumerable<KeyValuePair<string, string>> emailNamePairs)
        {
            MailRequest request = new MailRequest() {
                Body = body,
                Subject = subject
            };
            foreach (var emailNamePair in emailNamePairs)
            {
                request.ToEmail = emailNamePair.Key;
                request.Body = this.personalizeBody(body, emailNamePair.Value);
                try
                {
                    await _mail.SendEmailAsync(request);
                }
                catch (Exception ex)
                {
                    return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
                }
            }
            return new Response() { IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        private string getSuccessMessage([CallerMemberName] string callerName = "")
        {
            return $"{callerName} success";
        }
        private string personalizeBody(string body, string name)
        {
            return $"<b>Hello {name}</b>,<br /><br /> {body} <br /><br /> Thanks <br />The UniBuy Team!";
        }

        public Task<Response> NotifyBidChosenSupplier(string bidId, string body, string subject)
        {
            throw new NotImplementedException();
        }

        public Task<Response> NotifyBidAllCompletion(string bidId)
        {
            return NotifyBidAll(bidId, GroupCompletionBody, GroupCompletionSubject);
        }

        public Task<Response> NotifyBidParticipantsTimeToVote(string bidId)
        {
            return this.NotifyBidParticipants(bidId, TimeToVoteForSuppliersBody, TimeToVoteForSuppliersSubject);
        }

        public Task<Response> NotifyBidParticipantsTimeToPay(string bidId)
        {
            return this.NotifyBidParticipants(bidId, TimeToPayBody, TimeToPaySubject);
        }

        public Task<Response> NotifyBidParticipantsSupplierCancellation(string bidId)
        {
            return this.NotifyBidParticipants(bidId, SupplierCancellationBody, SupplierCancellationSubject);
        }

        public Task<Response> NotifyBidAllProgressBarCompletion(string bidId)
        {
            this.NotifyBidSuppliers(bidId, ProgressBarCompletionToSuppliersBody, ProgressBarCompletionSubject);
            return this.NotifyBidParticipants(bidId, ProgressBarCompletionToParticipantsBody, ProgressBarCompletionSubject);
        }
    }
}
