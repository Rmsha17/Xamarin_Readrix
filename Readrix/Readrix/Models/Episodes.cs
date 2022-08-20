using System;
using System.Collections.Generic;
using System.Text;

namespace Readrix.Models
{
    public class Episodes
    {
        public int EPISODE_ID { get; set; }

        public int ARTIFACT_FID { get; set; }

        public int EPISODE_NO { get; set; }

        public string EPISODE_FILE { get; set; }

        public DateTime EPISODE_DATE { get; set; }

        public bool? Isapproved { get; set; }

        public decimal? EPISODE_PRICE { get; set; }
    }
}
