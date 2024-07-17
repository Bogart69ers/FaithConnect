using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FaithConnect.Utils;
using System.Data.Entity;

namespace FaithConnect.Repository
{
    public class BaseRepository<T> : IBaseRepository<T>
        where T : class
    {

        public DbContext _db;

        public DbSet<T> _table;

        public BaseRepository()
        {
            _db = new FaithConnectEntities1();
            _table = _db.Set<T>();
        }
        public ErrorCode Create(T t, out string errorMsg)
        {
            try
            {
                _table.Add(t); // Adding the entity to the DbSet.
                _db.SaveChanges(); // Saving changes to the database.
                errorMsg = "Success"; // Setting success message.

                return ErrorCode.Success; // Returning success code.
            }
            catch (Exception ex)
            {
                // Handling exceptions and setting error message accordingly.
                errorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return ErrorCode.Error; // Returning error code.
            }
        }
        public ErrorCode Update(object id, T t, out string errorMsg)
        {
            try
            {
                var old_obj = Get(id);
                if (old_obj == null)
                {
                    errorMsg = "Entity not found";
                    return ErrorCode.Error;
                }

                _db.Entry(old_obj).CurrentValues.SetValues(t);
                _db.SaveChanges();
                errorMsg = "Updated";
                return ErrorCode.Success;
            }
            catch (Exception ex)
            {
                errorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return ErrorCode.Error;
            }
        }


        public ErrorCode Delete(object id, out string errorMsg)
        {
            try
            {
                var obj = Get(id); // Getting the entity by its id.
                _table.Remove(obj); // Removing the entity from the DbSet.
                _db.SaveChanges(); // Saving changes to the database.

                errorMsg = "Deleted"; // Setting success message.

                return ErrorCode.Success; // Returning success code.
            }
            catch (Exception ex)
            {
                // Handling exceptions and setting error message accordingly.
                errorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return ErrorCode.Error;
            }
        }

        public T Get(object id)
        {
            return _table.Find(id); // Find the entity id
        }

        public List<T> GetAll()
        {
            return _table.ToList();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}