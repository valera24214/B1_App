using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace B1_App.Models.Model2
{
    public partial class Model2 : DbContext
    {
        public Model2()
            : base("name=Model2")
        {
        }

        public virtual DbSet<Bank_accounts> Bank_accounts { get; set; }
        public virtual DbSet<Cash_turnover> Cash_turnover { get; set; }
        public virtual DbSet<Closing_balances> Closing_balances { get; set; }
        public virtual DbSet<Files> Files { get; set; }
        public virtual DbSet<Opening_balances> Opening_balances { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bank_accounts>()
                .Property(e => e.Class)
                .IsUnicode(false);

            modelBuilder.Entity<Bank_accounts>()
                .HasOptional(e => e.Cash_turnover)
                .WithRequired(e => e.Bank_accounts)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Bank_accounts>()
                .HasOptional(e => e.Closing_balances)
                .WithRequired(e => e.Bank_accounts)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Bank_accounts>()
                .HasOptional(e => e.Opening_balances)
                .WithRequired(e => e.Bank_accounts)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Files>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Files>()
                .Property(e => e.Path)
                .IsUnicode(false);

            modelBuilder.Entity<Files>()
                .Property(e => e.Bank_name)
                .IsUnicode(false);

            modelBuilder.Entity<Files>()
                .HasMany(e => e.Bank_accounts)
                .WithRequired(e => e.Files)
                .HasForeignKey(e => e.id_File)
                .WillCascadeOnDelete(false);
        }
    }
}
