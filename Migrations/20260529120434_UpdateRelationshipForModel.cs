using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicManagement.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationshipForModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Songs_ComposerId",
                table: "Songs",
                column: "ComposerId");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_SingerId",
                table: "Songs",
                column: "SingerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Composers_ComposerId",
                table: "Songs",
                column: "ComposerId",
                principalTable: "Composers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Singers_SingerId",
                table: "Songs",
                column: "SingerId",
                principalTable: "Singers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Composers_ComposerId",
                table: "Songs");

            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Singers_SingerId",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_Songs_ComposerId",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_Songs_SingerId",
                table: "Songs");
        }
    }
}
