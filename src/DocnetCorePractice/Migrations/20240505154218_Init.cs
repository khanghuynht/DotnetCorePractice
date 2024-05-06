using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocnetCorePractice.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CaffeEntity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Discount = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreateTimes = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateTimes = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateUser = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateUser = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaffeEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderItemEntity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaffeId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaffeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Volumn = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<double>(type: "float", nullable: false),
                    ItemPrice = table.Column<double>(type: "float", nullable: false),
                    IsActice = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateTimes = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateTimes = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateUser = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateUser = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Sex = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VerificationToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetTokenExpire = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Balance = table.Column<double>(type: "float", nullable: false),
                    TotalProduct = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Roles = table.Column<int>(type: "int", nullable: false),
                    CreateTimes = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateTimes = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateUser = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateUser = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderEntity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TotalPrice = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateTimes = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateTimes = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateUser = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateUser = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderEntity_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokensEntity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokensEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokensEntity_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderEntity_UserId",
                table: "OrderEntity",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokensEntity_UserId",
                table: "RefreshTokensEntity",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaffeEntity");

            migrationBuilder.DropTable(
                name: "OrderEntity");

            migrationBuilder.DropTable(
                name: "OrderItemEntity");

            migrationBuilder.DropTable(
                name: "RefreshTokensEntity");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
