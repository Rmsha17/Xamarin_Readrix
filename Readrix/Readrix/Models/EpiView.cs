using System;
using System.Collections.Generic;
using System.Text;

namespace Readrix.Models
{
   public class EpiView
    {
        public int VIEW_ID { get; set; }

        public DateTime VIEW_DATE { get; set; }

        public int READER_FID { get; set; }

        public int ARTIFACT_FID { get; set; }

        public int EPISODE_FID { get; set; }
    }
}
