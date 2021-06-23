using System;
using System.Reflection;
using Zoobook.Core.Properties;
using Zoobook.Shared;

namespace Zoobook.Core
{
    public class RequiredValidator : IValidator
    {
        public ValidationResult ValidateProperty<TEntity>(TEntity targetEntity, PropertyInfo propertyInfo)
        {
            var propertyValue = propertyInfo.GetValue(targetEntity);
            if (propertyValue == null)
            {
                return new ValidationResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = string.Format(Resources.PropertyRequiredError, propertyInfo.Name)
                };
            }

            var propertyType = propertyValue.GetType();
            if (propertyType.IsEnum)
            {
                var isFound = Enum.TryParse(propertyValue.GetType(), Constants.EnumNone, out var noneEnum);
                if (isFound)
                {
                    if (!Equals(propertyValue, noneEnum)) return null;
                }
                else
                    return null;
            }
            else
            {
                if (!Equals(propertyValue, null) && !Equals(propertyValue, Guid.Empty) && !Equals(propertyValue, string.Empty)) return null;
            }

            return new ValidationResult()
            {
                IsSuccessful = false,
                ErrorMessage = string.Format(Resources.PropertyRequiredError, propertyInfo.Name)
            };
        }
    }
}
