namespace B1_App.Models.Model2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Files
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Files()
        {
            Bank_accounts = new HashSet<Bank_accounts>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(1000)]
        public string Name { get; set; }

        [Required]
        [StringLength(1000)]
        public string Path { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date_start { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date_finish { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_create { get; set; }

        [StringLength(200)]
        public string Bank_name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bank_accounts> Bank_accounts { get; set; }
    }
}
