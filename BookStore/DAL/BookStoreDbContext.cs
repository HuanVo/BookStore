namespace DAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class BookStoreDbContext : DbContext
    {
        public BookStoreDbContext()
            : base("name=BookStoreDbContext")
        {
        }

        public virtual DbSet<CUSTOMER> CUSTOMERs { get; set; }
        public virtual DbSet<MAIL> MAILs { get; set; }
        public virtual DbSet<SYSTEM> SYSTEMS { get; set; }
        public virtual DbSet<USER> USERS { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MAIL>()
                .Property(e => e.from_address)
                .IsUnicode(false);

            modelBuilder.Entity<SYSTEM>()
                .Property(e => e.config_value)
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .Property(e => e.login_id)
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .Property(e => e.login_pass)
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .Property(e => e.loginkey)
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .Property(e => e.mail)
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .Property(e => e.member_modify)
                .IsUnicode(false);
        }
    }
}
