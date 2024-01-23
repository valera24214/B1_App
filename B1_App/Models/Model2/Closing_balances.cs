namespace B1_App.Models.Model2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Closing_balances
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public double Active { get; set; }

        public double Passive { get; set; }

        public virtual Bank_accounts Bank_accounts { get; set; }
    }
}
