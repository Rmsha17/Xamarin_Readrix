using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Readrix.Models
{
    [Serializable]
    public class Reader
    {
      
        public int READER_ID { get; set; }

        public string READER_NAME { get; set; }

        public string READER_EMAIL { get; set; }

        
        public string READER_PASSWORD { get; set; }

      
        public string READER_IMAGE { get; set; }

        public bool IS_ACCOUNT_ACTIVATE { get; set; }
        public string READER_CONTACT { get; set; }

        public string READER_ADDRESS { get; set; }
    }
}
