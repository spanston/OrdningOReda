using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNetCoreTodo.Data.Migrations
{
    public partial class tagoptionupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tag",
                table: "Items",
                newName: "ItemCategory");

            migrationBuilder.AddColumn<Guid>(
                name: "TodoItemId",
                table: "Items",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_TodoItemId",
                table: "Items",
                column: "TodoItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Items_TodoItemId",
                table: "Items",
                column: "TodoItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Items_TodoItemId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_TodoItemId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "TodoItemId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "ItemCategory",
                table: "Items",
                newName: "Tag");
        }
    }
}
