using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete
{
    public class Context : DbContext
    {
        
        public DbSet<Movie>? Movies { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<Watched>? Watcheds { get; set; }
        public DbSet<MovieGenre>? MovieGenres { get; set; }


        // watched tablosunun users ve movie tablosuna foreign key ile bağlanması
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Watched>()
            .HasKey(w => w.WatchedID);

            modelBuilder.Entity<Watched>()
                .HasOne(w => w.Movie)
                .WithMany(m => m.Watcheds)
                .HasForeignKey(w => w.MovieID);

            modelBuilder.Entity<Watched>()
                .HasOne(w => w.User)
                .WithMany(u => u.Watcheds)
                .HasForeignKey(w => w.UserID);
            

            modelBuilder.Entity<Movie>()
            .HasOne(m => m.MovieGenre)
            .WithMany()
            .HasForeignKey(m => m.MovieGenreID);

            base.OnModelCreating(modelBuilder);

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=DESKTOP-SP67UG4\\SQLEXPRESS;database=MovieAPIDb;integrated security=true");
        }

    }
}
