using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaithConnect.Repository
{
    public class ForumManager
    {
        private readonly BaseRepository<Forum> _forumRepository;

        public ForumManager()
        {
            _forumRepository = new BaseRepository<Forum>();
        }

        public IEnumerable<Forum> GetForumsByGroupId(int groupId)
        {
            return _forumRepository._table.Where(f => f.groupId == groupId).ToList();
        }

        public void AddForum(Forum forum, ref string errorMsg)
        {
            _forumRepository.Create(forum, out errorMsg);
        }
    }
}