using Microsoft.EntityFrameworkCore.Migrations;

namespace ChessApp.DAL.Migrations
{
    public partial class SeedMusicsAndArtistsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Players (FullName) Values ('Kupriyanov')");
            migrationBuilder.Sql("INSERT INTO Players (FullName) Values ('Solnzev')");
            migrationBuilder.Sql("INSERT INTO Players (FullName) Values ('Gorohov')");
            migrationBuilder.Sql("INSERT INTO Players (FullName) Values ('Surkov')");

            migrationBuilder.Sql("INSERT INTO Games (Diagnosis, PatientId) Values ('win', (SELECT Id FROM Players WHERE FullName = 'Solnzev'))");
            migrationBuilder.Sql("INSERT INTO Games (Diagnosis, PatientId) Values ('win', (SELECT Id FROM Players WHERE FullName = 'Solnzev'))");
            migrationBuilder.Sql("INSERT INTO Games (Diagnosis, PatientId) Values ('win', (SELECT Id FROM Players WHERE FullName = 'Solnzev'))");
            migrationBuilder.Sql("INSERT INTO Games (Diagnosis, PatientId) Values ('win', (SELECT Id FROM Players WHERE FullName = 'Solnzev'))");
            migrationBuilder.Sql("INSERT INTO Games (Diagnosis, PatientId) Values ('win', (SELECT Id FROM Players WHERE FullName = 'Solnzev'))");
            migrationBuilder.Sql("INSERT INTO Games (Diagnosis, PatientId) Values ('lose', (SELECT Id FROM Players WHERE FullName = 'Gorohov'))");
            migrationBuilder.Sql("INSERT INTO Games (Diagnosis, PatientId) Values ('win', (SELECT Id FROM Players WHERE FullName = 'Surkov'))");
            migrationBuilder.Sql("INSERT INTO Games (Diagnosis, PatientId) Values ('win', (SELECT Id FROM Players WHERE FullName = 'Surkov'))");
            migrationBuilder.Sql("INSERT INTO Games (Diagnosis, PatientId) Values ('win', (SELECT Id FROM Players WHERE FullName = 'Surkov'))");
            migrationBuilder.Sql("INSERT INTO Games (Diagnosis, PatientId) Values ('lose', (SELECT Id FROM Players WHERE FullName = 'Surkov'))");
            migrationBuilder.Sql("INSERT INTO Games (Diagnosis, PatientId) Values ('lose', (SELECT Id FROM Players WHERE FullName = 'Surkov'))");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Games");
            migrationBuilder.Sql("DELETE FROM Players");
        }
    }
}
