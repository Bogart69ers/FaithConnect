using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FaithConnect.Utils;

namespace FaithConnect.Repository
{
    public class RepostManager
    {
        private readonly BaseRepository<Repost> _repostRepository;

        public RepostManager()
        {
            _repostRepository = new BaseRepository<Repost>();
        }

        public ErrorCode AddRepost(Repost repost, ref string errorMsg)
        {
            try
            {
                _repostRepository.Create(repost, out errorMsg);
                return ErrorCode.Success;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return ErrorCode.Error;
            }
        }

        public List<Post> GetRepostedPostsByUser(int userId)
        {
            return _repostRepository._table
                .Where(r => r.userId == userId)
                .Select(r => r.Post) // Assuming there's a navigation property to the Post
                .ToList();
        }

        public bool UserHasReposted(int postId, int userId)
        {
            return _repostRepository._table.Any(r => r.postId == postId && r.userId == userId);
        }

        public int GetRepostCount(int postId)
        {
            return _repostRepository._table.Count(r => r.postId == postId);
        }
    }
}