using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using LibraryDomain.Model;

namespace LibraryInfrastructure;

public partial class DblibraryContext : DbContext
{
    public DblibraryContext()
    {
    }

    public DblibraryContext(DbContextOptions<DblibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Reader> Readers { get; set; }

    public virtual DbSet<ReaderBook> ReaderBooks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=PC; Database=DBLibrary; Trusted_Connection=True; TrustServerCertificate=True; ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.AuthorId).HasName("PK__Authors__70DAFC34DD7A047F");

            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__Books__3DE0C207BCF2F99E");

            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasMany(d => d.Authors).WithMany(p => p.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "AuthorBook",
                    r => r.HasOne<Author>().WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK__AuthorBoo__Autho__6D0D32F4"),
                    l => l.HasOne<Book>().WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK__AuthorBoo__BookI__6C190EBB"),
                    j =>
                    {
                        j.HasKey("BookId", "AuthorId").HasName("PK__AuthorBo__6AED6DC4C7ADBB47");
                        j.ToTable("AuthorBooks");
                    });

            entity.HasMany(d => d.Genres).WithMany(p => p.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "GenreBook",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK__GenreBook__Genre__693CA210"),
                    l => l.HasOne<Book>().WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK__GenreBook__BookI__68487DD7"),
                    j =>
                    {
                        j.HasKey("BookId", "GenreId").HasName("PK__GenreBoo__CDD89250DA500ECE");
                        j.ToTable("GenreBooks");
                    });
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("PK__Genres__0385057EE7A450B1");

            entity.Property(e => e.GenreName).HasMaxLength(50);
        });

        modelBuilder.Entity<Reader>(entity =>
        {
            entity.HasKey(e => e.ReaderId).HasName("PK__Readers__8E67A5E1D228476C");

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
        });

        modelBuilder.Entity<ReaderBook>(entity =>
        {
            entity.HasKey(e => e.ReaderBookId).HasName("PK__ReaderBo__B7C68EF01CC3A62C");

            entity.HasOne(d => d.Book).WithMany(p => p.ReaderBooks)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK__ReaderBoo__BookI__6477ECF3");

            entity.HasOne(d => d.Reader).WithMany(p => p.ReaderBooks)
                .HasForeignKey(d => d.ReaderId)
                .HasConstraintName("FK__ReaderBoo__Reade__656C112C");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}