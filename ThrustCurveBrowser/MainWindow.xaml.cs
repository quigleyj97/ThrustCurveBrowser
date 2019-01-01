﻿using System;
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
            ApiService srv = new ApiService();
            srv.TestConnection()
                .ContinueWith((res) =>
                {
                    if (res.Result)
                    {
                        Console.WriteLine("Connected!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to connect!");
                    }
                });
            srv.SearchMotors(new Models.searchrequest { manufacturer = "Cesaroni Technology", diameter = 29 })
                .ContinueWith(res =>
                {
                    return Dispatcher.InvokeAsync(() =>
                    {
                        dataGrid.ItemsSource = res.Result;
                    });
                });
            dataGrid.ItemsSource = RocketTable;
        }
    }

    public class MotorDetail
    {
        public string Foo { get; set; }
        public string Bar { get; set; }
    }
}
