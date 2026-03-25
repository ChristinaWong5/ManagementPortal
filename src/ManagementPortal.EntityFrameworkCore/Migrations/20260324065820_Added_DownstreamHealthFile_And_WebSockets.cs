using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManagementPortal.Migrations
{
    /// <inheritdoc />
    public partial class Added_DownstreamHealthFile_And_WebSockets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DownstreamHealthFile",
                table: "AppDownloaders",
                type: "TEXT",
                nullable: true);

            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS ""AppDownloaderWebSockets"" (
                    ""Id"" TEXT NOT NULL CONSTRAINT ""PK_AppDownloaderWebSockets"" PRIMARY KEY,
                    ""DownloaderId"" TEXT NOT NULL,
                    ""Host"" TEXT NULL,
                    ""Port"" INTEGER NOT NULL,
                    ""CreationTime"" TEXT NOT NULL,
                    ""CreatorId"" TEXT NULL,
                    ""LastModificationTime"" TEXT NULL,
                    ""LastModifierId"" TEXT NULL,
                    ""IsDeleted"" INTEGER NOT NULL DEFAULT 0,
                    ""DeleterId"" TEXT NULL,
                    ""DeletionTime"" TEXT NULL,
                    CONSTRAINT ""FK_AppDownloaderWebSockets_AppDownloaders_DownloaderId""
                        FOREIGN KEY (""DownloaderId"") REFERENCES ""AppDownloaders"" (""Id"") ON DELETE CASCADE
                );
                CREATE INDEX IF NOT EXISTS ""IX_AppDownloaderWebSockets_DownloaderId""
                    ON ""AppDownloaderWebSockets"" (""DownloaderId"");
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TABLE IF EXISTS ""AppDownloaderWebSockets"";");

            migrationBuilder.DropColumn(
                name: "DownstreamHealthFile",
                table: "AppDownloaders");
        }
    }
}