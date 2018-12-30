using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;

namespace ThrustCurveBrowser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<MotorDetail> RocketTable = new List<MotorDetail>();
        public MainWindow()
        {
            RocketTable.Add(new MotorDetail
            {
                Foo = "hello",
                Bar = "world"
            });
            RocketTable.Add(new MotorDetail
            {
                Foo = "test",
                Bar = "test"
            });
            Console.WriteLine("Loaded");
            InitializeComponent();
            dataGrid.ItemsSource = RocketTable;
            Console.WriteLine(dataGrid.ItemsSource);
        }
    }

    public class MotorDetail
    {
        public string Foo { get; set; }
        public string Bar { get; set; }
    }
}
