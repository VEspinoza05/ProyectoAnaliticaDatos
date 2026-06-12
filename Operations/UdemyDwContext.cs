using Microsoft.EntityFrameworkCore;
using Operations.SyntheticDataGenerator.Model;

namespace Operations
{
    public class UdemyDwContext : DbContext
    {
        public DbSet<Dim_Estudiante> Dim_Estudiante { get; set; }
        public DbSet<Dim_Curso> Dim_Curso { get; set; }
        public DbSet<Dim_Suscripcion> Dim_Suscripcion { get; set; }
        public DbSet<Dim_Tiempo> Dim_Tiempo { get; set; }
        public DbSet<Fact_Interacciones_Diarias> Fact_Interacciones_Diarias { get; set; }
        public DbSet<Fact_Rendimiento_Evaluaciones> Fact_Rendimiento_Evaluaciones { get; set; }
        public DbSet<Etl_Config> Etl_Config { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Reemplaza con tu cadena de conexión local a SQL Server
            optionsBuilder.UseSqlServer("Server=localhost;Database=DW_Udemy;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de llaves primarias y mapeo exacto (Identity = False)
            modelBuilder.Entity<Dim_Estudiante>().HasKey(e => e.Id_Estudiante);
            modelBuilder.Entity<Dim_Estudiante>().Property(e => e.Id_Estudiante).ValueGeneratedNever();

            modelBuilder.Entity<Dim_Curso>().HasKey(c => c.Id_Curso);
            modelBuilder.Entity<Dim_Curso>().Property(c => c.Id_Curso).ValueGeneratedNever();

            modelBuilder.Entity<Dim_Suscripcion>().HasKey(s => s.Id_Suscripcion);
            modelBuilder.Entity<Dim_Suscripcion>().Property(s => s.Id_Suscripcion).ValueGeneratedNever();

            modelBuilder.Entity<Dim_Tiempo>().HasKey(t => t.Id_Tiempo);
            modelBuilder.Entity<Dim_Tiempo>().Property(t => t.Id_Tiempo).ValueGeneratedNever();

            modelBuilder.Entity<Fact_Interacciones_Diarias>().HasKey(i => i.Id_Interaccion_Diaria);
            modelBuilder.Entity<Fact_Interacciones_Diarias>().Property(i => i.Id_Interaccion_Diaria).ValueGeneratedNever();

            modelBuilder.Entity<Fact_Rendimiento_Evaluaciones>().HasKey(r => r.Id_Rendimiento);
            modelBuilder.Entity<Fact_Rendimiento_Evaluaciones>().Property(r => r.Id_Rendimiento).ValueGeneratedNever();

            modelBuilder.Entity<Etl_Config>().HasKey(etl => etl.Id);
            modelBuilder.Entity<Etl_Config>().Property(etl => etl.Id).ValueGeneratedNever();
        }
    }
}