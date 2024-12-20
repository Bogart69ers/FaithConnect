using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FaithConnect.Utils;

namespace FaithConnect.Repository
{
    public class PostManager
    {
        private readonly BaseRepository<Post> _postRepository;
        private readonly GroupManager _groupManager;
        private readonly AccountManager _userManager;

        public PostManager()
        {
            _postRepository = new BaseRepository<Post>();
            _groupManager = new GroupManager();
            _userManager = new AccountManager();
        }

        public IEnumerable<Post> GetAllPosts()
        {
            var posts = _postRepository.GetAll().OrderByDescending(p => p.date_created).ToList();

            foreach (var post in posts)
            {
                // Fetch group name and user name based on foreign keys
                post.Groups.groupName = _groupManager.GetGroupById(post.groupId ?? 0)?.groupName ?? "Unknown Group";
                post.UserAccount.username = _userManager.GetUserNameById(post.createdBy); // Assuming 'createdBy' is the userId
            }

            return posts;
        }

        public IEnumerable<Post> GetPostsByGroupId(int groupId)
        {
            var posts = _postRepository._table.Where(p => p.groupId == groupId).OrderByDescending(p => p.date_created).ToList();

            foreach (var post in posts)
            {
                // Fetch group name and user name for each post
                post.Groups.groupName = _groupManager.GetGroupById(post.groupId ?? 0)?.groupName ?? "Unknown Group";
                post.UserAccount.username = _userManager.GetUserNameById(post.createdBy);
            }

            return posts;
        }

        public ErrorCode AddPost(Post post, ref string errorMsg)
        {
            try
            {
                post.postId = Utilities.gUid;
                post.date_created = DateTime.Now;
                post.status = 0;
                return _postRepository.Create(post, out errorMsg);

            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return ErrorCode.Error;
            }
        }
        public List<Post> GetPostByGroup(int groupId)
        {
            var posts = _postRepository.GetAll()
                                  .Where(m => m.groupId == groupId && m.status == 0)
                                  .OrderByDescending(p => p.date_created)
                                  .ToList();

            foreach (var post in posts)
            {
                post.Groups.groupName = _groupManager.GetGroupById(post.groupId ?? 0)?.groupName ?? "Unknown Group";
                post.UserAccount.username = _userManager.GetUserNameById(post.createdBy);
            }

            return posts;
        }
        public Post GetPostById(int postId)
        {
            var post = _postRepository.GetAll().FirstOrDefault(p => p.id == postId);

            if (post != null)
            {
                // Fetch group name and user name for the post
                post.Groups.groupName = _groupManager.GetGroupById(post.groupId ?? 0)?.groupName ?? "Unknown Group";
                post.UserAccount.username = _userManager.GetUserNameById(post.createdBy);
            }

            return post;
        }
    }
} 