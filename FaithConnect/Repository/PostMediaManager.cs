using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FaithConnect.Models;
using FaithConnect.Utils;

namespace FaithConnect.Repository
{
    public class PostMediaManager
    {
        private readonly BaseRepository<PostMedia> _postMediaRepo;

        public PostMediaManager()
        {
            _postMediaRepo = new BaseRepository<PostMedia>();
        }

        public List<PostMedia> ListMediaByPostId(int? postId)
        {
            if (postId == null)
                return new List<PostMedia>();

            return _postMediaRepo._table.Where(m => m.postId == postId).ToList();
        }

        
        public ErrorCode CreatePostMedia(PostMedia media, ref string err)
        {
            return _postMediaRepo.Create(media, out err);
        }

       
        public ErrorCode UpdatePostMedia(PostMedia media, ref string err)
        {
            return _postMediaRepo.Update(media.id, media, out err);
        }
        
        public ErrorCode DeletePostMedia(int? id, ref string err)
        {
            return _postMediaRepo.Delete(id, out err);
        }
    
    }
}