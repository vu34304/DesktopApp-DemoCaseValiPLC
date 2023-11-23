using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoCaseGui.Core.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inverter_LOG",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _NAME = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: true),
                    _NUMERICID = table.Column<int>(type: "int", nullable: true),
                    _VALUE = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: true),
                    _TIMESTAMP = table.Column<DateTime>(type: "datetime", nullable: false),
                    _QUALITY = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inverter_LOG", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ValiCompact_LOG",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _NAME = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: true),
                    _NUMERICID = table.Column<int>(type: "int", nullable: true),
                    _VALUE = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: true),
                    _TIMESTAMP = table.Column<DateTime>(type: "datetime", nullable: false),
                    _QUALITY = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValiCompact_LOG", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ValiIFM_LOG",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _NAME = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: true),
                    _NUMERICID = table.Column<int>(type: "int", nullable: true),
                    _VALUE = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: true),
                    _TIMESTAMP = table.Column<DateTime>(type: "datetime", nullable: false),
                    _QUALITY = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValiIFM_LOG", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ValiMicro_LOG",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _NAME = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: true),
                    _NUMERICID = table.Column<int>(type: "int", nullable: true),
                    _VALUE = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: true),
                    _TIMESTAMP = table.Column<DateTime>(type: "datetime", nullable: false),
                    _QUALITY = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValiMicro_LOG", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ValiSiemens_LOG",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _NAME = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: true),
                    _NUMERICID = table.Column<int>(type: "int", nullable: true),
                    _VALUE = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: true),
                    _TIMESTAMP = table.Column<DateTime>(type: "datetime", nullable: false),
                    _QUALITY = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValiSiemens_LOG", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inverter_LOG");

            migrationBuilder.DropTable(
                name: "ValiCompact_LOG");

            migrationBuilder.DropTable(
                name: "ValiIFM_LOG");

            migrationBuilder.DropTable(
                name: "ValiMicro_LOG");

            migrationBuilder.DropTable(
                name: "ValiSiemens_LOG");
        }
    }
}
