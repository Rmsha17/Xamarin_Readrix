using System;
using System.Collections.Generic;
using System.Text;

namespace Readrix.Models
{
   public class ReaderPremiums
    {
        public int BUYPREMIUM_ID { get; set; }

        public int READER_FID { get; set; }

        public int PREMIUM_FID { get; set; }

        public DateTime BUY_DATE { get; set; }

        public DateTime PREMIUM_END_DATE { get; set; }
    }
}
