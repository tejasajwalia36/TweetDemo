﻿using System.ComponentModel.DataAnnotations;

namespace TweetWeb.Model
{
    public class Tweet
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(144)]
        public string Message { get; set; }

        [Required]
        [MaxLength(50)]
        public string Tag { get; set; }

        public int Likes { get; set; }

        public Tweet? ParentTweet { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public User User { get; set; }

        public ICollection<TweetLike> TweetLike { get; set; }
    }
}
