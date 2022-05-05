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

namespace Wholesale_base.MVVM.ViewModel
{
    internal class DeliveriesViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Producer> _deliveries;

        public ICollectionView Deliveries
        {
            get;
        }

        private Producer _deliveriesSelected;
        public Producer DeliveriesSelected
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
                _deliveries = new ObservableCollection<Producer>(context.Producers.ToList());
            }

            Deliveries = CollectionViewSource.GetDefaultView(_deliveries);

            Deliveries.Filter = FilterProducer;
            Deliveries.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Producer.Middlename)));
            Deliveries.SortDescriptions.Add(new SortDescription(nameof(Producer.Firstname), ListSortDirection.Ascending));
        }

        private bool FilterProducer(object obj)
        {
            if (obj is Producer producer)
            {
                return producer.Firstname.Contains(DeliveriesFilter, StringComparison.InvariantCultureIgnoreCase) ||
                    producer.Middlename.Contains(DeliveriesFilter, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }

        private void OnUpdateCollection(WholesalebaseContext context)
        {
            var producers = new ObservableCollection<Producer>(context.Producers.ToList());
            _deliveries.Clear();

            foreach (var item in producers)
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
                    AddProducer addProducer = new AddProducer();
                    if (addProducer.ShowDialog() == true)
                    {
                        using (WholesalebaseContext context = new WholesalebaseContext())
                        {
                            Producer producer = new Producer() { Firstname = addProducer.FirstName, Middlename = addProducer.MiddleName, Lastname = addProducer.LastName };

                            context.Producers.Add(producer);
                            context.SaveChanges();

                            MessageBox.Show("Поставка добавлена");

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
                            context.Producers.Remove(DeliveriesSelected);
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
