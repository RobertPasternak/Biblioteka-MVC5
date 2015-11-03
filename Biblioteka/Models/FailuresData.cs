using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Biblioteka.Models
{
    public class FailuresData
    {
        public int Id { get; set; }

        public string EntryDate { get; set; }

        public string Topic { get; set; }

        public string Description { get; set; }

        public int Floor { get; set; }

        public string Area { get; set; }

        public string Worksite { get; set; }

        public bool Status { get; set; }




        public string Comment { get; set; }

    }
}