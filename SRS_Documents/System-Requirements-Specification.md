# System Requirements Specification (SRS)
## Machine Management System

### Document Information
- **Version**: 1.0
- **Date**: December 2024
- **Author**: Machine Management Team
- **Status**: Draft

---

## 1. Introduction

### 1.1 Purpose
This document specifies the requirements for the Machine Management System, an industrial monitoring and control solution designed for automotive manufacturing environments.

### 1.2 Scope
The system provides real-time monitoring, logging, and management capabilities for industrial machines organized in a hierarchical structure (Buyers → Production Lines → Stations → Machines).

### 1.3 Definitions
- **Buyer**: Top-level organization (e.g., BMW, Audi, Mercedes, VW)
- **Production Line**: Manufacturing line within a buyer's facility
- **Station**: Individual work station on a production line
- **Machine**: Physical equipment that performs manufacturing tasks

## 2. Overall Description

### 2.1 Product Perspective
The Machine Management System is a distributed solution consisting of:
- Backend REST API (.NET 8)
- Web Management Interface (Blazor Server)
- Desktop Client Application (WPF)
- MySQL Database

### 2.2 Product Functions
- Real-time machine status monitoring
- Historical data logging and analysis
- Hierarchical organization management
- Alert and notification system
- Reporting and analytics
- Remote machine control capabilities

### 2.3 User Classes
- **System Administrators**: Full system access and configuration
- **Plant Managers**: Production oversight and reporting
- **Operators**: Machine monitoring and basic control
- **Maintenance Staff**: Machine diagnostics and maintenance scheduling

## 3. System Features

### 3.1 Machine Monitoring
**Description**: Real-time monitoring of machine status, performance metrics, and operational parameters.

**Functional Requirements**:
- FR-1.1: Display current status of all machines (Running, Idle, Maintenance, Error, Warning)
- FR-1.2: Show real-time performance metrics (temperature, cycles, efficiency)
- FR-1.3: Provide hierarchical navigation (Buyer → Line → Station → Machine)
- FR-1.4: Support filtering and searching by various criteria

### 3.2 Logging System
**Description**: Comprehensive logging of machine events, status changes, and system activities.

**Functional Requirements**:
- FR-2.1: Log all machine state changes with timestamps
- FR-2.2: Support multiple log levels (INFO, WARNING, ERROR, DEBUG)
- FR-2.3: Provide log querying and filtering capabilities
- FR-2.4: Export logs in various formats (CSV, JSON, XML)

### 3.3 Data Management
**Description**: Centralized data storage and management for all system entities.

**Functional Requirements**:
- FR-3.1: Maintain buyer, line, station, and machine information
- FR-3.2: Support CRUD operations for all entities
- FR-3.3: Ensure data consistency and integrity
- FR-3.4: Provide data backup and recovery mechanisms

### 3.4 API Services
**Description**: RESTful API providing programmatic access to system functionality.

**Functional Requirements**:
- FR-4.1: Provide REST endpoints for all major operations
- FR-4.2: Support authentication and authorization
- FR-4.3: Include comprehensive API documentation (Swagger)
- FR-4.4: Implement proper error handling and status codes

## 4. Non-Functional Requirements

### 4.1 Performance
- NFR-1.1: Support up to 1000 concurrent machines
- NFR-1.2: API response time < 200ms for standard operations
- NFR-1.3: Handle 10,000+ log entries per hour
- NFR-1.4: Web interface load time < 3 seconds

### 4.2 Reliability
- NFR-2.1: System uptime > 99.9%
- NFR-2.2: Automatic failover for critical components
- NFR-2.3: Data loss tolerance < 0.01%

### 4.3 Security
- NFR-3.1: All API endpoints require authentication
- NFR-3.2: Data encryption in transit (HTTPS/TLS)
- NFR-3.3: Role-based access control
- NFR-3.4: Audit logging for security events

### 4.4 Scalability
- NFR-4.1: Horizontal scaling capability
- NFR-4.2: Database partitioning support
- NFR-4.3: Load balancing support
- NFR-4.4: Container deployment ready

## 5. System Architecture

### 5.1 High-Level Architecture
```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   WPF Desktop   │    │  Blazor Web App │    │  Mobile Apps    │
│     Client      │    │    (Manager)    │    │   (Future)      │
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                       │                       │
         └───────────────────────┼───────────────────────┘
                                 │
                    ┌─────────────────┐
                    │   REST API      │
                    │   (.NET 8)      │
                    └─────────────────┘
                                 │
                    ┌─────────────────┐
                    │  MySQL Database │
                    │   (Persistent)  │
                    └─────────────────┘
```

### 5.2 Technology Stack
- **Backend**: .NET 8, ASP.NET Core Web API, Entity Framework Core
- **Database**: MySQL 8.0+
- **Web UI**: Blazor Server, Bootstrap, Font Awesome
- **Desktop UI**: WPF, Material Design, MVVM Pattern
- **Logging**: Serilog with structured logging
- **Documentation**: Swagger/OpenAPI
- **Containerization**: Docker, Docker Compose

## 6. Data Model

### 6.1 Entity Relationship Diagram
```
Buyer (1) ──→ (N) ProductionLine (1) ──→ (N) Station (1) ──→ (N) Machine
                                                                    │
                                                                    │ (1)
                                                                    │
                                                                    ↓
                                                              (N) MachineLog
```

### 6.2 Key Entities
- **Buyer**: Id, Name, Code, Description, IsActive, CreatedAt, UpdatedAt
- **ProductionLine**: Id, Name, Code, Description, BuyerId, IsActive, CreatedAt, UpdatedAt
- **Station**: Id, Name, Code, Description, ProductionLineId, IsActive, CreatedAt, UpdatedAt
- **Machine**: Id, Name, Code, SerialNumber, Model, Manufacturer, StationId, Status, Description, IsActive, CreatedAt, UpdatedAt, LastMaintenanceDate, NextMaintenanceDate
- **MachineLog**: Id, MachineId, Timestamp, LogLevel, Message, Details, Source, Category, MachineStatusAtTime

## 7. API Specification

### 7.1 Base URL
- Development: `https://localhost:5000/api`
- Production: `https://api.machinemanagement.com/api`

### 7.2 Core Endpoints

#### Machines
- `GET /machines` - Get all machines
- `GET /machines/{id}` - Get specific machine
- `POST /machines` - Create new machine
- `PUT /machines/{id}` - Update machine
- `DELETE /machines/{id}` - Delete machine
- `PATCH /machines/{id}/status` - Update machine status
- `GET /machines/station/{id}` - Get machines by station

#### Logs
- `GET /logs/recent?count=100` - Get recent logs
- `GET /logs/machine/{id}` - Get logs for specific machine
- `GET /logs/daterange?start&end` - Get logs by date range
- `POST /logs` - Add new log entry

#### Buyers
- `GET /buyers` - Get all buyers
- `GET /buyers/with-lines` - Get buyers with production lines
- `POST /buyers` - Create new buyer

#### System
- `GET /health` - Health check endpoint

## 8. User Interface Requirements

### 8.1 Web Interface (Blazor)
- Responsive design supporting desktop and tablet
- Dashboard with key metrics and alerts
- Machine management interface with filtering/search
- Real-time status updates
- Log viewing and analysis tools

### 8.2 Desktop Interface (WPF)
- Native Windows application
- Material Design components
- Real-time machine monitoring
- Local log collection and forwarding
- Offline capability with sync when online

## 9. Deployment Requirements

### 9.1 Environment Support
- Windows Server 2019/2022
- Linux (Ubuntu 20.04+, CentOS 8+)
- Docker containers
- Cloud platforms (Azure, AWS, GCP)

### 9.2 Dependencies
- .NET 8 Runtime
- MySQL 8.0+ Server
- Reverse proxy (nginx/IIS) for production
- SSL certificates for HTTPS

## 10. Testing Requirements

### 10.1 Unit Testing
- Business logic coverage > 80%
- Repository pattern testing
- Service layer validation

### 10.2 Integration Testing
- API endpoint testing
- Database integration tests
- End-to-end workflow testing

### 10.3 Performance Testing
- Load testing for expected concurrent users
- Stress testing for system limits
- Database performance under load

## 11. Maintenance and Support

### 11.1 Monitoring
- Application performance monitoring
- Database performance metrics
- System health checks
- Log aggregation and analysis

### 11.2 Backup and Recovery
- Daily database backups
- Configuration backup
- Disaster recovery procedures
- Data retention policies

---

**Document Control**
- Last Updated: December 2024
- Review Cycle: Quarterly
- Next Review: March 2025