using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMS_api.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    company_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    company_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    package = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.company_id);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    roll_number = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.roll_number);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    userName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    pass = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.userName);
                });

            migrationBuilder.CreateTable(
                name: "AllowedStudents",
                columns: table => new
                {
                    company_id = table.Column<long>(type: "bigint", nullable: false),
                    student_roll_no = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllowedStudents", x => new { x.student_roll_no, x.company_id });
                    table.ForeignKey(
                        name: "FK_AllowedStudents_Companies_company_id",
                        column: x => x.company_id,
                        principalTable: "Companies",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AllowedStudents_Student_student_roll_no",
                        column: x => x.student_roll_no,
                        principalTable: "Student",
                        principalColumn: "roll_number",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Placed",
                columns: table => new
                {
                    roll_number = table.Column<long>(type: "bigint", nullable: false),
                    company_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Placed", x => x.roll_number);
                    table.ForeignKey(
                        name: "FK_Placed_Companies_company_id",
                        column: x => x.company_id,
                        principalTable: "Companies",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Placed_Student_roll_number",
                        column: x => x.roll_number,
                        principalTable: "Student",
                        principalColumn: "roll_number",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "userName", "pass" },
                values: new object[] { "admin", "1234" });

            migrationBuilder.CreateIndex(
                name: "IX_AllowedStudents_company_id",
                table: "AllowedStudents",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_Placed_company_id",
                table: "Placed",
                column: "company_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllowedStudents");

            migrationBuilder.DropTable(
                name: "Placed");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Student");
        }
    }
}
