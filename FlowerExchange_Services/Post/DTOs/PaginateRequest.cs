﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Post.DTOs
{
    public class PaginateRequest
    {
        [DefaultValue(1)]
        public int CurrentPage { get; set; }
        [DefaultValue(10)]
        public int PageSize { get; set; } = 10;
        public int PageCount { get; set; }
        public int TotalEntity {  get; set; }
    }
}
