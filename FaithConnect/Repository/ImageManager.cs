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


        public ImageManager()
        {
            _img = new BaseRepository<Image>();
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

        public ErrorCode DeleteImgByUserId(int? id, ref string err)
        {
            foreach (var i in _img._table.Where(m=>m.id == id).ToList())
            {
                DeleteImg(i.id, ref err);
            }
            return ErrorCode.Success;
        }
        public ErrorCode DeleteImgByProductId(int? id, ref String err)
        {
            foreach (var i in _img._table.Where(m => m.userId == id).ToList())
            {
                DeleteImg(i.id, ref err);
            }
            return ErrorCode.Success;
        }   
    }
}