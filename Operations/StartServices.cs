using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML;
using Operations.SyntheticDataGenerator;

namespace Operations;

public class StartServices
{
    public async Task StartServicesApp(IServiceCollection services)
    {
        try
        {
            Console.Write("### START SEEDING ###");
            await SyntheticDataGeneratorOperation.Start(new UdemyDwContext());
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
