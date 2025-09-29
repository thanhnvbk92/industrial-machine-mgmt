namespace ManagerApp.Models;

public enum MachineStatus
{
    Online,
    Offline,
    Warning,
    Error,
    Maintenance
}

public enum LogLevel
{
    Info,
    Warning,
    Error,
    Debug
}

public enum CommandStatus
{
    Pending,
    Running,
    Completed,
    Failed,
    Cancelled
}