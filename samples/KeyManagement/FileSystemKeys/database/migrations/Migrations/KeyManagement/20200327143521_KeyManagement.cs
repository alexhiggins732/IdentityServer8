/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

public partial class KeyManagement : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "DataProtectionKeys",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Created = table.Column<DateTime>(nullable: false),
                Name = table.Column<string>(maxLength: 200, nullable: true),
                Value = table.Column<string>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DataProtectionKeys", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "SigningKeys",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Created = table.Column<DateTime>(nullable: false),
                Name = table.Column<string>(maxLength: 200, nullable: true),
                Value = table.Column<string>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SigningKeys", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_DataProtectionKeys_Name",
            table: "DataProtectionKeys",
            column: "Name",
            unique: true,
            filter: "[Name] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_SigningKeys_Name",
            table: "SigningKeys",
            column: "Name",
            unique: true,
            filter: "[Name] IS NOT NULL");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "DataProtectionKeys");

        migrationBuilder.DropTable(
            name: "SigningKeys");
    }
}
