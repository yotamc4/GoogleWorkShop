// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Service.WebApi.PublicDataSchemas
{
    public class Response<DTObject>: Response  
    {
        public DTObject DTOObject { get; set; }
    }

    public class Response
    {
        public bool IsOperationSucceeded { get; set; }

        public string SuccessOrFailureMessage { get; set; }

    }
}
