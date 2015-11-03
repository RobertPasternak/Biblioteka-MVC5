using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Biblioteka.Models
{
    public class AddComment
    {
        public int Id { get; set; }

        public string OldComment { get; set; }

        [Required]
        [Display(Name = "Imię i Nazwisko")]
        public string User { get; set; }

        [Required]
        [Display(Name = "Komentarz")]
        public string NewComment { get; set; }

    }
}