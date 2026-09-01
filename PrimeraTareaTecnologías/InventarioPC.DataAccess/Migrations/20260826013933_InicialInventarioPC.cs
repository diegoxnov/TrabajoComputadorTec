using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InventarioPC.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InicialInventarioPC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Armados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PrecioTotal = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Armados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Categoria = table.Column<string>(type: "text", nullable: false),
                    Marca = table.Column<string>(type: "text", nullable: false),
                    Precio = table.Column<decimal>(type: "numeric", nullable: false),
                    Stock = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArmadoDetalles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ArmadoId = table.Column<int>(type: "integer", nullable: false),
                    ProductoId = table.Column<int>(type: "integer", nullable: false),
                    Cantidad = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArmadoDetalles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArmadoDetalles_Armados_ArmadoId",
                        column: x => x.ArmadoId,
                        principalTable: "Armados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArmadoDetalles_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArmadoDetalles_ArmadoId",
                table: "ArmadoDetalles",
                column: "ArmadoId");

            migrationBuilder.CreateIndex(
                name: "IX_ArmadoDetalles_ProductoId",
                table: "ArmadoDetalles",
                column: "ProductoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArmadoDetalles");

            migrationBuilder.DropTable(
                name: "Armados");

            migrationBuilder.DropTable(
                name: "Productos");
        }
    }
}
