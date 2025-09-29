param(
    [string]$Environment = "Development",
    [string]$Port = "5000",
    [string]$DbServer = "localhost",
    [string]$DbUser = "root",
    [string]$DbPassword = "password",
    [string]$Database = "MachineManagementDB"
)

Write-Host "🚀 Starting Machine Management Backend API..." -ForegroundColor Green

# Set environment variables
$env:ASPNETCORE_ENVIRONMENT = $Environment
$env:ASPNETCORE_URLS = "https://localhost:$($Port);http://localhost:$([int]$Port + 1000)"
$env:ConnectionStrings__DefaultConnection = "Server=$DbServer;Database=$Database;Uid=$DbUser;Pwd=$DbPassword;"

Write-Host "" -ForegroundColor White
Write-Host "📊 Configuration:" -ForegroundColor Cyan
Write-Host "  Environment: $Environment" -ForegroundColor White
Write-Host "  HTTPS Port: $Port" -ForegroundColor White
Write-Host "  HTTP Port: $([int]$Port + 1000)" -ForegroundColor White
Write-Host "  Database: $Database" -ForegroundColor White
Write-Host "" -ForegroundColor White

# Navigate to API project
Set-Location -Path "src/Backend/MachineManagement.API"

try {
    Write-Host "🔄 Building and starting the API..." -ForegroundColor Yellow
    Write-Host "" -ForegroundColor White
    
    # Build and run
    dotnet build
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ Build successful! Starting API server..." -ForegroundColor Green
        Write-Host "" -ForegroundColor White
        Write-Host "🌐 API will be available at:" -ForegroundColor Cyan
        Write-Host "  - Swagger UI: https://localhost:$Port" -ForegroundColor White
        Write-Host "  - Health Check: https://localhost:$Port/health" -ForegroundColor White
        Write-Host "  - API Base: https://localhost:$Port/api" -ForegroundColor White
        Write-Host "" -ForegroundColor White
        Write-Host "Press Ctrl+C to stop the server..." -ForegroundColor Yellow
        Write-Host "" -ForegroundColor White
        
        dotnet run
    }
    else {
        Write-Host "❌ Build failed!" -ForegroundColor Red
        exit 1
    }
}
catch {
    Write-Host "❌ Error starting API:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    exit 1
}
finally {
    Set-Location -Path "../../.."
}