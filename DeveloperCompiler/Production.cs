using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AST
{
    class Production
    {
        public int rule;
        public object[] alpha;

        public Production(int r, object[] alph)
        {
            rule = r;
            alpha = alph;
        }

    }
}
