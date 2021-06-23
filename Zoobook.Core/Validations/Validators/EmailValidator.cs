using System;
using System.Net.Mail;
using System.Reflection;
using Zoobook.Core.Properties;

namespace Zoobook.Core
{
    public class EmailValidator : IValidator
    {
        public ValidationResult ValidateProperty<TEntity>(TEntity targetEntity, PropertyInfo propertyInfo)
        {
            var propertyValue = propertyInfo.GetValue(targetEntity).ToString();

            try
            {
                var addr = new MailAddress(propertyValue);
                if (addr.Address != propertyValue)
                    throw new Exception();
            }
            catch
            {
                return new ValidationResult()
                {
                    IsSuccessful = false,
                    ErrorMessage = string.Format(Resources.PropertyEmailNotValidError, propertyValue)
                };
            }

            return null;
        }
    }
}
