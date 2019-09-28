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

        public string Number { get; set; }

        public string Name { get; set; }

        public UnitType Unit { get; set; }

        public SectionType Section { get; set; }

        public decimal QuantityTotal { get; set; }

        public decimal QuantityReserved { get; set; }

        public decimal QuantityAvailable { get; set; }

        public decimal UnitCost { get; set; }

        public decimal TotalCost { get; set; }

        public DateTime IncomingDate { get; set; }

        public Nullable<DateTime> ExpirationDate { get; set; }

        public DateTime LastActionDate { get; set; }

        public string Location { get; set; }

        public string BatchNumber { get; set; }

        public StockItem(string number, string name, string qTotal, string location, string uCost,
            string laDate, string inDate, string exDate, string unit)
        {
            this.Number = number;
            this.Name = name;
            this.QuantityTotal = decimal.Parse(qTotal);
            this.QuantityAvailable = this.QuantityTotal;
            this.Location = location;
            this.UnitCost = decimal.Parse(uCost);
            this.TotalCost = this.UnitCost * this.QuantityTotal;
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

            //if (exDate == "")
            //{
            //    this.ExpirationDate = null;
            //}
            //else if (exDate != "")
            //{
            //    this.ExpirationDate = DateTime.Parse(exDate);
            //}

            //else
            //{
            //    MessageBox.Show("=>"+exDate+"<=");
            //}

            if (unit == "szt")
            {
                this.Unit = UnitType.szt;
            }
            else if (unit == "kg")
            {
                this.Unit = UnitType.kg;
            }
            else if (unit == "m")
            {
                this.Unit = UnitType.m;
            }
            else
            {
                MessageBox.Show(unit);
            }

            //Item item = context.Items.First(x => x.Number == this.Number);
            //this.Section = item.Section;


        }

        public StockItem()
        {

        }

    }
}
