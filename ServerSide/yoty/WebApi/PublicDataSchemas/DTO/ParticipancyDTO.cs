namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public class ParticipancyDTO : BaseDTO
    {
        public string BuyerName { get; set; }

        public int NumOfUnits { get; set; }

        public bool HasPaid { get; set; }
    }
}