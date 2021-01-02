// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Data.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class BuyerAccountDetailsEntity
    {
        // TODO: after we settle on the fields connect this as a relationship to the user
        // and set delete behavior to cascade

        [Key]
        public string Id { get; set; }
    }
}