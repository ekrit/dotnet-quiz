using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiz.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kviz",
                columns: table => new
                {
                    KvizId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kviz", x => x.KvizId);
                });

            migrationBuilder.CreateTable(
                name: "RecikliranoPitanje",
                columns: table => new
                {
                    RecikliranoPitanjeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sadrzaj = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Odgovor = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecikliranoPitanje", x => x.RecikliranoPitanjeId);
                });

            migrationBuilder.CreateTable(
                name: "Pitanje",
                columns: table => new
                {
                    PitanjeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sadrzaj = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Odgovor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KvizId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pitanje", x => x.PitanjeId);
                    table.ForeignKey(
                        name: "FK_Pitanje_Kviz_KvizId",
                        column: x => x.KvizId,
                        principalTable: "Kviz",
                        principalColumn: "KvizId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pitanje_KvizId",
                table: "Pitanje",
                column: "KvizId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pitanje");

            migrationBuilder.DropTable(
                name: "RecikliranoPitanje");

            migrationBuilder.DropTable(
                name: "Kviz");
        }
    }
}
