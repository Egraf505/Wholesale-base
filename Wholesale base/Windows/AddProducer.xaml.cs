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
using Wholesale_base.MVVM.ViewModel;

namespace Wholesale_base.Windows
{
    /// <summary>
    /// Interaction logic for AddProducer.xaml
    /// </summary>
    public partial class AddProducer : Window
    {
        public AddProducer()
        {
            InitializeComponent();

            
        }

        public string FirstName
        {
            get { return FirstNameProducer.Text; }
        }

        public string MiddleName
        {
            get { return MiddleNameProducer.Text; }
        }

        public string LastName
        {
            get { return LastNameProducer.Text; }
        }

        private bool OnCheck()
        {
            if (FirstName == null || FirstName == String.Empty)
                return false;
            if (MiddleName == null || MiddleName == String.Empty)
                return false;          

            return true;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (!OnCheck())
            {
                MessageBox.Show("Имя и фамилия не должны быть пустыми");
            }
            else
            {
                this.DialogResult = true;
            }
        }
    }
}
