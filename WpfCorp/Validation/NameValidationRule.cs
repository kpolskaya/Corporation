using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace WpfCorp.Validation
{
    public class NameValidationRule : ValidationRule
    {
        public NameValidationRule()
        {

        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (String.IsNullOrWhiteSpace((string)value))
                return new ValidationResult(false, "Поле не может быть пустым");

            if (((string)value).Any(c => !char.IsLetter(c)))
                return new ValidationResult(false, "В имени допускаются только буквы"); //что сомнительно

            return ValidationResult.ValidResult;
        }
    }
}
