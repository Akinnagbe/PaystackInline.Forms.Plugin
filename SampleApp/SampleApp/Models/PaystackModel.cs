using System;
using System.Collections.Generic;
using System.Text;

namespace SampleApp.Models
{
    public class PaystackModel
    {
        public string key { get; set; }
        public string email { get; set; }
        public decimal amount { get; set; }
        public string subaccount { get; set; }
        public string bearer { get; set; }
        public string @ref { get; set; }
        public string currency { get; set; }

    }
}
