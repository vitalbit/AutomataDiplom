﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface.DTO
{
    public class DalTestType : IDalEntity
    {
        public int Id { get; set; }
        public string ModuleName { get; set; }
    }
}
