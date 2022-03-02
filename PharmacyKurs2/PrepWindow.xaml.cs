using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для PrepWindow.xaml
    /// </summary>
    public partial class PrepWindow : Window
    {

        public MainWindow window;
        IEnumerable<Preparation> preparations;

        //Метод реализующий ряд инструкций для вывода имени из БД и вывода данных из БД в listbox
        public PrepWindow(LK auth, MainWindow window)
        {
            InitializeComponent();
            UserNameLabel.Content = auth.Name;
            DataContext = DBEntities.GetContext().Preparation.ToList();
            updateData();
        }
        public void updateData()
        {
            DBEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            preparations = DBEntities.GetContext().Preparation.ToList();
           //Реализация фильтра по цене 
            if (PriceCheckBox.IsChecked == true)
            {
                int priceone = 0, pricetwo = 0;

                if (!int.TryParse(PriceBox1.Text, out priceone))
                    return;
                if (!int.TryParse(PriceBox2.Text, out pricetwo))
                    return;
                if (priceone > pricetwo)
                    return;
                //Вывод товаров относительно выбранных фильтров
                preparations = preparations.Where(p => p.Cost >= priceone && p.Cost <= pricetwo);
            }
            //Поиск по названию
            preparations = preparations.Where(p => p.PreparationName.Contains(SearchBox.Text)).ToList();
            listBox.ItemsSource = preparations;

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)TypeDescriptor.GetProperties(listBox.SelectedItem)[3].GetValue(listBox.SelectedItem);
            Preparation preparation = DBEntities.GetContext().Preparation.Where(p => p.ID_Preparation == id).First();

            RedactWindow redactWindow = new RedactWindow(preparation);
            redactWindow.Owner = this;
            redactWindow.Show();
        }

        private void AddButtonGoods_Click(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow();
            addWindow.Owner = this;
            addWindow.Show();
        }
        //Удаление данных из таблиц
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messegeBoxResult = MessageBox.Show("Удалить товар?", "Удаление записи о товаре", MessageBoxButton.YesNo);
            if (messegeBoxResult == MessageBoxResult.Yes)
            {
                int id = (int)TypeDescriptor.GetProperties(listBox.SelectedItem)[3].GetValue(listBox.SelectedItem);
                Preparation preparation = DBEntities.GetContext().Preparation.Where(p => p.ID_Preparation == id).First();
              //Ниже код отвечает за удаление из таблиц Supply(Поставки)
                IEnumerable<Supply> supply;
                supply = DBEntities.GetContext().Supply.ToList();
                supply = supply.Where(p => p.ID_Preparation == id).ToList();
                foreach (Supply s in supply)
                {
                    DBEntities.GetContext().Supply.Remove(s);
                }
                //Реализация удаления данных из таблицы-связки Поставщик-Поставка
                IEnumerable<BundleSupplies> bundlesupplies;
                bundlesupplies = DBEntities.GetContext().BundleSupplies.ToList();
                bundlesupplies = bundlesupplies.Where(p => p.ID_Supply == DBEntities.GetContext().Supply.Where(s => s.ID_Preparation == id).First().ID_Supply).ToList();
                foreach (BundleSupplies bs in bundlesupplies)
                {
                    DBEntities.GetContext().BundleSupplies.Remove(bs);
                }

                DBEntities.GetContext().Preparation.Remove(preparation);

                DBEntities.GetContext().SaveChanges();
                updateData();
                DeleteButton.IsEnabled = false;
            }

        }
        
        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DeleteButton.IsEnabled = true; 
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            updateData();
        }

        private void PriceCheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void PriceBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
           updateData();
        }

        private void PriceBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
           updateData();
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            ReportWindow reportWindow = new ReportWindow();
            reportWindow.Owner = this;
            reportWindow.Show();
        }
    }
}
