using IronBlock;
using IronBlock.Blocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BacklundTransformAnalytics
{
    public class Program
    {

        internal class CustomPrintBlock : IBlock
        {
            public List<string> Text { get; set; } = new List<string>();

            public override object Evaluate(Context context)
            {
                Text.Add((this.Values.FirstOrDefault(x => x.Name == "TEXT")?.Evaluate(context) ?? "").ToString());
                return base.Evaluate(context);
            }
        }
        [JSInvokable]
        public static string Evaluate(string xml)
        {
            var printBlock = new CustomPrintBlock();

            new Parser()
              .AddStandardBlocks()
              .AddBlock("text_print", printBlock)
              .Parse(xml)
              .Evaluate();

            return string.Join('\n', printBlock.Text);

        }
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
