using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlockingResourcesInConcurrentAccess.Core.Contract.Figures
{
    public class Circle : Figure
    {
        public float Radius { get; set; }


        public double GetArea() => Math.PI * Radius * Radius;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            if (Radius <= 0)
                result.Add(new ValidationResult("Circle restrictions not met"));

            return result;

        }
    }
}
