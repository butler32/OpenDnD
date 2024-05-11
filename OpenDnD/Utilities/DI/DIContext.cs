using Microsoft.Extensions.DependencyInjection;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static OpenDnD.Utilities.DI.DIContext;

namespace OpenDnD.Utilities.DI
{
    public class Service0
    {

    }
    public class Service1
    {
        [FromDI(DIInsertStrategy.Required)]
        public Service0 Service0 { get; set; }
    }
    
    public class Service2
    {
        [FromDI]
        public Service0 Service0 { get; set; }

        [DILook(DILookStrategy.NotRequired)]
        public Service1 Service1 { get; set; }
    }
    public class Service3
    {
        public Service0 Service0 { get; set; }
        [FromDI]
        public Service0 Service00 { get; set; }

        public Service2 Service2 { get; set; }
        [DILook(DILookStrategy.Required)]
        public Service2 Service22 { get; set; }
    }

    public static class AlfaZero
    {
        public static void AlfaMain()
        {
            var provider = new ServiceCollection()
                .AddSingleton<Service0>()
                .UseDI()
                .BuildServiceProvider();

            var s1 = provider.UseDI(new Service1());
            var s2 = provider.UseDI(new Service2());
            var s22 = provider.UseDI(new Service2() {  Service1 = new Service1 { } });
            var s3 = provider.UseDI(new Service3());
            int a = 0;
        }
    }

    

    public static class DIContextExt
    {
        public static T UseDI<T>(this IServiceProvider serviceProvider, T obj)
        {
            var context = serviceProvider.GetRequiredService<DIContext>();
            try
            {
                context.DoDI(serviceProvider, obj);
            }
            catch (DIException e)
            {
                //flush stack info for DIException
                throw e;
            }
            return obj;
        }
        public static IServiceCollection UseDI(this IServiceCollection serviceCollection) => serviceCollection.AddSingleton<DIContext>();
    }
    public class DIContext
    {
        
        Dictionary<Type, DITypeInformation> TypeToTypeContext = new Dictionary<Type, DITypeInformation>();
        object locker = new object();
        bool TypeExistInCache(Type type, [NotNullWhen(true)] out DITypeInformation dITypeContext) => TypeToTypeContext.TryGetValue(type, out dITypeContext);


        static bool IsNeedDILook(MemberInfo mi, [NotNullWhen(true)] out DILookStrategy? serviceType)
        {
            serviceType = null;
            var attr = mi.GetCustomAttribute<DILookAttribute>();
            if (attr != null)
            {
                serviceType = attr.Strategy;
                return true;
            }
            return false;
        }
        static bool IsNeedUseDI(MemberInfo mi, [NotNullWhen(true)] out DIInsertStrategy? serviceType)
        {
            serviceType = null;
            var attr = mi.GetCustomAttribute<FromDIAttribute>();
            if (attr != null)
            {
                serviceType = attr.Strategy;
                return true;
            }
            return false;
        }
        
        static bool CheckIfNotProcessedBefore(HashSet<PropertyInfo> propertyInfos, PropertyInfo propertyInfo)
        {
            if (!propertyInfos.Contains(propertyInfo))
            {
                propertyInfos.Add(propertyInfo);
                return true;
            }
            return false;
        }
        IEnumerable<DIPropertyInformation> GetProperties(HashSet<PropertyInfo> propertyInfos, Type? type)
        {
            if (type == null || type.Equals(typeof(object)))
                yield break;

            foreach (var item in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (CheckIfNotProcessedBefore(propertyInfos, item))
                {
                    if (IsNeedUseDI(item, out DIInsertStrategy? dIStrategy))
                    {
                        var setter = item.GetSetMethod(true)
                            ?? throw new DIException($"In {type} property {item} marked with FromDI({dIStrategy}), but dont have setter");
                        yield return new DIPropertyInformation(type, setter, item.PropertyType, item.Name, dIStrategy.Value, (_, x) => x);
                    }
                    else if (IsNeedDILook(item, out DILookStrategy? dILookStrategy))
                    {
                        var getter = item.GetGetMethod(true)
                            ?? throw new DIException($"In {type} property {item} marked with DILook({dILookStrategy}), but dont have getter");
                        var preparedContext = GetTypeInformation(item.PropertyType);

                        foreach (var setter in preparedContext.PropertiesToProcess)
                        {
                            var ne = setter.CloneWithAccessor((dis, x) =>
                            {
                                if (x == null)
                                    return null;

                                var proxy = getter.Invoke(x, null);
                                if (proxy is null && dILookStrategy == DILookStrategy.Required)
                                    throw new DIException($"In type {type} property {item.Name} marked with DILook({dILookStrategy}), but returns null value");
                                return proxy;
                            });
                            yield return ne;
                        }
                    }
                }
            }

            foreach (var item in GetProperties(propertyInfos, type.BaseType))
                yield return item;

            yield break;

        }
        IEnumerable<DIPropertyInformation> GetProperties(Type type) => GetProperties(new HashSet<PropertyInfo>(), type);
        DITypeInformation GetTypeInformation(Type type)
        {
            if (TypeExistInCache(type, out var context))
                return context;

            //Fix async with locker
            lock (locker)
            {
                if (TypeExistInCache(type, out context))
                    return context;

                var props = GetProperties(type).ToList();
                var typeContext = new DITypeInformation
                {
                    PropertiesToProcess = props
                };
                TypeToTypeContext.Add(type, typeContext);
                return typeContext;
            }
        }
        public void DoDI<T>(IServiceProvider serviceProvider, T obj)
        {
            var type = typeof(T);
            var typeInformation = GetTypeInformation(type);
            DoDIByTypeInformation(serviceProvider, obj, typeInformation);
        }
        void DoDIByTypeInformation(IServiceProvider serviceProvider, object? obj, DITypeInformation dITypeContext)
        {
            var array = new object?[1];
            foreach (var ptp in dITypeContext.PropertiesToProcess)
            {
                var realObj = ptp.GetPropertyContainer(obj);
                if (realObj != null)
                {
                    var service = serviceProvider.GetService(ptp.PropertyType);
                    if (ptp.Strategy == DIInsertStrategy.Required && service == null)
                        throw new DIException($"In {ptp.PropertyContainerType} property {ptp.PropertyName} marked as {DIInsertStrategy.Required}, but DI couldn't resolve {ptp.PropertyType}");
                    array[0] = service;
                    ptp.PropertySetter.Invoke(realObj, array);
                }

            }
        }
    }

}
