using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Insurance.Hiring.Infra.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContractId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "proposal_id",
                schema: "insurance",
                table: "contracts",
                newName: "propost_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "propost_id",
                schema: "insurance",
                table: "contracts",
                newName: "proposal_id");
        }
    }
}
