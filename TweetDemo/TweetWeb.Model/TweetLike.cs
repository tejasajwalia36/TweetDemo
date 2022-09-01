﻿namespace TweetWeb.Model
{
    public class TweetLike
    {
        public int Id { get; set; }

        public Tweet Tweet { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
