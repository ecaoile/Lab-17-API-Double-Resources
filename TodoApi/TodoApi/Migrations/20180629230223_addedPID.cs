using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoApi.Migrations
{
    public partial class addedPID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_TodoLists_TodoListID",
                table: "TodoItems");

            migrationBuilder.RenameColumn(
                name: "TodoListID",
                table: "TodoItems",
                newName: "DatListIDID");

            migrationBuilder.RenameIndex(
                name: "IX_TodoItems_TodoListID",
                table: "TodoItems",
                newName: "IX_TodoItems_DatListIDID");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_TodoLists_DatListIDID",
                table: "TodoItems",
                column: "DatListIDID",
                principalTable: "TodoLists",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_TodoLists_DatListIDID",
                table: "TodoItems");

            migrationBuilder.RenameColumn(
                name: "DatListIDID",
                table: "TodoItems",
                newName: "TodoListID");

            migrationBuilder.RenameIndex(
                name: "IX_TodoItems_DatListIDID",
                table: "TodoItems",
                newName: "IX_TodoItems_TodoListID");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_TodoLists_TodoListID",
                table: "TodoItems",
                column: "TodoListID",
                principalTable: "TodoLists",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
