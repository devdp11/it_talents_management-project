using System;
using System.Collections.Generic;
using BusinessLogic.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.databaseContext;

public partial class databaseContext : DbContext
{
    public databaseContext()
    {
    }

    public databaseContext(DbContextOptions<databaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Experience> Experiences { get; set; }

    public virtual DbSet<Jobproposal> Jobproposals { get; set; }

    public virtual DbSet<JobproposalSkill> JobproposalSkills { get; set; }

    public virtual DbSet<Professional> Professionals { get; set; }

    public virtual DbSet<ProfessionalSkill> ProfessionalSkills { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=es2;Username=es2;Password=es2;SearchPath=public;Port=5480");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("postgis")
            .HasPostgresExtension("uuid-ossp")
            .HasPostgresExtension("topology", "postgis_topology");

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Clientid).HasName("clients_pkey");

            entity.ToTable("clients");

            entity.Property(e => e.Clientid).HasColumnName("clientid");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Clients)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("clients_userid_fkey");
        });

        modelBuilder.Entity<Experience>(entity =>
        {
            entity.HasKey(e => e.Experienceid).HasName("experiences_pkey");

            entity.ToTable("experiences");

            entity.Property(e => e.Experienceid).HasColumnName("experienceid");
            entity.Property(e => e.Company)
                .HasMaxLength(100)
                .HasColumnName("company");
            entity.Property(e => e.Endyear).HasColumnName("endyear");
            entity.Property(e => e.Professionalid).HasColumnName("professionalid");
            entity.Property(e => e.Startyear).HasColumnName("startyear");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("title");

            entity.HasOne(d => d.Professional).WithMany(p => p.Experiences)
                .HasForeignKey(d => d.Professionalid)
                .HasConstraintName("experiences_professionalid_fkey");
        });

        modelBuilder.Entity<Jobproposal>(entity =>
        {
            entity.HasKey(e => e.Jobproposalid).HasName("jobproposals_pkey");

            entity.ToTable("jobproposals");

            entity.Property(e => e.Jobproposalid).HasColumnName("jobproposalid");
            entity.Property(e => e.Clientid).HasColumnName("clientid");
            entity.Property(e => e.Jobdescription).HasColumnName("jobdescription");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Talentcategory)
                .HasMaxLength(100)
                .HasColumnName("talentcategory");
            entity.Property(e => e.Totalhours).HasColumnName("totalhours");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Client).WithMany(p => p.Jobproposals)
                .HasForeignKey(d => d.Clientid)
                .HasConstraintName("jobproposals_clientid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Jobproposals)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("jobproposals_userid_fkey");
        });

        modelBuilder.Entity<JobproposalSkill>(entity =>
        {
            entity.HasKey(e => new { e.Jobproposalid, e.Skillid }).HasName("jobproposal_skills_pkey");

            entity.ToTable("jobproposal_skills");

            entity.Property(e => e.Jobproposalid).HasColumnName("jobproposalid");
            entity.Property(e => e.Skillid).HasColumnName("skillid");
            entity.Property(e => e.Minyearsexperience).HasColumnName("minyearsexperience");

            entity.HasOne(d => d.Jobproposal).WithMany(p => p.JobproposalSkills)
                .HasForeignKey(d => d.Jobproposalid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("jobproposal_skills_jobproposalid_fkey");

            entity.HasOne(d => d.Skill).WithMany(p => p.JobproposalSkills)
                .HasForeignKey(d => d.Skillid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("jobproposal_skills_skillid_fkey");
        });

        modelBuilder.Entity<Professional>(entity =>
        {
            entity.HasKey(e => e.Professionalid).HasName("professionals_pkey");

            entity.ToTable("professionals");

            entity.Property(e => e.Professionalid).HasColumnName("professionalid");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .HasColumnName("country");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Hourlyrate).HasColumnName("hourlyrate");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Visibility)
                .HasMaxLength(50)
                .HasColumnName("visibility");

            entity.HasOne(d => d.User).WithMany(p => p.Professionals)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("professionals_userid_fkey");
        });

        modelBuilder.Entity<ProfessionalSkill>(entity =>
        {
            entity.HasKey(e => new { e.Professionalid, e.Skillid }).HasName("professional_skills_pkey");

            entity.ToTable("professional_skills");

            entity.Property(e => e.Professionalid).HasColumnName("professionalid");
            entity.Property(e => e.Skillid).HasColumnName("skillid");
            entity.Property(e => e.Yearsexperience).HasColumnName("yearsexperience");

            entity.HasOne(d => d.Professional).WithMany(p => p.ProfessionalSkills)
                .HasForeignKey(d => d.Professionalid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("professional_skills_professionalid_fkey");

            entity.HasOne(d => d.Skill).WithMany(p => p.ProfessionalSkills)
                .HasForeignKey(d => d.Skillid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("professional_skills_skillid_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.Skillid).HasName("skills_pkey");

            entity.ToTable("skills");

            entity.Property(e => e.Skillid).HasColumnName("skillid");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Professionalarea)
                .HasMaxLength(100)
                .HasColumnName("professionalarea");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.Roleid)
                .HasConstraintName("users_roleid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
