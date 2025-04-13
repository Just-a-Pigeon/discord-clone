using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordClone.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SetServerNodeParentIdToNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_server_nodes_server_nodes_parent_id",
                table: "server_nodes");

            migrationBuilder.AlterColumn<Guid>(
                name: "parent_id",
                table: "server_nodes",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "fk_server_nodes_server_nodes_parent_id",
                table: "server_nodes",
                column: "parent_id",
                principalTable: "server_nodes",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_server_nodes_server_nodes_parent_id",
                table: "server_nodes");

            migrationBuilder.AlterColumn<Guid>(
                name: "parent_id",
                table: "server_nodes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_server_nodes_server_nodes_parent_id",
                table: "server_nodes",
                column: "parent_id",
                principalTable: "server_nodes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
