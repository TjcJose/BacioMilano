using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Util
{
    public class FilterLink : Filter
    {
        protected override bool IsMatch(string str)
        {
            return true;
        }

        public FilterLink(params Filter[] filters)
        {
            if (filters.Length > 0)
            {
                this.filter = filters[0];
            }
            for (var i = 1; i < filters.Length; i++)
            {
                filters[i - 1].filter = filters[i];
            }
        }
    }
}
