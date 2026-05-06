using ShoesDE.DataBase;
using ShoesDE.Helpers;
using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Windows;

namespace ShoesDE
{
    public partial class AddEditProductWindow : Window
    {
        private ShoesDEEntities _db = new ShoesDEEntities();
        private MessageHelper _mh = new MessageHelper();

        private bool _isEditing;
        private Product _product;

        public AddEditProductWindow(int? id)
        {
            InitializeComponent();

            if (id == null)
            {
                _isEditing = false;
            }
            else
            {
                _isEditing = true;
                _product = _db.Product.Find(id);
            }

            LoadData();
        }

        private void LoadData()
        {
            var units = _db.Unit.ToList();
            var categories = _db.Category.ToList();
            var producers = _db.Producer.ToList();
            var providers = _db.Provider.ToList();
            
            UnitComboBox.ItemsSource = units;
            UnitComboBox.DisplayMemberPath = "Name";
            UnitComboBox.SelectedValuePath = "Id";
            UnitComboBox.SelectedIndex = 0;

            ProducerComboBox.ItemsSource = producers;
            ProducerComboBox.DisplayMemberPath = "Name";
            ProducerComboBox.SelectedValuePath = "Id";
            ProducerComboBox.SelectedIndex = 0;

            ProviderComboBox.ItemsSource = providers;
            ProviderComboBox.DisplayMemberPath = "Name";
            ProviderComboBox.SelectedValuePath = "Id";
            ProviderComboBox.SelectedIndex = 0;

            CategoryComboBox.ItemsSource = categories;
            CategoryComboBox.DisplayMemberPath = "Name";
            CategoryComboBox.SelectedValuePath = "Id";
            CategoryComboBox.SelectedIndex = 0;

            if (_isEditing == true)
                FillData();
        }

        private void FillData()
        {
            ArticleTextBox.Text = _product.Article;
            NameTextBox.Text = _product.Name;
            PriceTextBox.Text = _product.Price.ToString();
            DiscountTextBox.Text = _product.Discount.ToString();
            AmountInStockTextBox.Text = _product.AmountInStock.ToString();
            DescriptionTextBox.Text = _product.Description;
            PhotoTextBox.Text = _product.Photo;

            UnitComboBox.SelectedValue = _product.UnitId;
            ProducerComboBox.SelectedValue = _product.ProducerId;
            ProviderComboBox.SelectedValue = _product.ProviderId;
            CategoryComboBox.SelectedValue = _product.CategoryId;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            if (_isEditing == true)
                UpdateProduct();
            else
                CreateProduct();
        }

        private void CreateProduct()
        {
            Product product = new Product();

            string article = ArticleTextBox.Text;
            string name = NameTextBox.Text;
            decimal price = Convert.ToDecimal(PriceTextBox.Text);
            decimal discount = Convert.ToDecimal(DiscountTextBox.Text);
            decimal amount = Convert.ToDecimal(AmountInStockTextBox.Text);
            string description = DescriptionTextBox.Text;
            string photo = PhotoTextBox.Text;

            product.Article = article;
            product.Name = name;
            product.Price = price;
            product.Discount = discount;
            product.AmountInStock = amount;
            product.Description = description;
            product.UnitId = (int)UnitComboBox.SelectedValue;
            product.ProducerId = (int)ProducerComboBox.SelectedValue;
            product.ProviderId = (int)ProviderComboBox.SelectedValue;
            product.CategoryId = (int)CategoryComboBox.SelectedValue;

            product.Photo = photo;

            try
            {
                _db.Product.AddOrUpdate(product);
                _db.SaveChanges();
                _mh.ShowInfo("Продукт успешно создан!");
                CancelButton_Click(null, null);
            }
            catch (Exception ex)
            {
                _mh.ShowError(ex.Message);
                return;
            }
        }

        private void UpdateProduct()
        {
            string article = ArticleTextBox.Text;
            string name = NameTextBox.Text;
            decimal price = Convert.ToDecimal(PriceTextBox.Text);
            decimal discount = Convert.ToDecimal(DiscountTextBox.Text);
            decimal amount = Convert.ToDecimal(AmountInStockTextBox.Text);
            string description = DescriptionTextBox.Text;

            _product.Article = article;
            _product.Name = name;
            _product.Price = price;
            _product.Discount = discount;
            _product.AmountInStock = amount;
            _product.Description = description;

            _product.UnitId = (int)UnitComboBox.SelectedValue;
            _product.ProducerId = (int)ProducerComboBox.SelectedValue;
            _product.ProviderId = (int)ProviderComboBox.SelectedValue;
            _product.CategoryId = (int)CategoryComboBox.SelectedValue;

            try
            {
                _db.Product.AddOrUpdate(_product);
                _db.SaveChanges();
                _mh.ShowInfo("Продукт успешно изменен!");
                CancelButton_Click(null, null);
            }
            catch (Exception ex)
            {
                _mh.ShowError(ex.Message);
                return;
            }
        }

        private bool ValidateInput()
        {
            StringBuilder errors = new StringBuilder();

            string article = ArticleTextBox.Text;
            string name = NameTextBox.Text;
            string price = PriceTextBox.Text;
            string discount = DiscountTextBox.Text;
            string amount = AmountInStockTextBox.Text;
            string description = DescriptionTextBox.Text;

            if (string.IsNullOrWhiteSpace(article))
                errors.AppendLine("Поле артикула не заполнено!");

            if (string.IsNullOrWhiteSpace(name))
                errors.AppendLine("Поле наименования не заполнено!");

            if (string.IsNullOrWhiteSpace(price)
                    || !decimal.TryParse(price, out decimal priceDecimal))
                errors.AppendLine("Поле цены не заполнено!");

            if (string.IsNullOrWhiteSpace(discount)
                    || !decimal.TryParse(discount, out decimal discountDecimal)
                    || discountDecimal > 100 || discountDecimal < 0)
                errors.AppendLine("Поле скидки не заполнено!");

            if (string.IsNullOrWhiteSpace(amount)
                    || !decimal.TryParse(amount, out decimal amountDecimal)
                    || amountDecimal < 0)
                errors.AppendLine("Поле количества в наличии не заполнено!");

            if (string.IsNullOrWhiteSpace(description))
                errors.AppendLine("Поле описвния не заполнено!");

            if (errors.Length > 0)
            {
                _mh.ShowError(errors.ToString());
                return false;
            }

            return true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            new ProductWindow().Show();
            this.Close();
        }
    }
}
