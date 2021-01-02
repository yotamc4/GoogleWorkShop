namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public class ParticipancyDTO : BaseDTO
    {
        public string BidId { get; set; }

        public string BuyerName { get; set; }

        public string BuyerId { get; set; }

        public int NumOfUnits { get; set; }

        public bool HasVoted { get; set; }

        public bool HasPaid { get; set; }
    }
}