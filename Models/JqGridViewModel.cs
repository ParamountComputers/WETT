using System.Collections.Generic;

namespace WETT.Models
{
    public class Rule
    {
        public string field { get; set; }
        public string op { get; set; }
        public string data { get; set; }
    }

    public class Filters
    {
        public string groupOp { get; set; }
        public List<Rule> rules { get; set; }
    }

    public class JqGridViewModel
    {
        public bool _search { get; set; }
        public long nd { get; set; }
        public int rows { get; set; }
        public int page { get; set; }
        public string sidx { get; set; }
        public string sord { get; set; }

        public Filters searchfilters
        {
            get
            {
                if (filters != null)
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<Filters>(filters);
                else
                    return null;
            }
        }
        public string filters { get; set; }
    }
}
