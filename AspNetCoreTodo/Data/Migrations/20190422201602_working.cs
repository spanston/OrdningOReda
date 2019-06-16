using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNetCoreTodo.Data.Migrations
{
    public partial class working : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriorityTags_Items_TodoItemId",
                table: "ItemCategory");

            migrationBuilder.DropIndex(
                name: "IX_PriorityTags_TodoItemId",
                table: "ItemCategory");

            migrationBuilder.DropColumn(
                name: "TodoItemId",
                table: "ItemCategory");

            migrationBuilder.RenameColumn(
                name: "ItemCategory",
                table: "ItemCategory",
                newName: "CategoryName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "ItemCategory",
                newName: "ItemCategory");

            migrationBuilder.AddColumn<Guid>(
                name: "TodoItemId",
                table: "ItemCategory",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PriorityTags_TodoItemId",
                table: "ItemCategory",
                column: "TodoItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_PriorityTags_Items_TodoItemId",
                table: "ItemCategory",
                column: "TodoItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
