namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public class OrderDetailsDTO : BaseDTO
    {
        public string BuyerName { get; set; }

        public string BuyerEmail { get; set; }

        public string BuyerAddress { get; set; }

        public string BuyerPostalCode { get; set; }

        public string BuyerPhoneNumber { get; set; }

        public int NumOfOrderedUnits { get; set; }
    }
}
