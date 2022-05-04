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
using System.Windows.Input;
using Wholesale_base.Windows;

namespace Wholesale_base.MVVM.ViewModel
{
    internal class ProducerViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<Producer> _producers;

        public ObservableCollection<Producer> Producers
        {
            get { return _producers; }
            set 
            { 
                _producers = value;
                OnPropertyChanged();
            }
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

        public ProducerViewModel()
        {
            using (WholesalebaseContext context = new WholesalebaseContext())
            {
               _producers = new ObservableCollection<Producer>(context.Producers.ToList());
            }
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
                                Producers = new ObservableCollection<Producer>(context.Producers.ToList());
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
                            Producers = new ObservableCollection<Producer>(context.Producers.ToList());                           
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
