using ShoesDE.DataBase;
using ShoesDE.Helpers;
using ShoesDE.Statics;
using System.Linq;
using System.Windows;

namespace ShoesDE
{
    public partial class MainWindow : Window
    {
        private ShoesDEEntities _db = new ShoesDEEntities();
        private MessageHelper _mh = new MessageHelper();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginEnter.Text;
            string password = PasswordEnter.Password;

            var user = _db.User.Where(u => u.Login == login && u.Password == password).FirstOrDefault();

            if (user == null)
            {
                _mh.ShowError("Введён неправильный логин или пароль!");
                return;
            }
            else
            {
                CurrentSession.CurrentUser = user;
                new ProductWindow().Show();
                this.Close();
            }
        }

        private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            new ProductWindow().Show();
            this.Close();
        }
    }
}