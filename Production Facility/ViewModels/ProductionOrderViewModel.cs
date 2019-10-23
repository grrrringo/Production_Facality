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

        private ObservableCollection<ProductionOrder> _AllOrders;

        public ObservableCollection<ProductionOrder> AllOrders // var myObservableCollection = new ObservableCollection<YourType>(myIEnumerable);
        {
            get

            {
                using (FacilityDBContext dbContext = new FacilityDBContext())
                {
                    return _AllOrders = new ObservableCollection<ProductionOrder>(dbContext.ProductionOrders.Where(q => q.OrderStatus == "PLANNED"));
                    //return _AllOrders = dbContext.ProductionOrders.Where(q => q.OrderStatus == "PLANNED").ToList();
                }

            }
            set
            {
                _AllOrders = value;
                OnPropertyChanged("AllOrders");
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

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }

        // ======================================== ORDER FROM mainComboBox CHOSEN (MEMBER GROUP) ===============================================================

        private ICommand _OrderChosen_Command;

        public ICommand OrderChosen_Command
        {
            get
            {
                if (_OrderChosen_Command == null)
                {
                    _OrderChosen_Command = new RelayCommand(OrderChosen, Can_OrderChosen_Execute);
                }
                return _OrderChosen_Command;
            }
        }

        public void OrderChosen(object obj)
        {
            using (FacilityDBContext dbContext = new FacilityDBContext())
            {
                int orderID = Convert.ToInt32((string)obj);

                if (Order != null)
                    Order.Clear();
                Order = recipe.GetRecipe(productionOrder.OrderComposition);
                ProductionOrder = dbContext.ProductionOrders.Where(q => q.OrderID == orderID).SingleOrDefault<ProductionOrder>();
                Item = dbContext.Items.Where(i => i.Number == productionOrder.ItemKey).FirstOrDefault<Item>();
            }

        }

        private bool Can_OrderChosen_Execute(object obj)
        {
            if (productionOrder != null)
                return true;
            else
                return false;
        }

        // ========================================= FILL searchComboBox (MEMBER GROUP) ===============================================================

        private ICommand _Fill_SearchComboBox_Command;

        public ICommand Fill_SearchComboBox_Command
        {
            get
            {
                if (_Fill_SearchComboBox_Command == null)
                {
                    _Fill_SearchComboBox_Command = new RelayCommand(Fill_SearchComboBox);
                }
                return _Fill_SearchComboBox_Command;
            }
        }

        public void Fill_SearchComboBox(object obj)
        {
            using (FacilityDBContext dbContext = new FacilityDBContext())
            {
                string s = obj as string;

                var entities = (from q in dbContext.Items
                                       from x in dbContext.Recipes
                                       where q.Name.Contains(s)
                                       where x.RecipeOwner == q.Number
                                       select q).ToList();

                UserChoice = entities;
            }
        }

        // ========================================= ITEM FROM searchComboBox CHOSEN (MEMBER GROUP) ===============================================================

        private ICommand _ItemChosen_Command;

        public ICommand ItemChosen_Command
        {
            get
            {
                if (_ItemChosen_Command == null)
                {
                    _ItemChosen_Command = new RelayCommand(ItemChosen);
                }
                return _ItemChosen_Command;
            }
        }

        public void ItemChosen(object obj)
        {
            using (FacilityDBContext dbContext = new FacilityDBContext())
            {
                string s = obj as string;

                Item = (from q in dbContext.Items where q.Number == s select q).FirstOrDefault<Item>();
            }

        }

        // ========================================= STARTING NEW ORDER (MEMBER GROUP) ===============================================================

        private ICommand _LoadOrderCommand;

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

        public void SetDataGrid(object obj)
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

        // ========================================= SAVING NEW ORDER (MEMBER GROUP) ===============================================================

        private ICommand _SaveOrder_Command;

        public ICommand SaveOrder_Command
        {
            get
            {
                if (_SaveOrder_Command == null)
                {
                    _SaveOrder_Command = new RelayCommand(SaveOrder,Can_SaveOrder_Execute);
                }
                return _SaveOrder_Command;
            }
        }

        public void SaveOrder(object obj)
        {
            using (FacilityDBContext dbContext = new FacilityDBContext())
            {
                var values = (object[])obj;

                DateTime date = ((DateTime)values[2]);

                int orderID;

                bool isInt = Int32.TryParse(values[3].ToString(), out orderID);

                if (!isInt)
                {
                    MessageBox.Show("if");
                    ProductionOrder = dbContext.ProductionOrders.Add(new ProductionOrder(values[0].ToString(), Convert.ToInt32(values[1]), date, recipe.GetRecipeComposition(Order)));
                    dbContext.SaveChanges();
                }
                else
                {
                    MessageBox.Show("else");
                    var order = dbContext.ProductionOrders.Where(xx => xx.OrderID == orderID).FirstOrDefault<ProductionOrder>();
                    order.OrderComposition = recipe.GetRecipeComposition(Order);
                    order.Quantity = ProductionOrder.Quantity;
                    order.PlannedDateTime = ProductionOrder.PlannedDateTime;
                    dbContext.SaveChanges();
                }
            }
        }

        public bool Can_SaveOrder_Execute(object obj)
        {
            return true;
            //var values = (object[])obj;

            //if (values == null)
            //    return false;
            //else if (values!= null && Int32.TryParse(values[3].ToString(),out int orderID))
            //{
            //    if(ProductionOrder != AllOrders.Where(q => q.OrderID == orderID).SingleOrDefault<ProductionOrder>())
            //        return true;
            //    return false;

            //}
                
            
            //else
            //    return false;



        }













        private ICommand _ProduceOrderCommand;
        
        











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
                foreach (Recipe.RecipeLine line in Order)
                {
                    var qAvailable = Convert.ToDecimal(line.RecipeLine_Amount);
                    var xxx = dbContext.StockItems.Where(xx => xx.Number == line.RecipeLine_Key).FirstOrDefault<StockItem>(); 

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










        public ICommand ProduceOrderCommand 
        {
            get
            {
                if (_ProduceOrderCommand == null)
                {
                    _ProduceOrderCommand = new RelayCommand(ProduceOrder);
                }
                return _ProduceOrderCommand;
            }
        }




        public ProductionOrderViewModel()
        {
            
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

        private ObservableCollection<Recipe.RecipeLine> order;
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
