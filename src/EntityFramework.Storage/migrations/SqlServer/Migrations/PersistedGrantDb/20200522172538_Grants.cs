/*
 Copyright (c) 2024, Martijn van Put - https://github.com/mvput/ 

 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace SqlServer.Migrations.PersistedGrantDb;

public partial class Grants : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "DeviceCodes",
            columns: table => new
            {
                UserCode = table.Column<string>(maxLength: 200, nullable: false),
                DeviceCode = table.Column<string>(maxLength: 200, nullable: false),
                SubjectId = table.Column<string>(maxLength: 200, nullable: true),
                SessionId = table.Column<string>(maxLength: 100, nullable: true),
                ClientId = table.Column<string>(maxLength: 200, nullable: false),
                Description = table.Column<string>(maxLength: 200, nullable: true),
                CreationTime = table.Column<DateTime>(nullable: false),
                Expiration = table.Column<DateTime>(nullable: false),
                Data = table.Column<string>(maxLength: 50000, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DeviceCodes", x => x.UserCode);
            });

        migrationBuilder.CreateTable(
            name: "PersistedGrants",
            columns: table => new
            {
                Key = table.Column<string>(maxLength: 200, nullable: false),
                Type = table.Column<string>(maxLength: 50, nullable: false),
                SubjectId = table.Column<string>(maxLength: 200, nullable: true),
                SessionId = table.Column<string>(maxLength: 100, nullable: true),
                ClientId = table.Column<string>(maxLength: 200, nullable: false),
                Description = table.Column<string>(maxLength: 200, nullable: true),
                CreationTime = table.Column<DateTime>(nullable: false),
                Expiration = table.Column<DateTime>(nullable: true),
                ConsumedTime = table.Column<DateTime>(nullable: true),
                Data = table.Column<string>(maxLength: 50000, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PersistedGrants", x => x.Key);
            });

        migrationBuilder.CreateIndex(
            name: "IX_DeviceCodes_DeviceCode",
            table: "DeviceCodes",
            column: "DeviceCode",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_DeviceCodes_Expiration",
            table: "DeviceCodes",
            column: "Expiration");

        migrationBuilder.CreateIndex(
            name: "IX_PersistedGrants_Expiration",
            table: "PersistedGrants",
            column: "Expiration");

        migrationBuilder.CreateIndex(
            name: "IX_PersistedGrants_SubjectId_ClientId_Type",
            table: "PersistedGrants",
            columns: new[] { "SubjectId", "ClientId", "Type" });

        migrationBuilder.CreateIndex(
            name: "IX_PersistedGrants_SubjectId_SessionId_Type",
            table: "PersistedGrants",
            columns: new[] { "SubjectId", "SessionId", "Type" });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "DeviceCodes");

        migrationBuilder.DropTable(
            name: "PersistedGrants");
    }
}
