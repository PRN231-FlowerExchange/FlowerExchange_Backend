using System.Linq.Expressions;
using System.Reflection;

namespace CrossCuttingConcerns.Utils
{
    public static class QueryableExtensions
    {
        // Extension method for dynamic OrderBy
        public static IOrderedQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string propertyName, bool descending)
        {
            var entityType = typeof(T);
            var parameter = Expression.Parameter(entityType, "p");

            // Xử lý nếu propertyName bao gồm các liên kết với thực thể khác (ví dụ: Flower.Price)
            Expression propertyAccess;
            if (propertyName.Contains('.'))
            {
                var properties = propertyName.Split('.');
                propertyAccess = parameter;
                foreach (var prop in properties)
                {
                    propertyAccess = Expression.PropertyOrField(propertyAccess, prop);
                }
            }
            else
            {
                propertyAccess = Expression.PropertyOrField(parameter, propertyName);
            }

            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            var orderByMethod = descending ? "OrderByDescending" : "OrderBy";
            var resultExp = Expression.Call(typeof(Queryable), orderByMethod, new Type[] { entityType, propertyAccess.Type },
                                            source.Expression, Expression.Quote(orderByExp));
            return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(resultExp);
        }


        // Extension method for dynamic ThenBy
        public static IOrderedQueryable<T> ThenByDynamic<T>(this IOrderedQueryable<T> source, string propertyName, bool descending)
        {
            var entityType = typeof(T);
            var parameter = Expression.Parameter(entityType, "p");

            Expression propertyAccess;
            if (propertyName.Contains('.'))
            {
                var properties = propertyName.Split('.');
                propertyAccess = parameter;
                foreach (var prop in properties)
                {
                    propertyAccess = Expression.PropertyOrField(propertyAccess, prop);
                }
            }
            else
            {
                propertyAccess = Expression.PropertyOrField(parameter, propertyName);
            }

            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            var thenByMethod = descending ? "ThenByDescending" : "ThenBy";
            var resultExp = Expression.Call(typeof(Queryable), thenByMethod, new Type[] { entityType, propertyAccess.Type },
                                            source.Expression, Expression.Quote(orderByExp));
            return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(resultExp);
        }

    }
}
