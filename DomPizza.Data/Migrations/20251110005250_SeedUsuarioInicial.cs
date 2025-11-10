using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomPizza.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsuarioInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Email", "Nome", "SenhaHash" },
                values: new object[] { 1, "DomPizza@adm", "Administrador DomPizza", "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92" });

            migrationBuilder.InsertData(
                table: "UsuarioRoles",
                columns: new[] { "RoleId", "UsuarioId" },
                values: new object[] { 1, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UsuarioRoles",
                keyColumns: new[] { "RoleId", "UsuarioId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
