﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEndpoints
{
    public interface ITestEndpoints
    {
        string Grade(string result);
        string Grade(string result, string answer);
    }
}
