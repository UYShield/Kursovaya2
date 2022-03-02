using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Word = Microsoft.Office.Interop.Word;

namespace PharmacyKurs_2
{
    /// <summary>
    /// Логика взаимодействия для ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        public ReportWindow()
        {
            InitializeComponent();
         //Задание заголовка
            chart1.Titles.Add("Данные о товарах");
            chart1.ChartAreas.Add(new ChartArea("Default"));
            chart1.Series.Add(new Series("Цена")
            {
                ChartType = SeriesChartType.Column
            });
            //Задание данных
            List<String> info_product = new List<string>();
            List<int> count_extra = new List<int>();
            foreach (Preparation preparation in DBEntities.GetContext().Preparation)
            {
                info_product.Add(preparation.PreparationName);
                count_extra.Add(Convert.ToInt32(preparation.Cost));
            }
            chart1.Series["Цена"].Points.DataBindXY(info_product, count_extra);
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Word document(*.docx) | *.docx";
            object oMissing = System.Reflection.Missing.Value;
           
            //Создание и вывод отчета в Word документ
            Word.Application word_app = new Word.Application();
            word_app.Visible = true;
            Word.Document doc = word_app.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
           
            //Реализация заголовка
            Word.Paragraph par_zag = doc.Content.Paragraphs.Add(ref oMissing);
            par_zag.Range.Text = "Отчёт фармацевтических товаров";
            par_zag.Range.Font.Color = Word.WdColor.wdColorBlack;
            par_zag.Range.Font.Bold = 1;
            par_zag.Range.Font.Size = 14f;
            par_zag.Range.Font.Name = "Times New Roman";
            par_zag.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            par_zag.Range.InsertParagraphAfter();
           
            //Создание таблицы в Word документе
            Word.Paragraph table_par = doc.Content.Paragraphs.Add(ref oMissing);
            Word.Table table = doc.Content.Tables.Add(table_par.Range, DBEntities.GetContext().Preparation.Count() + 1, 3, ref oMissing, ref oMissing);
            table.Range.Font.Size = 12f;
            table.Range.Font.Bold = 0;
            table.Rows[1].Range.Font.Bold = 1;
            table.Cell(1, 1).Range.Text = "Наименование товара";
            table.Cell(1, 2).Range.Text = "Количество";
            table.Cell(1, 3).Range.Text = "Цена";
            table.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
           //Заполнение Word-документа
            for (int i = 0; i < DBEntities.GetContext().Preparation.Count(); i++)
            {
                for (int j = 1; j <= table.Columns.Count; j++)
                {
                    switch (j)
                    {
                        case 1:
                            table.Cell(i + 2, j).Range.Text = DBEntities.GetContext().Preparation.ToList()[i].PreparationName;
                            break;
                        
                        case 2:
                            table.Cell(i + 2, j).Range.Text = DBEntities.GetContext().Preparation.ToList()[i].Cost.ToString();
                            break;
                    }
                }
            }
            //Сохранение документа
            doc.SaveAs2(saveFileDialog.FileName = "Product Report", ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
        }
    }
}
