using CSharpTask2.Models;
using System.Configuration;
using System.Data;
using System.Windows;

namespace CSharpTask2;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{

    private void ApplicationStartup(object sender, StartupEventArgs e)
    {
        _ = DataManager.Instance;
    }

    private void ApplicationExit(object sender, ExitEventArgs e)
    {
        DataManager.Instance.SaveAll();
    }


}

