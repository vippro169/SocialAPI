using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Application.Posts;
using SocialNetwork.Application.Posts.RequestModel;
using SocialNetwork.Domain;

namespace SocialNetwork.Service.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/post")]
    public class PostController : BaseController
    {
        private readonly IPostHandler _postHandler;

        public PostController(IPostHandler postHandler)
        {
            _postHandler = postHandler;
        }

        [Route("create")]
        [HttpPost]
        public IActionResult CreatePost([FromBody]CreatePostRequest request)
        {
            _postHandler.CreatePost(request);
            return Ok();
        }

        [Route("get-profile-posts/{userId}")]
        [HttpGet]
        public IActionResult GetProfilePosts(string userId)
        {
            var result = _postHandler.GetUserPosts(userId, AuthUserId);
            return Ok(result);
        }

        [Route("get-all-posts/{userId}")]
        [HttpGet]
        public IActionResult GetAllPosts(string userId)
        {
            var result = _postHandler.GetAllPosts(userId);
            return Ok(result);
        }

        [Route("edit/{postId}")]
        [HttpPut]
        public IActionResult EditPost([FromBody]Post post)
        {
            _postHandler.EditPost(post);
            return Ok();
        }

        [Route("delete/{postId}")]
        [HttpDelete]
        public IActionResult DeletePost(string postId)
        {
            _postHandler.DeletePost(postId);
            return Ok();
        }
    }
}