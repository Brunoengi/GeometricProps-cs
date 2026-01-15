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
    public class IWithoutCorbel
    {
        public IDistance bf { get; }
        public IDistance hf { get; }
        public IDistance bw { get; }
        public IDistance h { get; }
        public IDistance bi { get; }
        public IDistance hi { get; }

        public IGeometricProps GeometricProps { get; }
        public List<IBidimensionalPoint> Points { get; }

        public IWithoutCorbel(Dictionary<IWithoutCorbelProperty, IDistance> props){
            this.bf = props[IWithoutCorbelProperty.bf];
            this.hf = props[IWithoutCorbelProperty.hf];
            this.bw = props[IWithoutCorbelProperty.bw];
            this.h = props[IWithoutCorbelProperty.h];
            this.bi = props[IWithoutCorbelProperty.bi];
            this.hi = props[IWithoutCorbelProperty.hi];

             this.Points = new List<IBidimensionalPoint>
            {
                new BidimensionalPoint(-bi.Value/2, 0),                                 // point 1
                new BidimensionalPoint(bi.Value/2, 0),                                  // point 2
                new BidimensionalPoint(bi.Value/2, hi.Value),                           // point 3
                new BidimensionalPoint(bw.Value/2, hi.Value),                           // point 4
                new BidimensionalPoint(bw.Value/2, h.Value - hf.Value),                 // point 5
                new BidimensionalPoint(bf.Value/2, h.Value - hf.Value),                 // point 6
                new BidimensionalPoint(bf.Value/2, h.Value),                            // point 7
                new BidimensionalPoint(-bf.Value/2, h.Value),                           // point 8
                new BidimensionalPoint(-bf.Value/2, h.Value - hf.Value),                // point 9
                new BidimensionalPoint(-bw.Value/2, h.Value - hf.Value),                // point 10
                new BidimensionalPoint(-bw.Value/2, hi.Value),                          // point 11
                new BidimensionalPoint(-bi.Value/2, hi.Value),                          // point 12
                new BidimensionalPoint(-bi.Value/2, 0)                                  // point 13
            };

            this.GeometricProps = new GeometricProps2D(this.Points);
        }
    }
}
