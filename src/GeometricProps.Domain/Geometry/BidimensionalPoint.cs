using GeometricProps.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeometricProps.Domain.Geometry
{
    public class BidimensionalPoint : IBidimensionalPoint
    {
        public double X { get; }
        public double Y { get; }
        public BidimensionalPoint(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
