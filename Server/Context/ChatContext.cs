using System;
using System.Collections.Generic;
using BlazingChat.Server.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazingChat.Server.Context;

public partial class ChatContext : DbContext
{
    public ChatContext()
    {
    }

    public ChatContext(DbContextOptions<ChatContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AreaCode>? AreaCodes { get; set; }

    public virtual DbSet<Contact>? Contacts { get; set; }

    public virtual DbSet<Email>? Emails { get; set; }

    public virtual DbSet<Login>? Logins { get; set; }

    public virtual DbSet<Message>? Messages { get; set; }

    public virtual DbSet<Phone>? Phones { get; set; }

    public virtual DbSet<User>? Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AreaCode>(entity =>
        {
            entity.HasKey(e => e.AreaCode1);

            entity.ToTable("Area_Code");

            entity.Property(e => e.AreaCode1).HasColumnName("area_code");
            entity.Property(e => e.Country).HasColumnName("country");
            entity.Property(e => e.CountryCode).HasColumnName("country_code");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.ToTable("Contact");

            entity.Property(e => e.ContactId).HasColumnName("contact_id");
            entity.Property(e => e.ContactLastName).HasColumnName("contact_last_name");
            entity.Property(e => e.ContactName).HasColumnName("contact_name");
            entity.Property(e => e.ContactUserId).HasColumnName("contact_user_id");
            entity.Property(e => e.HasConversation)
                .HasColumnType("BOOLEAN")
                .HasColumnName("has_conversation");
            entity.Property(e => e.PrincipalUserId).HasColumnName("principal_user_id");

            entity.HasOne(d => d.ContactUser).WithMany(p => p.ContactContactUsers)
                .HasForeignKey(d => d.ContactUserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.PrincipalUser).WithMany(p => p.ContactPrincipalUsers)
                .HasForeignKey(d => d.PrincipalUserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Email>(entity =>
        {
            entity.ToTable("Email");

            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.EmailAddress).HasColumnName("email_address");
            entity.Property(e => e.HasPrincipal)
                .HasColumnType("BOOLEAN")
                .HasColumnName("has_principal");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Emails)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Login>(entity =>
        {
            entity.ToTable("Login");

            entity.Property(e => e.LoginId).HasColumnName("login_id");
            entity.Property(e => e.EmailAddres).HasColumnName("email_addres");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.UserName).HasColumnName("user_name");

            entity.HasOne(d => d.User).WithMany(p => p.Logins)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.ToTable("Message");

            entity.Property(e => e.MessageId).HasColumnName("message_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreateAt)
                .HasColumnType("DATETIME")
                .HasColumnName("create_at");
            entity.Property(e => e.MessageFrom).HasColumnName("message_from");
            entity.Property(e => e.MessageTo).HasColumnName("message_to");

            entity.HasOne(d => d.MessageFromNavigation).WithMany(p => p.MessageMessageFromNavigations)
                .HasForeignKey(d => d.MessageFrom)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.MessageToNavigation).WithMany(p => p.MessageMessageToNavigations)
                .HasForeignKey(d => d.MessageTo)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Phone>(entity =>
        {
            entity.ToTable("Phone");

            entity.Property(e => e.PhoneId).HasColumnName("phone_id");
            entity.Property(e => e.AreaCode).HasColumnName("area_code");
            entity.Property(e => e.Phone1).HasColumnName("phone");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.AreaCodeNavigation).WithMany(p => p.Phones)
                .HasForeignKey(d => d.AreaCode)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.Phones)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.AboutMe).HasColumnName("about_me");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("DATE")
                .HasColumnName("created_date");
            entity.Property(e => e.DarkTheme)
                .HasColumnType("BOOLEAN")
                .HasColumnName("dark_theme");
            entity.Property(e => e.DateOfBirth)
                .HasColumnType("DATETIME")
                .HasColumnName("date_of_birth");
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.LastName).HasColumnName("last_name");
            entity.Property(e => e.Notifications)
                .HasColumnType("BOOLEAN")
                .HasColumnName("notifications");
            entity.Property(e => e.ProfilePictureUrl).HasColumnName("profile_picture_url");
            entity.Property(e => e.SecondName).HasColumnName("second_name");
            entity.Property(e => e.Source).HasColumnName("source");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
