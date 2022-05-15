using DB;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Wholesale_base.Windows;

namespace Wholesale_base.MVVM.ViewModel
{
    internal class OrderViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Order> _orders;
        public ICollectionView Orders
        {
            get;
        }

        private Order _selectedOrder;
        public Order SelectedOrder
        {
            get { return _selectedOrder; }
            set
            {
                _selectedOrder = value;
                OnPropertyChanged();
            }
        }

        private string _OrderFilter = string.Empty;
        public string OrderFilter
        {
            get { return _OrderFilter; }
            set
            {
                _OrderFilter = value;
                OnPropertyChanged();
                Orders.Refresh();
            }
        }
        public OrderViewModel()
        {
            using (WholesalebaseContext context = new WholesalebaseContext())
            {
                _orders = new ObservableCollection<Order>(context.Orders.ToList());
            }

            Orders = CollectionViewSource.GetDefaultView(_orders);

            Orders.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Order.IdProducer)));
            Orders.SortDescriptions.Add(new SortDescription(nameof(Order.IdProduct), ListSortDirection.Ascending));
        }

        private void OnUpdateCollection(WholesalebaseContext context)
        {
            var orders = new ObservableCollection<Order>(context.Orders.ToList());
            _orders.Clear();

            foreach (var item in orders)
            {
                _orders.Add(item);
            }
            Orders.Refresh();
        }

        public ICommand OnAdd
        {
            get
            {
                return new RelayCommand(() =>
                {
                    AddOrder addOrder = new AddOrder();
                    if (addOrder.ShowDialog() == true)
                    {
                        DateTime datetime = DateTime.ParseExact(addOrder.OrderData.Text, "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                        using (WholesalebaseContext context = new WholesalebaseContext())
                        {
                            Producer producer = context.Producers.FirstOrDefault(x => x.Middlename == addOrder.OrderProducer.SelectedItem.ToString())!;
                            Product product = context.Products.FirstOrDefault(x => x.Description == addOrder.OrderProduct.SelectedItem.ToString())!;

                            //Условия тут
                            if (producer == null)
                            {
                                MessageBox.Show("Поставщик не найден");
                            }
                            if (product == null)
                            {
                                MessageBox.Show("Товар не найден");
                            }
                            else
                            {
                                //Условие к продукту
                                Order order = new Order() { IdProducerNavigation = producer!, IdProductNavigation = product!, Address = addOrder.OrderAddress.Text,
                                Data = datetime, CountProduct = int.Parse(addOrder.OrderQuantity.Text), Status = 1};
                                context.Orders.Add(order);
                                context.SaveChanges();

                                MessageBox.Show("Заказ добавлен");
                            }

                            OnUpdateCollection(context);
                        }
                    }
                });
            }
        }

        public ICommand OnDelete
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (SelectedOrder != null)
                    {
                        using (WholesalebaseContext context = new WholesalebaseContext())
                        {
                            context.Orders.Remove(SelectedOrder);
                            MessageBox.Show("Удаление успешно");
                            context.SaveChanges();

                            OnUpdateCollection(context);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Поставщик не выбран");
                    }
                });
            }
        }
      
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
