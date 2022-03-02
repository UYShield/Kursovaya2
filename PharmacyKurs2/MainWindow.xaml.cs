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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PharmacyKurs_2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }
        //Реализация действия авторизации по нажатию на кнопке для перехода на следующую форму при соответсвии данных с БД
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(LoginBox.Text))
            {
                if (!String.IsNullOrEmpty(PasswordBox.Password))
                {
                    IQueryable<LK> авторизация_list = DBEntities.GetContext().LK.Where(p => p.Login == LoginBox.Text && p.Password == PasswordBox.Password);
                    if (авторизация_list.Count() == 1)
                    {
                        //Переход на следующую форму при правильно введеных данных
                        MessageBox.Show("Добро пожаловать," + авторизация_list.First().Name, "Welcome", MessageBoxButton.OK, MessageBoxImage.Information);
                        PrepWindow window = new PrepWindow(авторизация_list.First(), this);
                        window.Owner = this;
                        window.Show();
                        this.Hide();
                    }

                }
                //Окно при неверно введеных данных
                else MessageBox.Show("WRONG LOGIN OR PASSWORD1!");
                {
                    this.LoginBox.Text = "";
                    this.PasswordBox.Password = "";
                }
            }
        }
    }
}
