using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Util
{
    public class SelItem<K, T>
    {
        public K Value { get; set; }
        public T Text { get; set; }

        public SelItem(K k, T t)
        {
            this.Value = k;
            this.Text = t;
        }

        public const string SourceValue = "Value";
        public const string SourceText = "Text";
    }
}
