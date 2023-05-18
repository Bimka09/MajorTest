using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajorTest.Model
{
    internal class FilterData
    {
        public string AdressSender { get; set; }
        public string AdressReceiver { get; set; }
        public double? Weight { get; set; }
        public double? Length { get; set; }
        public double? Width { get; set; }
        public double? Height { get; set; }
    }
}
