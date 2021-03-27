using TestClasses;
using ThaGet.Cqrs.Include;

namespace IncludeExpressionSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var include = new IncludeExpression<TestEntity, int>();
            var list = include.IncludeList;
        }
    }
}
