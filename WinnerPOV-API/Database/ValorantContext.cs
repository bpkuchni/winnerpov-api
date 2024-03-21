using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WinnerPOV_API.Database;

public partial class ValorantContext : DbContext
{
    public ValorantContext()
    {
    }

    public ValorantContext(DbContextOptions<ValorantContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Agent> Agents { get; set; }

    public virtual DbSet<HenrikMatchMapping> HenrikMatchMappings { get; set; }

    public virtual DbSet<HenrikPlayerMapping> HenrikPlayerMappings { get; set; }

    public virtual DbSet<Map> Maps { get; set; }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<PlayerMatch> PlayerMatches { get; set; }

    public virtual DbSet<Rank> Ranks { get; set; }

    public virtual DbSet<Season> Seasons { get; set; }

    private static string Server = Environment.GetEnvironmentVariable("VALDB_SERVER");
    private static string UserName = Environment.GetEnvironmentVariable("VALDB_USERNAME");
    private static string Password = Environment.GetEnvironmentVariable("VALDB_PASS");

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySQL($"server={Server};uid={UserName};pwd={Password};database=valorant");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agent>(entity =>
        {
            entity.HasKey(e => e.AgentId).HasName("PRIMARY");

            entity.ToTable("agent");

            entity.HasIndex(e => e.AgentId, "AgentId_UNIQUE").IsUnique();

            entity.Property(e => e.AgentId).HasColumnName("AgentID");
            entity.Property(e => e.Name).HasMaxLength(45);
            entity.Property(e => e.PortraitUrl).HasMaxLength(255);
            entity.Property(e => e.ThumbnailUrl).HasMaxLength(255);
        });

        modelBuilder.Entity<HenrikMatchMapping>(entity =>
        {
            entity.HasKey(e => new { e.HenrikId, e.MatchId }).HasName("PRIMARY");

            entity.ToTable("henrik_match_mapping");

            entity.HasIndex(e => e.MatchId, "MatchID_idx");

            entity.Property(e => e.HenrikId).HasColumnName("HenrikID");
            entity.Property(e => e.MatchId).HasColumnName("MatchID");

            entity.HasOne(d => d.Match).WithMany(p => p.HenrikMatchMappings)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("HenrikMatchID");
        });

        modelBuilder.Entity<HenrikPlayerMapping>(entity =>
        {
            entity.HasKey(e => new { e.HenrikId, e.PlayerId }).HasName("PRIMARY");

            entity.ToTable("henrik_player_mapping");

            entity.HasIndex(e => e.PlayerId, "HenrikPlayerID_idx");

            entity.Property(e => e.HenrikId).HasColumnName("HenrikID");
            entity.Property(e => e.PlayerId).HasColumnName("PlayerID");

            entity.HasOne(d => d.Player).WithMany(p => p.HenrikPlayerMappings)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("HenrikPlayerID");
        });

        modelBuilder.Entity<Map>(entity =>
        {
            entity.HasKey(e => e.MapId).HasName("PRIMARY");

            entity.ToTable("map");

            entity.HasIndex(e => e.MapId, "MapID_UNIQUE").IsUnique();

            entity.Property(e => e.MapId).HasColumnName("MapID");
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(45);
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasKey(e => e.MatchId).HasName("PRIMARY");

            entity.ToTable("match");

            entity.HasIndex(e => e.MapId, "MapID_idx");

            entity.HasIndex(e => e.MatchId, "MatchID_UNIQUE").IsUnique();

            entity.Property(e => e.MatchId).HasColumnName("MatchID");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Duration).HasColumnType("timestamp");
            entity.Property(e => e.MapId).HasColumnName("MapID");
            entity.Property(e => e.OpponentImageUrl).HasMaxLength(255);
            entity.Property(e => e.OpponentName).HasMaxLength(45);
            entity.Property(e => e.OpponentTag).HasMaxLength(45);

            entity.HasOne(d => d.Map).WithMany(p => p.Matches)
                .HasForeignKey(d => d.MapId)
                .HasConstraintName("MapID");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.PlayerId).HasName("PRIMARY");

            entity.ToTable("player");

            entity.HasIndex(e => e.PlayerId, "PlayerID_UNIQUE").IsUnique();

            entity.HasIndex(e => e.RankId, "RankID_idx");

            entity.Property(e => e.PlayerId).HasColumnName("PlayerID");
            entity.Property(e => e.LandscapeUrl).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(45);
            entity.Property(e => e.PortraitUrl).HasMaxLength(255);
            entity.Property(e => e.RankId).HasColumnName("RankID");
            entity.Property(e => e.Tag).HasMaxLength(45);
            entity.Property(e => e.ThumbnailUrl).HasMaxLength(255);

            entity.HasOne(d => d.Rank).WithMany(p => p.Players)
                .HasForeignKey(d => d.RankId)
                .HasConstraintName("RankID");
        });

        modelBuilder.Entity<PlayerMatch>(entity =>
        {
            entity.HasKey(e => new { e.PlayerId, e.MatchId }).HasName("PRIMARY");

            entity.ToTable("player_match");

            entity.HasIndex(e => e.AgentId, "AgentID_idx");

            entity.HasIndex(e => e.MatchId, "MatchID_idx");

            entity.Property(e => e.PlayerId).HasColumnName("PlayerID");
            entity.Property(e => e.MatchId).HasColumnName("MatchID");
            entity.Property(e => e.AgentId).HasColumnName("AgentID");

            entity.HasOne(d => d.Agent).WithMany(p => p.PlayerMatches)
                .HasForeignKey(d => d.AgentId)
                .HasConstraintName("AgentID");

            entity.HasOne(d => d.Match).WithMany(p => p.PlayerMatches)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MatchID");

            entity.HasOne(d => d.Player).WithMany(p => p.PlayerMatches)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PlayerID");
        });

        modelBuilder.Entity<Rank>(entity =>
        {
            entity.HasKey(e => e.RankId).HasName("PRIMARY");

            entity.ToTable("rank");

            entity.HasIndex(e => e.RankId, "RankID_UNIQUE").IsUnique();

            entity.Property(e => e.RankId).HasColumnName("RankID");
            entity.Property(e => e.IconBigUrl).HasMaxLength(255);
            entity.Property(e => e.IconSmallUrl).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(45);
        });

        modelBuilder.Entity<Season>(entity =>
        {
            entity.HasKey(e => e.SeasonId).HasName("PRIMARY");

            entity.ToTable("season");

            entity.HasIndex(e => e.SeasonId, "SeasonID_UNIQUE").IsUnique();

            entity.Property(e => e.SeasonId).HasColumnName("SeasonID");
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.Name).HasMaxLength(45);
            entity.Property(e => e.StartDate).HasColumnType("date");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
