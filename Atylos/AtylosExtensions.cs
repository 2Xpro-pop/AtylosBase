using Atylos.ScopableServiceProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Text;

namespace Atylos
{
    public static class AtylosExtensions
    {
        public static T GetService<T>(this IServiceProvider serviceProvider)
        {
            return (T)serviceProvider.GetService(TypeOf<T>.Type);
        }

        public static void AddBattle<T, U>(this ScopableServiceProviderBuilder builder)
        {
            builder.AddScoped<T, U>(AtylosScopes.BattleScope);
        }

        public static void AddBattle<T>(this ScopableServiceProviderBuilder builder) => AddBattle<T, T>(builder);


        public static void AddSorted<T>(this List<T> @this, T item) where T : IComparable<T>
        {
            if (@this.Count == 0)
            {
                @this.Add(item);
                return;
            }
            if (@this[@this.Count - 1].CompareTo(item) <= 0)
            {
                @this.Add(item);
                return;
            }
            if (@this[0].CompareTo(item) >= 0)
            {
                @this.Insert(0, item);
                return;
            }
            int index = @this.BinarySearch(item);
            if (index < 0)
                index = ~index;
            @this.Insert(index, item);
        }
    }
}
