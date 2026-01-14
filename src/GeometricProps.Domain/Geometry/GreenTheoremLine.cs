using System;
using System.Collections.Generic;
using System.Text;

namespace GeometricProps.Domain.Geometry
{
    internal readonly struct GreenTheoremLine(double x0, double x1, double y0, double y1)
    {
        public double X0 { get; } = x0;
        public double X1 { get; } = x1;
        public double Y0 { get; } = y0;
        public double Y1 { get; } = y1;
        public double Dx { get; } = x1 - x0;
        public double Dy { get; } = y1 - y0;
    }
}
