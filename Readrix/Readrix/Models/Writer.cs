using System;
using System.Collections.Generic;
using System.Text;

namespace Readrix.Models
{
    public class Writer
    {
        public int WRITER_ID { get; set; }

        public string WRITER_NAME { get; set; }

        public string WRITER_EMAIL { get; set; }

        public string WRITER_PASSWORD { get; set; }

        public string WRITER_CONTACT { get; set; }

        public string WRITER_ADDRESS { get; set; }

        public string WEITER_GENDER { get; set; }

        public int ADMIN_FID { get; set; }

        public string WRITER_IMAGE { get; set; }

        public int? STATUS { get; set; }

        public string EXPERIENCE { get; set; }

        public string SHARE_FILE_WORK { get; set; }

    }
}
