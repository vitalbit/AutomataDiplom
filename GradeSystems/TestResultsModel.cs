using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeSystems
{
    public class TestResultsModel
    {
        public int Id { get; set; }
        public string[][] TableArray { get; set; }
        public int Cols { get; set; }
        public int Rows { get; set; }
    }
}
