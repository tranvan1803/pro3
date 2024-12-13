using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShowroomManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddImagePathToVehicles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Thêm cột ImagePath vào bảng Vehicles nếu chưa tồn tại
            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = 'Vehicles' AND COLUMN_NAME = 'ImagePath'
                )
                THEN
                    ALTER TABLE `Vehicles` ADD `ImagePath` LONGTEXT CHARACTER SET utf8mb4 NULL;
                END IF;
            ");

            // Thêm cột Avatar vào bảng AspNetUsers nếu chưa tồn tại
            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = 'AspNetUsers' AND COLUMN_NAME = 'Avatar'
                )
                THEN
                    ALTER TABLE `AspNetUsers` ADD `Avatar` LONGTEXT CHARACTER SET utf8mb4 NOT NULL;
                END IF;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Xóa cột ImagePath khỏi bảng Vehicles nếu tồn tại
            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT 1 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = 'Vehicles' AND COLUMN_NAME = 'ImagePath'
                )
                THEN
                    ALTER TABLE `Vehicles` DROP COLUMN `ImagePath`;
                END IF;
            ");

            // Xóa cột Avatar khỏi bảng AspNetUsers nếu tồn tại
            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT 1 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = 'AspNetUsers' AND COLUMN_NAME = 'Avatar'
                )
                THEN
                    ALTER TABLE `AspNetUsers` DROP COLUMN `Avatar`;
                END IF;
            ");
        }
    }
}
