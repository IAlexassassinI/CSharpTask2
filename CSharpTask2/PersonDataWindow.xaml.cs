using CommunityToolkit.Mvvm.Input;
using CSharpTask1.Models;
using CSharpTask1.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CSharpTask2;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
/// 



public partial class PersonDataWindow : Window
{
    
    public PersonDataWindow(Person? person = null)
    {
        InitializeComponent();

        var viewModel = new PersonViewModel(person, this);

        DataContext = viewModel;
    }

}