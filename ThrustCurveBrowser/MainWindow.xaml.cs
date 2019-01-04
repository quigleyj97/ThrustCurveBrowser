using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data;

namespace ThrustCurveBrowser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ApiService apiService;

        public MainWindow()
        {
            InitializeComponent();
            apiService = new ApiService();
            apiService.TestConnection()
                .ContinueWith((res) =>
                {
                    if (res.Result)
                    {
                        Console.WriteLine("Connected!");
                    }
                    else
                    {
                        Dispatcher.InvokeAsync(() =>
                        {
                            const string msg = "Failed to connect to ThrustCurve, check your internet connection";
                            MessageBox.Show(this, msg, "Connection Error");
                            Close();
                        });
                    }
                });
            apiService.SearchMotors(new Models.searchrequest { manufacturer = "Cesaroni Technology", diameter = 29 })
                .ContinueWith(res =>
                {
                    return Dispatcher.InvokeAsync(() =>
                    {
                        dataGrid.ItemsSource = res.Result;
                    });
                });
        }

        private void MfrListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

    public class MotorDetail
    {
        public string Foo { get; set; }
        public string Bar { get; set; }
    }
}
