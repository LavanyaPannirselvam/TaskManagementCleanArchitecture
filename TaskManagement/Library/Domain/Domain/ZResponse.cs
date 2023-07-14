﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementLibrary.Domain
{
    public class ZResponse<R> : IResponse
    {
        public string Response { get; set ; }
        public R Data;
    }
}
