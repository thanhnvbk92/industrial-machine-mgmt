using Microsoft.EntityFrameworkCore;
using MachineManagement.Core.Entities;

namespace MachineManagement.Infrastructure.Data.SeedData;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(MachineManagementDbContext context)
    {
        // Check if data already exists
        if (await context.Buyers.AnyAsync())
        {
            return; // Data already seeded
        }

        // Create Buyers
        var buyers = new List<Buyer>
        {
            new() { 
                Name = "BMW Group", 
                Code = "BMW", 
                Description = "Bayerische Motoren Werke AG - Premium automotive manufacturer",
                CreatedAt = DateTime.UtcNow
            },
            new() { 
                Name = "Audi AG", 
                Code = "AUDI", 
                Description = "Premium automotive manufacturer - Part of Volkswagen Group",
                CreatedAt = DateTime.UtcNow
            },
            new() { 
                Name = "Mercedes-Benz", 
                Code = "MB", 
                Description = "Mercedes-Benz Group - Luxury automotive manufacturer",
                CreatedAt = DateTime.UtcNow
            },
            new() { 
                Name = "Volkswagen", 
                Code = "VW", 
                Description = "Volkswagen AG - Mass market automotive manufacturer",
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.Buyers.AddRangeAsync(buyers);
        await context.SaveChangesAsync();

        // Create Production Lines
        var productionLines = new List<ProductionLine>
        {
            // BMW Lines
            new() { Name = "Assembly Line 1", Code = "BMW-L1", BuyerId = buyers[0].Id, Description = "Main assembly line for BMW 3 Series", CreatedAt = DateTime.UtcNow },
            new() { Name = "Paint Line 1", Code = "BMW-P1", BuyerId = buyers[0].Id, Description = "Automated paint line for BMW vehicles", CreatedAt = DateTime.UtcNow },
            new() { Name = "Quality Control Line", Code = "BMW-QC1", BuyerId = buyers[0].Id, Description = "Final quality inspection line", CreatedAt = DateTime.UtcNow },
            
            // Audi Lines
            new() { Name = "Assembly Line A", Code = "AUDI-LA", BuyerId = buyers[1].Id, Description = "Main assembly line for Audi A4/A6", CreatedAt = DateTime.UtcNow },
            new() { Name = "Paint Line A", Code = "AUDI-PA", BuyerId = buyers[1].Id, Description = "Premium paint facility", CreatedAt = DateTime.UtcNow },
            
            // Mercedes Lines
            new() { Name = "C-Class Line", Code = "MB-CC", BuyerId = buyers[2].Id, Description = "C-Class production line", CreatedAt = DateTime.UtcNow },
            new() { Name = "E-Class Line", Code = "MB-EC", BuyerId = buyers[2].Id, Description = "E-Class production line", CreatedAt = DateTime.UtcNow },
            
            // Volkswagen Lines
            new() { Name = "Golf Line", Code = "VW-GL", BuyerId = buyers[3].Id, Description = "Golf model production line", CreatedAt = DateTime.UtcNow },
            new() { Name = "Passat Line", Code = "VW-PL", BuyerId = buyers[3].Id, Description = "Passat model production line", CreatedAt = DateTime.UtcNow }
        };

        await context.ProductionLines.AddRangeAsync(productionLines);
        await context.SaveChangesAsync();

        // Create Stations
        var stations = new List<Station>();
        foreach (var line in productionLines)
        {
            for (int i = 1; i <= 4; i++)
            {
                stations.Add(new Station
                {
                    Name = $"Station {i}",
                    Code = $"{line.Code}-S{i}",
                    ProductionLineId = line.Id,
                    Description = $"Work station {i} on {line.Name}",
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        await context.Stations.AddRangeAsync(stations);
        await context.SaveChangesAsync();

        // Create Machines
        var machines = new List<Machine>();
        var machineTypes = new[] { "CNC Miller", "Robotic Welder", "Paint Sprayer", "Assembly Robot", "Quality Scanner", "Drilling Station" };
        var manufacturers = new[] { "Siemens", "ABB", "KUKA", "Fanuc", "Bosch", "Schneider" };
        var statuses = new[] { MachineStatus.Running, MachineStatus.Idle, MachineStatus.Running, MachineStatus.Running };

        int machineCounter = 1;
        foreach (var station in stations)
        {
            var random = new Random(station.Id); // Deterministic randomness based on station ID
            var numMachines = random.Next(1, 4); // 1-3 machines per station

            for (int i = 1; i <= numMachines; i++)
            {
                var machineType = machineTypes[random.Next(machineTypes.Length)];
                var manufacturer = manufacturers[random.Next(manufacturers.Length)];
                var status = statuses[random.Next(statuses.Length)];

                machines.Add(new Machine
                {
                    Name = $"{station.Code}-M{i:D3}",
                    Code = $"M{machineCounter:D6}",
                    SerialNumber = $"SN{DateTime.UtcNow.Year}{machineCounter:D6}",
                    Model = $"{machineType} {random.Next(1000, 9999)}",
                    Manufacturer = manufacturer,
                    StationId = station.Id,
                    Status = status,
                    Description = $"{machineType} manufactured by {manufacturer}",
                    CreatedAt = DateTime.UtcNow.AddDays(-random.Next(30, 365)),
                    LastMaintenanceDate = DateTime.UtcNow.AddDays(-random.Next(1, 30)),
                    NextMaintenanceDate = DateTime.UtcNow.AddDays(random.Next(1, 90))
                });

                machineCounter++;
            }
        }

        await context.Machines.AddRangeAsync(machines);
        await context.SaveChangesAsync();

        // Create Sample Machine Logs
        var logs = new List<MachineLog>();
        var logLevels = new[] { "INFO", "WARNING", "ERROR", "DEBUG" };
        var logMessages = new[]
        {
            "Production cycle completed successfully",
            "Temperature within normal range",
            "Maintenance check performed",
            "Material feed detected",
            "Quality check passed",
            "Warning: High temperature detected",
            "Error: Sensor malfunction",
            "Debug: Motor speed adjusted",
            "Production started",
            "Production stopped",
            "Calibration completed"
        };

        foreach (var machine in machines.Take(20)) // Add logs for first 20 machines
        {
            var random = new Random(machine.Id);
            var numLogs = random.Next(5, 15);

            for (int i = 0; i < numLogs; i++)
            {
                var logLevel = logLevels[random.Next(logLevels.Length)];
                var message = logMessages[random.Next(logMessages.Length)];
                
                logs.Add(new MachineLog
                {
                    MachineId = machine.Id,
                    Timestamp = DateTime.UtcNow.AddHours(-random.Next(1, 24)).AddMinutes(-random.Next(0, 60)),
                    LogLevel = logLevel,
                    Message = message,
                    Details = logLevel == "ERROR" ? "Additional error details and stack trace..." : null,
                    Source = "Machine Controller",
                    Category = "Production",
                    MachineStatusAtTime = machine.Status
                });
            }
        }

        await context.MachineLogs.AddRangeAsync(logs);
        await context.SaveChangesAsync();
    }
}