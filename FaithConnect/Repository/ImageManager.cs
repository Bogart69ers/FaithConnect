using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FaithConnect.Utils;


namespace FaithConnect.Repository
{
    public class ImageManager
    {
        BaseRepository<Image> _img;
        BaseRepository<GroupImage> _grImg;


        public ImageManager()
        {
            _img = new BaseRepository<Image>();
            _grImg = new BaseRepository<GroupImage>();
        }

        public List<GroupImage> ListImgAttachByGroupId(int? id)
        {
            return _grImg._table.Where(m => m.groupId == id).ToList();
        }
        
        public ErrorCode CreateGroupImg(GroupImage img, ref String err)
        {
            return _grImg.Create(img, out err);
        }
        public ErrorCode UpdateGroupImg(GroupImage img, ref String err)
        {
            return _grImg.Update(img.id, img, out err);
        }
        public ErrorCode DeleteGroupImg(int? id, ref String err)
        {
            return _grImg.Delete(id, out err);
        }


        public List<Image> ListImgAttachByUserId(int? id)
        {
            return _img._table.Where(m => m.userId == id).ToList();
        }
        public ErrorCode CreateImg(Image img, ref String err)
        {
            return _img.Create(img, out err);
        }
        public ErrorCode UpdateImg(Image img, ref String err)
        {
            return _img.Update(img.id, img, out err);
        }
        public ErrorCode DeleteImg(int? id, ref String err)
        {
            return _img.Delete(id, out err);
        }
    }
}