using DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Wholesale_base.Windows
{
    /// <summary>
    /// Interaction logic for AddDeliveries.xaml
    /// </summary>
    public partial class AddDeliveries : Window
    {
        public AddDeliveries()
        {
            InitializeComponent();
            this.Loaded += AddDeliveries_Loaded;
        }

        private void AddDeliveries_Loaded(object sender, RoutedEventArgs e)
        {
            using(WholesalebaseContext context = new WholesalebaseContext())
            {
                _producers = (context.Producers.ToList());
            }

            DeliveriesProducer.Items.Clear();
            foreach (Producer producer in _producers)
            {
                DeliveriesProducer.Items.Add(producer.Middlename);
            }

            DeliveryData.PreviewTextInput += DeliveryData_PreviewTextInput;

            DeliveryQuantity.PreviewTextInput += DeliveryQuantity_PreviewTextInput;
        }

        private void DeliveryData_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+");
            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void DeliveryQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Char key = Convert.ToChar(e.Text);
            if (Char.IsNumber(key))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            } 
        }

        private List<Producer> _producers;

        public string Date
        {
            get { return DeliveryData.Text; }
        }

        public string Count
        {
            get { return DeliveryQuantity.Text; }
        }

        private bool OnCheck()
        {
            if (DeliveriesProducer.SelectedItem == null)
            {
                return false;
            }
            if (DeliveryData.Text != null)
            {
                DateTime datetime;
                if (!DateTime.TryParse(Date, out datetime))
                {
                    return false;
                }
            }
            if (DeliveryQuantity.Text == null)
            {
                return false;
            }
            return true;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (!OnCheck())
            {
                MessageBox.Show("Данные введены не корретно");
            }
            else
            {               
                this.DialogResult = true;
            }
        }
    }
}
