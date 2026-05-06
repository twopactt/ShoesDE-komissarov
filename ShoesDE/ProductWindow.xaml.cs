using ShoesDE.DataBase;
using ShoesDE.Statics;
using System.Linq;
using System.Windows;

namespace ShoesDE
{
    public partial class ProductWindow : Window
    {
        private ShoesDEEntities _db = new ShoesDEEntities();
        public ProductWindow()
        {
            InitializeComponent();
            LoadProducts();
        }

        public ProductWindow(User user)
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            ProductList.ItemsSource = _db.Product.ToList();
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentSession.CurrentUser = null;
            new MainWindow().Show();
            this.Close();
        }
    }
}