using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNetCoreTodo.Data.Migrations
{
    public partial class namechanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TodoItemList",
                table: "TodoItemList");

            migrationBuilder.RenameTable(
                name: "TodoItemList",
                newName: "TodoList");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TodoList",
                table: "TodoList",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TodoList",
                table: "TodoList");

            migrationBuilder.RenameTable(
                name: "TodoList",
                newName: "TodoItemList");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TodoItemList",
                table: "TodoItemList",
                column: "Id");
        }
    }
}
