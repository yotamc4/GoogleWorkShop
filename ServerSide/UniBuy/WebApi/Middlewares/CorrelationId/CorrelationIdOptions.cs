// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.WebApi.Middlewares.CorrelationId
{
    public class CorrelationIdOptions
    {

        //src:https://www.stevejgordon.co.uk/asp-net-core-correlation-ids

        private const string DefaultHeader = "X-Correlation-ID";

        /// <summary>
        /// The header field name where the correlation ID will be stored
        /// </summary>
        public string Header { get; set; } = DefaultHeader;

        /// <summary>
        /// Controls whether the correlation ID is returned in the response headers
        /// </summary>
        public bool IncludeInResponse { get; set; } = true;
    }
}
