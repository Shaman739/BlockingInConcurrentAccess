using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlockingResourcesInConcurrentAccess.Core.Contract.Figures
{
    public class Square : Figure
    {
        public float SideA { get; set; }

        public double GetArea() => SideA * SideA;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            if (SideA <= 0)
                result.Add(new ValidationResult("Square restrictions not met"));

            return result;

        }
    }
}
