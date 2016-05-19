using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Entities
{
    public class TestTypeEntity
    {
        public int Id { get; set; }
        public string ModuleName { get; set; }
        public string CssFileName { get; set; }
        public string JsFileName { get; set; }
        public string DllFileName { get; set; }
    }
}
