using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Production_Facility.Models
{
    [Table("StockItems")]
    public class StockItem

    {
        [Key]
        public int StockItem_ID { get; set; }

        [ForeignKey("Item")]
        public string  NumberRef { get; set; }
        public virtual Item Item { get; set; }

        public double QTotal { get; set; }

        public double QReserved { get; set; }

        public double QAvailable { get; set; }


        public decimal UnitCost { get; set; }

        public decimal TotalCost { get; set; }

        public DateTime IncomingDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public DateTime LastActionDate { get; set; }

        public string Location { get; set; }

        public string BatchNumber { get; set; }

        public StockItem(string number,string qTotal, string location, string uCost,
            string laDate, string inDate, string exDate, string batch)//string number, string name, string unit,
        {
            //this.Number = number;
            //this.Name = name;
            //this.Item = new Item(number);
            
            this.NumberRef = number;
            this.QTotal = double.Parse(qTotal);
            this.QAvailable = this.QTotal;
            this.Location = location;
            this.UnitCost = decimal.Parse(uCost);
            this.TotalCost = UnitCost * (Convert.ToDecimal(QTotal));
            this.LastActionDate = DateTime.Parse(laDate);
            this.IncomingDate = DateTime.Parse(inDate);

            try
            {
                this.ExpirationDate = DateTime.Parse(exDate);
            }
            catch (System.FormatException e)
            {
                //MessageBox.Show(name + '\n' + "=>" + exDate + "<=");
                this.ExpirationDate = null;
            }

            if (batch != null)
            {
                var batch_temp = String.Concat(Enumerable.Repeat("0", 6 - batch.Length));
                this.BatchNumber = "PO/" + batch_temp + batch;
            }
            else
            {
                this.BatchNumber = String.Format("{0:yyyyMMdd}", this.IncomingDate);
            }

            //if (unit == "szt")
            //{
            //    this.Unit = UnitType.szt;
            //}
            //else if (unit == "kg")
            //{
            //    this.Unit = UnitType.kg;
            //}
            //else if (unit == "m")
            //{
            //    this.Unit = UnitType.m;
            //}
            //else
            //{
            //    MessageBox.Show(unit);
            //}



        }

        public StockItem()
        {

        }

    }
}
