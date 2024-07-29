using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            return _postRepository._table.Where(p => p.groupId == groupId).ToList();
        }

        public void AddPost(Post post, ref string errorMsg)
        {
            _postRepository.Create(post, out errorMsg);
        }
    }
}