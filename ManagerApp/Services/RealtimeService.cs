using Microsoft.AspNetCore.SignalR;
using ManagerApp.Hubs;
using ManagerApp.Models;

namespace ManagerApp.Services;

public class RealtimeService : IRealtimeService
{
    private readonly IHubContext<MachineHub> _machineHub;
    private readonly IHubContext<LogHub> _logHub;
    private readonly IHubContext<CommandHub> _commandHub;
    private readonly IHubContext<NotificationHub> _notificationHub;

    public RealtimeService(
        IHubContext<MachineHub> machineHub,
        IHubContext<LogHub> logHub,
        IHubContext<CommandHub> commandHub,
        IHubContext<NotificationHub> notificationHub)
    {
        _machineHub = machineHub;
        _logHub = logHub;
        _commandHub = commandHub;
        _notificationHub = notificationHub;
    }

    public async Task BroadcastMachineStatusAsync(int machineId, MachineStatus status)
    {
        await _machineHub.Clients.Group($"Machine_{machineId}")
            .SendAsync("MachineStatusUpdated", machineId, status.ToString());
        
        await _machineHub.Clients.All
            .SendAsync("MachineStatusChanged", machineId, status.ToString());
    }

    public async Task BroadcastNewLogAsync(LogEntry logEntry)
    {
        await _logHub.Clients.Group($"LogStream_{logEntry.MachineId}")
            .SendAsync("NewLogEntry", new
            {
                logEntry.Id,
                logEntry.MachineId,
                Level = logEntry.Level.ToString(),
                logEntry.Message,
                logEntry.Timestamp,
                logEntry.Source,
                MachineName = logEntry.Machine?.Name ?? "Unknown"
            });
            
        await _logHub.Clients.All
            .SendAsync("LogEntryAdded", new
            {
                logEntry.Id,
                logEntry.MachineId,
                Level = logEntry.Level.ToString(),
                logEntry.Message,
                logEntry.Timestamp,
                logEntry.Source,
                MachineName = logEntry.Machine?.Name ?? "Unknown"
            });
    }

    public async Task BroadcastCommandStatusAsync(int commandId, CommandStatus status)
    {
        await _commandHub.Clients.All
            .SendAsync("CommandStatusUpdated", commandId, status.ToString());
    }

    public async Task BroadcastHeartbeatAsync(int machineId, DateTime timestamp)
    {
        await _machineHub.Clients.Group($"Machine_{machineId}")
            .SendAsync("HeartbeatReceived", machineId, timestamp);
            
        await _machineHub.Clients.All
            .SendAsync("MachineHeartbeat", machineId, timestamp);
    }
}

public class NotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> _notificationHub;

    public NotificationService(IHubContext<NotificationHub> notificationHub)
    {
        _notificationHub = notificationHub;
    }

    public async Task SendNotificationAsync(string message, string type = "info")
    {
        await _notificationHub.Clients.All
            .SendAsync("NotificationReceived", message, type);
    }

    public async Task SendMachineAlertAsync(int machineId, string message)
    {
        await _notificationHub.Clients.All
            .SendAsync("MachineAlert", machineId, message);
    }

    public async Task SendSystemAlertAsync(string message)
    {
        await _notificationHub.Clients.All
            .SendAsync("SystemAlert", message);
    }
}