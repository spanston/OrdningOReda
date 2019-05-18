using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNetCoreTodo.Data.Migrations
{
    public partial class getprioritytags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
