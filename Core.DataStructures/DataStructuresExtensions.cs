using System.Collections.Generic;

namespace Core.DataStructures
{
    public static class DataStructuresExtensions
    {
        /// <summary>
        /// Convert all the <see cref="IService{T}"/> subscribers as a <see cref="List{T}"/>
        /// </summary>
        /// <typeparam name="T">The type of the items contained in the <see cref="IService{T}"/></typeparam>
        /// <param name="source">The source <see cref="IService{T}"/></param>
        /// <returns>The <see cref="List{T}"/> with all the subscribers</returns>
        public static List<IProperty> ToList<T>(this IService<T> source) where T : IProperty
            => source.GetAll().ToList();
    }
}