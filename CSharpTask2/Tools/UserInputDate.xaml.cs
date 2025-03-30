using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CSharpTask2.Tools
{
    /// <summary>
    /// Interaction logic for UserInputDate.xaml
    /// </summary>
    public partial class UserInputDate : UserControl
    {
        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(UserInputDate), new PropertyMetadata(null));

        public string Caption
        {
            get => TbCaption.Text;
            set => TbCaption.Text = value;
        }

        public DateTime? SelectedDate
        {
            get => (DateTime?)GetValue(SelectedDateProperty);
            set => SetValue(SelectedDateProperty, value);
        }

        public UserInputDate()
        {
            InitializeComponent();
        }
    }
}
