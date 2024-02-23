using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Test_FilestreamSQL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                });
            //sometimes won't work for me so use that comands manually at MSLQMSM query
            //and of course you have to configure db from mssql as filestream database
            //https://learn.microsoft.com/en-us/sql/relational-databases/blob/create-a-table-for-storing-filestream-data?view=sql-server-ver16
            migrationBuilder.Sql("alter table [dbo].[Attachments] add [RowId] uniqueidentifier rowguidcol not null");
            migrationBuilder.Sql("alter table [dbo].[Attachments] add constraint [UQ_Attachment_RowId] UNIQUE NONCLUSTERED ([RowId])");
            migrationBuilder.Sql("alter table [dbo].[Attachments] add constraint [DF_Attachment_RowId] default (newid()) for [RowId]");
            migrationBuilder.Sql("alter table [dbo].[Attachments] add [Data] varbinary(max) FILESTREAM not null");
            migrationBuilder.Sql("alter table [dbo].[Attachments] add constraint [DF_Attachment_Data] default(0x) for [Data]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("alter table [dbo].[Attachments] drop constraint [DF_Attachment_Data]");
            migrationBuilder.Sql("alter table [dbo].[Attachments] drop column [Data]");
            migrationBuilder.Sql("alter table [dbo].[Attachments] drop constraint [UQ_Attachment_RowId]");
            migrationBuilder.Sql("alter table [dbo].[Attachments] drop constraint [DF_Attachment_RowId]");
            migrationBuilder.Sql("alter table [dbo].[Attachments] drop column [RowId]");
            migrationBuilder.DropTable(
                name: "Attachments");
        }
    }
}
