using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace signalRchat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Friendship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Friendships",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FriendId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DateOfFriendship = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendships", x => new { x.UserId, x.FriendId });
                    table.ForeignKey(
                        name: "FK_Friendships_AspNetUsers_FriendId",
                        column: x => x.FriendId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Friendships_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2870e842-b346-4bf5-bd46-4df9f539b66b", "AQAAAAIAAYagAAAAEGmKoQ6MNdlkmXRRa29hLWBcfROsfiG5qjH+lFwDi4Y24Of7VqE4ECCMJ583u9l7Cw==", "b5f3994d-081c-4d69-a26b-1082bae19d6a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d0dc17e4-3f93-4233-b950-7d0aad19fac7", "AQAAAAIAAYagAAAAEG5Mw2+MONiAyXBYlPhYNZRI2RgE/G5drf2eL7ixDnbznLwfK1qetb7YPd10nlSptw==", "a7e326e0-7719-4518-bf4b-3f0ec1c5d7dc" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ac2a01dd-af5f-4eb1-b258-1e2af19e4138", "AQAAAAIAAYagAAAAEIzpD+LyEx8n50GAPAQRdSvL6jSKOygV8Ms9yCeIkn2yB1uAhrk87RB9eYzwS2GuNQ==", "cd9d146f-0990-4edc-8fc1-855e39cdcdb7" });

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_FriendId",
                table: "Friendships",
                column: "FriendId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friendships");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5caec33b-1e24-4af7-8250-b70630b483a4", "AQAAAAIAAYagAAAAEG2A0dFmV6vj5eAiUc3e8DaCOhuU6ZCI0OxTfVDoDsMRE8DvhdW0/m0HDbrt4EN9ZA==", "4de3d709-66a3-4ad5-923a-01fdc20dc0c8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "831b86d7-a7b9-454e-b7ca-8bf07610c2f6", "AQAAAAIAAYagAAAAEIUXmmFF9BmBp1REKj9rMEDMlExcQL7ev6ObHD+/eC1eVHYYLQwH1jH9xRvltzIvSA==", "0be98631-64ee-4566-8637-1760a3475c1d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000000"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fe41fe1e-905a-48e9-937b-58ac3ee2b53b", "AQAAAAIAAYagAAAAEFVypOp+Y5cnDUvXQDaIAXJ9mkDsjWJ6rqTM+BGY8wuqdHI6+tgFbTvH7ZGXGZeXdQ==", "45eb5c51-ba2b-4904-82b8-1994d4199a37" });
        }
    }
}
