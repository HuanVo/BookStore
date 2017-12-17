using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{

    public class MailModel
    {
        String mail_id { get; set; }
        String from_address { get; set; }
        String subject { get; set; }
        String body { get; set; }

        public MailModel()
        { }
    }
    
}