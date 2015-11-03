using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Biblioteka.Models
{
    public class AddFailure
    {
        public int Id { get; set; }

        
        [Required]
        [Display(Name = "Data Wystąpienia")]
        [RegularExpression("^([0]?[0-9]|[12][0-9]|[3][01])[-]([0]?[1-9]|[1][0-2])[-]([0-9]{4}|[0-9]{2})$", ErrorMessage = "Proszę podać prawidłowy format daty. (DD-MM-YYYY)")]
        public string EntryDate { get; set; }
        

        [Required]
        [Display(Name = "Temat")]
        [MaxLength(49, ErrorMessage = "Temat powinnien być krótszy niż 50 znaków")]
        public string Topic { get; set; }
        
        [Required]
        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Piętro")]
        public int Floor { get; set; }

        [Required]
        [Display(Name = "Strefa")]
        public string Area { get; set; }

        [Required]
        [Display(Name = "Stanowisko")]
        public string Worksite { get; set; }

        [Required]
        [Display(Name = "Status")]
        public bool? Status { get; set; }


    }
}