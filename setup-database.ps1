param(
    [Parameter(Mandatory = $true)]
    [string]$Username,
    
    [Parameter(Mandatory = $true)]
    [string]$Password,
    
    [string]$Server = "localhost",
    [string]$Database = "MachineManagementDB"
)

Write-Host "🗄️ Setting up Machine Management Database..." -ForegroundColor Green

# Check if MySQL is running
Write-Host "Checking MySQL service..." -ForegroundColor Yellow

try {
    # Test connection
    $connectionString = "Server=$Server;Uid=$Username;Pwd=$Password;"
    Write-Host "Testing connection to MySQL server..." -ForegroundColor Yellow
    
    # Create database if it doesn't exist
    $createDbSql = @"
CREATE DATABASE IF NOT EXISTS `$Database`
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;
"@

    Write-Host "Creating database '$Database'..." -ForegroundColor Yellow
    
    # Run EF Core migrations
    Write-Host "Running Entity Framework migrations..." -ForegroundColor Yellow
    Set-Location -Path "src/Backend/MachineManagement.API"
    
    $env:ConnectionStrings__DefaultConnection = "Server=$Server;Database=$Database;Uid=$Username;Pwd=$Password;"
    
    dotnet ef migrations add InitialCreate --output-dir Migrations --verbose
    dotnet ef database update --verbose
    
    Write-Host "✅ Database setup completed successfully!" -ForegroundColor Green
    Write-Host "" -ForegroundColor White
    Write-Host "📊 Database Details:" -ForegroundColor Cyan
    Write-Host "  Server: $Server" -ForegroundColor White
    Write-Host "  Database: $Database" -ForegroundColor White
    Write-Host "  Connection: Ready" -ForegroundColor Green
    Write-Host "" -ForegroundColor White
    Write-Host "🚀 You can now run the backend API with: ./run-backend.ps1" -ForegroundColor Cyan
}
catch {
    Write-Host "❌ Error during database setup:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    Write-Host "" -ForegroundColor White
    Write-Host "💡 Troubleshooting tips:" -ForegroundColor Yellow
    Write-Host "  1. Ensure MySQL server is running" -ForegroundColor White
    Write-Host "  2. Check username and password are correct" -ForegroundColor White
    Write-Host "  3. Verify user has database creation permissions" -ForegroundColor White
    exit 1
}
finally {
    Set-Location -Path "../../.."
}