using System.ComponentModel.DataAnnotations;

namespace YOTY.Service.Data.Entities
{
    public class BuyerAccountDetailsEntity
    {
        // TODO: after we settle on the fields connect this as a relationship to the user
        // and set delete behavior to cascade

        [Key]
        public string Id { get; set; }
    }
}