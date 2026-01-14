using System;
using System.Collections.Generic;
using System.Text;

namespace GeometricProps.Domain.Interfaces
{
   public interface IGeometricProps
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

        double Xmax { get; }
        double Ymax { get; }
        double Xmin { get; }
        double Ymin { get; }

        double Xg { get; }
        double Yg { get; }

        double Height { get; }
        double Base { get; }
    }
}
