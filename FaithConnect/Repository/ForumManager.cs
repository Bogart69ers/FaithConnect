using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FaithConnect.Utils;

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
        public ErrorCode AddForum(Forum fo, ref string errorMsg)
        {
            try
            {
                fo.forumId = Utilities.gUid;
                fo.date_created = DateTime.Now;
                fo.status = 0;
                return _forumRepository.Create(fo, out errorMsg);

            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return ErrorCode.Error;
            }
        }
        public List<Forum> GetForumByGroup(int groupId)
        {
            return _forumRepository.GetAll()
                                  .Where(m => m.groupId == groupId && m.status == 0).OrderByDescending(p => p.date_created)
                                  .ToList(); // Get all group memberships with status = 1
        }


    }
}