using System.Drawing.Text;
using Microsoft.EntityFrameworkCore;
using Operations.SyntheticDataGenerator.Model;

namespace Operations.AnaliticOperations.Model
{
    public class DimEstudianteOperation
    {
        private readonly UdemyDwContext _context;

        public DimEstudianteOperation(UdemyDwContext context)
        {
            _context = context;
        }

        public async Task<List<Dim_Estudiante>> getStudents()
        {
            var data = await _context.Dim_Estudiante.Skip(0).Take(20).ToListAsync();
            return data;
        }
    }
}