﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaithConnect.Utils;

namespace FaithConnect.Repository
{
    interface IBaseRepository<T>
    {
        T Get(object id);
        List<T> GetAll();
        ErrorCode Create(T t, out String errorMsg);
        ErrorCode Update(object id, T t, out String errorMsg);
        ErrorCode Delete(object id, out String errorMsg);
    }
}
