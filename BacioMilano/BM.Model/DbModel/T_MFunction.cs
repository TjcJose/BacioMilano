using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Model.DbModel
{
    public partial class T_MFunction : IEquatable<T_MFunction>
    {
        public bool Equals(T_MFunction other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            return other.FunctionId.Value.Equals(this.FunctionId.Value);
        }

        public override int GetHashCode()
        {
            //Get hash code for the Name field if it is not null.
            int name = FunctionName == null ? 0 : FunctionName.GetHashCode();

            //Get hash code for the Code field.
            int code = FunctionId.GetHashCode();

            //Calculate the hash code for the product.
            return name ^ code;
        }
    }
}
