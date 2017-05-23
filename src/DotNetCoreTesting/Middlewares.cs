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
        private readonly RequestDelegate _next;

        public WrappingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            //If you make a sync call like this, the BTs are detected
            //Thread.Sleep(1000);

            //Any sort of async call I make seems to be breaking BT detection
            await Before();

            await _next.Invoke(httpContext);
        }

        public Task Before()
        {
            //This breaks BT Detection
            //return Task.Delay(1000);

            //This breaks BT Detection
            //return Task.Run(() => Thread.Sleep(1000));

            //This breaks BT Detection
            return Task.Factory.StartNew(() => Thread.Sleep(1000));
        }
    }
}
