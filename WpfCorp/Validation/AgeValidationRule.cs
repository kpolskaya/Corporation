using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfCorp.Validation
{
    public class AgeValidationRule : ValidationRule
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public AgeValidationRule()
        {

        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int age = 0;

            try
            {
                if (((string)value).Length > 0)
                    age = int.Parse((string)value);
            }
            catch (Exception)
            {
                return new ValidationResult(false, $"Недопустимые символы!");
            }

            if (age < Min || age > Max)
                return new ValidationResult(false, $"от {Min} до {Max}!");

            return ValidationResult.ValidResult;


        }
    }
}
