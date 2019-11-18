using Microsoft.EntityFrameworkCore.Migrations;

// The bottom two files tells the Entity framework at what point
// our db is at, which migration has been applied
// and what to do if we remove our migration
namespace Dating_App.API.Migrations
{
    public partial class InitialCreate : Migration
    {
        // Partial class thats been derived from Migration pkg
        protected override void Up(MigrationBuilder migrationBuilder)
        {
             // creates a table based from our DataContext
            // with 2 columns ID and Name
            migrationBuilder.CreateTable(
                name: "Values",
                columns: table => new
                {
                    // Entity Framework knows Id will be our primary 
                    // key based on the property name 'Id'
                    // Generates our numbers automatically because of <int>
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Values", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Values");
        }
    }
}
