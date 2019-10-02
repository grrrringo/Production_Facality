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

        public void SetStockItems(object obj)
        {
            var values = (object[])obj;

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

            var unit = (string)values[1];
            switch (unit)
            {
                case ("szt"):
                    unit = "0";
                    break;
                case ("kg"):
                    unit = "1";
                    break;
                case ("m"):
                    unit = "2";
                    break;
                default:
                    unit = "";
                    break;
            }

            var key = (string)values[2];
            var name = (string)values[3];
            var location = (string)values[4];
            var batch = (string)values[5];
            var qTotal = (string)values[6];
            var qReserved = (string)values[7];
            var qAvailable = (string)values[8];
            var uCost = (string)values[9];
            var tCost = (string)values[10];

            //string incomDate_1, incomDate_2;

            //if (values[11] == null)
            //{
            //    incomDate_1 = "";
            //}
            //else
            //{
            //    var incomDate_11 = (DateTime)values[11];
            //    incomDate_1 = incomDate_11.ToString("yyyy-MM-dd");
            //}

            //if (values[12] == null)
            //{
            //    incomDate_2 = "";
            //}
            //else
            //{
            //    var incomDate_22 = (DateTime)values[12];
            //    incomDate_2 = incomDate_22.ToString("yyyy-MM-dd");
            //}



            //MessageBox.Show(incomDate_1);










            //incomDate_1.ToShortDateString();
            //MessageBox.Show(incomDate_1.ToShortDateString());
            //if (values[12] != null)
            //{
            //    var incomDate_2 = (DateTime)values[12];
            //    incomDate_2.ToShortDateString();
            //}
            //else
            //{
            //    var incomDate_2 = (string)values[12];
            //}



            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM StockItems ");

            bool isBuildingStarted = false;

            for (int i = 0; i < values.Count(); i++)
            {
                if (!string.IsNullOrEmpty(values[i]!=null ? values[i].ToString() : "" ))
                    switch (i)
                    {
                        case (0):
                            queryBuilder.Append("WHERE Section LIKE '%" + section + "%'");
                            isBuildingStarted = true;
                            break;
                        case (1):
                            if (!isBuildingStarted)
                            {
                                queryBuilder.Append("WHERE Unit LIKE '%" + unit + "%'");
                                isBuildingStarted = true;
                            }
                            else
                            {
                                queryBuilder.Append(" AND Unit LIKE '%" + unit + "%'");
                            }
                            break;
                        case (2):
                            if (!isBuildingStarted)
                            {
                                queryBuilder.Append("WHERE Number LIKE '%" + key + "%'");
                                isBuildingStarted = true;
                            }
                            else
                            {
                                queryBuilder.Append(" AND Number LIKE '%" + key + "%'");
                            }
                            break;
                        case (3):
                            if (!isBuildingStarted)
                            {
                                queryBuilder.Append("WHERE Name LIKE '%" + name + "%'");
                            }
                            else
                            {
                                queryBuilder.Append(" AND Name LIKE '%" + name + "%'");
                            }
                            break;
                        case (4):
                            if (!isBuildingStarted)
                            {
                                queryBuilder.Append("WHERE Location LIKE '%" + location + "%'");
                                isBuildingStarted = true;
                            }
                            else
                            {
                                queryBuilder.Append(" AND Location LIKE '%" + location + "%'");
                            }
                            break;
                        case (5):
                            if (!isBuildingStarted)
                            {
                                queryBuilder.Append("WHERE BatchNumber LIKE '%" + batch + "%'");
                                isBuildingStarted = true;
                            }
                            else
                            {
                                queryBuilder.Append(" AND BatchNumber LIKE '%" + batch + "%'");
                            }
                            break;
                        case (6):
                            if (!qTotal.Contains('-'))
                            {
                                if (!isBuildingStarted)
                                {
                                    queryBuilder.Append("WHERE QuantityTotal = " + qTotal + "");
                                    isBuildingStarted = true;
                                }
                                else
                                {
                                    queryBuilder.Append(" AND QuantityTotal = " + qTotal + "");
                                }
                            }
                            else
                            {
                                string[] qTotalCut = qTotal.Split('-');

                                if (!isBuildingStarted)
                                {
                                    queryBuilder.Append("WHERE QuantityTotal BETWEEN " + qTotalCut[0] + " AND " + qTotalCut[1] + " ");
                                    isBuildingStarted = true;
                                }
                                else
                                {
                                    queryBuilder.Append(" AND QuantityTotal BETWEEN " + qTotalCut[0] + " AND " + qTotalCut[1] + " ");
                                }
                            }

                            break;
                        case (7):
                            if (!qReserved.Contains('-'))
                            {
                                if (!isBuildingStarted)
                                {
                                    queryBuilder.Append("WHERE QuantityReserved = " + qReserved + "");
                                    isBuildingStarted = true;
                                }
                                else
                                {
                                    queryBuilder.Append(" AND QuantityReserved = " + qReserved + "");
                                }
                            }
                            else
                            {
                                string[] qReservedCut = qReserved.Split('-');

                                if (!isBuildingStarted)
                                {
                                    queryBuilder.Append("WHERE QuantityReserved BETWEEN " + qReservedCut[0] + " AND " + qReservedCut[1] + " ");
                                    isBuildingStarted = true;
                                }
                                else
                                {
                                    queryBuilder.Append(" AND QuantityReserved BETWEEN " + qReservedCut[0] + " AND " + qReservedCut[1] + " ");
                                }
                            }
                            break;
                        case (8):
                            if (!qAvailable.Contains('-'))
                            {
                                if (!isBuildingStarted)
                                {
                                    queryBuilder.Append("WHERE QuantityAvailable = " + qAvailable + "");
                                    isBuildingStarted = true;
                                }
                                else
                                {
                                    queryBuilder.Append(" AND QuantityAvailable = " + qAvailable + "");
                                }
                            }
                            else
                            {
                                string[] qAvailableCut = qAvailable.Split('-');

                                if (!isBuildingStarted)
                                {
                                    queryBuilder.Append("WHERE QuantityAvailable BETWEEN " + qAvailableCut[0] + " AND " + qAvailableCut[1] + " ");
                                    isBuildingStarted = true;
                                }
                                else
                                {
                                    queryBuilder.Append(" AND QuantityAvailable BETWEEN " + qAvailableCut[0] + " AND " + qAvailableCut[1] + " ");
                                }
                            }
                            break;
                        case (9):
                            if (!uCost.Contains('-'))
                            {
                                if (!isBuildingStarted)
                                {
                                    queryBuilder.Append("WHERE UnitCost = " + uCost + "");
                                    isBuildingStarted = true;
                                }
                                else
                                {
                                    queryBuilder.Append(" AND UnitCost = " + uCost + "");
                                }
                            }
                            else
                            {
                                string[] uCostCut = uCost.Split('-');

                                if (!isBuildingStarted)
                                {
                                    queryBuilder.Append("WHERE UnitCost BETWEEN " + uCostCut[0] + " AND " + uCostCut[1] + " ");
                                    isBuildingStarted = true;
                                }
                                else
                                {
                                    queryBuilder.Append(" AND UnitCost BETWEEN " + uCostCut[0] + " AND " + uCostCut[1] + " ");
                                }
                            }
                            break;
                        case (10):
                            if (!tCost.Contains('-'))
                            {
                                if (!isBuildingStarted)
                                {
                                    queryBuilder.Append("WHERE TotalCost = " + tCost + "");
                                    isBuildingStarted = true;
                                }
                                else
                                {
                                    queryBuilder.Append(" AND TotalCost = " + tCost + "");
                                }
                            }
                            else
                            {
                                string[] tCostCut = tCost.Split('-');

                                if (!isBuildingStarted)
                                {
                                    queryBuilder.Append("WHERE TotalCost BETWEEN " + tCostCut[0] + " AND " + tCostCut[1] + " ");
                                    isBuildingStarted = true;
                                }
                                else
                                {
                                    queryBuilder.Append(" AND TotalCost BETWEEN " + tCostCut[0] + " AND " + tCostCut[1] + " ");
                                }
                            }
                            break;
                        case (11):

                            var incomDate_11 = (DateTime)values[11];
                            string incomDate_1 = incomDate_11.ToString("yyyy-MM-dd");

                            if (values[12] == null)
                            {
                                if (!isBuildingStarted)
                                {
                                    queryBuilder.Append("WHERE IncomingDate = '" + incomDate_1 + "'");
                                    isBuildingStarted = true;
                                }
                                else
                                {
                                    queryBuilder.Append(" AND IncomingDate = '" + incomDate_1 + "'");
                                }
                                MessageBox.Show(queryBuilder.ToString());
                            }
                            else
                            {
                                var incomDate_22 = (DateTime)values[12];
                                string incomDate_2 = incomDate_22.ToString("yyyy-MM-dd");

                                if (!isBuildingStarted)
                                {
                                    queryBuilder.Append("WHERE IncomingDate BETWEEN '" + incomDate_1 + "' AND '" + incomDate_2 + "' ");
                                    isBuildingStarted = true;
                                }
                                else
                                {
                                    queryBuilder.Append(" AND IncomingDate BETWEEN '" + incomDate_1 + "' AND '" + incomDate_2 + "' ");
                                }
                            }
                            break;
                        case (13):

                            var expirDate_11 = (DateTime)values[13];
                            string expirDate_1 = expirDate_11.ToString("yyyy-MM-dd");

                            if (values[14] == null)
                            {
                                if (!isBuildingStarted)
                                {
                                    queryBuilder.Append("WHERE ExpirationDate = '" + expirDate_1 + "'");
                                    isBuildingStarted = true;
                                }
                                else
                                {
                                    queryBuilder.Append(" AND ExpirationDate = '" + expirDate_1 + "'");
                                }
                                MessageBox.Show(queryBuilder.ToString());
                            }
                            else
                            {
                                var expirDate_22 = (DateTime)values[14];
                                string expirDate_2 = expirDate_22.ToString("yyyy-MM-dd");

                                if (!isBuildingStarted)
                                {
                                    queryBuilder.Append("WHERE ExpirationDate BETWEEN '" + expirDate_1 + "' AND '" + expirDate_2 + "' ");
                                    isBuildingStarted = true;
                                }
                                else
                                {
                                    queryBuilder.Append(" AND ExpirationDate BETWEEN '" + expirDate_1 + "' AND '" + expirDate_2 + "' ");
                                }
                            }
                            break;
                        case (15):

                            var actionDate_11 = (DateTime)values[15];
                            string actionDate_1 = actionDate_11.ToString("yyyy-MM-dd");

                            if (values[16] == null)
                            {
                                if (!isBuildingStarted)
                                {
                                    queryBuilder.Append("WHERE LastActionDate = '" + actionDate_1 + "'");
                                    isBuildingStarted = true;
                                }
                                else
                                {
                                    queryBuilder.Append(" AND LastActionDate = '" + actionDate_1 + "'");
                                }
                                MessageBox.Show(queryBuilder.ToString());
                            }
                            else
                            {
                                var actionDate_22 = (DateTime)values[16];
                                string actionDate_2 = actionDate_22.ToString("yyyy-MM-dd");

                                if (!isBuildingStarted)
                                {
                                    queryBuilder.Append("WHERE LastActionDate BETWEEN '" + actionDate_1 + "' AND '" + actionDate_2 + "' ");
                                    isBuildingStarted = true;
                                }
                                else
                                {
                                    queryBuilder.Append(" AND LastActionDate BETWEEN '" + actionDate_1 + "' AND '" + actionDate_2 + "' ");
                                }
                            }
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
