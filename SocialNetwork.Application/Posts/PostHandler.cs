using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SocialNetwork.Application.Posts.RequestModel;
using SocialNetwork.Domain;
using SocialNetwork.Persistence.MySql.FriendRepository;
using SocialNetwork.Persistence.MySql.PostRepository;

namespace SocialNetwork.Application.Posts
{
    public class PostHandler : IPostHandler
    {
        private readonly IPostRepository _postRepository;
        private readonly IFriendRepository _friendRepository;

        public PostHandler(IPostRepository postRepository, IFriendRepository friendRepository)
        {
            _postRepository = postRepository;
            _friendRepository = friendRepository;
        }

        public void CreatePost(CreatePostRequest request)
        {
            var post = new Post()
            {
                Id = Guid.NewGuid().ToString("N"),
                UserId = request.UserId,
                Content = request.Content,
                Privacy = request.Privacy
            };
            _postRepository.CreatePost(post);
        }

        public List<Post> GetUserPosts(string userId, string authId)
        {
            List<Post> posts = new List<Post>();
            if (userId == authId) posts = _postRepository.GetAllUserPosts(userId);
            else if (_friendRepository.CheckFriendship(authId, userId)) posts = _postRepository.GetPublicFriendsUserPosts(userId);
            else posts = _postRepository.GetPublicUserPosts(userId);
            return posts;
        }

        public List<Post> GetAllPosts(string userId)
        {
            List<Post> posts = new List<Post>();
            posts = _postRepository.GetAllUserPosts(userId);
            _friendRepository.GetListFriend(userId).ForEach(friend =>
            {
                posts = posts.Concat(_postRepository.GetPublicFriendsUserPosts(friend.Id)).ToList();
            });
            posts = posts.OrderByDescending(x => x.CreatedDate).ToList();
            return posts;
        }

        public void EditPost(Post post)
        {
            _postRepository.Edit(post);
        }

        public void DeletePost(string postId)
        {
            _postRepository.Delete(postId);
        }
    }
}
