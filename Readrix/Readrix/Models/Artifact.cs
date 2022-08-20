using System;
using System.Collections.Generic;
using System.Text;

namespace Readrix.Models
{
    public class Artifact
    {
        public int ARTIFACT_ID { get; set; }

        public string ARTIFACT_NAME { get; set; }


        public string ARTIFACT_PICTURE { get; set; }

        public string ARTIFACT_DATE { get; set; }

        public string ARTIFACT_DESCRIPTION { get; set; }

        public string ARTIFACT_STATUS { get; set; }

        public int SUB_CATEGORY_FID { get; set; }

        public int WRITER_FID { get; set; }

        public int? SCHEDULE_FID { get; set; }

        public int shopartidactid { get; set; }

        public int? Bookmarkid { get; set; }

        public decimal PURCHASE_PRICE { get; set; }

        public decimal SALE_PRICE { get; set; }

        public string SubCategory_Name {get; set;}


    }
}