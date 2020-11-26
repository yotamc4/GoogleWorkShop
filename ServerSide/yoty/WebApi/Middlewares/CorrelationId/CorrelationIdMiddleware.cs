﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private readonly CorrelationIdOptions _options;

    public CorrelationIdMiddleware(RequestDelegate next, IOptions<CorrelationIdOptions> options)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        _next = next ?? throw new ArgumentNullException(nameof(next));

        _options = options.Value;
    }

    public Task Invoke(HttpContext context)
    {

        this.SetCorrelationID(context);
        if (_options.IncludeInResponse)
        {
            // apply the correlation ID to the response header for client side tracking
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Add(_options.Header, new[] { context.TraceIdentifier });
                return Task.CompletedTask;
            });
        }

        return _next(context);
    }

    private void SetCorrelationID(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(_options.Header, out StringValues correlationId))
        {
            context.TraceIdentifier = correlationId;
        }
        else
        {
            context.TraceIdentifier = new Guid().ToString();
        }
    }
}