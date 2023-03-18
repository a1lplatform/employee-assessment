using System.Linq.Expressions;
using System.Reflection;

namespace A1.SAS.Api.Query
{
    public static class FilterHelper
    {
        private static char QuerySeparatorChar = '&';
        private const string Query_Operator_Equal = "[eq]";
        private const string Query_Operator_Not_Equal = "[neq]";
        private const string Query_Operator_Greater_Than = "[gt]";
        private const string Query_Operator_Lower_Than = "[lt]";
        private const string Query_Operator_Nlike = "[nlike]";
        private const string Query_Operator_Like = "[like]";
        private const string Query_Operator_ASC = "+";
        private const string Query_Operator_DESC = "-";
        private const string Query_Operator_Greater_Than_Eq = "[gte]";
        private const string Query_Operator_Lower_Than_Eq = "[lte]";
        private const string Query_Operator_IN = "[in]";
        private const string Query_Operator_NIN = "[nin]";

        public static Dictionary<string, string> QUERY_OPERATORS = new Dictionary<string, string>
        {
            { Query_Operator_Lower_Than , "<"},
            { Query_Operator_Like , "LIKE"},
            { Query_Operator_Nlike , "NLIKE"},
            { Query_Operator_IN , "IN"},
            { Query_Operator_NIN , "NIN"},
            { Query_Operator_Equal, "="},
            { Query_Operator_Not_Equal,  "!=" },
            { Query_Operator_Greater_Than_Eq , ">="},
            { Query_Operator_Lower_Than_Eq , "<="},
            { Query_Operator_Greater_Than , ">"}
        };

        /// <summary>
        /// Split filter string into subcomponents
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static string[] SplitPropertyFilters(string filter)
        {
            if (string.IsNullOrEmpty(filter)) { return Array.Empty<string>(); }

            var atrfilter = filter.Split('=');
            for (int i = 1; i < atrfilter.Length - 1; i++)
            {
                var indexOf = atrfilter[i].LastIndexOf(QuerySeparatorChar);
                if (indexOf != -1)
                {
                    atrfilter[i] = atrfilter[i].Insert(indexOf, "+");
                }
            }

            var newQueryFilters = string.Join("=", atrfilter);

            return newQueryFilters.Split($"+&");
        }

        /// <summary>
        /// Add more conditions for where queries
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filters"></param>
        /// <param name="queryableEntity"></param>
        /// <returns></returns>
        public static IQueryable<T> SetQueryDataFromDb<T>(string[] filters, IQueryable<T> queryableEntity)
        {
            if (filters == null || filters.Length == 0)
            {
                return queryableEntity;
            }

            foreach (var filterName in filters)
            {
                string propertyName = filterName.Split("=")[0].Trim();
                string operation = Query_Operator_Equal;// Default [eq]

                foreach (var operatorItem in QUERY_OPERATORS)
                {
                    if (propertyName.Contains(operatorItem.Key))
                    {
                        operation = operatorItem.Key;
                        propertyName = propertyName.Replace(operation, ""); // Remove operation [eq] after get operation
                        break;
                    }
                }

                // name[eq]=value
                object value = filterName.Split("=")[1].Trim();
                Expression<Func<T, bool>> resultConditions = DynamicWhere<T>(propertyName, value, operation);
                queryableEntity = queryableEntity.Where(resultConditions);
            }

            return queryableEntity;
        }

        public static Expression<Func<T, bool>> DynamicWhere<T>(string propertyName, object value, string operation)
        {
            var typeEntity = Expression.Parameter(typeof(T), "t");
            PropertyInfo childproperty = null;

            // Case filter normal fields
            GetPropertyInfoFilter<T>(propertyName, out PropertyInfo property, out PropertyInfo[] propertyInfos);

            // Incase entity has relationship with other table, should get by Id
            if (property == null && propertyName.Contains("Id"))
            {
                propertyName = propertyName.Replace("Id", "");
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    if (propertyInfo.Name.Equals(propertyName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        property = propertyInfo;
                        childproperty = property.PropertyType.GetProperty("Id");
                        break;
                    }
                }
            }

            var left = Expression.Property(typeEntity, property);

            // Incase entity has relationship with other table
            if (childproperty != null)
            {
                left = Expression.Property(left, childproperty);
            }

            dynamic result = ConvertValueInput(value, left);
            var right = Expression.Constant(result);

            // Get operation.
            dynamic condition = BuildConditions(operation, left, right);

            return Expression.Lambda<Func<T, bool>>(condition, typeEntity);
        }

        private static dynamic BuildConditions(string operation, MemberExpression left, dynamic right)
        {
            dynamic condition = operation switch
            {
                Query_Operator_Equal => Expression.Equal(left, right),
                Query_Operator_Not_Equal => Expression.NotEqual(left, right),
                Query_Operator_Greater_Than_Eq => Expression.GreaterThanOrEqual(left, right),
                Query_Operator_Lower_Than_Eq => Expression.LessThanOrEqual(left, right),
                Query_Operator_Greater_Than => Expression.GreaterThan(left, right),
                Query_Operator_Lower_Than => Expression.LessThan(left, right),
                Query_Operator_Like => Expression.Call(left, typeof(string).GetMethods().First(m => m.Name == "Contains" && m.GetParameters().Length == 1), right),

                _ => Expression.Equal(left, right),
            };

            return condition;
        }

        private static dynamic ConvertValueInput(object value, MemberExpression left)
        {
            dynamic result = left.Type.FullName switch
            {
                "System.Boolean" => Convert.ToBoolean(value),
                "System.Object" => value,
                "System.Guid" => new Guid(value.ToString()),
                "System.String" => value.ToString(),
                "System.Decimal" => decimal.Parse(value.ToString()),
                "System.Int64" => Convert.ToInt64(value),
                "System.Int16" => Convert.ToInt16(value),
                "System.Byte" => Convert.ToByte(value),
                "System.DateTime" => (dynamic)Convert.ToDateTime(value),

                _ => value,// Default is Object
            };

            return result;
        }

        private static void GetPropertyInfoFilter<T>(string propertyName, out PropertyInfo property, out PropertyInfo[] propertyInfos)
        {
            property = null;
            propertyInfos = typeof(T).GetProperties();

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (propertyInfo.Name.Equals(propertyName, StringComparison.CurrentCultureIgnoreCase))
                {
                    property = propertyInfo;
                    break;
                }
            }
        }
    }
}
