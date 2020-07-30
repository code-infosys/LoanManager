using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanManager.Web
{

    public class Dunut
    {
        public int value { get; set; }
        public string color { get; set; }
        public string highlight { get; set; }
        public string label { get; set; }
    }


    public class Datasets
    {
        public string label { get; set; }
        public string fillColor { get; set; } = "#d2d6de";
        public string strokeColor { get; set; } = "#d2d6de";
        public string pointColor { get; set; } = "#b960dc";
        public string pointStrokeColor { get; set; } = "#c1c7d1";
        public string pointHighlightFill { get; set; } = "#ffffff";
        public string pointHighlightStroke { get; set; } = "#dcdcdc";
        public int[] data { get; set; }
    }
    public class Chart
    {
        public string[] labels { get; set; }
        public List<Datasets> datasets { get; set; }
    }
}

