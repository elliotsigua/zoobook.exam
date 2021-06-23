using System.Reflection;

namespace Zoobook.Core
{
    public interface IValidator
    {
        ValidationResult ValidateProperty<TEntity>(TEntity targetEntity, PropertyInfo propertyInfo);
    }
}
