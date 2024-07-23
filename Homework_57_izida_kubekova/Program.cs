using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Homework_57_izida_kubekova
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

        public static List<Currency> Currencies { get; set; } = GetCurrencyList();
        
        public static List<Currency> GetCurrencyList()
        {
            string path = "wwwroot/currencyList.json";
    
            if (System.IO.File.Exists(path))
            {
                string result = System.IO.File.ReadAllText(path);
                if (result != null && result != "[]")
                {
                    var currencies = JsonSerializer.Deserialize<List<Currency>>(result);
                    return currencies;
                }
            }
            return null;
        }
    }
}