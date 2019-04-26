using POCO.Mapper.Common;
using System.Collections.Generic;
using System.Linq;

namespace POCO.Mapper.Extension
{
    /// <summary>Inherit from this class to use extensions</summary>
    public abstract class ModelMap { }

    /// <summary>Model Mapper Extension Methods</summary>
    public static class POCOMapperExtensions
    {
        private readonly static ModelMapperCommon Common = new ModelMapperCommon();

        /// <summary>Map values from current POCO to Target Type</summary>
        /// <typeparam name="T">Target Type</typeparam>
        public static T MapTo<T>(this ModelMap me) => (T)Common.Map(me, typeof(T));

        /// <summary>Map values from current POCO to Target Type</summary>
        /// <typeparam name="T">Target Type</typeparam>
        public static IList<T> MapToList<T>(this IEnumerable<ModelMap> me) => me.Select(obj => (T)Common.Map(obj, typeof(T))).ToList();

        /// <summary>Map values from current POCO to Target Type</summary>
        /// <typeparam name="T">Target Type</typeparam>
        public static IList<T> MapToList<T>(this ModelMap[] me) => me.Select(obj => (T)Common.Map(obj, typeof(T))).ToList();

        /// <summary>Map values from current POCO to Target Type</summary>
        /// <typeparam name="T">Target Type</typeparam>
        public static T[] MapToArray<T>(this IEnumerable<ModelMap> me) => me.Select(obj => (T)Common.Map(obj, typeof(T))).ToArray();

        /// <summary>Map values from current POCO to Target Type</summary>
        /// <typeparam name="T">Target Type</typeparam>
        public static T[] MapToArray<T>(this ModelMap[] me) => me.Select(obj => (T)Common.Map(obj, typeof(T))).ToArray();
    }
}
