using System;
using System.Collections.Generic;
using System.Text;

namespace Readrix.Models
{
   public class Premium
    {
        public int PREMIUM_ID { get; set; }

        public string PREMIUM_NAME { get; set; }

        public decimal PREMIUM_PRICE { get; set; }

        public int DURATION_IN_MONTHS { get; set; }
    }
}
