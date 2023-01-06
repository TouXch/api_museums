using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API_museum.Models;

public partial class BdMuseumContext : DbContext
{
    public BdMuseumContext()
    {
    }

    public BdMuseumContext(DbContextOptions<BdMuseumContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbArticle> TbArticles { get; set; }

    public virtual DbSet<TbMuseum> TbMuseums { get; set; }

    public virtual DbSet<TbTheme> TbThemes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TbArticle>(entity =>
        {
            entity.HasKey(e => e.Idarticle).HasName("PK__tb_artic__2384F45DBF379DD3");

            entity.ToTable("tb_article");

            entity.Property(e => e.Idarticle).HasColumnName("idarticle");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Idmuseum).HasColumnName("idmuseum");
            entity.Property(e => e.Isdamaged).HasColumnName("isdamaged");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");

            entity.HasOne(d => d.oMuseum).WithMany(p => p.TbArticles)
                .HasForeignKey(d => d.Idmuseum)
                .HasConstraintName("fk_idtmuseum");
        });

        modelBuilder.Entity<TbMuseum>(entity =>
        {
            entity.HasKey(e => e.Idmuseum).HasName("PK__tb_museu__EE214FF3604DF2E1");

            entity.ToTable("tb_museum");

            entity.Property(e => e.Idmuseum).HasColumnName("idmuseum");
            entity.Property(e => e.Idtheme).HasColumnName("idtheme");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");

            entity.HasOne(d => d.oTheme).WithMany(p => p.TbMuseums)
                .HasForeignKey(d => d.Idtheme)
                .HasConstraintName("fk_idtheme");
        });

        modelBuilder.Entity<TbTheme>(entity =>
        {
            entity.HasKey(e => e.Idtheme).HasName("PK__tb_theme__94DF9585A45A0B22");

            entity.ToTable("tb_theme");

            entity.Property(e => e.Idtheme).HasColumnName("idtheme");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
