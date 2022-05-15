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
    /// Interaction logic for UpdateOrder.xaml
    /// </summary>
    public partial class UpdateOrder : Window
    {
        private List<Status> _statuses;
        public UpdateOrder()
        {
            InitializeComponent();
            this.Loaded += UpdateOrder_Loaded;
        }

        private void UpdateOrder_Loaded(object sender, RoutedEventArgs e)
        {
            using(WholesalebaseContext context = new WholesalebaseContext())
            {
                _statuses = context.Statuses.ToList();
            }

            OrderUpdateStatus.Items.Clear();
            foreach (var item in _statuses)
            {
                OrderUpdateStatus.Items.Add(item.Name);
            }
        }

        private bool OnCheck()
        {
            if (OrderUpdateStatus.SelectedItem == null)
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
