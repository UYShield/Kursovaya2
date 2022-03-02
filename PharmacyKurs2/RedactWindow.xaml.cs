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

namespace PharmacyKurs_2
{
    /// <summary>
    /// Логика взаимодействия для RedactWindow.xaml
    /// </summary>
    public partial class RedactWindow : Window
    {
        public RedactWindow(object id)
        {
            InitializeComponent();
            DataContext = id;
            //Вывод данных в комбобксы
            GroupComboBox.ItemsSource = DBEntities.GetContext().Group.ToList();
            SupplyComboBox.ItemsSource = DBEntities.GetContext().Supplier.ToList();
        }
       
        private void RedButton_Click(object sender, RoutedEventArgs e)
        {
            //Проверка данных
            if (GroupComboBox.SelectedIndex == 0)
            {
                GroupComboBox.SelectedIndex = 1;
            }
            else
            {
                DBEntities.GetContext().SaveChanges();
                ((PrepWindow)this.Owner).updateData();
                this.Close();
            }

            if (SupplyComboBox.SelectedIndex == 0)
            {
                SupplyComboBox.SelectedIndex = 1;
            }
            //Сохранение изменений и закрытие формы
            else
            {
                DBEntities.GetContext().SaveChanges();
                ((PrepWindow)this.Owner).updateData();
                this.Close();
            }
        }
    }
}