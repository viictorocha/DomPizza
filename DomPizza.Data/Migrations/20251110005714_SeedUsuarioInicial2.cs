using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomPizza.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsuarioInicial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "SenhaHash",
                value: "AQAAAAIAAYagAAAAEFyhC+iz5ABqnQQcYhzlmStnBaOGghPJOJPyK5jTrFo1Blt5ceMGtZ7+JuBSZ7daYg==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "SenhaHash",
                value: "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92");
        }
    }
}
