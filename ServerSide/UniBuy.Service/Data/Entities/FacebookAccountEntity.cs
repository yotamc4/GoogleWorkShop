// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Service.Data.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class FacebookAccountEntity
    {
        // TODO: once we add authentication connect this as a relationship to the user
        // and set delete behavior to cascade
        [Key]
        public string Id { get; set; }
        string ProfileAccountUrl { get; set; }
    }
}