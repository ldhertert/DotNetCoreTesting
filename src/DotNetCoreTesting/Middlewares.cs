using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetCoreTesting
{
    public class WrappingMiddleware
    {
        // The middleware delegate to call after this one finishes processing
        private readonly RequestDelegate _next;

        public WrappingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await Before();

            //httpContext.Response.OnStarting((state) => After(), null);

            await _next.Invoke(httpContext);
        }

        public Task Before()
        {
            Console.WriteLine("Before");

            //This breaks BT Detection
            //return Task.Delay(1000);

            //This breaks BT Detection
            //return Task.Run(() => Thread.Sleep(1000));

            //This breaks BT Detection
            return Task.Factory.StartNew(() => Thread.Sleep(1000));
        }

        public Task After()
        {
            Console.WriteLine("After");
            //return Task.Delay(1000);
            //return Task.Run(() => Thread.Sleep(1000));
            return Task.Factory.StartNew(() => Thread.Sleep(1000));
        }
    }

    public class CustomResponseMiddleware {
        private readonly RequestDelegate _next;

        public CustomResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.Value.Contains("custom"))
            {
                //await Task.Delay(1000);
                Thread.Sleep(1000);
                await httpContext.Response.WriteAsync("custom response");
                return;
            }

            await _next.Invoke(httpContext);
        }
    }
}
