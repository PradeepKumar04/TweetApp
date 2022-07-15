using com.tweetapp.domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.tweetapp.infrastructure.EntityConfigrations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users","TweetApp");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.FirstName)
                .HasColumnName("first_name")
                .HasColumnType("NVARCHAR(100)")
                .HasMaxLength(90)
                .IsRequired(true);

            builder.Property(e => e.LastName)
                .HasColumnName("last_name")
                .HasColumnType("NVARCHAR(100)")
                .HasMaxLength(90)
                .IsRequired(false);

            builder.Property(e => e.Gender)
                .HasColumnName("gender")
                .HasColumnType("int")
                .IsRequired(true);

            builder.Property(e => e.Email)
                .HasColumnName("email")
                .HasColumnType("NVARCHAR(100)")
                .HasMaxLength(90)
                .IsRequired(true);

            builder.Property(e => e.Password)
                .HasColumnName("password")
                .HasColumnType("NVARCHAR(100)")
                .IsRequired(true)
                .HasMaxLength(100);

            builder.Property(e => e.DateOfBirth)
                .HasColumnName("dob")
                .HasColumnType("datetime")
                .HasDefaultValue(null);

            builder.Property(e => e.LastSeen)
                .HasColumnName("last_seen")
                .HasColumnType("datetime")
                .HasDefaultValue(null);

            builder.Property(e => e.IsActive)
                .HasColumnName("is_active")
                .HasColumnType("bit")
                .HasDefaultValue(0)
                .IsRequired(true);
        }
    }
}
