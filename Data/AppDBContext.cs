using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using testana_api.Data.Models;

namespace testana_api.Data;

public partial class AppDBContext : DbContext
{
    public AppDBContext()
    {
    }

    public AppDBContext(DbContextOptions<AppDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AnswersQuestionsUser> AnswersQuestionsUsers { get; set; }

    public virtual DbSet<Collaborator> Collaborators { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionAnswer> QuestionAnswers { get; set; }

    public virtual DbSet<QuestionType> QuestionTypes { get; set; }

    public virtual DbSet<Recommendation> Recommendations { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersAnswer> UsersAnswers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AnswersQuestionsUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AnswersQ__3213E83F79199C2A");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Correct).HasColumnName("correct");
            entity.Property(e => e.OtherAnswer)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("otherAnswer");
            entity.Property(e => e.QuestionAnswerId).HasColumnName("questionAnswerId");
            entity.Property(e => e.UserAnswerId).HasColumnName("userAnswerId");

            entity.HasOne(d => d.QuestionAnswer).WithMany(p => p.AnswersQuestionsUsers)
                .HasForeignKey(d => d.QuestionAnswerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AnswersQuestionsUsers.questionAnswerId");

            entity.HasOne(d => d.UserAnswer).WithMany(p => p.AnswersQuestionsUsers)
                .HasForeignKey(d => d.UserAnswerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AnswersQuestionsUsers.userAnswerId");
        });

        modelBuilder.Entity<Collaborator>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Collabor__3213E83FEF3D988F");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TestId).HasColumnName("testId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Test).WithMany(p => p.Collaborators)
                .HasForeignKey(d => d.TestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Collaborators.testId");

            entity.HasOne(d => d.User).WithMany(p => p.Collaborators)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Collaborators.userId");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3213E83F4789B26D");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CaseSensitivity).HasColumnName("caseSensitivity");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Image)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("image");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.Points).HasColumnName("points");
            entity.Property(e => e.QuestionTypeId).HasColumnName("questionTypeId");
            entity.Property(e => e.TestId).HasColumnName("testId");

            entity.HasOne(d => d.QuestionType).WithMany(p => p.Questions)
                .HasForeignKey(d => d.QuestionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Questions.questionTypeId");

            entity.HasOne(d => d.Test).WithMany(p => p.Questions)
                .HasForeignKey(d => d.TestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Questions.testId");
        });

        modelBuilder.Entity<QuestionAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3213E83FF1BCECCE");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Correct).HasColumnName("correct");
            entity.Property(e => e.QuestionId).HasColumnName("questionId");
            entity.Property(e => e.Text)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("text");

            entity.HasOne(d => d.Question).WithMany(p => p.Answers)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuestionAnswers.questionId");
        });

        modelBuilder.Entity<QuestionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3213E83F56E7976A");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Recommendation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Recommen__3213E83F3B0FD02A");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CollaboratorId).HasColumnName("collaboratorId");
            entity.Property(e => e.QuestionId).HasColumnName("questionId");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.Note)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("note");

            entity.HasOne(d => d.Collaborator).WithMany(p => p.Recommendations)
                .HasForeignKey(d => d.CollaboratorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recommendations.collaboratorId");
            
            entity.HasOne(d => d.Question).WithMany(p => p.Recommendations)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recommendations.questionId");
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tests__3213E83F02C97DD8");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("color");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Dislikes).HasColumnName("dislikes");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.EvaluateByQuestion).HasColumnName("evaluateByQuestion");
            entity.Property(e => e.Image)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("image");
            entity.Property(e => e.Likes).HasColumnName("likes");
            entity.Property(e => e.Random).HasColumnName("random");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Tags)
                .IsUnicode(false)
                .HasColumnName("tags");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.Visibility)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("visibility");

            entity.HasOne(d => d.User).WithMany(p => p.Tests)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tests.userId");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3213E83FD40F3D29");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Avatar)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("avatar");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password");
        });

        modelBuilder.Entity<UsersAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UsersAns__3213E83FFD5D686D");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("completionDate");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.TestId).HasColumnName("testId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Test).WithMany(p => p.UsersAnswers)
                .HasForeignKey(d => d.TestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsersAnswers.testId");

            entity.HasOne(d => d.User).WithMany(p => p.UsersAnswers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsersAnswers.userId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
