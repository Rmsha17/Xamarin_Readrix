using System;
using System.Collections.Generic;
using System.Text;

namespace Readrix.Models
{
    public class Shopartifact
    {

        public int ID { get; set; }

        public int? ARTIFACT_FID { get; set; }

        public decimal PURCHASE_PRICE { get; set; }

        public decimal SALE_PRICE { get; set; }
    }
}
