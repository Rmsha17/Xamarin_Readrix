using System;
using System.Collections.Generic;
using System.Text;

namespace Readrix.Models
{
   public class Order
    {
        public int ORDER_ID { get; set; }

        public DateTime ORDER_DATE { get; set; }

        public string ORDER_STATUS { get; set; }

        public string ORDER_TYPE { get; set; }

        public int? READER_FID { get; set; }

        public string PAYMENT_TYPE { get; set; }

        public string Reader_Name { get; set; }

    }
}
