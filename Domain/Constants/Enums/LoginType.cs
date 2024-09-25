﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constants.Enums
{
    public enum LoginType
    {
        [Display(Name = "Google")]
        Google,
        [Display(Name = "Basic")]
        Baisc
    }
}
