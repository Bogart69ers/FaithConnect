using System;
using System.Collections.Generic;
using System.Linq;
using FaithConnect.Utils;

namespace FaithConnect.Repository
{
    public class CommentManager
    {
        private readonly BaseRepository<PostComments> _commentRepo;
        private readonly AccountManager _userManager;
        private readonly BaseRepository<ForumComments> _forumCommentRepo;

        public CommentManager()
        {
            _commentRepo = new BaseRepository<PostComments>();
            _userManager = new AccountManager();
            _forumCommentRepo = new BaseRepository<ForumComments>();
        }

        // Create a new comment
        public ErrorCode AddComment(PostComments comment, ref string errorMsg)
        {
            try
            {
                comment.date_created = DateTime.Now; // Set date_created
                return _commentRepo.Create(comment, out errorMsg); // Save comment to the database
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return ErrorCode.Error;
            }
        }

        // Get all comments for a specific post
      
        // Delete a comment by its ID
        public ErrorCode DeleteComment(int commentId, ref string errorMsg)
        {
            try
            {
                var comment = _commentRepo.Get(commentId);
                if (comment != null)
                {
                    return _commentRepo.Delete(commentId, out errorMsg);
                }
                else
                {
                    errorMsg = "Comment not found.";
                    return ErrorCode.Error;
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return ErrorCode.Error;
            }
        }
        public ErrorCode AddForumComment(ForumComments comment, ref string errorMsg)
        {
            try
            {
                comment.date_created = DateTime.Now; // Set date_created
                return _forumCommentRepo.Create(comment, out errorMsg); // Save comment to the database
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return ErrorCode.Error;
            }
        }

        // Get all comments for a specific post

        // Delete a comment by its ID
        public ErrorCode DeleteForumComment(int commentId, ref string errorMsg)
        {
            try
            {
                var comment = _commentRepo.Get(commentId);
                if (comment != null)
                {
                    return _forumCommentRepo.Delete(commentId, out errorMsg);
                }
                else
                {
                    errorMsg = "Comment not found.";
                    return ErrorCode.Error;
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return ErrorCode.Error;
            }
        }
        // You can add more methods like updating comments if needed.
    }
}
