using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Zoobook.Shared;

namespace Zoobook.Core
{
    public class ValidationManager
    {
        private readonly IDictionary<Type, IValidator> ValidatorFactories;

        public ValidationManager()
        {
            ValidatorFactories = new Dictionary<Type, IValidator>()
            {
                { typeof(RequiredAttribute), new RequiredValidator() },
                { typeof(EmailAttribute), new EmailValidator() }
            };
        }

        public static ValidationManager InitializeFactories() => new ValidationManager();

        public IList<ValidationResult> ValidateEntityProperties<TEntity>(TEntity entity)
        {
            if (Equals(entity, null)) return null;

            var consolidatedValidationResults = new List<ValidationResult>();
            var entityProperties = entity.GetType().GetProperties();

            foreach (var propertyInfo in entityProperties)
            {
                var propertyValue = propertyInfo.GetValue(entity);
                var isPropertyArray = typeof(IEnumerable<object>).IsAssignableFrom(propertyInfo.PropertyType) ||
                    typeof(ICollection<object>).IsAssignableFrom(propertyInfo.PropertyType);
                if (isPropertyArray && propertyValue != null)
                {
                    var propertyValues = new List<object>(propertyValue as IEnumerable<object>);
                    propertyValues.ForEach(objectData =>
                    {
                        var objectValidationResults = ValidateEntityProperties(objectData);
                        if (!Equals(objectValidationResults, null))
                        {
                            consolidatedValidationResults.AddRange(objectValidationResults);
                        }
                    });
                }
                else 
                {
                    var validationResults = ValidateProperty(entity, propertyInfo);
                    consolidatedValidationResults.AddRange(validationResults);
                }
            }

            return consolidatedValidationResults;
        }

        private IList<ValidationResult> ValidateProperty<TEntity>(TEntity targetEntity, PropertyInfo propertyInfo)
        {
            var validationResults = new List<ValidationResult>();
            var propertyAttributes = propertyInfo.GetCustomAttributes();

            foreach (var propertyAttribute in propertyAttributes)
            {
                if (Equals(propertyAttribute, null)) continue;

                var propertyAttributeType = propertyAttribute.GetType();
                ValidatorFactories.TryGetValue(propertyAttributeType, out var validatorFactory);
                if (Equals(validatorFactory, null)) continue;

                var validationResult = validatorFactory.ValidateProperty(targetEntity, propertyInfo);
                var hasValidationError = !Equals(validationResult, null) && !validationResult.IsSuccessful;
                if (hasValidationError) validationResults.Add(validationResult);
            }
            return validationResults;
        }
    }
}
