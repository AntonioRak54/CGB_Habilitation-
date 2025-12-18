using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGB_Habilitation.Migrations
{
    /// <inheritdoc />
    public partial class InitOracle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agences",
                columns: table => new
                {
                    CodeAgence = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    NomAgence = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    AdresseAgence = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agences", x => x.CodeAgence);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IdRole = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    NomRole = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IdRole);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    CodeService = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    NomService = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TypeService = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.CodeService);
                });

            migrationBuilder.CreateTable(
                name: "Agents",
                columns: table => new
                {
                    IdAgent = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    NomAgent = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    PrenomAgent = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    EmailAgent = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    LoginAgent = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    PasswordAgent = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    SousCaisseAgent = table.Column<int>(type: "NUMBER(1)", nullable: false),
                    DateLogin = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EstValide = table.Column<int>(type: "NUMBER(1)", nullable: false),
                    IdRole = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    CodeAgence = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    CodeService = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.IdAgent);
                    table.ForeignKey(
                        name: "FK_Agents_Agences_CodeAgence",
                        column: x => x.CodeAgence,
                        principalTable: "Agences",
                        principalColumn: "CodeAgence",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Agents_Roles_IdRole",
                        column: x => x.IdRole,
                        principalTable: "Roles",
                        principalColumn: "IdRole",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Agents_Services_CodeService",
                        column: x => x.CodeService,
                        principalTable: "Services",
                        principalColumn: "CodeService",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Habilitations",
                columns: table => new
                {
                    IdHabilitation = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    TypeHabilitation = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Etat = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    DateCreationDemande = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DateDebut = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DateFin = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    Motif = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    PieceJoint = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    IdAgent = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    CreerPar = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    TraiterPar = table.Column<Guid>(type: "RAW(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Habilitations", x => x.IdHabilitation);
                    table.ForeignKey(
                        name: "FK_Habilitations_Agents_IdAgent",
                        column: x => x.IdAgent,
                        principalTable: "Agents",
                        principalColumn: "IdAgent",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agents_CodeAgence",
                table: "Agents",
                column: "CodeAgence");

            migrationBuilder.CreateIndex(
                name: "IX_Agents_CodeService",
                table: "Agents",
                column: "CodeService");

            migrationBuilder.CreateIndex(
                name: "IX_Agents_IdRole",
                table: "Agents",
                column: "IdRole");

            migrationBuilder.CreateIndex(
                name: "IX_Habilitations_IdAgent",
                table: "Habilitations",
                column: "IdAgent");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Habilitations");

            migrationBuilder.DropTable(
                name: "Agents");

            migrationBuilder.DropTable(
                name: "Agences");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Services");
        }
    }
}
