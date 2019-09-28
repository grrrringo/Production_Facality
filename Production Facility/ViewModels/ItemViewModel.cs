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
    public class ItemViewModel : INotifyPropertyChanged
    {
        FacilityDBContext dbContext = new FacilityDBContext();


        private string name = "Indeksy";
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }

        public ItemViewModel()
        {
            DataGridLoader = new RelayCommand(SetItems);
        }

        public ICommand DataGridLoader { get; set; }

        public void SetItems(object obj)
        {
            Items = dbContext.Items.Take(20).ToList();
        }

        private List<Item> items;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public List<Item> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged("Items");
            }

        }
    }

}
