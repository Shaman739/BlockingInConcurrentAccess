using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlockingResourcesInConcurrentAccess.Core.Contract.Figures
{
    public class Triangle : Figure
    {
        public float SideA { get; set; }
        public float SideB { get; set; }
        public float SideC { get; set; }

        public double GetArea()
        {
            var p = (SideA + SideB + SideC) / 2;
            return Math.Sqrt(p * (p - SideA) * (p - SideB) * (p - SideC));
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();


            bool CheckTriangleInequality(float a, float b, float c) => a < b + c;

            if (CheckTriangleInequality(SideA, SideB, SideC)
                && CheckTriangleInequality(SideB, SideA, SideC)
                && CheckTriangleInequality(SideC, SideB, SideA))
                return result;

            result.Add(new ValidationResult("Triangle restrictions not met"));

            return result;

        }
    }
}
