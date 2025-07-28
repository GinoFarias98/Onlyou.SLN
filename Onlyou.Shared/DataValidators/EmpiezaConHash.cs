using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DataValidators
{
    public class EmpiezaConHash : ValidationAttribute
    {
        public EmpiezaConHash()
        {
            ErrorMessage = "El valor debe comenzar con '#'. "; // mensaje default
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return ValidationResult.Success;
            }

            var str = value as string;
            if (!string.IsNullOrEmpty(str) && str.StartsWith("#"))
            {
                return ValidationResult.Success;
            }

            var regex = new System.Text.RegularExpressions.Regex(@"^#([0-9a-fA-F]{6}|[0-9a-fA-F]{8})$");
            if (regex.IsMatch(str))
                return ValidationResult.Success;

            return new ValidationResult(ErrorMessage);

        }
    }
}


