using System.Reflection;

namespace OpenDnD.Utilities.DI
{
    public class DIPropertyInformation
    {
        //Base type of current object
        public Type PropertyContainerType { get; set; }
        public DIInsertStrategy Strategy { get; set; }
        //Way to access target object from root object
        public Func<DIPropertyInformation, object?, object?> PropertyContainerAccessorImpl { get; set; }
        //setter for target object
        public MethodInfo PropertySetter { get; set; }
        //Type of target object
        public Type PropertyType { get; set; }
        public string PropertyName { get; }

        public DIPropertyInformation(Type baseType, MethodInfo setter, Type propertyType, string propertyName, DIInsertStrategy serviceType, Func<DIPropertyInformation, object?, object?> accessor)
        {
            Strategy = serviceType;
            PropertySetter = setter;
            PropertyType = propertyType;
            PropertyName = propertyName;
            PropertyContainerAccessorImpl = accessor;
            PropertyContainerType = baseType;
        }

        public DIPropertyInformation CloneWithAccessor(Func<DIPropertyInformation, object?, object?> accessor)
        {
            return new DIPropertyInformation(PropertyContainerType, PropertySetter, PropertyType, PropertyName, Strategy, (dis, obj) => GetPropertyContainer(accessor(dis, obj)));
        }
        public object? GetPropertyContainer(object? obj) => PropertyContainerAccessorImpl(this, obj);
    }

}
