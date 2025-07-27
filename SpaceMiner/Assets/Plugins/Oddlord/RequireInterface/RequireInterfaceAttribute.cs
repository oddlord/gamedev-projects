using UnityEngine;

namespace Oddlord.RequireInterface
{
    public class RequireInterfaceAttribute : PropertyAttribute
    {
        public System.Type requiredType { get; private set; }

        public RequireInterfaceAttribute(System.Type type)
        {
            requiredType = type;
        }
    }
}