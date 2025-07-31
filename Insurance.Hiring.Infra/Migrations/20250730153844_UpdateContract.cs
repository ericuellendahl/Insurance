using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Insurance.Hiring.Infra.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "contract_status",
                schema: "insurance",
                table: "contracts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "contract_status",
                schema: "insurance",
                table: "contracts",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
