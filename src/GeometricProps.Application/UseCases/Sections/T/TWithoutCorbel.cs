using System;
using System.Collections.Generic;
using System.Text;
using GeometricProps.Domain.Enums;
using GeometricProps.Domain.Geometry;
using GeometricProps.Domain.Interfaces;
using GeometricProps.Domain;
using System.Runtime.CompilerServices;

namespace GeometricProps.Application.UseCases.Sections.T
{
    public class TWithoutCorbel
    {
        public IDistance bf { get; }
        public IDistance hf { get; }
        public IDistance bw { get; }
        public IDistance h { get; }

        public IGeometricProps GeometricProps { get; }

        public List<IBidimensionalPoint> Points { get; }

        public TWithoutCorbel(Dictionary<TWithoutCorbelProperty, IDistance> props){
            this.bf = props[TWithoutCorbelProperty.bf];
            this.hf = props[TWithoutCorbelProperty.hf];
            this.bw = props[TWithoutCorbelProperty.bw];
            this.h = props[TWithoutCorbelProperty.h];

            this.Points = new List<IBidimensionalPoint>
            {
                new BidimensionalPoint(bw.Value/2, 0),                              // point 1
                new BidimensionalPoint(bw.Value/2, h.Value - hf.Value),             // point 2
                new BidimensionalPoint(bf.Value/2, h.Value - hf.Value),             // point 3
                new BidimensionalPoint(bf.Value/2, h.Value),                        // point 4
                new BidimensionalPoint(-bf.Value/2, h.Value),                       // point 5
                new BidimensionalPoint(-bf.Value/2, h.Value - hf.Value),            // point 6
                new BidimensionalPoint(-bw.Value/2, h.Value - hf.Value),            // point 7
                new BidimensionalPoint(-bw.Value/2, 0),                             // point 8
                new BidimensionalPoint(bw.Value/2, 0)                               // point 9
            };

            this.GeometricProps = new GeometricProps2D(this.Points);
        }
    }
}
