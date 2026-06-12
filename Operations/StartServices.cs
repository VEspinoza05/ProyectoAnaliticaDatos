using APPCORE;
using BusinessLogic.Connection;
using Operations.SyntheticDataGenerator;
using Operations.SyntheticDataGenerator.Model;

namespace Operations;

public class StartServices
{
    public async Task<bool> StartServicesApp()
    {
        try
        {
            Console.Write("### START SEEDING ###");
            await SyntheticDataGeneratorOperation.Start();
            Console.Write("### END SEEDING ###");
            return true;
        }
        catch (System.Exception ex)
        {
            Console.Write(ex.Message);
            throw;
        }
    }

}
