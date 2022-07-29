using com.tweetapp.domain.Base;
using com.tweetapp.domain.Models;
using com.tweetapp.infrastructure.EntityConfigrations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace com.tweetapp.infrastructure.DataContext
{
   public class TwitterDbContext :DbContext, IUnitOfWork
    {
        public TwitterDbContext(DbContextOptions<TwitterDbContext> options):base(options)
        {

        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Tweet> Tweets  { get; set; }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await base.SaveChangesAsync(cancellationToken);
            return true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TweetEntityTypeConfiguration());

            


        }
    }
}
