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

        public PostManager()
        {
            _postRepository = new BaseRepository<Post>();
        }

        public IEnumerable<Post> GetPostsByGroupId(int groupId)
        {
            return _postRepository._table.Where(p => p.groupId == groupId).OrderByDescending(p => p.date_created).ToList();
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
            return _postRepository.GetAll()
                                  .Where(m => m.groupId == groupId && m.status == 0).OrderByDescending(p => p.date_created)
                                  .ToList(); // Get all group memberships with status = 1
        }
    }
}