using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BM.Model
{
    public static class RegAd
    {
        public static readonly Regex Reg_IdentityCard = new Regex(ConstAd.reg_identitycard,
           RegexOptions.Compiled |
           RegexOptions.IgnoreCase |
           RegexOptions.ExplicitCapture);
    }
}
