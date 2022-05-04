﻿using System;
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
using Wholesale_base.MVVM.ViewModel;

namespace Wholesale_base.Pages
{
    /// <summary>
    /// Interaction logic for ProducerPage.xaml
    /// </summary>
    public partial class ProducerPage : Page
    {
        public ProducerPage()
        {
            InitializeComponent();

            DataContext = new ProducerViewModel();
        }
    }
}
