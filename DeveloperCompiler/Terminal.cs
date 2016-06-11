using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AST
{
    class Terminal
    {
        public int iTA;
        public string tokenLine;

        public Terminal(int iTA,string token_Line)
        {
            this.iTA = iTA;
            this.tokenLine = token_Line;
        }
    }
}
