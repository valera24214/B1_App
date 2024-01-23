namespace B1_App.Models.Model1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Notes
    {
        public int id { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(10)]
        public string English_signs { get; set; }

        [Required]
        [StringLength(10)]
        public string Russian_signs { get; set; }

        public int Even_int { get; set; }

        public decimal Decimal { get; set; }
    }
}
