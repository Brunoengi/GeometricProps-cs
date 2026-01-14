using System;
using System.Collections.Generic;
using GeometricProps.Domain.Interfaces;
using GeometricProps.Domain.Geometry;

namespace GeometricProps.Domain
{
    public sealed class GeometricProps2D : IGeometricProps
    {
        private double _A = 0;
        private double _Sx = 0;
        private double _Sy = 0;
        private double _Ix = 0;
        private double _Iy = 0;
        private double _Ixy = 0;

        private double _Ixg;
        private double _Iyg;
        private double _Ixyg;

        private double _Y1;
        private double _Y2;
        private double _W1;
        private double _W2;

        private double? _Xmax;
        private double? _Ymax;
        private double? _Xmin;
        private double? _Ymin;

        private double _Xg;
        private double _Yg;

        private double _height;
        private double _base;

        public GeometricProps2D(IReadOnlyList<IBidimensionalPoint> vector)
        {
            if (vector == null) throw new ArgumentNullException(nameof(vector));
            if (vector.Count < 2)
                throw new ArgumentException(
                    "Vector must contain at least 2 points.",
                    nameof(vector)
                );

            for (int i = 0; i < vector.Count - 1; i++)
            {
                var p0 = vector[i];
                var p1 = vector[i + 1];

                var line = new GreenTheoremLine(p0.X, p1.X, p0.Y, p1.Y);

                CalculateArea(line);
                CalculateSx(line);
                CalculateSy(line);
                CalculateIx(line);
                CalculateIy(line);
                CalculateIxy(line);
            }

            CalculateXg();
            CalculateYg();
            CalculateIxg();
            CalculateIyg();
            CalculateIxyg();

            for (int j = 0; j < vector.Count; j++)
            {
                CalculateXMax(vector[j].X);
                CalculateXMin(vector[j].X);
                CalculateYMax(vector[j].Y);
                CalculateYMin(vector[j].Y);
            }

            CalculateHeight();
            CalculateBase();
            CalculateY1();
            CalculateY2();
            CalculateW1();
            CalculateW2();

            SumSignCorrection();
        }

        private void CalculateArea(GreenTheoremLine line)
        {
            _A += (line.X0 + line.Dx / 2.0) * line.Dy;
        }

        private void CalculateSx(GreenTheoremLine line)
        {
            _Sx +=
                (
                    line.X0 * (line.Y0 + line.Dy / 2.0)
                    + line.Dx * (line.Y0 / 2.0 + line.Dy / 3.0)
                ) * line.Dy;
        }

        private void CalculateSy(GreenTheoremLine line)
        {
            _Sy +=
                (line.X0 * (line.X0 + line.Dx) + Math.Pow(line.Dx, 2) / 3.0)
                * line.Dy
                / 2.0;
        }

        private void CalculateIx(GreenTheoremLine line)
        {
            _Ix +=
                (
                    line.X0
                        * (
                            line.Y0 * (line.Dy + line.Y0)
                            + Math.Pow(line.Dy, 2) / 3.0
                        )
                    + line.Dx
                        * (
                            line.Y0 * (line.Y0 / 2.0 + 2.0 * line.Dy / 3.0)
                            + Math.Pow(line.Dy, 2) / 4.0
                        )
                ) * line.Dy;
        }

        private void CalculateIy(GreenTheoremLine line)
        {
            _Iy +=
                (
                    Math.Pow(line.Dx, 3) / 4.0
                    + line.X0
                        * (
                            Math.Pow(line.Dx, 2)
                            + line.X0 * (3.0 * line.Dx / 2.0 + line.X0)
                        )
                ) * line.Dy
                / 3.0;
        }

        private void CalculateIxy(GreenTheoremLine line)
        {
            _Ixy +=
                (
                    line.X0
                        * (
                            line.X0 * (line.Y0 + line.Dy / 2.0)
                            + line.Dx * (line.Y0 + 2.0 * line.Dy / 3.0)
                        )
                    + Math.Pow(line.Dx, 2) * (line.Y0 / 3.0 + line.Dy / 4.0)
                ) * line.Dy
                / 2.0;
        }

        private void CalculateXg()
        {
            _Xg = _Sy / _A;
        }

        private void CalculateYg()
        {
            _Yg = _Sx / _A;
        }

        private void CalculateIxg()
        {
            _Ixg = _Ix - Math.Pow(_Yg, 2) * _A;
        }

        private void CalculateIyg()
        {
            _Iyg = _Iy - Math.Pow(_Xg, 2) * _A;
        }

        private void CalculateIxyg()
        {
            _Ixyg = _Ixy - _Xg * _Yg * _A;
        }

        private void CalculateY1()
        {
            _Y1 = Math.Abs(_Yg - Ymin);
        }

        private void CalculateY2()
        {
            _Y2 = Math.Abs(Ymax - _Yg);
        }

        private void CalculateW1()
        {
            _W1 = _Ixg / _Y1;
        }

        private void CalculateW2()
        {
            _W2 = _Ixg / _Y2;
        }

        private void CalculateHeight()
        {
            _height = Math.Abs(Ymax - Ymin);
        }

        private void CalculateBase()
        {
            _base = Math.Abs(Xmax - Xmin);
        }

        private void CalculateXMax(double x)
        {
            if (!_Xmax.HasValue || x >= _Xmax.Value) _Xmax = x;
        }

        private void CalculateXMin(double x)
        {
            if (!_Xmin.HasValue || x <= _Xmin.Value) _Xmin = x;
        }

        private void CalculateYMax(double y)
        {
            if (!_Ymax.HasValue || y >= _Ymax.Value) _Ymax = y;
        }

        private void CalculateYMin(double y)
        {
            if (!_Ymin.HasValue || y <= _Ymin.Value) _Ymin = y;
        }

        private void SumSignCorrection()
        {
            _Y1 *= -1;
            _W1 *= -1;
        }

        public double A => _A;
        public double Sx => _Sx;
        public double Sy => _Sy;
        public double Ix => _Ix;
        public double Iy => _Iy;
        public double Ixy => _Ixy;

        public double Xmax => _Xmax ?? throw new InvalidOperationException("Xmax not calculated.");
        public double Xmin => _Xmin ?? throw new InvalidOperationException("Xmin not calculated.");
        public double Ymax => _Ymax ?? throw new InvalidOperationException("Ymax not calculated.");
        public double Ymin => _Ymin ?? throw new InvalidOperationException("Ymin not calculated.");

        public double Xg => _Xg;
        public double Yg => _Yg;

        public double Ixg => _Ixg;
        public double Iyg => _Iyg;
        public double Ixyg => _Ixyg;

        public double Y1 => _Y1;
        public double Y2 => _Y2;
        public double W1 => _W1;
        public double W2 => _W2;

        public double Height => _height;
        public double Base => _base;
    }
}