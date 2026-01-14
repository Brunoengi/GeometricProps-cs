using System;
using System.Collections.Generic;
using System.Text;

namespace GeometricProps.Domain.Interfaces
{
    internal interface IGreenTheoremProps
    {
        double A { get; }
        double Sx { get; }
        double Sy { get; }
        double Ix { get; }
        double Iy { get; }
        double Ixy { get; }

        double Ixg { get; }
        double Iyg { get; }
        double Ixyg { get; }

        double Y1 { get; }
        double Y2 { get; }
        double W1 { get; }
        double W2 { get; }
    }
}
