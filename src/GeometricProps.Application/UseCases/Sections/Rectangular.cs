using GeometricProps.Domain.Enums;
using GeometricProps.Domain.Geometry;
using GeometricProps.Domain.Interfaces;
using GeometricProps.Domain;
using System;
using System.Collections.Generic;

namespace GeometricProps.Application.UseCases.Sections
{
    public class Rectangular
    {
        public IDistance bw { get; }
        public IDistance h { get; }

        public IGeometricProps GeometricProps { get; }
        public List<IBidimensionalPoint> Points { get; }

        public Rectangular(Dictionary<RectangularProperty, IDistance> props)
        {
            this.bw = props[RectangularProperty.bw];
            this.h = props[RectangularProperty.h];
            
            this.Points = new List<IBidimensionalPoint>
            {
                new BidimensionalPoint(0, 0),             
                new BidimensionalPoint(bw.Value, 0),      
                new BidimensionalPoint(bw.Value, h.Value),
                new BidimensionalPoint(0, h.Value),       
                new BidimensionalPoint(0, 0)              
            };

            this.GeometricProps = new GeometricProps2D(this.Points);
        }
    }   
}
