using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;


namespace mp.ce.fdid.FdidProjetos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseSetting(WebHostDefaults.DetailedErrorsKey, "true")
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
    }
}
