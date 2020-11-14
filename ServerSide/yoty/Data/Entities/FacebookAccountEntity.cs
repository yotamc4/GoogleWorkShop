using System.ComponentModel.DataAnnotations;

namespace Yoty.Data.Entities
{
    public  class FacebookAccountEntity
    {
        [Key]
        public int Id { get; set; }
        string ProfileAccountUrl { get; set; }
    }
}