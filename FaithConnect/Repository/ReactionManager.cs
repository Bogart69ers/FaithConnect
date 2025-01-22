using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FaithConnect.Utils;

namespace FaithConnect.Repository
{
    public class ReactionManager
    {
        private readonly BaseRepository<Reaction> _reactionRepository;

        public ReactionManager()
        {
            _reactionRepository = new BaseRepository<Reaction>();
        }
        public bool UserHasLiked(int postId, int userId)
        {
            return _reactionRepository._table
                .Any(r => r.postId == postId && r.userId == userId && r.hasLiked == true);
        }

        public void AddOrUpdateReaction(int postId, int userId, ref string errorMsg)
        {
            var existingReaction = _reactionRepository._table
                .FirstOrDefault(r => r.postId == postId && r.userId == userId);

            if (existingReaction == null)
            {
                // Add a new reaction
                var reaction = new Reaction
                {
                    postId = postId,
                    userId = userId,
                    hasLiked = true,
                    dateCreated = DateTime.Now
                };
                _reactionRepository.Create(reaction, out errorMsg);
            }
            else
            {
                // Update the existing reaction
                existingReaction.hasLiked = true;
                _reactionRepository.Update(existingReaction.id, existingReaction, out errorMsg);
            }
        }
        public Reaction GetReaction(int postId, int userId)
        {
            return _reactionRepository._table.FirstOrDefault(r => r.postId == postId && r.userId == userId);
        }

        public int GetReactionCount(int postId)
        {
            return _reactionRepository._table.Count(r => r.postId == postId && r.hasLiked == true);
        }

        public ErrorCode AddReaction(Reaction reaction, ref string errMsg)
        {
            return _reactionRepository.Create(reaction, out errMsg);
        }

        public ErrorCode RemoveReaction(int postId, int userId, ref string errMsg)
        {
            var reaction = GetReaction(postId, userId);
            if (reaction != null)
            {
                return _reactionRepository.Delete(reaction.id, out errMsg);
            }

            errMsg = "Reaction not found";
            return ErrorCode.NotFound;
        }
    }

}