namespace NewLibraryCD.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class ModelDataBaseContext : DbContext
    {
        static ModelDataBaseContext()
        {
            Database.SetInitializer<ModelDataBaseContext>(new MyContextInitializer());
        }
        public ModelDataBaseContext()
            : base("LibraryDB")
        {
        }

        public virtual DbSet<Direction> Directions { get; set; }
        public virtual DbSet<Disk> Disks { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Disk>().Property(x => x.DiskId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Direction>().Property(x => x.DirectionId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Disk>().HasRequired(x => x.Direction);
            modelBuilder.Entity<Photo>().Property(x => x.PhotoId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Disk>().HasOptional(x => x.Photo);

            modelBuilder.Entity<Rating>().Property(x => x.RatingId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Rating>().HasRequired(x => x.Disk);
            modelBuilder.Entity<Rating>().HasRequired(x => x.User);


            // использование Fluent API
            base.OnModelCreating(modelBuilder);
        }

        /*protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Direction>()
                .HasMany(e => e.Disk)
                .WithRequired(e => e.Direction1)
                .HasForeignKey(e => e.Direction)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Image>()
                .HasMany(e => e.Disk)
                .WithRequired(e => e.Image)
                .HasForeignKey(e => e.Photo)
                .WillCascadeOnDelete(false);
        }*/
    }

    //class MyContextInitializer : DropCreateDatabaseAlways<ModelDataBaseContext>
    class MyContextInitializer : DropCreateDatabaseIfModelChanges<ModelDataBaseContext>
    {
        protected override void Seed(ModelDataBaseContext db){}
    }
}
