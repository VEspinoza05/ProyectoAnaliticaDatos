using Microsoft.EntityFrameworkCore;
using Operations.SyntheticDataGenerator.Model;
using UdemyAnalytics.Models;

namespace Operations.SyntheticDataGenerator
{
    public class UdemyDwContext : DbContext
    {
        public DbSet<DimEstudiante> DimEstudiantes { get; set; }
        public DbSet<DimCurso> DimCursos { get; set; }
        public DbSet<DimTiempo> DimTiempos { get; set; }
        public DbSet<DimDispositivo> DimDispositivos { get; set; }
        public DbSet<DimPromocion> DimPromociones { get; set; }
        public DbSet<FactInteraccionesProgreso> FactInteraccionesProgreso { get; set; }
        public DbSet<FactEvaluaciones> FactEvaluaciones { get; set; }
        public DbSet<FactVentasInscripciones> FactVentasInscripciones { get; set; }
        public DbSet<EtlConfig> EtlConfig { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Reemplaza con tu cadena de conexión local a SQL Server
            optionsBuilder.UseSqlServer("Server=localhost;Database=DW_Udemy;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de precisión para tipos Decimal (Evita truncados automáticos)
            modelBuilder.Entity<DimCurso>().Property(c => c.PrecioBase).HasPrecision(18, 2);
            modelBuilder.Entity<DimCurso>().Property(c => c.RatingPromedio).HasPrecision(3, 2);
            modelBuilder.Entity<DimPromocion>().Property(p => p.PorcentajeDescuento).HasPrecision(5, 2);
            modelBuilder.Entity<FactInteraccionesProgreso>().Property(f => f.PorcentajeProgresoAcumulado).HasPrecision(5, 2);
            modelBuilder.Entity<FactEvaluaciones>().Property(f => f.CalificacionObtenida).HasPrecision(5, 2);
            modelBuilder.Entity<FactVentasInscripciones>().Property(f => f.MontoPagado).HasPrecision(18, 2);
            modelBuilder.Entity<FactVentasInscripciones>().Property(f => f.ProgresoFinalPorcentaje).HasPrecision(5, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}