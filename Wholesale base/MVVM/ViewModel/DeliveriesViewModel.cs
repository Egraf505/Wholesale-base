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
    internal class DeliveriesViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Delivery> _deliveries;

        public ICollectionView Deliveries
        {
            get;
        }

        private Delivery _deliveriesSelected;
        public Delivery DeliveriesSelected
        {
            get { return _deliveriesSelected; }
            set
            {
                _deliveriesSelected = value;
                OnPropertyChanged();
            }
        }

        private string _deliveriesFilter = string.Empty;
        public string DeliveriesFilter
        {
            get { return _deliveriesFilter; }
            set
            {
                _deliveriesFilter = value;
                OnPropertyChanged();
                Deliveries.Refresh();
            }
        }

        public DeliveriesViewModel()
        {
            using (WholesalebaseContext context = new WholesalebaseContext())
            {
                _deliveries = new ObservableCollection<Delivery>(context.Deliveries.ToList());
            }

            Deliveries = CollectionViewSource.GetDefaultView(_deliveries);
         
            Deliveries.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Delivery.Data)));
            Deliveries.SortDescriptions.Add(new SortDescription(nameof(Delivery.IdProducer), ListSortDirection.Ascending));
        }      

        private void OnUpdateCollection(WholesalebaseContext context)
        {
            var deliveries = new ObservableCollection<Delivery>(context.Deliveries.ToList());
            _deliveries.Clear();

            foreach (var item in deliveries)
            {
                _deliveries.Add(item);
            }
            Deliveries.Refresh();
        }

        public ICommand OnAdd
        {
            get
            {
                return new RelayCommand(() =>
                {
                    AddDeliveries addDeliveries = new AddDeliveries();
                    if (addDeliveries.ShowDialog() == true)
                    {   
                        DateTime datetime = DateTime.ParseExact(addDeliveries.Date, "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                        int count = int.Parse(addDeliveries.Count);
                        using (WholesalebaseContext context = new WholesalebaseContext())
                        {
                            Producer producer = context.Producers.FirstOrDefault(x => x.Middlename == addDeliveries.DeliveriesProducer.SelectedItem.ToString())!;

                            if (producer != null)
                            {
                                Delivery delivery = new Delivery() { IdProducer = producer.Id, Data = datetime, Quantity = count };
                                context.Deliveries.Add(delivery);
                                context.SaveChanges();

                                MessageBox.Show("Поставка добавлена");
                            }
                            else
                            {
                                MessageBox.Show("Поставщик отсуствует");
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
                    if (DeliveriesSelected != null)
                    {
                        using (WholesalebaseContext context = new WholesalebaseContext())
                        {
                            context.Deliveries.Remove(DeliveriesSelected);
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
