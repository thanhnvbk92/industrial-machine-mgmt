using Microsoft.AspNetCore.SignalR;
using ManagerApp.Models;

namespace ManagerApp.Hubs;

public class MachineHub : Hub
{
    public async Task JoinMachineGroup(string machineId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Machine_{machineId}");
    }

    public async Task LeaveMachineGroup(string machineId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Machine_{machineId}");
    }

    public async Task BroadcastMachineStatus(string machineId, MachineStatus status)
    {
        await Clients.Group($"Machine_{machineId}").SendAsync("MachineStatusUpdated", machineId, status);
        await Clients.All.SendAsync("MachineStatusChanged", machineId, status);
    }

    public async Task BroadcastHeartbeat(string machineId, DateTime timestamp)
    {
        await Clients.Group($"Machine_{machineId}").SendAsync("HeartbeatReceived", machineId, timestamp);
        await Clients.All.SendAsync("MachineHeartbeat", machineId, timestamp);
    }
}

public class LogHub : Hub
{
    public async Task StreamLogs(string machineId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"LogStream_{machineId}");
    }

    public async Task StopLogStream(string machineId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"LogStream_{machineId}");
    }

    public async Task BroadcastNewLog(LogEntry logEntry)
    {
        await Clients.Group($"LogStream_{logEntry.MachineId}").SendAsync("NewLogEntry", logEntry);
        await Clients.All.SendAsync("LogEntryAdded", logEntry);
    }
}

public class CommandHub : Hub
{
    public async Task BroadcastCommandStatus(int commandId, string status)
    {
        await Clients.All.SendAsync("CommandStatusUpdated", commandId, status);
    }

    public async Task BroadcastCommandResult(int commandId, string result)
    {
        await Clients.All.SendAsync("CommandCompleted", commandId, result);
    }
}

public class NotificationHub : Hub
{
    public async Task SendNotification(string message, string type = "info")
    {
        await Clients.All.SendAsync("NotificationReceived", message, type);
    }

    public async Task SendMachineAlert(int machineId, string message)
    {
        await Clients.All.SendAsync("MachineAlert", machineId, message);
    }

    public async Task SendSystemAlert(string message)
    {
        await Clients.All.SendAsync("SystemAlert", message);
    }
}