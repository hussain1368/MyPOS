﻿using POS.WPF.Models.ViewModels;
using System.Windows;

namespace POS.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainVM context)
        {
            InitializeComponent();
            DataContext = context;
        }
    }
}
