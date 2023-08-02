using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace JescoDev.MovementGraph.Editor.Editor.Utility {
    public static class ReflectionUtility {
        
        public static IEnumerable<Type> GetAllInheritors<T>() where T : class {
            List<Type> types = TypeCache.GetTypesDerivedFrom(typeof(T))
                .Where(p => p.IsPublic || p.IsNestedPublic)
                .ToList();
            return types;
        }
        
        public static IEnumerable<Type> WhereInstantiable(this IEnumerable<Type> iter) {
            return iter.Where(p
                => !p.IsAbstract && !p.IsGenericType && !typeof(UnityEngine.Object).IsAssignableFrom(p));
        }
        
        public static IEnumerable<Type> WhereSerializable(this IEnumerable<Type> iter) {
            return iter.Where(p => Attribute.IsDefined(p,typeof(SerializableAttribute)));
        }
        
        public static IEnumerable<T> MoveToBack<T>(this IEnumerable<T> iter, T compare)
            => iter.OrderBy(x => x.Equals(compare));
        
        public static bool HasAttribute<T>(this Type type) where T : Attribute {
            return type.GetCustomAttributes<T>().Any();
        }
        
        public static T GetAttribute<T>(this Type type) where T : Attribute {
            return type.GetCustomAttributes<T>().FirstOrDefault();
        }
        
    }
}