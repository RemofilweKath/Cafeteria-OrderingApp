﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CafeteriaOrderingApp.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnEmployeesDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyDepositBalance",
                table: "Employees",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MonthlyDepositBalance",
                table: "Employees");
        }
    }
}
