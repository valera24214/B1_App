using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace B1_App.Models.Model1
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<Notes> Notes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notes>()
                .Property(e => e.English_signs)
                .IsUnicode(false);

            modelBuilder.Entity<Notes>()
                .Property(e => e.Russian_signs)
                .IsUnicode(false);

            modelBuilder.Entity<Notes>()
                .Property(e => e.Decimal)
                .HasPrecision(8, 2);
        }
    }
}
