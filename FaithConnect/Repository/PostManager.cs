using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FaithConnect.Utils;
using System.Data.Entity;


namespace FaithConnect.Repository
{
    public class PostManager
    {
        private readonly BaseRepository<Post> _postRepository;
        private readonly GroupManager _groupManager;
        private readonly AccountManager _userManager;
        private readonly BaseRepository<PostMedia> _postMediaRepository;


        public PostManager()
        {
            _postRepository = new BaseRepository<Post>();
            _groupManager = new GroupManager();
            _userManager = new AccountManager();
            _postMediaRepository = new BaseRepository<PostMedia>();

        }

        public IQueryable<Post> GetPostByGroupQueryable(int groupId)
        {
            return _postRepository._table
                .Where(m => m.groupId == groupId && m.status == 0)
                .OrderByDescending(p => p.date_created)
                .Include(p => p.Groups)
                .Include(p => p.UserAccount)
                .Include(p => p.PostMedia);
        }

        public IEnumerable<Post> GetAllPosts()
        {
            return _postRepository._table
                .OrderByDescending(p => p.date_created)
                .Include(p => p.Groups)
                .Include(p => p.UserAccount)
                .Include(p => p.PostMedia)
                .ToList();
        }


        public IEnumerable<Post> GetPostsByGroupId(int groupId)
        {
            return _postRepository._table
                .Where(p => p.groupId == groupId)
                .OrderByDescending(p => p.date_created)
                .Include(p => p.Groups)
                .Include(p => p.UserAccount)
                .Include(p => p.PostMedia)
                .ToList();
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

        public ErrorCode DeletePost(int id, ref string errMsg)
        {
            try
            {
                var post = GetPostById(id);
                if (post == null)
                {
                    errMsg = "Post not found";
                    return ErrorCode.Error;
                }
                return _postRepository.Delete(id, out errMsg);
            }
            catch (Exception ex)
            {
                errMsg = $"An error occurred: {ex.Message}";
                return ErrorCode.Error;
            }
        }

        public List<Post> GetPostByGroup(int groupId)
        {
            return _postRepository._table
                .Where(m => m.groupId == groupId && m.status == 0)
                .OrderByDescending(p => p.date_created)
                .Include(p => p.Groups)
                .Include(p => p.UserAccount)
                .Include(p => p.PostMedia)
                .ToList();
        }

        public Post GetPostById(int postId)
        {
            return _postRepository._table
                .Include(p => p.Groups)
                .Include(p => p.UserAccount)
                .Include(p => p.PostMedia) 
                .FirstOrDefault(p => p.id == postId);
        }

    }
} 