using Production_Facility.Models;
using Production_Facility.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Production_Facility.ViewModels
{
    public class StockItemViewModel : INotifyPropertyChanged
    {
        FacilityDBContext dbContext = new FacilityDBContext();


        private string name = "Asortyment";
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }

        public StockItemViewModel()
        {
            DataGridLoader = new RelayCommand(SetStockItems);
        }

        public ICommand DataGridLoader { get; set; }

        public void SetStockItems(object obj)
        {
            StockItems = dbContext.StockItems.Take(20).ToList();
        }

        private List<StockItem> stockItems;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public List<StockItem> StockItems
        {
            get { return stockItems; }
            set
            {
                stockItems = value;
                OnPropertyChanged("StockItems");

            }

        }
    }

}
