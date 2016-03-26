using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeSystems
{
    public class TableGradeSystem : IGradeSystem
    {
        public double Grade(TestResultsModel result, TestResultsModel answer)
        {
            int same = 0;
            for (int i = 0; i!=Math.Min(result.Rows, answer.Rows); i++)
            {
                for (int j = 0; j!=Math.Min(result.Cols, answer.Cols); j++)
                {
                    if (result.TableArray[i][j] == answer.TableArray[i][j])
                        ++same;
                }
            }
            return (double)same / (answer.Cols * answer.Rows) * 10;
        }
    }
}
