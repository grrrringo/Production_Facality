using Production_Facility.Models;
using Production_Facility.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        private void QueryBuildHelper (StringBuilder queryBuilder, bool isBuildingStarted, string x, object obj, object nextObj)
        {
            if (x=="Unit" || x=="Number" || x=="Name" || x=="Location" || x=="Batch")
            {
                var y = (string)obj;

                if (x == "Unit")
                    switch (y)
                    {
                        case ("szt"):
                            y = "0";
                            break;
                        case ("kg"):
                            y = "1";
                            break;
                        case ("m"):
                            y = "2";
                            break;
                    }

                if (!isBuildingStarted)
                {
                    queryBuilder.Append("WHERE " + x + " LIKE '%" + y + "%'");
                    isBuildingStarted = true;
                }
                else
                {
                    queryBuilder.Append(" AND " + x + " LIKE '%" + y + "%'");
                }
            }

            else if (x == "QuantityTotal" || x == "QuantityReserved" || x == "QuantityAvailable" || x== "UnitCost" || x=="TotalCost")
            {
                var y = (string)obj;

                if (!y.Contains('-'))
                {
                    if (!isBuildingStarted)
                    {
                        queryBuilder.Append("WHERE " + x + " = " + y + "");
                        isBuildingStarted = true;
                    }
                    else
                    {
                        queryBuilder.Append(" AND " + x + " = " + y + "");
                    }
                }
                else
                {
                    string[] yCut = y.Split('-');

                    if (!isBuildingStarted)
                    {
                        queryBuilder.Append("WHERE " + x + " BETWEEN " + yCut[0] + " AND " + yCut[1] + " ");
                        isBuildingStarted = true;
                    }
                    else
                    {
                        queryBuilder.Append(" AND " + x + " BETWEEN " + yCut[0] + " AND " + yCut[1] + " ");
                    }
                }
            }

            else if (x == "IncomingDate" || x=="ExpirationDate" || x=="LastActionDate")
            {
                var date = (DateTime)obj;
                var date_1 = date.ToString("yyyy-MM-dd");

                if (nextObj == null)
                {
                    if (!isBuildingStarted)
                    {
                        queryBuilder.Append("WHERE " + x + " = '" + date_1 + "'");
                        isBuildingStarted = true;
                    }
                    else
                    {
                        queryBuilder.Append(" AND " + x + " = '" + date_1 + "'");
                    }
                }
                else
                {
                    date = (DateTime)nextObj;
                    var date_2 = date.ToString("yyyy-MM-dd");

                    if (!isBuildingStarted)
                    {
                        queryBuilder.Append("WHERE " + x + " BETWEEN '" + date_1 + "' AND '" + date_2 + "' ");
                        isBuildingStarted = true;
                    }
                    else
                    {
                        queryBuilder.Append(" AND " + x + " BETWEEN '" + date_1 + "' AND '" + date_2 + "' ");
                    }
                }
            }
        }

        public void SetStockItems(object obj)
        {
            var values = (object[])obj;

            bool isBuildingStarted = false;

            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM StockItems ");

            for (int i = 0; i < values.Count(); i++)
            {
                if (!string.IsNullOrEmpty(values[i]!=null ? values[i].ToString() : "" ))
                    switch (i)
                    {
                        case (0):
                            var section = (string)values[0];
                            switch (section)
                            {
                                case ("Product"):
                                    section = "0";
                                    break;
                                case ("Intermediate"):
                                    section = "1";
                                    break;
                                case ("Substance"):
                                    section = "2";
                                    break;
                                case ("Article"):
                                    section = "3";
                                    break;
                                default:
                                    section = "";
                                    break;
                            }

                            queryBuilder.Append("WHERE Section LIKE '%" + section + "%'");
                            isBuildingStarted = true;
                            break;
                        case (1):
                            QueryBuildHelper(queryBuilder, isBuildingStarted, "Unit", values[i], values[i+1]);
                            break;
                        case (2):
                            QueryBuildHelper(queryBuilder, isBuildingStarted, "Number", values[i], values[i + 1]);
                            break;
                        case (3):
                            QueryBuildHelper(queryBuilder, isBuildingStarted, "Name", values[i], values[i + 1]);
                            break;
                        case (4):
                            QueryBuildHelper(queryBuilder, isBuildingStarted, "Location", values[i], values[i + 1]);
                            break;
                        case (5):
                            QueryBuildHelper(queryBuilder, isBuildingStarted, "Batch", values[i], values[i + 1]);
                            break;
                        case (6):
                            QueryBuildHelper(queryBuilder, isBuildingStarted, "QuantityTotal", values[i], values[i + 1]);
                            break;
                        case (7):
                            QueryBuildHelper(queryBuilder, isBuildingStarted, "QuantityReserved", values[i], values[i + 1]);
                            break;
                        case (8):
                            QueryBuildHelper(queryBuilder, isBuildingStarted, "QuantityAvailable", values[i], values[i + 1]);
                            break;
                        case (9):
                            QueryBuildHelper(queryBuilder, isBuildingStarted, "UnitCost", values[i], values[i + 1]);
                            break;
                        case (10):
                            QueryBuildHelper(queryBuilder, isBuildingStarted, "TotalCost", values[i], values[i + 1]);
                            break;
                        case (11):
                            QueryBuildHelper(queryBuilder, isBuildingStarted, "IncomingDate", values[i], values[i + 1]);
                            break;
                        case (13):
                            QueryBuildHelper(queryBuilder, isBuildingStarted, "ExpirationDate", values[i], values[i + 1]);
                            break;
                        case (15):
                            QueryBuildHelper(queryBuilder, isBuildingStarted, "LastActionDate", values[i], values[i + 1]);
                            break;
                        default:
                            break;
                    }
            }

            string s = queryBuilder.ToString();
            MessageBox.Show(s);
            var stockItems = dbContext.StockItems.SqlQuery(s).ToList();

            StockItems = stockItems;
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
