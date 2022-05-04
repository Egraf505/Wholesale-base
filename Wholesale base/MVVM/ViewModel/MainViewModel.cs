using DB;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wholesale_base.Pages;

namespace Wholesale_base.MVVM.ViewModel
{
    internal class MainViewModel : INotifyPropertyChanged
    {   
        private Page _producer;
        private Page _deliveries;
        private Page _product;
        private Page _order;

        private Page _currentPage;
        public Page CurrentPage { 
            get { return _currentPage; }
            private set { 
                _currentPage = value;
                OnPropertyChanged();
            }
        }    

        public MainViewModel()
        {
            if (CheckConnection())
            {               
                _producer = new ProducerPage();
                _deliveries = new DeliveriesPage();
                _product = new ProductPage();
                _order = new OrderPage();

                CurrentPage = _producer;
            }
            else
            {
                MessageBox.Show("Подключение отсустсвуюет");
            }
            
        }      

        public ICommand bMenuProducer_Click
        {
            get
            {
                return new RelayCommand(() => CurrentPage = _producer);
            }
        }

        public ICommand bMenuDeliveries_Click
        {
            get
            {
                return new RelayCommand(() => CurrentPage = _deliveries);
            }
        }

        public ICommand bMenuProduct_Click
        {
            get
            {
                return new RelayCommand(()=> CurrentPage = _product);
            }
        }

        public ICommand bMenuOrder_Click
        {
            get
            {
                return new RelayCommand(() => CurrentPage = _order);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private bool CheckConnection()
        {
            try
            {
                using(WholesalebaseContext context = new WholesalebaseContext())
                {
                    MessageBox.Show("Подключение есть");
                }  
                return true;
            }
            catch (Exception)
            {               
                return false;
            }
        }
    }
}
