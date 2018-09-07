using SocialNetwork.Application.Posts.RequestModel;
using SocialNetwork.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Application.Posts
{
    public interface IPostHandler
    {
        void CreatePost(CreatePostRequest request);
        List<Post> GetUserPosts(string userId, string authId);
        List<Post> GetAllPosts(string userId);
        void EditPost(Post post);
        void DeletePost(string postId);
    }
}
