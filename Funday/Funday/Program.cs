using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace Funday
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Licensing.RegisterLicense("DARREN-e1JlZjpEQVJSRU4sTmFtZTpEYXJyZW4gUmVpZCxUeXBlOkJ1c2luZXNzLEhhc2g6a3d2TFdaYmJSY2oydWNxTFp4NFh1L0t0eXdqaXBhUExlSFM4VlhodjducEt6cCtheDlvVWI4OTJPZ1FxUmNLZVVlbksxV3JQZUYybGYwdTYxMTFpc2p0Tm9QRDFGbGtweExpZUxmS05zQUErSkFUb1N3V0JwTk16d0g2SWF0ei9qY0dzdXBJcFVUVVk1RFUwUTJDK3Z3UkpoYWhlY0JPZVE1ajBrc0YxYkRnPSxFeHBpcnk6MjAyMC0wMS0yMn0=");

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
