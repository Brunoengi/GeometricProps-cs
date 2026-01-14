using System;
using System.Collections.Generic;
using System.Text;

namespace GeometricProps.Domain.Interfaces
{
    public interface IDistance
    {
        float Value { get; }
        string Unit => "cm";
    }

}
