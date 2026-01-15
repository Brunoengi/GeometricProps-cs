using GeometricProps.Domain;
using GeometricProps.Domain.Enums;
using GeometricProps.Domain.Enums.T;
using GeometricProps.Domain.Geometry;
using GeometricProps.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace GeometricProps.Application.UseCases.Sections.T
{
    public class TTriangularCorbel
    {
        public IDistance bf { get; }
        public IDistance hf { get; }
        public IDistance bw { get; }
        public IDistance h { get; }
        public IDistance bmis { get; }
        public IDistance hmis { get; }

        public IGeometricProps GeometricProps { get; }

        public List<IBidimensionalPoint> Points { get; }

        public TTriangularCorbel(Dictionary<TTriangularCorbelProperty, IDistance> props)
        {
            this.bf = props[TTriangularCorbelProperty.bf];
            this.hf = props[TTriangularCorbelProperty.hf];
            this.bw = props[TTriangularCorbelProperty.bw];
            this.h = props[TTriangularCorbelProperty.h];
            this.bmis = props[TTriangularCorbelProperty.bmis];
            this.hmis = props[TTriangularCorbelProperty.hmis];

            this.Points = new List<IBidimensionalPoint>
            {
                new BidimensionalPoint(-bw.Value / 2, 0),                                           // point 1
                new BidimensionalPoint(bw.Value / 2, 0),                                            // point 2
                new BidimensionalPoint(bw.Value / 2, h.Value - hf.Value - hmis.Value),              // point 3
                new BidimensionalPoint(bw.Value / 2 + bmis.Value, h.Value - hf.Value),              // point 4
                new BidimensionalPoint(bf.Value / 2, h.Value - hf.Value),                           // point 5
                new BidimensionalPoint(bf.Value / 2, h.Value),                                      // point 6
                new BidimensionalPoint(-bf.Value / 2, h.Value),                                     // point 7
                new BidimensionalPoint(-bf.Value / 2, h.Value - hf.Value),                          // point 8
                new BidimensionalPoint(-bw.Value / 2 - bmis.Value, h.Value - hf.Value),             // point 9
                new BidimensionalPoint(-bw.Value / 2, h.Value - hf.Value - hmis.Value),             // point 10
                new BidimensionalPoint(-bw.Value / 2, 0)                                            // point 11
            };

            this.GeometricProps = new GeometricProps2D(this.Points);
        }
    }
}
