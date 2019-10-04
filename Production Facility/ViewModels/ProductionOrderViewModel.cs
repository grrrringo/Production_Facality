using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Production_Facility.Models;
using System.Windows.Input;
using Production_Facility.ViewModels.Commands;
using System.Windows;

namespace Production_Facility.ViewModels
{
    public class ProductionOrderViewModel : INotifyPropertyChanged
    {
        FacilityDBContext dbContext = new FacilityDBContext();

        Recipe recipe = new Recipe();

        private Item item;
        public Item Item
        {
            get { return item; }
            set
            {
                item = value;
                OnPropertyChanged("Item");

            }
        }

        private string name = "Zlecenie Produkcyjne";
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }

        public void SetComboBox(object obj)
        {
            string s = obj as string;

            List<Item> entities = (from q in dbContext.Items
                                   from x in dbContext.Recipes
                                   where q.Name.Contains(s)
                                   where x.RecipeOwner == q.Number
                                   select q).ToList();

            UserChoice = entities;
        }

        public void SetOrdersParams(object obj)
        {
            string s = obj as string;

            Item = (from q in dbContext.Items where q.Number == s select q).FirstOrDefault<Item>();


        }

        public void SetDataGrid (object obj)
        {
            var values = (object[])obj;

            var key = (string)values[0];

            var quantity_temp = (string)values[1];

            var quantity = int.Parse(quantity_temp);

            var recipe = (from q in dbContext.Recipes where q.RecipeOwner == key select q).FirstOrDefault<Recipe>();

            var temp = new List<Recipe.RecipeLine>();

            foreach (Recipe.RecipeLine line in recipe.GetRecipe(dbContext, key))
            {
                
                line.RecipeLine_Amount = line.RecipeLine_Amount * quantity;
                temp.Add(line);
            }
            Order = temp;
        }

        public ICommand ComboBoxLoader { get; set; }

        public ICommand OrdersParamsLoader { get; set; }

        public ICommand DataGridLoader { get; set; }

        public ProductionOrderViewModel()
        {
            ComboBoxLoader = new RelayCommand(SetComboBox);
            OrdersParamsLoader = new RelayCommand(SetOrdersParams);
            DataGridLoader = new RelayCommand(SetDataGrid);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }


        private List<Item> userChoice = new List<Item>();

        public List<Item> UserChoice
        {
            get { return userChoice; }
            set
            {
                userChoice = value;
                OnPropertyChanged("UserChoice");
            }
        }

        private List<Recipe.RecipeLine> order = new List<Recipe.RecipeLine>();

        public List<Recipe.RecipeLine> Order
        {
            get { return order; }
            set
            {
                order = value;
                OnPropertyChanged("Order");
            }
        }
    }

    
}
