using DB;
using System;
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
using System.Windows.Shapes;

namespace Wholesale_base.Windows
{
    /// <summary>
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        public AddProduct()
        {
            InitializeComponent();
            this.Loaded += AddProduct_Loaded;
        }

        private void AddProduct_Loaded(object sender, RoutedEventArgs e)
        {
            using(WholesalebaseContext context = new WholesalebaseContext())
            {
                _deliveries = context.Deliveries.ToList();
                _types = context.Types.ToList();
            }

            ProductDeliveries.Items.Clear();
            foreach (var item in _deliveries)
            {
                ProductDeliveries.Items.Add(item.Id);
            }

            ProductType.Items.Clear();
            foreach (var item in _types)
            {
                ProductType.Items.Add(item.Name);
            }

            ProductQuantity.PreviewTextInput += OnlyNumber_PreviewTextInput;

            ProductPrice.PreviewTextInput += OnlyNumber_PreviewTextInput;
        }
   
        private void OnlyNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private List<Delivery> _deliveries;
        private List<DB.Type> _types;

        public string Count
        {
            get { return ProductQuantity.Text; }
        }

        public string Price
        {
            get { return ProductPrice.Text; }
        }

        public string Description
        {
            get { return ProductDescription.Text; }
        }

        private bool OnCheck()
        {
            if (ProductDeliveries.SelectedItem == null)
            {
                return false;
            }
            if (ProductType.SelectedItem == null)
            {
                return false;
            }
            if (ProductQuantity.Text == null)
            {
                return false;
            }
            if (ProductPrice.Text == null)
            {
                return false;
            }
            if (ProductDescription.Text == null)
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
