using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Model.DbModel
{
    public partial class T_MOperation : IEquatable<T_MOperation>
    {

        public bool Equals(T_MOperation other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            return other.OperationId.Value.Equals(this.OperationId.Value);
        }

        public override int GetHashCode()
        {
            //Get hash code for the Name field if it is not null.
            int name = OperationName == null ? 0 : OperationName.GetHashCode();

            //Get hash code for the Code field.
            int code = OperationId.GetHashCode();

            //Calculate the hash code for the product.
            return name ^ code;
        }
    }
}
