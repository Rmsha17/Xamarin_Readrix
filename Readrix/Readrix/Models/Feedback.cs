using System;
using System.Collections.Generic;
using System.Text;

namespace Readrix.Models
{
   public class Feedback
    {
      
        public int FEEDBACK_ID { get; set; }

        public string FEEDBACK_DESCRIPTION { get; set; }
        public string READER_NAME { get; set; }
        public string READER_IMAGE { get; set; }

        public int READER_FID { get; set; }

        public DateTime FEEDBACK_DATE { get; set; }

        public int ARTIFACT_FID { get; set; }

        public int? EPISODE_FID { get; set; }

        public int? FEEDBACK_FID { get; set; }
    }
}
