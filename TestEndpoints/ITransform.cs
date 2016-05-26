using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEndpoints
{
    public interface ITransform
    {
        string TransformFileToClient(string file);
        string TransformFileFromClient(string file);
    }
}
