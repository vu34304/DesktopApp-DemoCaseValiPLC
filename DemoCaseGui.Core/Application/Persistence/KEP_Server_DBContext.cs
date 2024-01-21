   using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DemoCaseGui.Core.Application.Models;
public partial class KEP_Server_DBContext : DbContext
{
    public KEP_Server_DBContext()
    {
    }

    public KEP_Server_DBContext(DbContextOptions<KEP_Server_DBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<InverterLog> InverterLogs { get; set; } = null!;
    public virtual DbSet<ValiIfmLog> ValiIfmLogs { get; set; } = null!;
    public virtual DbSet<ValiSiemensLog> ValiSiemensLogs { get; set; } = null!;
    public virtual DbSet<StepMotorLog> StepMotorLogs { get; set; } = null!;
    public virtual DbSet<ValiMicroLog> ValiMicroLogs { get; set; } = null!;
    public virtual DbSet<ValiMicro820Log> ValiMicro820Logs { get; set; } = null!;
    public virtual DbSet<ValiCompactLog> ValiCompactLogs { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
#pragma warning disable CS1030 // #warning directive
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            //optionsBuilder.UseSqlServer("Data Source=DESKTOP-BL3N4U5\\WINCC;Initial Catalog=KEP_Server_DB;Integrated Security=False;TrustServerCertificate=True");
#pragma warning restore CS1030 // #warning directive
            var configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json");

            var config = configuration.Build();
            var connectionString = config.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString, builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });
            base.OnConfiguring(optionsBuilder);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Vietnamese_CI_AS");

        modelBuilder.Entity<InverterLog>(entity =>
        {
            entity.ToTable("Inverter_LOG");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("_NAME");

            entity.Property(e => e.Numericid).HasColumnName("_NUMERICID");

            entity.Property(e => e.Quality).HasColumnName("_QUALITY");

            entity.Property(e => e.Timestamp)
                .HasColumnType("datetime")
                .HasColumnName("_TIMESTAMP");

            entity.Property(e => e.Value)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("_VALUE");
        });

        modelBuilder.Entity<ValiIfmLog>(entity =>
        {
            entity.ToTable("ValiIFM_LOG");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("_NAME");

            entity.Property(e => e.Numericid).HasColumnName("_NUMERICID");

            entity.Property(e => e.Quality).HasColumnName("_QUALITY");

            entity.Property(e => e.Timestamp)
                .HasColumnType("datetime")
                .HasColumnName("_TIMESTAMP");

            entity.Property(e => e.Value)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("_VALUE");
        });
        modelBuilder.Entity<ValiCompactLog>(entity =>
        {
            entity.ToTable("ValiCompact_LOG");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("_NAME");

            entity.Property(e => e.Numericid).HasColumnName("_NUMERICID");

            entity.Property(e => e.Quality).HasColumnName("_QUALITY");

            entity.Property(e => e.Timestamp)
                .HasColumnType("datetime")
                .HasColumnName("_TIMESTAMP");

            entity.Property(e => e.Value)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("_VALUE");
        });
        modelBuilder.Entity<ValiMicroLog>(entity =>
        {
            entity.ToTable("ValiMicro_LOG");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("_NAME");

            entity.Property(e => e.Numericid).HasColumnName("_NUMERICID");

            entity.Property(e => e.Quality).HasColumnName("_QUALITY");

            entity.Property(e => e.Timestamp)
                .HasColumnType("datetime")
                .HasColumnName("_TIMESTAMP");

            entity.Property(e => e.Value)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("_VALUE");
        });

        modelBuilder.Entity<ValiMicro820Log>(entity =>
        {
            entity.ToTable("ValiMicro820_LOG");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("_NAME");

            entity.Property(e => e.Numericid).HasColumnName("_NUMERICID");

            entity.Property(e => e.Quality).HasColumnName("_QUALITY");

            entity.Property(e => e.Timestamp)
                .HasColumnType("datetime")
                .HasColumnName("_TIMESTAMP");

            entity.Property(e => e.Value)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("_VALUE");
        });

        modelBuilder.Entity<StepMotorLog>(entity =>
        {
            entity.ToTable("StepMotor_LOG");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("_NAME");

            entity.Property(e => e.Numericid).HasColumnName("_NUMERICID");

            entity.Property(e => e.Quality).HasColumnName("_QUALITY");

            entity.Property(e => e.Timestamp)
                .HasColumnType("datetime")
                .HasColumnName("_TIMESTAMP");

            entity.Property(e => e.Value)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("_VALUE");
        });


        modelBuilder.Entity<ValiSiemensLog>(entity =>
        {
            entity.ToTable("ValiSiemens_LOG");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("_NAME");

            entity.Property(e => e.Numericid).HasColumnName("_NUMERICID");

            entity.Property(e => e.Quality).HasColumnName("_QUALITY");

            entity.Property(e => e.Timestamp)
                .HasColumnType("datetime")
                .HasColumnName("_TIMESTAMP");

            entity.Property(e => e.Value)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("_VALUE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
