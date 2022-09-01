﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TweetWeb.Service;

namespace TweetWebApi.Controllers
{
    [Authorize]
    [ApiController]
    public class TweetLikeController : ControllerBase
    {
        private readonly ITweetLikeService tweetLikeService;

        public TweetLikeController(ITweetLikeService tweetLikeService)
        {
            this.tweetLikeService = tweetLikeService;
        }

        [HttpPut("/api/v1.0/tweets/{userName}/like/{id}")]
        public async Task<IActionResult> Update(string userName, int id)
        {
            var response = await this.tweetLikeService.TweetLike(id, userName);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
