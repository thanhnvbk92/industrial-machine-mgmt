using System.Configuration;
using System.Data;
using System.Windows;

namespace MachineClient.WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        
        // Initialize logging, configuration, etc.
        InitializeServices();
    }

    private void InitializeServices()
    {
        // TODO: Initialize dependency injection container
        // TODO: Configure HTTP client for API communication
        // TODO: Setup Serilog logging
    }
}