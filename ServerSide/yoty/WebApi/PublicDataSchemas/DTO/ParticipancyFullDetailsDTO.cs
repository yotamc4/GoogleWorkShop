namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public class ParticipancyFullDetailsDTO : BaseDTO
    {
        public string BidId { get; set; }

        public string BuyerName { get; set; }

        public string BuyerId { get; set; }

        public int NumOfUnits { get; set; }

        public bool HasPaid { get; set; }

        public string BuyerAddress { get; set; }

        public string BuyerPostalCode { get; set; }

        public string BuyerPhoneNumber { get; set; }
    }
}