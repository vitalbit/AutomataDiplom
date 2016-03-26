using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeSystems
{
    public interface IGradeSystem
    {
        double Grade(TestResultsModel result, TestResultsModel answer);
    }
}
