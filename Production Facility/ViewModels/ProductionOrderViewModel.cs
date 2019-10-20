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
using System.Collections.ObjectModel;

namespace Production_Facility.ViewModels
{
    public class ProductionOrderViewModel : INotifyPropertyChanged
    {
        

        Recipe recipe = new Recipe();
        RecipeViewModel recipeVM = new RecipeViewModel();

        private ProductionOrder productionOrder;

        public ProductionOrder ProductionOrder
        {
            get
            { return productionOrder; }
            set
            {
                productionOrder = value;
                OnPropertyChanged("ProductionOrder");
            }
        }

        private List<ProductionOrder> pOrders;

        public List<ProductionOrder> POrders
        {
            get

            {
                using (FacilityDBContext dbContext = new FacilityDBContext())
                {
                    return pOrders = dbContext.ProductionOrders.Where(q => q.OrderStatus == "PLANNED").ToList();
                }

            }
            set
            {
                pOrders = value;
            }
        }
        

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
        private ICommand _LoadOrderCommand;
        private ICommand _ProdOrderChosenCommand;
        private ICommand _SaveOrderCommand;
        private ICommand _ComboBoxLoader;

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
            using (FacilityDBContext dbContext = new FacilityDBContext())
            {
                string s = obj as string;

                List<Item> entities = (from q in dbContext.Items
                                       from x in dbContext.Recipes
                                       where q.Name.Contains(s)
                                       where x.RecipeOwner == q.Number
                                       select q).ToList();

                UserChoice = entities;
            }
        }

        public void SetOrdersParams(object obj)
        {
            using (FacilityDBContext dbContext = new FacilityDBContext())
            {
                string s = obj as string;

                Item = (from q in dbContext.Items where q.Number == s select q).FirstOrDefault<Item>();
            }
                
        }

        public void SetDataGrid (object obj)
        {
            using (FacilityDBContext dbContext = new FacilityDBContext())
            {
                if (Order != null)
                {
                    Order.Clear();
                }
                else
                {
                    Order = new ObservableCollection<Recipe.RecipeLine>();
                }


                var values = (object[])obj;

                var key = (string)values[0];

                var quantity_temp = (string)values[1];

                var quantity = int.Parse(quantity_temp);

                var recipe = (from q in dbContext.Recipes where q.RecipeOwner == key select q).FirstOrDefault<Recipe>();

                foreach (Recipe.RecipeLine line in recipe.GetRecipe(dbContext, key))
                {
                    line.RecipeLine_Amount = line.RecipeLine_Amount * quantity;
                    Order.Add(line);
                }

            }
                
        }

        public void SaveOrder (object parameter)
        {
            using (FacilityDBContext dbContext = new FacilityDBContext())
            {
                var values = (object[])parameter;

                string date = ((DateTime)values[2]).ToShortDateString();

                //bool isDate = DateTime.TryParse(values[2].ToString(),out )

                int orderID;

                bool isInt = Int32.TryParse(values[3].ToString(),out orderID);

                if (!isInt)
                {
                    MessageBox.Show("if");
                    dbContext.ProductionOrders.Add(new ProductionOrder(values[0].ToString(), Convert.ToInt32(values[1]), date, recipe.GetRecipeComposition(order)));
                    dbContext.SaveChanges();
                }
                else
                {
                    MessageBox.Show("else");
                    //var orderID = Convert.ToInt32(values[3]);

                    var order = dbContext.ProductionOrders.Where(xx => xx.OrderID == orderID).FirstOrDefault<ProductionOrder>();
                    order.OrderComposition = recipe.GetRecipeComposition(Order);
                    order.Quantity = ProductionOrder.Quantity;
                    order.PlannedDate = ProductionOrder.PlannedDate;
                    dbContext.SaveChanges();
                }
            }
                
        }

        public void ProduceOrder(object parameter)
        {
            using (FacilityDBContext dbContext = new FacilityDBContext())
            {
                var obj = (object[])parameter;

                var key = (string)obj[0];
                var name = (string)obj[1];
                var quantityTemp = (string)obj[2];
                var quantity = Convert.ToDecimal(quantityTemp);
                var unit = (string)obj[3];
                var orderiD = (string)obj[4];

                var tCost = new decimal();
                foreach (Recipe.RecipeLine line in order)
                {
                    var qAvailable = Convert.ToDecimal(line.RecipeLine_Amount);
                    var xxx = dbContext.StockItems.Where(xx => xx.Number == line.RecipeLine_Key).FirstOrDefault<StockItem>(); //Where(xx => xx.QuantityAvailable <= qAvailable)
                    MessageBox.Show(xxx.StockItem_ID.ToString());
                    xxx.QTotal = xxx.QTotal - line.RecipeLine_Amount;
                    xxx.QAvailable = xxx.QTotal;
                    xxx.LastActionDate = DateTime.Now;
                    tCost += xxx.UnitCost * Convert.ToDecimal(line.RecipeLine_Amount);
                    dbContext.SaveChanges();
                }
                var uCost = tCost / quantity;
                var orderID_temp = Convert.ToInt32(orderiD);
                var prOrder = dbContext.ProductionOrders.Where(q => q.OrderID == orderID_temp).FirstOrDefault<ProductionOrder>();
                prOrder.OrderStatus = "COMPLETED";
                prOrder.ProductionDate = DateTime.Now;
                var newStockItem = new StockItem(key, name, quantity.ToString(), "WR-PR-WG", uCost.ToString(), DateTime.Now.ToString(), DateTime.Now.ToString(), DateTime.Now.AddYears(2).ToString(), unit, orderiD);
                dbContext.StockItems.Add(newStockItem);
                dbContext.SaveChanges();
            }
                
        }
        public void ProdOrderChosen(object obj)
        {
            using (FacilityDBContext dbContext = new FacilityDBContext())
            {
                string orderID = (string)obj;

                if (Order != null)
                    Order.Clear();
                Order = recipe.GetRecipe(productionOrder.OrderComposition);
                Item = dbContext.Items.Where(i => i.Number == productionOrder.ItemKey).FirstOrDefault<Item>();
            }
                
        }
        public ICommand ProdOrderChosenCommand
        {
            get
            {
                if (_ProdOrderChosenCommand == null)
                {
                    _ProdOrderChosenCommand = new RelayCommand(ProdOrderChosen, Can_ProdOrderChosen_Execute);
    }
                return _ProdOrderChosenCommand;
            }
        }

        public ICommand ComboBoxLoader //{ get; set; }
        {
            get
            {
                if (_ComboBoxLoader == null)
                {
                    _ComboBoxLoader = new RelayCommand(SetComboBox); 
                }
                return _ComboBoxLoader;
            }
        }

        public ICommand OrdersParamsLoader { get; set; }

        public ICommand LoadOrderCommand
        {
            get
            {
                if (_LoadOrderCommand == null)
                {
                    _LoadOrderCommand = new RelayCommand(SetDataGrid, Can_SetDataGrid_Execute);
                }
                return _LoadOrderCommand;
            }
        }
        public ICommand SaveOrderCommand
        {
            get
            {
                if (_SaveOrderCommand == null)
                {
                    _SaveOrderCommand = new RelayCommand(SaveOrder);
    }
                return _SaveOrderCommand;
            }
        }

        public ICommand ProduceOrderCommand { get; set; }

        private bool Can_ProdOrderChosen_Execute(object parameter)
        {
            if (productionOrder != null)
                return true;
            else
                return false;
        }

        private bool Can_SetDataGrid_Execute(object parameter)
        {
            using (FacilityDBContext dbContext = new FacilityDBContext())
            {
                var view = (object[])parameter;
                if (view == null || view[0] == null || view[1] == null)
                    return false;

                string key = (string)view[0];

                bool isInteger = int.TryParse((string)view[1], out int quantity);

                if (isInteger && dbContext.Recipes.Where(xx => xx.RecipeOwner == key).Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
                
        }
        public ProductionOrderViewModel()
        {
            OrdersParamsLoader = new RelayCommand(SetOrdersParams);
            ProduceOrderCommand = new RelayCommand(ProduceOrder);
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

        private ObservableCollection<Recipe.RecipeLine> order;//= new ObservableCollection<Recipe.RecipeLine>()
        public ObservableCollection<Recipe.RecipeLine> Order
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
