﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TweetWeb.Dto;
using TweetWeb.Service;

namespace TweetWebApi.Controllers
{
    [Authorize]
    [ApiController]
    public class TweetController : ControllerBase
    {
        private readonly ITweetService tweetService;

        public TweetController(ITweetService tweetService)
        {
            this.tweetService = tweetService;
        }

        [HttpGet("/api/v1.0/tweets/all")]
        public async Task<IActionResult> Get()
        {
            var response = await this.tweetService.GetTweets();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("/api/v1.0/tweets/{userName}/all")]
        public async Task<IActionResult> Get(string userName)
        {
            var response = await this.tweetService.GetTweets(userName);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("/api/v1.0/tweets/{userName}/add")]
        public async Task<IActionResult> Add(string userName, TweetDto tweetDto)
        {
            var response = await this.tweetService.AddTweet(tweetDto, userName);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("/api/v1.0/tweets/{userName}/reply/{id}")]
        public async Task<IActionResult> Reply(string userName, int id, TweetDto tweetDto)
        {
            var response = await this.tweetService.ReplyTweet(tweetDto, userName, id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("/api/v1.0/tweets/{userName}/udpate/{id}")]
        public async Task<IActionResult> Update(string userName, int id, UpdateTweetDto tweetDto)
        {
            var response = await this.tweetService.UpdateTweet(id, tweetDto, userName);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("/api/v1.0/tweets/{userName}/delete/{id}")]
        public async Task<IActionResult> Delete(string userName, int id)
        {
            var response = await this.tweetService.DeleteTweet(id, userName);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
