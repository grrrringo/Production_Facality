using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_Facility.Models
{
    public class ProductionOrder
    {
        public int OrderID { get; set; }

        public string ProducedItem { get; set; }

        public string ProducedQuantity { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime PlannedDate { get; set; }

        public DateTime ProductionDate { get; set; }

        public string OrderStatus { get; set; }

        public string OrderComposition { get; set; }

    }
}
