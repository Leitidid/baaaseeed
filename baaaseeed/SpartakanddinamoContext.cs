using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace baaaseeed;

public partial class SpartakanddinamoContext : DbContext
{
    public SpartakanddinamoContext()
    {
    }

    public SpartakanddinamoContext(DbContextOptions<SpartakanddinamoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Krasavci> Krasavcis { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\sqlexpress; Database=spartakanddinamo; User=исп-34; Password=1234567890; Encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Krasavci>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("krasavci");

            entity.Property(e => e.ДатаПриемаВКоманду).HasColumnName("дата приема в команду");
            entity.Property(e => e.Имя)
                .HasMaxLength(255)
                .IsFixedLength()
                .HasColumnName("имя");
            entity.Property(e => e.КоличествоГолевыхПередач).HasColumnName("количество голевых передач");
            entity.Property(e => e.КолмчествоСыгранныхМатчей).HasColumnName("колмчество сыгранных матчей");
            entity.Property(e => e.НазваниеКоманды)
                .HasMaxLength(255)
                .IsFixedLength()
                .HasColumnName("название команды");
            entity.Property(e => e.Отчество)
                .HasMaxLength(255)
                .IsFixedLength()
                .HasColumnName("отчество");
            entity.Property(e => e.Фамилия)
                .HasMaxLength(255)
                .IsFixedLength();
            entity.Property(e => e.ЧислоЗаброшенныхШайб).HasColumnName("число заброшенных шайб");
            entity.Property(e => e.ШтрафноеВремя).HasColumnName("штрафное время");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Players__3214EC27A84FFFDA");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Имя).HasMaxLength(255);
            entity.Property(e => e.НазваниеКоманды).HasMaxLength(255);
            entity.Property(e => e.Отчество).HasMaxLength(255);
            entity.Property(e => e.Фамилия).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
