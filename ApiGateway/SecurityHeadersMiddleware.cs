using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway
{
    public sealed class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            if (context.Response.Headers.ContainsKey("Server"))
            {
                context.Response.Headers.Remove("Server"); // For security reasons
            }

            if (context.Response.Headers.ContainsKey("x-powered-by") || context.Response.Headers.ContainsKey("X-Powered-By"))
            {
                context.Response.Headers.Remove("x-powered-by");
                context.Response.Headers.Remove("X-Powered-By");
            }

            context.Response.Headers.Add("referrer-policy", new StringValues("strict-origin-when-cross-origin"));
            context.Response.Headers.Add("x-content-type-options", new StringValues("nosniff"));
            context.Response.Headers.Add("x-frame-options", new StringValues("DENY"));
            context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", new StringValues("none"));
            context.Response.Headers.Add("x-xss-protection", new StringValues("1; mode=block"));
            context.Response.Headers.Add("Expect-CT", new StringValues("max-age=0, enforce, report-uri=\"https://example.report-uri.com/r/d/ct/enforce\""));
            context.Response.Headers.Add("Feature-Policy", new StringValues(
                "accelerometer 'none';" +
                "ambient-light-sensor 'none';" +
                "autoplay 'none';" +
                "battery 'none';" +
                "camera 'none';" +
                "display-capture 'none';" +
                "document-domain 'none';" +
                "encrypted-media 'none';" +
                "execution-while-not-rendered 'none';" +
                "execution-while-out-of-viewport 'none';" +
                "gyroscope 'none';" +
                "magnetometer 'none';" +
                "microphone 'none';" +
                "midi 'none';" +
                "navigation-override 'none';" +
                "payment 'none';" +
                "picture-in-picture 'none';" +
                "publickey-credentials-get 'none';" +
                "sync-xhr 'none';" +
                "usb 'none';" +
                "wake-lock 'none';" +
                "xr-spatial-tracking 'none';"
                ));
            context.Response.Headers.Add("Content-Security-Policy", new StringValues(
                "base-uri 'none';" +
                "block-all-mixed-content;" +
                "child-src 'none';" +
                "connect-src 'none';" +
                "default-src 'none';" +
                "font-src 'none';" +
                "form-action 'none';" +
                "frame-ancestors 'none';" +
                "frame-src 'none';" +
                "img-src 'none';" +
                "manifest-src 'none';" +
                "media-src 'none';" +
                "object-src 'none';" +
                "sandbox;" +
                "script-src 'none';" +
                "script-src-attr 'none';" +
                "script-src-elem 'none';" +
                "style-src 'none';" +
                "style-src-attr 'none';" +
                "style-src-elem 'none';" +
                "upgrade-insecure-requests;" +
                "worker-src 'none';"
                ));

            return _next(context);
        }
    }
}
