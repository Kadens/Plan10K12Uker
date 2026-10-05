using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Plan10K12Uker.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "blocks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    focus = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    first_week = table.Column<int>(type: "integer", nullable: false),
                    last_week = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_blocks", x => x.id);
                    table.CheckConstraint("ck_blocks_weeks", "first_week >= 1 AND last_week <= 12 AND first_week <= last_week");
                });

            migrationBuilder.CreateTable(
                name: "nutrition_guidelines",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    meal = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    day_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_nutrition_guidelines", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "planned_sessions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    block_id = table.Column<int>(type: "integer", nullable: false),
                    day_of_week = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_planned_sessions", x => x.id);
                    table.ForeignKey(
                        name: "fk_planned_sessions_blocks_block_id",
                        column: x => x.block_id,
                        principalTable: "blocks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "blocks",
                columns: new[] { "id", "first_week", "focus", "last_week", "name" },
                values: new object[,]
                {
                    { 1, 1, "Bryte platå, øke VO2max", 4, "Blokk 1" },
                    { 2, 5, "Øke kapasitet, øke FTP", 8, "Blokk 2" },
                    { 3, 9, "Peak vinterform, maksimal fettmobilisering", 12, "Blokk 3" }
                });

            migrationBuilder.InsertData(
                table: "nutrition_guidelines",
                columns: new[] { "id", "day_type", "description", "meal" },
                values: new object[,]
                {
                    { 1, "Rest", "Protein shake + peanut butter + berries", "Breakfast" },
                    { 2, "Training", "Protein shake + peanut butter + banana or bread slice", "Breakfast" },
                    { 3, "Any", "Salad + eggs + mackerel + extra protein", "Lunch" },
                    { 4, "Any", "Chicken, cod or beef + vegetables + carbs after hard sessions", "Dinner" },
                    { 5, "Any", "Chocolate Saturdays, moderate alcohol", "Snacks" }
                });

            migrationBuilder.InsertData(
                table: "planned_sessions",
                columns: new[] { "id", "block_id", "day_of_week", "description" },
                values: new object[,]
                {
                    { 10, 1, 0, "Bike Z2 2–3 h or rest" },
                    { 11, 1, 1, "Run 4x4 intervals" },
                    { 12, 1, 2, "Strength: squat, deadlift, row, hip thrust" },
                    { 13, 1, 3, "Zwift Z2 60–75 min" },
                    { 14, 1, 4, "Zwift VO2 6x2 min" },
                    { 15, 1, 5, "Strength: lunge, step-up, press, pulldown" },
                    { 16, 1, 6, "Run Z2 5–10 km" },
                    { 20, 2, 0, "Bike Z2 or rest" },
                    { 21, 2, 1, "Run 4x4/5x4 progression" },
                    { 22, 2, 2, "Strength heavy: squat 5x5, deadlift 4x5" },
                    { 23, 2, 3, "Zwift Z2 60–90 min" },
                    { 24, 2, 4, "Zwift FTP 2x20 or 3x12" },
                    { 25, 2, 5, "Strength volume: lunge, step-ups, press, pulldown" },
                    { 26, 2, 6, "Run Z2 8–12 km" },
                    { 30, 3, 0, "Bike Z2 or rest" },
                    { 31, 3, 1, "Run VO2 10x1 or 6x3" },
                    { 32, 3, 2, "Strength maintenance" },
                    { 33, 3, 3, "Zwift Z2 45–75 min" },
                    { 34, 3, 4, "Zwift VO2 8x2 or 5x3" },
                    { 35, 3, 5, "Rest or light strength" },
                    { 36, 3, 6, "Run Z2 10–14 km" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_nutrition_guidelines_meal_day_type",
                table: "nutrition_guidelines",
                columns: new[] { "meal", "day_type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_planned_sessions_block_id_day_of_week",
                table: "planned_sessions",
                columns: new[] { "block_id", "day_of_week" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "nutrition_guidelines");

            migrationBuilder.DropTable(
                name: "planned_sessions");

            migrationBuilder.DropTable(
                name: "blocks");
        }
    }
}
