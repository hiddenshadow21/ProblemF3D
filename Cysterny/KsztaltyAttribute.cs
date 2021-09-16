using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cysterny
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class KsztaltyAttribute : Attribute
    {
        public Type Type { get; private set; }
        public string Nazwa { get; private set; }

        public KsztaltyAttribute(Type type, string nazwa)
        {
            Type = type;
            Nazwa = nazwa;
        }

        public static List<KsztaltyAttribute> DostepneKsztalty()
        {
            Type[] typy = Assembly.GetExecutingAssembly().GetTypes()
              .Where(t => String.Equals(t.Namespace, "Cysterny", StringComparison.Ordinal)).ToArray();
            int i = 0;
            List<KsztaltyAttribute> ksztalty = new List<KsztaltyAttribute>(typy.Length);
            foreach (var typ in typy)
            {
                KsztaltyAttribute MyAttribute = (KsztaltyAttribute)Attribute.GetCustomAttribute(typ, typeof(KsztaltyAttribute));
                if (MyAttribute != null)
                {
                    ksztalty.Add(MyAttribute);
                    i++;
                }
            }
            return ksztalty;
        }
    }
}
