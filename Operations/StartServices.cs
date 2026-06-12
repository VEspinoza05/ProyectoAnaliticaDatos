using APPCORE;
using BusinessLogic.Connection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Operations.SyntheticDataGenerator;
using Operations.SyntheticDataGenerator.Model;

namespace Operations;

public class StartServices
{
    public async Task StartServicesApp(IServiceCollection services)
    {
        try
        {
            Console.Write("### START SEEDING ###");
            await SyntheticDataGeneratorOperation.Start();
            Console.Write("### END SEEDING ###");
        }
        catch (System.Exception ex)
        {
            Console.Write(ex.Message);
            throw;
        }

        services.AddDbContext<UdemyDwContext>();
    }

}
