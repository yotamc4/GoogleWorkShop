// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.Core.Managers.Bids
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using YOTY.Service.Data.Entities;
    using YOTY.Service.WebApi.PublicDataSchemas;

    public class MockedBidManager : IBidsManager
    {
        private readonly IMapper _mapper;
        private Dictionary<string, BidEntity> mockedBidsSet;

        public MockedBidManager(IMapper mapper)
        {
            _mapper = mapper;
            mockedBidsSet = new Dictionary<string, BidEntity>();
        }

        public Task<Response<BuyerDTO>> AddBuyer(BidBuyerJoinRequest bidBuyerJoinRequest)
        {
            throw new NotImplementedException();
        }

        public Task<Response<SupplierProposalDTO>> AddSupplierProposal(SupplierProposalRequest supplierProposal)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<BidDTO>> CreateNewBid(NewBidRequest bidRquest)
        {
            // create
            BidEntity bidEnitity = _mapper.Map<BidEntity>(bidRquest);
            bidEnitity.CreationDate = DateTime.Now;
            bidEnitity.Id = Guid.NewGuid().ToString();

            // add to db 
            if (mockedBidsSet.TryAdd(bidEnitity.Id, bidEnitity))
            {
                BidDTO bidDto = _mapper.Map<BidDTO>(bidEnitity);
                return new Response<BidDTO> {
                    IsOperationSucceeded  = true,
                    //SuccessOrFailureMessage = bidDto.GetType().GetProperties().ToList().ForEach(entity => { })
                    DTOObject = bidDto,
                };
            }
            else
            {
                return new Response<BidDTO> {
                    IsOperationSucceeded = false,
                    SuccessOrFailureMessage = "failed to add to db"
                };
            }

        }

        public Task<Response> DeleteBid(string bidId)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteBuyer(string bidId, string buyerId)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteSupplierProposal(string bidId, string ProposalId)
        {
            throw new NotImplementedException();
        }

        public Task<Response<BidDTO>> EditBid(EditBidRequest editBidRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<BidDTO>> GetBid(string bidId)
        {
            // add to db 
            if (mockedBidsSet.TryGetValue(bidId,out BidEntity bidEnitity))
            {
                BidDTO bidDto = _mapper.Map<BidDTO>(bidEnitity);
                return new Response<BidDTO> {
                    IsOperationSucceeded = true,
                    //SuccessOrFailureMessage = bidDto.GetType().GetProperties().ToList().ForEach(entity => { })
                    DTOObject = bidDto,
                };
            }
            else
            {
                return new Response<BidDTO> {
                    IsOperationSucceeded = false,
                    SuccessOrFailureMessage = "bid is not existed"
                };
            }
        }

        public Task<Response<List<BuyerDTO>>> GetBidBuyers(string bidId)
        {
            throw new NotImplementedException();
        }

        public Task<Response<List<BidDTO>>> GetBids(BidsQueryOptions bidsFilters)
        {
            throw new NotImplementedException();
        }

        public Task<Response<List<SupplierProposalDTO>>> GetBidSuplliersProposals(string bidId)
        {
            throw new NotImplementedException();
        }

        Task<Response<BidsDTO>> IBidsManager.GetBids(BidsQueryOptions bidsFilters)
        {
            throw new NotImplementedException();
        }
    }
}
