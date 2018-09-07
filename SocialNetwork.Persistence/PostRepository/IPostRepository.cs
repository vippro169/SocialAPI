using SocialNetwork.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Persistence.MySql.PostRepository
{
    public interface IPostRepository
    {
        void CreatePost(Post post);
        void Edit(Post post);
        void Delete(string postId);
        List<Post> GetAllUserPosts(string userId);
        List<Post> GetPublicUserPosts(string userId);
        List<Post> GetPublicFriendsUserPosts(string userId);
    }
}
