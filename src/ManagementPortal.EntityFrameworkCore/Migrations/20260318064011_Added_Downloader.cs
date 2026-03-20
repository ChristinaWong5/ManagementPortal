using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManagementPortal.Migrations
{
    /// <inheritdoc />
    public partial class Added_Downloader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "AppDownloaderWebSockets");

            migrationBuilder.CreateTable(
                name: "AppDownloaders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ExtraProperties = table.Column<string>(type: "TEXT", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatorId = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DownloaderEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    DownloaderPollarName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppDownloaders", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppDownloaders");

            //migrationBuilder.CreateTable(
            //    name: "AppDownloaderWebSockets",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "TEXT", nullable: false),
            //        ConcurrencyStamp = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
            //        Host = table.Column<string>(type: "TEXT", nullable: false),
            //        Port = table.Column<int>(type: "INTEGER", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AppDownloaderWebSockets", x => x.Id);
            //    });
        }
    }
}

