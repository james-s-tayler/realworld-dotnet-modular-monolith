using System.Reflection;

namespace Conduit.Identity.Domain
{
    public class IdentityDomain
    {
        public static readonly Assembly Assembly = typeof(IdentityDomain).Assembly;
        public static readonly string AssemblyName = Assembly.GetName().Name;
    }
}