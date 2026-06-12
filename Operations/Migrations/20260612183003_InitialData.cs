using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Operations.Migrations
{
    /// <inheritdoc />
    public partial class InitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dim_Curso",
                columns: table => new
                {
                    Id_Curso = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nivel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cantidad_Lecciones = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dim_Curso", x => x.Id_Curso);
                });

            migrationBuilder.CreateTable(
                name: "Dim_Estudiante",
                columns: table => new
                {
                    Id_Estudiante = table.Column<int>(type: "int", nullable: false),
                    Nombre_Completo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pais = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fecha_Registro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dim_Estudiante", x => x.Id_Estudiante);
                });

            migrationBuilder.CreateTable(
                name: "Dim_Suscripcion",
                columns: table => new
                {
                    Id_Suscripcion = table.Column<int>(type: "int", nullable: false),
                    Tipo_Acceso = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado_Suscripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dim_Suscripcion", x => x.Id_Suscripcion);
                });

            migrationBuilder.CreateTable(
                name: "Dim_Tiempo",
                columns: table => new
                {
                    Id_Tiempo = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Anio = table.Column<int>(type: "int", nullable: false),
                    Trimestre = table.Column<int>(type: "int", nullable: false),
                    Mes = table.Column<int>(type: "int", nullable: false),
                    Nombre_Mes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dia = table.Column<int>(type: "int", nullable: false),
                    Dia_Semana = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dim_Tiempo", x => x.Id_Tiempo);
                });

            migrationBuilder.CreateTable(
                name: "Etl_Config",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    BeginDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Update_At = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etl_Config", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fact_Interacciones_Diarias",
                columns: table => new
                {
                    Id_Interaccion_Diaria = table.Column<long>(type: "bigint", nullable: false),
                    Id_Estudiante = table.Column<int>(type: "int", nullable: false),
                    Id_Curso = table.Column<int>(type: "int", nullable: false),
                    Id_Tiempo = table.Column<int>(type: "int", nullable: false),
                    Tiempo_Visualizacion_Minutos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cantidad_Reproducciones_Video = table.Column<int>(type: "int", nullable: false),
                    Preguntas_Realizadas = table.Column<int>(type: "int", nullable: false),
                    Respuestas_Dadas = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fact_Interacciones_Diarias", x => x.Id_Interaccion_Diaria);
                });

            migrationBuilder.CreateTable(
                name: "Fact_Rendimiento_Evaluaciones",
                columns: table => new
                {
                    Id_Rendimiento = table.Column<long>(type: "bigint", nullable: false),
                    Id_Estudiante = table.Column<int>(type: "int", nullable: false),
                    Id_Curso = table.Column<int>(type: "int", nullable: false),
                    Id_Suscripcion = table.Column<int>(type: "int", nullable: false),
                    Id_Tiempo = table.Column<int>(type: "int", nullable: false),
                    Calificacion_Final = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quizzes_Completados = table.Column<int>(type: "int", nullable: false),
                    Progreso_Actual_Porcentaje = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Curso_Finalizado = table.Column<byte>(type: "tinyint", nullable: false),
                    Es_Abandono_Temprano = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fact_Rendimiento_Evaluaciones", x => x.Id_Rendimiento);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dim_Curso");

            migrationBuilder.DropTable(
                name: "Dim_Estudiante");

            migrationBuilder.DropTable(
                name: "Dim_Suscripcion");

            migrationBuilder.DropTable(
                name: "Dim_Tiempo");

            migrationBuilder.DropTable(
                name: "Etl_Config");

            migrationBuilder.DropTable(
                name: "Fact_Interacciones_Diarias");

            migrationBuilder.DropTable(
                name: "Fact_Rendimiento_Evaluaciones");
        }
    }
}
