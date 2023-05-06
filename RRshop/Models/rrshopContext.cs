using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace RRshop.Models
{
    public partial class rrshopContext : DbContext
    {
        public rrshopContext()
        {
        }

        public rrshopContext(DbContextOptions<rrshopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Prod> Prods { get; set; } = null!;
        public virtual DbSet<Size> Sizes { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserCart> UserCarts { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new ArgumentNullException("Unset conection string");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category");

                entity.HasIndex(e => e.Title, "name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Title).HasColumnName("title");
            });

            modelBuilder.Entity<Prod>(entity =>
            {
                entity.ToTable("prod");

                entity.HasIndex(e => e.CategoryId, "FK_cat_id_idx");

                entity.HasIndex(e => e.Title, "title_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Color)
                    .HasMaxLength(45)
                    .HasColumnName("color");

                entity.Property(e => e.ImgPath)
                    .HasMaxLength(255)
                    .HasColumnName("img_path")
                    .HasDefaultValueSql("'default.png'");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.SaleQuantity).HasColumnName("sale_quantity");

                entity.Property(e => e.Title).HasColumnName("title");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Prods)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_cat_id");
            });

            modelBuilder.Entity<Size>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("size");

                entity.HasIndex(e => e.ProdId, "FK_size_prod_id_idx");

                entity.Property(e => e.ProdId).HasColumnName("prod_id");

                entity.Property(e => e.Size1).HasColumnName("size");

                entity.HasOne(d => d.Prod)
                    .WithMany()
                    .HasForeignKey(d => d.ProdId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_size_prod_id");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.Name, "email_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.Id, "id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.Phone, "phone_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.City)
                    .HasMaxLength(45)
                    .HasColumnName("city");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("timestamp")
                    .HasColumnName("create_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Password)
                    .HasMaxLength(32)
                    .HasColumnName("password");

                entity.Property(e => e.Phone)
                    .HasMaxLength(45)
                    .HasColumnName("phone");

                entity.Property(e => e.Role)
                    .HasMaxLength(45)
                    .HasColumnName("role");
            });

            modelBuilder.Entity<UserCart>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("user_cart");

                entity.HasIndex(e => e.UserId, "FK_cart_user_id_idx");

                entity.HasIndex(e => new { e.ProdId, e.UserId }, "FK_prod_id_idx");

                entity.Property(e => e.ProdId).HasColumnName("prod_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Prod)
                    .WithMany()
                    .HasForeignKey(d => d.ProdId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_cart_prod_id");

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_cart_user_id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
