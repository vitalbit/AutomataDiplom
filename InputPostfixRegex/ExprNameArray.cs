using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputPostfixRegex
{
    public struct ExprNameArray
    {
        public string shortName;
        public string name;
        public string[] arrPolish;
        public ExprNameArray(string shortName, string name, params  string[] arrPolish)
        {
            this.shortName = shortName;
            this.name = name;
            this.arrPolish = arrPolish;

        }
        //public ExprNameArray(string name, string[] arrPolish)
        //{
        //    this.name = name;
        //    this.arrPolish = arrPolish;

        //}

    }
}
