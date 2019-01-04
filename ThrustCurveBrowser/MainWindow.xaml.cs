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
            apiService.GetSearchMetadata(new Models.metadatarequest { })
                .ContinueWith(x =>
                {
                    return Dispatcher.InvokeAsync(() =>
                    {
                        mfrListBox.ItemsSource = x.Result.manufacturers.Select((i) => i.Value);
                        diameterListBox.ItemsSource = x.Result.diameters;
                        typeListBox.ItemsSource = x.Result.types;
                    });
                });
        }

        private string _mfr;

        public string Manufacturer {
            get { return _mfr; }
            set {
                _mfr = value;
                RunSearch();
            }
        }

        private decimal? _diam;
        public decimal? Diameter {
            get { return _diam; }
            set {
                _diam = value;
                RunSearch();
            }
        }

        private string _type;
        public string Type {
            get { return _type; }
            set {
                _type = value;
                RunSearch();
            }
        }

        private void RunSearch()
        {
            Enum.TryParse(Type, out Models.searchrequestType motorType);
            Models.searchrequest req = new Models.searchrequest
            {
                manufacturer = Manufacturer,
                type = motorType
            };
            if (Diameter != null)
            {
                req.diameter = (decimal) Diameter;
            }
            apiService.SearchMotors(req)
                .ContinueWith(res =>
                {
                    return Dispatcher.InvokeAsync(() =>
                    {
                        dataGrid.ItemsSource = res.Result;
                    });
                });
        }
    }
}
