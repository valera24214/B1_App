namespace B1_App.Models.Model2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Bank_accounts
    {
        public int id { get; set; }

        public int Number { get; set; }

        public int id_File { get; set; }

        [Required]
        [StringLength(1000)]
        public string Class { get; set; }

        public virtual Files Files { get; set; }

        public virtual Cash_turnover Cash_turnover { get; set; }

        public virtual Closing_balances Closing_balances { get; set; }

        public virtual Opening_balances Opening_balances { get; set; }
    }
}
