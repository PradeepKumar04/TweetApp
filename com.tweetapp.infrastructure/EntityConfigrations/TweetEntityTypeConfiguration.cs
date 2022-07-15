using com.tweetapp.domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.tweetapp.infrastructure.EntityConfigrations
{
    public class TweetEntityTypeConfiguration : IEntityTypeConfiguration<Tweet>
    {
        public void Configure(EntityTypeBuilder<Tweet> builder)
        {
            builder.ToTable("Tweets", "TweetApp");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.Message)
              .HasColumnName("message")
              .HasColumnType("NVARCHAR(144)")
              .HasMaxLength(144)
              .IsRequired(true);

            builder.Property(e => e.CreatedDate)
              .HasColumnName("created_date")
              .HasColumnType("datetime")
              .IsRequired(true);

            
        }
    }
}
