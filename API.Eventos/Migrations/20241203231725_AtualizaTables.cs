using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Eventos.Migrations
{
    /// <inheritdoc />
    public partial class AtualizaTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Locais_LocalId",
                table: "Eventos");

            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Organizadores_OrganizadorId",
                table: "Eventos");

            migrationBuilder.DropForeignKey(
                name: "FK_Inscricoes_Participantes_ParticipanteId",
                table: "Inscricoes");

            migrationBuilder.DropIndex(
                name: "IX_Inscricoes_ParticipanteId",
                table: "Inscricoes");

            migrationBuilder.DropIndex(
                name: "IX_Eventos_LocalId",
                table: "Eventos");

            migrationBuilder.DropIndex(
                name: "IX_Eventos_OrganizadorId",
                table: "Eventos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Inscricoes_ParticipanteId",
                table: "Inscricoes",
                column: "ParticipanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_LocalId",
                table: "Eventos",
                column: "LocalId");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_OrganizadorId",
                table: "Eventos",
                column: "OrganizadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Locais_LocalId",
                table: "Eventos",
                column: "LocalId",
                principalTable: "Locais",
                principalColumn: "LocalId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Organizadores_OrganizadorId",
                table: "Eventos",
                column: "OrganizadorId",
                principalTable: "Organizadores",
                principalColumn: "OrganizadorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inscricoes_Participantes_ParticipanteId",
                table: "Inscricoes",
                column: "ParticipanteId",
                principalTable: "Participantes",
                principalColumn: "ParticipanteId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
