using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGA_ITLA.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Transporte");

            migrationBuilder.CreateTable(
                name: "Autobuses",
                schema: "Transporte",
                columns: table => new
                {
                    AutobusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FichaInstitucional = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Placa = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CapacidadFisica = table.Column<int>(type: "int", nullable: false),
                    EstaOperativo = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationUser = table.Column<int>(type: "int", nullable: false),
                    ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserMod = table.Column<int>(type: "int", nullable: true),
                    UserDeleted = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autobuses", x => x.AutobusId);
                });

            migrationBuilder.CreateTable(
                name: "Rutas",
                schema: "Transporte",
                columns: table => new
                {
                    RutaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreRuta = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PuntoOrigen = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PuntoDestino = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationUser = table.Column<int>(type: "int", nullable: false),
                    ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserMod = table.Column<int>(type: "int", nullable: true),
                    UserDeleted = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rutas", x => x.RutaId);
                });

            migrationBuilder.CreateTable(
                name: "Viajes",
                schema: "Transporte",
                columns: table => new
                {
                    ViajeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RutaId = table.Column<int>(type: "int", nullable: false),
                    AutobusId = table.Column<int>(type: "int", nullable: false),
                    ConductorId = table.Column<int>(type: "int", nullable: false),
                    HorarioSalida = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstadoViaje = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationUser = table.Column<int>(type: "int", nullable: false),
                    ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserMod = table.Column<int>(type: "int", nullable: true),
                    UserDeleted = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Viajes", x => x.ViajeId);
                    table.ForeignKey(
                        name: "FK_Viajes_Autobuses_AutobusId",
                        column: x => x.AutobusId,
                        principalSchema: "Transporte",
                        principalTable: "Autobuses",
                        principalColumn: "AutobusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Viajes_Rutas_RutaId",
                        column: x => x.RutaId,
                        principalSchema: "Transporte",
                        principalTable: "Rutas",
                        principalColumn: "RutaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Viajes_AutobusId",
                schema: "Transporte",
                table: "Viajes",
                column: "AutobusId");

            migrationBuilder.CreateIndex(
                name: "IX_Viajes_RutaId",
                schema: "Transporte",
                table: "Viajes",
                column: "RutaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Viajes",
                schema: "Transporte");

            migrationBuilder.DropTable(
                name: "Autobuses",
                schema: "Transporte");

            migrationBuilder.DropTable(
                name: "Rutas",
                schema: "Transporte");
        }
    }
}
