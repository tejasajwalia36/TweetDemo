using Microsoft.EntityFrameworkCore;
using TweetWeb.Model;

namespace TweetWeb.Model.Data
{
    public class TweetDbContext : DbContext
    {
        public TweetDbContext(DbContextOptions<TweetDbContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Tweet> Tweets { get; set; }

        public DbSet<TweetLike> TweetLikes { get; set; }

    }
}
