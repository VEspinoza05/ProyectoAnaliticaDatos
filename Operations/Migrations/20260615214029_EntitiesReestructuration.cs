using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Operations.Migrations
{
    /// <inheritdoc />
    public partial class EntitiesReestructuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dim_curso",
                columns: table => new
                {
                    id_curso = table.Column<int>(type: "int", nullable: false),
                    titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    categoria = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    nivel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    rating_promedio = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: false),
                    precio_base = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dim_curso", x => x.id_curso);
                });

            migrationBuilder.CreateTable(
                name: "dim_dispositivo",
                columns: table => new
                {
                    id_dispositivo = table.Column<int>(type: "int", nullable: false),
                    tipo_dispositivo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sistema_operativo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dim_dispositivo", x => x.id_dispositivo);
                });

            migrationBuilder.CreateTable(
                name: "dim_estudiante",
                columns: table => new
                {
                    id_estudiante = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    pais = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    fecha_registro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dim_estudiante", x => x.id_estudiante);
                });

            migrationBuilder.CreateTable(
                name: "dim_promocion",
                columns: table => new
                {
                    id_promocion = table.Column<int>(type: "int", nullable: false),
                    nombre_promocion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    porcentaje_descuento = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    tipo_campania = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dim_promocion", x => x.id_promocion);
                });

            migrationBuilder.CreateTable(
                name: "dim_tiempo",
                columns: table => new
                {
                    id_tiempo = table.Column<int>(type: "int", nullable: false),
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dia = table.Column<int>(type: "int", nullable: false),
                    semana = table.Column<int>(type: "int", nullable: false),
                    mes = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    anio = table.Column<int>(type: "int", nullable: false),
                    trimestre = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dim_tiempo", x => x.id_tiempo);
                });

            migrationBuilder.CreateTable(
                name: "etl_config",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BeginDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Update_At = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_etl_config", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "fact_evaluaciones",
                columns: table => new
                {
                    id_evaluacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_estudiante = table.Column<int>(type: "int", nullable: false),
                    id_curso = table.Column<int>(type: "int", nullable: false),
                    id_tiempo = table.Column<int>(type: "int", nullable: false),
                    id_dispositivo = table.Column<int>(type: "int", nullable: false),
                    calificacion_obtenida = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    intentos_realizados = table.Column<int>(type: "int", nullable: false),
                    aprobado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fact_evaluaciones", x => x.id_evaluacion);
                    table.ForeignKey(
                        name: "FK_fact_evaluaciones_dim_curso_id_curso",
                        column: x => x.id_curso,
                        principalTable: "dim_curso",
                        principalColumn: "id_curso",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_fact_evaluaciones_dim_dispositivo_id_dispositivo",
                        column: x => x.id_dispositivo,
                        principalTable: "dim_dispositivo",
                        principalColumn: "id_dispositivo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_fact_evaluaciones_dim_estudiante_id_estudiante",
                        column: x => x.id_estudiante,
                        principalTable: "dim_estudiante",
                        principalColumn: "id_estudiante",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_fact_evaluaciones_dim_tiempo_id_tiempo",
                        column: x => x.id_tiempo,
                        principalTable: "dim_tiempo",
                        principalColumn: "id_tiempo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "fact_interacciones_progreso",
                columns: table => new
                {
                    id_interaccion_progreso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_estudiante = table.Column<int>(type: "int", nullable: false),
                    id_curso = table.Column<int>(type: "int", nullable: false),
                    id_tiempo = table.Column<int>(type: "int", nullable: false),
                    id_dispositivo = table.Column<int>(type: "int", nullable: false),
                    tiempo_permanencia_segundos = table.Column<int>(type: "int", nullable: false),
                    videos_vistos = table.Column<int>(type: "int", nullable: false),
                    modulos_completados_count = table.Column<int>(type: "int", nullable: false),
                    porcentaje_progreso_acumulado = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fact_interacciones_progreso", x => x.id_interaccion_progreso);
                    table.ForeignKey(
                        name: "FK_fact_interacciones_progreso_dim_curso_id_curso",
                        column: x => x.id_curso,
                        principalTable: "dim_curso",
                        principalColumn: "id_curso",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_fact_interacciones_progreso_dim_dispositivo_id_dispositivo",
                        column: x => x.id_dispositivo,
                        principalTable: "dim_dispositivo",
                        principalColumn: "id_dispositivo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_fact_interacciones_progreso_dim_estudiante_id_estudiante",
                        column: x => x.id_estudiante,
                        principalTable: "dim_estudiante",
                        principalColumn: "id_estudiante",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_fact_interacciones_progreso_dim_tiempo_id_tiempo",
                        column: x => x.id_tiempo,
                        principalTable: "dim_tiempo",
                        principalColumn: "id_tiempo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "fact_ventas_inscripciones",
                columns: table => new
                {
                    id_venta_inscripcion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_estudiante = table.Column<int>(type: "int", nullable: false),
                    id_curso = table.Column<int>(type: "int", nullable: false),
                    id_tiempo = table.Column<int>(type: "int", nullable: false),
                    id_promocion = table.Column<int>(type: "int", nullable: true),
                    id_dispositivo = table.Column<int>(type: "int", nullable: false),
                    monto_pagado = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    completado = table.Column<int>(type: "int", nullable: false),
                    progreso_final_porcentaje = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    dias_para_terminar = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fact_ventas_inscripciones", x => x.id_venta_inscripcion);
                    table.ForeignKey(
                        name: "FK_fact_ventas_inscripciones_dim_curso_id_curso",
                        column: x => x.id_curso,
                        principalTable: "dim_curso",
                        principalColumn: "id_curso",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_fact_ventas_inscripciones_dim_dispositivo_id_dispositivo",
                        column: x => x.id_dispositivo,
                        principalTable: "dim_dispositivo",
                        principalColumn: "id_dispositivo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_fact_ventas_inscripciones_dim_estudiante_id_estudiante",
                        column: x => x.id_estudiante,
                        principalTable: "dim_estudiante",
                        principalColumn: "id_estudiante",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_fact_ventas_inscripciones_dim_promocion_id_promocion",
                        column: x => x.id_promocion,
                        principalTable: "dim_promocion",
                        principalColumn: "id_promocion");
                    table.ForeignKey(
                        name: "FK_fact_ventas_inscripciones_dim_tiempo_id_tiempo",
                        column: x => x.id_tiempo,
                        principalTable: "dim_tiempo",
                        principalColumn: "id_tiempo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_fact_evaluaciones_id_curso",
                table: "fact_evaluaciones",
                column: "id_curso");

            migrationBuilder.CreateIndex(
                name: "IX_fact_evaluaciones_id_dispositivo",
                table: "fact_evaluaciones",
                column: "id_dispositivo");

            migrationBuilder.CreateIndex(
                name: "IX_fact_evaluaciones_id_estudiante",
                table: "fact_evaluaciones",
                column: "id_estudiante");

            migrationBuilder.CreateIndex(
                name: "IX_fact_evaluaciones_id_tiempo",
                table: "fact_evaluaciones",
                column: "id_tiempo");

            migrationBuilder.CreateIndex(
                name: "IX_fact_interacciones_progreso_id_curso",
                table: "fact_interacciones_progreso",
                column: "id_curso");

            migrationBuilder.CreateIndex(
                name: "IX_fact_interacciones_progreso_id_dispositivo",
                table: "fact_interacciones_progreso",
                column: "id_dispositivo");

            migrationBuilder.CreateIndex(
                name: "IX_fact_interacciones_progreso_id_estudiante",
                table: "fact_interacciones_progreso",
                column: "id_estudiante");

            migrationBuilder.CreateIndex(
                name: "IX_fact_interacciones_progreso_id_tiempo",
                table: "fact_interacciones_progreso",
                column: "id_tiempo");

            migrationBuilder.CreateIndex(
                name: "IX_fact_ventas_inscripciones_id_curso",
                table: "fact_ventas_inscripciones",
                column: "id_curso");

            migrationBuilder.CreateIndex(
                name: "IX_fact_ventas_inscripciones_id_dispositivo",
                table: "fact_ventas_inscripciones",
                column: "id_dispositivo");

            migrationBuilder.CreateIndex(
                name: "IX_fact_ventas_inscripciones_id_estudiante",
                table: "fact_ventas_inscripciones",
                column: "id_estudiante");

            migrationBuilder.CreateIndex(
                name: "IX_fact_ventas_inscripciones_id_promocion",
                table: "fact_ventas_inscripciones",
                column: "id_promocion");

            migrationBuilder.CreateIndex(
                name: "IX_fact_ventas_inscripciones_id_tiempo",
                table: "fact_ventas_inscripciones",
                column: "id_tiempo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "etl_config");

            migrationBuilder.DropTable(
                name: "fact_evaluaciones");

            migrationBuilder.DropTable(
                name: "fact_interacciones_progreso");

            migrationBuilder.DropTable(
                name: "fact_ventas_inscripciones");

            migrationBuilder.DropTable(
                name: "dim_curso");

            migrationBuilder.DropTable(
                name: "dim_dispositivo");

            migrationBuilder.DropTable(
                name: "dim_estudiante");

            migrationBuilder.DropTable(
                name: "dim_promocion");

            migrationBuilder.DropTable(
                name: "dim_tiempo");
        }
    }
}
