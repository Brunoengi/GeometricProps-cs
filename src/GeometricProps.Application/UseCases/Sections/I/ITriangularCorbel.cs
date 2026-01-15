using System;
using System.Collections.Generic;
using System.Text;
using GeometricProps.Domain.Enums.I;
using GeometricProps.Domain.Geometry;
using GeometricProps.Domain.Interfaces;
using GeometricProps.Domain;
using System.Runtime.CompilerServices;

namespace GeometricProps.Application.UseCases.Sections.I
{
    public class ITriangularCorbel
    {
        public IDistance bf { get; }
        public IDistance hf { get; }
        public IDistance bw { get; }
        public IDistance h { get; }
        public IDistance bi { get; }
        public IDistance hi { get; }
        public IDistance bmissup { get; }
        public IDistance hmissup { get; }
        public IDistance bmisinf { get; }
        public IDistance hmisinf { get; }

        public IGeometricProps GeometricProps { get; }
        public List<IBidimensionalPoint> Points { get; }

        public ITriangularCorbel(Dictionary<ITriangularCorbelProperty, IDistance> props){
            this.bf = props[ITriangularCorbelProperty.bf];
            this.hf = props[ITriangularCorbelProperty.hf];
            this.bw = props[ITriangularCorbelProperty.bw];
            this.h = props[ITriangularCorbelProperty.h];
            this.bi = props[ITriangularCorbelProperty.bi];
            this.hi = props[ITriangularCorbelProperty.hi];
            this.bmissup = props[ITriangularCorbelProperty.bmissup];
            this.hmissup = props[ITriangularCorbelProperty.hmissup];
            this.bmisinf = props[ITriangularCorbelProperty.bmisinf];
            this.hmisinf = props[ITriangularCorbelProperty.hmisinf];

            this.Points = new List<IBidimensionalPoint>();

            // Bottom-left corner
            this.Points.Add(new BidimensionalPoint(-bi.Value / 2, 0));
            this.Points.Add(new BidimensionalPoint(bi.Value / 2, 0));
            this.Points.Add(new BidimensionalPoint(bi.Value / 2, hi.Value));

            // Bottom corbel (right side)
            if (bmisinf.Value > 0 && hmisinf.Value > 0)
            {
                this.Points.Add(new BidimensionalPoint(bw.Value / 2 + bmisinf.Value, hi.Value));
                this.Points.Add(new BidimensionalPoint(bw.Value / 2, hi.Value + hmisinf.Value));
            }
            else
            {
                this.Points.Add(new BidimensionalPoint(bw.Value / 2, hi.Value));
            }

            // Web (right side)
            if (bmissup.Value > 0 && hmissup.Value > 0)
            {
                this.Points.Add(new BidimensionalPoint(bw.Value / 2, h.Value - hf.Value - hmissup.Value));
                this.Points.Add(new BidimensionalPoint(bw.Value / 2 + bmissup.Value, h.Value - hf.Value));
            }
            else
            {
                this.Points.Add(new BidimensionalPoint(bw.Value / 2, h.Value - hf.Value));
            }

            // Top flange
            this.Points.Add(new BidimensionalPoint(bf.Value / 2, h.Value - hf.Value));
            this.Points.Add(new BidimensionalPoint(bf.Value / 2, h.Value));
            this.Points.Add(new BidimensionalPoint(-bf.Value / 2, h.Value));
            this.Points.Add(new BidimensionalPoint(-bf.Value / 2, h.Value - hf.Value));

            // Top corbel (left side)
            if (bmissup.Value > 0 && hmissup.Value > 0)
            {
                this.Points.Add(new BidimensionalPoint(-bw.Value / 2 - bmissup.Value, h.Value - hf.Value));
                this.Points.Add(new BidimensionalPoint(-bw.Value / 2, h.Value - hf.Value - hmissup.Value));
            }
            else
            {
                this.Points.Add(new BidimensionalPoint(-bw.Value / 2, h.Value - hf.Value));
            }

            // Web and bottom corbel (left side)
            if (bmisinf.Value > 0 && hmisinf.Value > 0)
            {
                this.Points.Add(new BidimensionalPoint(-bw.Value / 2, hi.Value + hmisinf.Value));
                this.Points.Add(new BidimensionalPoint(-bw.Value / 2 - bmisinf.Value, hi.Value));
            }
            else
            {
                this.Points.Add(new BidimensionalPoint(-bw.Value / 2, hi.Value));
            }

            // Close polygon
            this.Points.Add(new BidimensionalPoint(-bi.Value / 2, hi.Value));
            this.Points.Add(new BidimensionalPoint(-bi.Value / 2, 0));

            this.GeometricProps = new GeometricProps2D(this.Points);
        }
    }
}
