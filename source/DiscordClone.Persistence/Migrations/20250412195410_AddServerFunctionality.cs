using System;
using DiscordClone.Domain.Entities.Consultation.ServerEntities;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordClone.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddServerFunctionality : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "server_id",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "servers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    image_path = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    banner_image_path = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_servers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "server_invite_urls",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    server_id = table.Column<Guid>(type: "uuid", nullable: false),
                    uri_parameter = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    amount_of_uses = table.Column<int>(type: "integer", nullable: false),
                    uses = table.Column<int>(type: "integer", nullable: false),
                    valid_till = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_server_invite_urls", x => x.id);
                    table.ForeignKey(
                        name: "fk_server_invite_urls_servers_server_id",
                        column: x => x.server_id,
                        principalTable: "servers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "server_members",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    server_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_owner = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_server_members", x => new { x.user_id, x.server_id });
                    table.ForeignKey(
                        name: "fk_server_members_servers_server_id",
                        column: x => x.server_id,
                        principalTable: "servers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_server_members_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "server_nodes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    type = table.Column<string>(type: "text", maxLength: 50, nullable: false),
                    channel_topic = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    is_private = table.Column<bool>(type: "boolean", nullable: false),
                    is_age_restricted = table.Column<bool>(type: "boolean", nullable: false),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: false),
                    server_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_server_nodes", x => x.id);
                    table.ForeignKey(
                        name: "fk_server_nodes_server_nodes_parent_id",
                        column: x => x.parent_id,
                        principalTable: "server_nodes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_server_nodes_servers_server_id",
                        column: x => x.server_id,
                        principalTable: "servers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "server_roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    server_id = table.Column<Guid>(type: "uuid", nullable: false),
                    permissions = table.Column<ServerRolePermissions>(type: "jsonb", nullable: false),
                    server_member_server_id = table.Column<Guid>(type: "uuid", nullable: true),
                    server_member_user_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_server_roles", x => x.id);
                    table.ForeignKey(
                        name: "fk_server_roles_server_members_server_member_user_id_server_me",
                        columns: x => new { x.server_member_user_id, x.server_member_server_id },
                        principalTable: "server_members",
                        principalColumns: new[] { "user_id", "server_id" });
                    table.ForeignKey(
                        name: "fk_server_roles_servers_server_id",
                        column: x => x.server_id,
                        principalTable: "servers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_messages_receiver",
                table: "messages",
                column: "receiver");

            migrationBuilder.CreateIndex(
                name: "ix_messages_sender",
                table: "messages",
                column: "sender");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_users_server_id",
                table: "AspNetUsers",
                column: "server_id");

            migrationBuilder.CreateIndex(
                name: "ix_server_invite_urls_server_id",
                table: "server_invite_urls",
                column: "server_id");

            migrationBuilder.CreateIndex(
                name: "ix_server_invite_urls_uri_parameter",
                table: "server_invite_urls",
                column: "uri_parameter");

            migrationBuilder.CreateIndex(
                name: "ix_server_members_server_id",
                table: "server_members",
                column: "server_id");

            migrationBuilder.CreateIndex(
                name: "ix_server_nodes_parent_id",
                table: "server_nodes",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_server_nodes_server_id",
                table: "server_nodes",
                column: "server_id");

            migrationBuilder.CreateIndex(
                name: "ix_server_roles_server_id",
                table: "server_roles",
                column: "server_id");

            migrationBuilder.CreateIndex(
                name: "ix_server_roles_server_member_user_id_server_member_server_id",
                table: "server_roles",
                columns: new[] { "server_member_user_id", "server_member_server_id" });

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_users_servers_server_id",
                table: "AspNetUsers",
                column: "server_id",
                principalTable: "servers",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_users_servers_server_id",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "server_invite_urls");

            migrationBuilder.DropTable(
                name: "server_nodes");

            migrationBuilder.DropTable(
                name: "server_roles");

            migrationBuilder.DropTable(
                name: "server_members");

            migrationBuilder.DropTable(
                name: "servers");

            migrationBuilder.DropIndex(
                name: "ix_messages_receiver",
                table: "messages");

            migrationBuilder.DropIndex(
                name: "ix_messages_sender",
                table: "messages");

            migrationBuilder.DropIndex(
                name: "ix_asp_net_users_server_id",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "server_id",
                table: "AspNetUsers");
        }
    }
}
