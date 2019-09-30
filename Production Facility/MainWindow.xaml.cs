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
        Dictionary<string, Recipe> bazaReceptur = new Dictionary<string, Recipe>();
        //SortedDictionary<string, StockItem> bazaStockItem = new SortedDictionary<string, StockItem>();
        //List<StockItem> listaSI = new List<StockItem>();
        FunctionTemp ft = new FunctionTemp();

        FacilityDBContext context = new FacilityDBContext();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModelNavigator();

            


            //ft.RecipeReader(bazaReceptur, context);


            

            //var set = new SortedSet<string>();
            //foreach (Recipe pozycja in context.Recipes)
            //{


            //    string[]cutLine = pozycja.RecipeComposition.Split('|');

            //    foreach(string line in cutLine)
            //    {
            //        string[] cut = line.Split('=');
            //        string key = cut[1];
            //        var item = context.Items.Where(xx => xx.Number == key).FirstOrDefault();

            //        if (item != null)
            //        {
            //            Recipe.RecipeLine newRecipeLine = new Recipe.RecipeLine(int.Parse(cut[0]), cut[1], item.Name, double.Parse(cut[2]));

            //            if (!bazaReceptur.ContainsKey(pozycja.RecipeOwner))
            //            {
            //                Recipe nowaReceptura = new Recipe(pozycja.RecipeOwner);
            //                bazaReceptur.Add(pozycja.RecipeOwner, nowaReceptura);
            //                bazaReceptur[pozycja.RecipeOwner].ItemRecipe.Add(newRecipeLine);
            //            }
            //            else
            //            {
            //                bazaReceptur[pozycja.RecipeOwner].ItemRecipe.Add(newRecipeLine);
            //            }
            //        }
            //        else
            //        {
            //            if (!set.Contains(key))
            //            {
            //                set.Add(key);
            //            }
            //        }
            //    }
            //}

            //foreach (KeyValuePair<string, Recipe> pozycja in bazaReceptur)
            //{
            //    StringBuilder sb = new StringBuilder();

            //    foreach (Recipe.RecipeLine line in pozycja.Value.ItemRecipe)
            //    {
            //        sb.Append(line.RecipeLine_Nr.ToString() + '=' + line.RecipeLine_Key + '=' + line.RecipeLine_Name + '=' + line.RecipeLine_Amount.ToString() + '|');
            //    }
            //    sb.Remove(sb.Length - 1, 1);
            //    pozycja.Value.RecipeComposition = sb.ToString();
            //}

            //foreach (Recipe r in context.Recipes)
            //{
            //    r.RecipeComposition = bazaReceptur[r.RecipeOwner].RecipeComposition;
            //}
            //context.SaveChanges();

            //tempDG.ItemsSource = context.Recipes.ToList();
        }
    }
}
