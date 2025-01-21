using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AchatService.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Achats",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateAchat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    user_Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achats", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "LignePaniers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    quantite_ligne = table.Column<int>(type: "int", nullable: false),
                    prixdevente = table.Column<double>(type: "float", nullable: false),
                    id_produit = table.Column<int>(type: "int", nullable: false),
                    id_achat = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LignePaniers", x => x.id);
                    table.ForeignKey(
                        name: "FK_LignePaniers_Achats_id_achat",
                        column: x => x.id_achat,
                        principalTable: "Achats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LignePaniers_id_achat",
                table: "LignePaniers",
                column: "id_achat");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LignePaniers");

            migrationBuilder.DropTable(
                name: "Achats");
        }
    }
}
