using Production_Facility.Models;
using Production_Facility.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Production_Facility
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SortedDictionary<string, Item> bazaDanych = new SortedDictionary<string, Item>();
        //Dictionary<string, Recipe> bazaReceptur = new Dictionary<string, Recipe>();
        //SortedDictionary<string, StockItem> bazaStockItem = new SortedDictionary<string, StockItem>();
        //List<StockItem> listaSI = new List<StockItem>();
        //FunctionTemp ft = new FunctionTemp();

        

        FacilityDBContext context = new FacilityDBContext();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModelNavigator();

            //StreamReader sr = new StreamReader(@"C:\Temp\StockItems.csv", Encoding.GetEncoding("UTF-8"));
            //string line;
            //string[] cut;

            //while ((line = sr.ReadLine()) != null)
            //{
            //    cut = line.Split('\t');
            //    var newSI = new StockItem(cut[1], cut[5], cut[13], cut[8], cut[12], cut[10], cut[11], null);
            //    context.StockItems.Add(newSI);
            //    context.SaveChanges();
            //}
            //sr.Close();

            //tempDG.ItemsSource = context.StockItems.ToList();

            //tempDG.ItemsSource = context.StockItems.Include("Item.Number");
        }
    }
}
