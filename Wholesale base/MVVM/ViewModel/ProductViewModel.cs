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
    internal class ProductViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Product> _products;

        public ICollectionView Products
        {
            get;
        }

        private Product _productSelected;
        public Product ProductSelected
        {
            get { return _productSelected; }
            set
            {
                _productSelected = value;
                OnPropertyChanged();
            }
        }

        private string _productFilter = string.Empty;
        public string ProductFilter
        {
            get { return _productFilter; }
            set
            {
                _productFilter = value;
                OnPropertyChanged();
                Products.Refresh();
            }
        }

        public ProductViewModel()
        {
            using (WholesalebaseContext context = new WholesalebaseContext())
            {
                _products = new ObservableCollection<Product>(context.Products.ToList());
            }

            Products = CollectionViewSource.GetDefaultView(_products);

            Products.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Product.Description)));
            Products.SortDescriptions.Add(new SortDescription(nameof(Product.Id), ListSortDirection.Ascending));
        }

        private void OnUpdateCollection(WholesalebaseContext context)
        {
            var products = new ObservableCollection<Product>(context.Products.ToList());
            _products.Clear();

            foreach (var item in products)
            {
                _products.Add(item);
            }
            Products.Refresh();
        }

        public ICommand OnAdd
        {
            get
            {
                return new RelayCommand(() =>
                {
                    AddProduct addDeliveries = new AddProduct();
                    if (addDeliveries.ShowDialog() == true)
                    {
                        using (WholesalebaseContext context = new WholesalebaseContext())
                        {
                            //Условия тут
                            Delivery delivery = context.Deliveries.FirstOrDefault()!;
                            DB.Type type = context.Types.FirstOrDefault()!;                          

                            if (delivery == null)
                            {
                                MessageBox.Show("Ошибка поставка не найдена");
                            }
                            if (type == null)
                            {
                                MessageBox.Show("Не найдена категория");
                            }
                            else
                            {
                                //Условие к продукту
                                Product product = new Product {IdDeliveries = delivery.Id, Type = type.Id, CountProductOnWarehouse = AddProduct.CountProduct, Price = AddProduct.Price, Description = AddProduct.Description };
                                context.Products.Add(product);
                                context.SaveChanges();

                                MessageBox.Show("Товар добавлен добавлена");
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
                    if (ProductSelected != null)
                    {
                        using (WholesalebaseContext context = new WholesalebaseContext())
                        {
                            context.Products.Remove(ProductSelected);
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
