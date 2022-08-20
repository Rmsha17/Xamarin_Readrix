using System;
using System.Collections.Generic;
using System.Text;

namespace Readrix.Models
{
   public class Order_details
    {
        public int ORDERDETAIL_ID { get; set; }

        public int ORDER_FID { get; set; }

        public int SHOPARTIFACT_FID { get; set; }

        public decimal ARTIFACT_SALEPRICE { get; set; }

        public decimal ARTIFACT_PURCHASEPRICE { get; set; }

        public decimal QUANTITY { get; set; }
        public decimal? Total { get; set; }
      

        public string ARTIFACT_NAME { get; set; }

        public string ARTIFACT_IMAGE { get; set; }

        
    }
}
