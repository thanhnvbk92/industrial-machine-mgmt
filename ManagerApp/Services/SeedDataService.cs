using Microsoft.EntityFrameworkCore;
using ManagerApp.Data;
using ManagerApp.Models;

namespace ManagerApp.Services;

public class SeedDataService
{
    public static async Task SeedDatabaseAsync(ApplicationDbContext context)
    {
        await context.Database.EnsureCreatedAsync();
        
        if (!await context.Machines.AnyAsync())
        {
            // Add sample machines
            var machines = new[]
            {
                new Machine
                {
                    Name = "Production Line A",
                    Description = "Main assembly line for product A",
                    Location = "Factory Floor 1",
                    Status = MachineStatus.Online,
                    LastHeartbeat = DateTime.UtcNow.AddMinutes(-2),
                    Version = "v2.1.0",
                    IpAddress = "192.168.1.101",
                    IsConnected = true,
                },
                new Machine
                {
                    Name = "Quality Control Station",
                    Description = "Automated quality inspection system",
                    Location = "Factory Floor 1", 
                    Status = MachineStatus.Online,
                    LastHeartbeat = DateTime.UtcNow.AddMinutes(-1),
                    Version = "v1.8.5",
                    IpAddress = "192.168.1.102",
                    IsConnected = true,
                },
                new Machine
                {
                    Name = "Packaging Unit B",
                    Description = "Automated packaging and labeling",
                    Location = "Factory Floor 2",
                    Status = MachineStatus.Warning,
                    LastHeartbeat = DateTime.UtcNow.AddMinutes(-15),
                    Version = "v1.9.2",
                    IpAddress = "192.168.1.103",
                    IsConnected = true,
                },
                new Machine
                {
                    Name = "Conveyor System",
                    Description = "Main transport conveyor belt",
                    Location = "Factory Floor 1",
                    Status = MachineStatus.Online,
                    LastHeartbeat = DateTime.UtcNow.AddSeconds(-30),
                    Version = "v3.0.1",
                    IpAddress = "192.168.1.104",
                    IsConnected = true,
                },
                new Machine
                {
                    Name = "Maintenance Robot",
                    Description = "Automated maintenance and cleaning robot",
                    Location = "Factory Floor 2",
                    Status = MachineStatus.Maintenance,
                    LastHeartbeat = DateTime.UtcNow.AddHours(-2),
                    Version = "v1.5.3",
                    IpAddress = "192.168.1.105",
                    IsConnected = false,
                }
            };

            await context.Machines.AddRangeAsync(machines);
            await context.SaveChangesAsync();

            // Add sample log entries
            var machineIds = await context.Machines.Select(m => m.Id).ToListAsync();
            var logEntries = new List<LogEntry>();

            foreach (var machineId in machineIds)
            {
                logEntries.AddRange(new[]
                {
                    new LogEntry
                    {
                        MachineId = machineId,
                        Level = Models.LogLevel.Info,
                        Message = "System started successfully",
                        Timestamp = DateTime.UtcNow.AddHours(-6),
                        Source = "SystemController"
                    },
                    new LogEntry
                    {
                        MachineId = machineId,
                        Level = Models.LogLevel.Info,
                        Message = "Operational parameters within normal range",
                        Timestamp = DateTime.UtcNow.AddMinutes(-30),
                        Source = "MonitoringService"
                    },
                    new LogEntry
                    {
                        MachineId = machineId,
                        Level = machineId == 3 ? Models.LogLevel.Warning : Models.LogLevel.Info,
                        Message = machineId == 3 ? "Temperature sensor reading elevated" : "Heartbeat signal received",
                        Timestamp = DateTime.UtcNow.AddMinutes(-5),
                        Source = "SensorController"
                    }
                });
            }

            await context.LogEntries.AddRangeAsync(logEntries);
            await context.SaveChangesAsync();

            // Add sample commands
            var commands = new[]
            {
                new Command
                {
                    MachineId = machineIds[0],
                    CommandType = "START_PRODUCTION",
                    Parameters = "{\"batchSize\":100, \"speed\":\"normal\"}",
                    Status = CommandStatus.Completed,
                    CreatedAt = DateTime.UtcNow.AddHours(-4),
                    ExecutedAt = DateTime.UtcNow.AddHours(-4).AddMinutes(1),
                    CompletedAt = DateTime.UtcNow.AddHours(-4).AddMinutes(2),
                    Result = "Production started successfully",
                    CreatedBy = "admin"
                },
                new Command
                {
                    MachineId = machineIds[1],
                    CommandType = "CALIBRATE_SENSORS",
                    Parameters = "{\"sensorType\":\"temperature\"}",
                    Status = CommandStatus.Pending,
                    CreatedAt = DateTime.UtcNow.AddMinutes(-10),
                    CreatedBy = "operator1"
                },
                new Command
                {
                    MachineId = machineIds[2],
                    CommandType = "RESET_WARNINGS",
                    Status = CommandStatus.Running,
                    CreatedAt = DateTime.UtcNow.AddMinutes(-5),
                    ExecutedAt = DateTime.UtcNow.AddMinutes(-4),
                    CreatedBy = "technician"
                }
            };

            await context.Commands.AddRangeAsync(commands);
            await context.SaveChangesAsync();
        }
    }
}