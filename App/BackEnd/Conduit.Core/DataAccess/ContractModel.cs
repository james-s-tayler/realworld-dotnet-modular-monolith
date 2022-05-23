using TracerAttributes;

namespace Conduit.Core.DataAccess
{
    public class ContractModel
    {
        [NoTrace]
        public override string ToString()
        {
            return base.ToString();
        }
    }
}