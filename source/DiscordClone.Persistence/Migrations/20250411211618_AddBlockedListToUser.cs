using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordClone.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBlockedListToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "application_user_id",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_users_application_user_id",
                table: "AspNetUsers",
                column: "application_user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_users_asp_net_users_application_user_id",
                table: "AspNetUsers",
                column: "application_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_users_asp_net_users_application_user_id",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "ix_asp_net_users_application_user_id",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "application_user_id",
                table: "AspNetUsers");
        }
    }
}
