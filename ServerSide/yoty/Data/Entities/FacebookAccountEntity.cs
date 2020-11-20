using System.ComponentModel.DataAnnotations;

namespace YOTY.Service.Data.Entities
{
    public  class FacebookAccountEntity
    {
        [Key]
        public int Id { get; set; }
        string ProfileAccountUrl { get; set; }
    }
}