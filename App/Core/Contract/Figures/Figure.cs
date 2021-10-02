using System.ComponentModel.DataAnnotations;

namespace BlockingResourcesInConcurrentAccess.Core.Contract.Figures
{
    public interface Figure : IValidatableObject
    {
        double GetArea();
    }
}
