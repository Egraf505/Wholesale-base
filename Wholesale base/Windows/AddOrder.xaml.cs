using DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Wholesale_base.Windows
{
    /// <summary>
    /// Interaction logic for AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Window
    {
        private List<Producer> _producers;
        private List<Product> _products;       
        
        public AddOrder()
        {
            InitializeComponent();
            this.Loaded += AddOrder_Loaded;          
        }

        private void OrderData_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void OrderQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void AddOrder_Loaded(object sender, RoutedEventArgs e)
        {
            using (WholesalebaseContext context = new WholesalebaseContext())
            {
                _producers = context.Producers.ToList();            
            }

            _products = new List<Product>();

            OrderProducer.Items.Clear();
            foreach (var item in _producers)
            {
                OrderProducer.Items.Add(item.Middlename);
            }

            OrderProducer.SelectionChanged += OrderProducer_SelectionChanged;
            OrderData.PreviewTextInput += OrderData_PreviewTextInput;
            OrderQuantity.PreviewTextInput += OrderQuantity_PreviewTextInput;
        }

        private void OrderProducer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _products.Clear();
            ComboBox comboBox = (ComboBox)sender;

            using (WholesalebaseContext context = new WholesalebaseContext())
            {
                Producer producer = context.Producers.FirstOrDefault(x => x.Middlename == OrderProducer.SelectedItem.ToString())!;
                List<Delivery> delivery = context.Deliveries.Where(x => x.IdProducerNavigation == producer).ToList();               
                foreach (var item in delivery)
                {                  
                   List<Product> products =  new List<Product>(context.Products.Where(x => x.IdDeliveries == item.Id));
                    foreach (var product in products)
                    {
                        _products.Add(product);
                    }
                }               
            }

            OrderProduct.Items.Clear();
            foreach (var item in _products)
            {
                OrderProduct.Items.Add(item.Description);
            }
        }

        private bool OnCheck()
        {
            if (OrderProducer.SelectedItem == null)
            {
                return false;
            }
            if (OrderProduct.SelectedItem == null)
            {
                return false;
            }          
            if (OrderData.Text != null)
            {
                DateTime datetime;
                if (!DateTime.TryParse(OrderData.Text, out datetime))
                {
                    return false;
                }
            }            
            if (OrderAddress.Text == null)
            {
                return false;
            }
            if (OrderQuantity.Text != null)
            {
                Product product;
                using (WholesalebaseContext context = new WholesalebaseContext())
                {
                    product = context.Products.FirstOrDefault(x => x.Description == OrderProduct.SelectedItem.ToString())!;

                    int count = int.Parse(OrderQuantity.Text!);

                    if (count > product.CountProductOnWarehouse)
                    {
                        MessageBox.Show("Вы пытаетесь заказать товара больше чем его есть");
                        return false;
                    }
                    else
                    {
                        product.CountProductOnWarehouse -= count;
                        context.SaveChanges();
                    }
                }                
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
