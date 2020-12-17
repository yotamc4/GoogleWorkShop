using System.ComponentModel.DataAnnotations;

namespace YOTY.Service.Data.Entities
{
    public  class FacebookAccountEntity
    {
        // TODO: once we add authentication connect this as a relationship to the user
        // and set delete behavior to cascade
        [Key]
        public string Id { get; set; }
        string ProfileAccountUrl { get; set; }
    }
}