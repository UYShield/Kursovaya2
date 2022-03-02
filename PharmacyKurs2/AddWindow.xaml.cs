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
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public AddWindow()
        {
            InitializeComponent();
            //Связка combobox с таблицами бд
            GroupComboBox.ItemsSource = DBEntities.GetContext().Group.ToList();
            SupplyComboBox.ItemsSource = DBEntities.GetContext().Supplier.ToList();
        }
        
        //Реализация добавления
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            //Проверка достоверности
            StringBuilder errors = new StringBuilder();
            if (NameBox.Text == "")
                errors.AppendLine("Укажите название товара");
            if (PriceBox.Text == "")
                errors.AppendLine("Укажите цену товара");
            if (GroupComboBox.SelectedIndex == 0)
                errors.AppendLine("Укажите группу");
            if (SupplyComboBox.SelectedIndex == 0)
                errors.AppendLine("Укажите поставщика");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            //Добавление товара
            DBEntities.GetContext().Preparation.Add(new Preparation()
            {
                PreparationName = NameBox.Text,
                Cost = Convert.ToInt32(PriceBox.Text),
                ID_Group = GroupComboBox.SelectedIndex,
                ID_Supplier = SupplyComboBox.SelectedIndex
            });
            //Сохранение изменений и закрытие окна
            DBEntities.GetContext().SaveChanges();
            ((PrepWindow)this.Owner).updateData();
            MessageBox.Show("Новый товар добавлен", " ", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
