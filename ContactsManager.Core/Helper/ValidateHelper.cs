using System.ComponentModel.DataAnnotations;


namespace Services.ValidationHelper
{
    public class ValidateHelper
    {
        public static void ValidateModel(object? obj)
        {
            if(obj == null) throw new ArgumentNullException(nameof(obj));
            
            ValidationContext validationContext = new ValidationContext(obj);

            List<ValidationResult> validationResults = new List<ValidationResult>();

           bool IsValid = 
                Validator.TryValidateObject(obj, validationContext, validationResults , true);

            if (!IsValid)
                throw new ArgumentException();
        }
    }
}
