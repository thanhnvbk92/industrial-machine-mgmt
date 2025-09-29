# 🏭 Machine Management System

> **Complete industrial solution with Backend API (.NET 8), WPF Client, and Blazor Manager Web for production monitoring**

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![MySQL](https://img.shields.io/badge/MySQL-8.0+-4479A1?style=flat-square&logo=mysql&logoColor=white)](https://www.mysql.com/)
[![Blazor](https://img.shields.io/badge/Blazor-512BD4?style=flat-square&logo=blazor)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![WPF](https://img.shields.io/badge/WPF-.NET%208-512BD4?style=flat-square&logo=windows)](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/)

## 📋 Overview

A comprehensive machine management system designed for industrial environments, featuring real-time monitoring, hierarchical organization (Buyers → Production Lines → Stations → Machines), and multi-client architecture.

## 🏗️ Architecture

```
machine-management-system/
├── src/
│   ├── Backend/                     # ✅ Backend API (.NET 8)
│   │   ├── MachineManagement.Core/  # Domain entities, interfaces
│   │   ├── MachineManagement.Infrastructure/ # Data access, repositories
│   │   └── MachineManagement.API/   # Web API controllers, middleware
│   ├── ClientApp/                   # WPF Desktop Client
│   │   └── MachineClient.WPF/       # WPF app with Material Design
│   └── ManagerApp/                  # ✅ Blazor Manager Web
│       └── MachineManager.Web/      # Web-based management interface
├── SRS_Documents/                   # System Requirements Specification
├── .github/workflows/               # GitHub Actions CI/CD
├── setup-database.ps1               # ✅ Database setup script
├── run-backend.ps1                  # ✅ Backend run script
└── README.md                        # This file
```

## 🛠️ Technical Stack

### Backend API ✅
- **Framework**: ASP.NET Core Web API 8.0
- **Database**: MySQL 8.0+ with Entity Framework Core
- **Architecture**: Clean Architecture with Domain-Driven Design
- **Features**: RESTful APIs, Swagger documentation, Health checks
- **Logging**: Serilog with structured logging
- **ORM**: Entity Framework Core with Pomelo MySQL provider

### Blazor Manager Web ✅
- **Framework**: Blazor Server (.NET 8)
- **UI**: Bootstrap with modern responsive design
- **Real-time**: SignalR integration ready
- **Features**: Machine management dashboard, reporting interface

### WPF Desktop Client (Structure Ready)
- **Framework**: WPF with .NET 8-windows
- **Pattern**: MVVM with CommunityToolkit.Mvvm
- **UI**: Material Design with MaterialDesignThemes
- **Services**: HTTP communication, log collection, configuration management

### Database ✅
- **Engine**: MySQL 8.0+
- **Design**: Hierarchical structure (BUYERS→LINES→STATIONS→MACHINES)
- **Features**: Entity relationships, proper indexing, audit trails
- **Migrations**: Entity Framework Code-First approach

## 🚀 Quick Start

### Prerequisites
- .NET 8 SDK
- MySQL Server 8.0+
- Visual Studio 2022 or VS Code
- PowerShell (for setup scripts)

### 1. Clone Repository
```bash
git clone https://github.com/thanhnvbk92/industrial-machine-mgmt.git
cd industrial-machine-mgmt
```

### 2. Setup Database
```powershell
# PowerShell
./setup-database.ps1 -Username "root" -Password "your_mysql_password"
```

### 3. Run Backend API
```powershell
# PowerShell
./run-backend.ps1
```

The API will be available at:
- **Swagger UI**: https://localhost:5000
- **Health Check**: https://localhost:5000/health
- **API Base**: https://localhost:5000/api

### 4. Run Blazor Manager Web
```bash
cd src/ManagerApp/MachineManager.Web
dotnet run
```

Access at: https://localhost:5001

## 📊 Database Schema

### Hierarchical Structure
```
Buyers (BMW, Audi, Mercedes, VW)
  └── Production Lines (Assembly, Painting, Quality)
      └── Stations (Station A, B, C)
          └── Machines (Machine details with status)
              └── Machine Logs (Real-time logging)
```

### Core Entities
- **Buyer**: Top-level organization (automotive manufacturers)
- **ProductionLine**: Manufacturing lines within buyer facilities  
- **Station**: Work stations along production lines
- **Machine**: Individual machines with status tracking
- **MachineLog**: Time-series logging for machine events

## 🔌 API Endpoints

### Machine Management
```http
GET    /api/machines              # Get all machines
GET    /api/machines/{id}         # Get specific machine
POST   /api/machines              # Create new machine
PUT    /api/machines/{id}         # Update machine
DELETE /api/machines/{id}         # Delete machine
PATCH  /api/machines/{id}/status  # Update machine status
GET    /api/machines/station/{id} # Get machines by station
```

### Logging
```http
GET  /api/logs/recent?count=100        # Get recent logs
GET  /api/logs/machine/{id}            # Get logs for specific machine
GET  /api/logs/daterange?start&end     # Get logs by date range
POST /api/logs                         # Add new log entry
```

### Organization
```http
GET  /api/buyers                    # Get all buyers
GET  /api/buyers/with-lines         # Get buyers with production lines
POST /api/buyers                    # Create new buyer
```

### System Health
```http
GET /health        # Health check endpoint
GET /api/health    # Detailed health status
```

## 🔧 Configuration

### Database Connection
Update `appsettings.json` in the API project:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MachineManagementDB;Uid=root;Pwd=your_password;"
  }
}
```

### Logging Configuration
Serilog is configured to log to both console and files:
```json
{
  "Serilog": {
    "MinimumLevel": "Information",
    "WriteTo": [
      { "Name": "Console" },
      { "Name": "File", "Args": { "path": "logs/machine-api-.log" } }
    ]
  }
}
```

## 🏃‍♂️ Development

### Building the Solution
```bash
dotnet build
```

### Running Tests
```bash
dotnet test
```

### Database Migrations
```bash
cd src/Backend/MachineManagement.API
dotnet ef migrations add MigrationName
dotnet ef database update
```

## 📈 Current Status

### ✅ Completed Components
- [x] **Backend API**: Complete with controllers, services, repositories
- [x] **Database Layer**: Entity Framework with MySQL provider
- [x] **Domain Models**: Hierarchical structure implemented
- [x] **Swagger Documentation**: Auto-generated API docs
- [x] **Health Checks**: Database connectivity monitoring
- [x] **Logging Infrastructure**: Serilog with structured logging
- [x] **Blazor Manager**: Basic web interface created
- [x] **Setup Scripts**: PowerShell automation scripts
- [x] **Clean Architecture**: Proper separation of concerns

### 🔄 In Development
- [ ] **WPF Client**: Implementation pending
- [ ] **SignalR Integration**: Real-time communication
- [ ] **Authentication/Authorization**: Security implementation
- [ ] **Advanced Reporting**: Analytics and dashboards
- [ ] **Docker Containerization**: Deployment containers
- [ ] **CI/CD Pipeline**: GitHub Actions workflows

### 🎯 Next Steps
1. Complete WPF client implementation
2. Add SignalR for real-time updates
3. Implement authentication system
4. Create sample data seeding
5. Add comprehensive unit tests
6. Setup Docker containers
7. Configure CI/CD pipeline

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📞 Support

For support and questions:
- Create an issue in this repository
- Email: support@machinemanagement.com (placeholder)
- Documentation: See `/docs` folder for detailed guides

---

**Built with ❤️ for industrial automation and monitoring**
