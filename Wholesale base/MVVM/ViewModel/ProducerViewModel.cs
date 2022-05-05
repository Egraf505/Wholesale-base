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
    internal class ProducerViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<Producer> _producers;

        public ICollectionView Producers
        {
            get;
        }

        private Producer _producerSelected;
        public Producer ProducerSelected
        {
            get { return _producerSelected; }
            set 
            {
                _producerSelected = value; 
                OnPropertyChanged();
            }
        }

        private string _producerFilter = string.Empty;
        public string ProducerFilter
        {
            get { return _producerFilter; }
            set 
            { 
                _producerFilter = value; 
                OnPropertyChanged();
                Producers.Refresh();
            }
        }

        public ProducerViewModel()
        {
            using (WholesalebaseContext context = new WholesalebaseContext())
            {
               _producers = new ObservableCollection<Producer>(context.Producers.ToList());
            }         

            Producers = CollectionViewSource.GetDefaultView(_producers);

            Producers.Filter = FilterProducer;
            Producers.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Producer.Middlename)));
            Producers.SortDescriptions.Add(new SortDescription(nameof(Producer.Firstname), ListSortDirection.Ascending));
        }

        private bool FilterProducer(object obj)
        {
            if (obj is Producer producer)
            {
                return producer.Firstname.Contains(ProducerFilter, StringComparison.InvariantCultureIgnoreCase) || 
                    producer.Middlename.Contains(ProducerFilter, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }

        private void OnUpdateCollection(WholesalebaseContext context)
        {
            var producers = new ObservableCollection<Producer>(context.Producers.ToList());
            _producers.Clear();

            foreach (var item in producers)
            {
                _producers.Add(item);
            }
            Producers.Refresh();
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
                            using(WholesalebaseContext context = new WholesalebaseContext())
                            {
                                Producer producer = new Producer() { Firstname = addProducer.FirstName, Middlename = addProducer.MiddleName, Lastname = addProducer.LastName};

                                context.Producers.Add(producer);
                                context.SaveChanges();

                                MessageBox.Show("Поставщик добавлен");

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
                    if (ProducerSelected != null)
                    {
                        using(WholesalebaseContext context = new WholesalebaseContext())
                        {
                            context.Producers.Remove(ProducerSelected);                           
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
