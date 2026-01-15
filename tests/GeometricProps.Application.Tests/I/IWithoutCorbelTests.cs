using GeometricProps.Application.UseCases.Sections;
using GeometricProps.Application.UseCases.Sections.I;
using GeometricProps.Domain.Enums.I;
using GeometricProps.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeometricProps.Application.Tests.I
{
    public class IWithoutCorbelTests
    {
        [Fact]
        public void Constructor_AssignsPropertiesCorrectly()
        {
            var bfMock = new Mock<IDistance>();
            bfMock.Setup(bf => bf.Value).Returns(80);

            var hfMock = new Mock<IDistance>();
            hfMock.Setup(hf => hf.Value).Returns(20);

            var bwMock = new Mock<IDistance>();
            bwMock.Setup(bw => bw.Value).Returns(20);

            var biMock = new Mock<IDistance>();
            biMock.Setup(bi => bi.Value).Returns(60);

            var hiMock = new Mock<IDistance>();
            hiMock.Setup(hi => hi.Value).Returns(20);

            var hMock = new Mock<IDistance>();
            hMock.Setup(h => h.Value).Returns(120);

            var props = new Dictionary<IWithoutCorbelProperty, IDistance>
        {
            { IWithoutCorbelProperty.bf, bfMock.Object },
            { IWithoutCorbelProperty.hf, hfMock.Object },
            { IWithoutCorbelProperty.bw, bwMock.Object },
            { IWithoutCorbelProperty.bi, biMock.Object },
            { IWithoutCorbelProperty.hi, hiMock.Object },
            { IWithoutCorbelProperty.h, hMock.Object }
        };

            // Act
            var SectionI = new IWithoutCorbel(props);

            // Assert
            Assert.Equal(80, SectionI.bf.Value);
            Assert.Equal(20, SectionI.hf.Value);
            Assert.Equal(20, SectionI.bw.Value);
            Assert.Equal(60, SectionI.bi.Value);
            Assert.Equal(20, SectionI.hi.Value);
            Assert.Equal(120, SectionI.h.Value);

            // Verifica os pontos
            Assert.Equal(13, SectionI.Points.Count);

            // Verifica que geometricProps foi criado
            Assert.NotNull(SectionI.GeometricProps);

            //Verifica as propriedades geométricas
            Assert.Equal(4400, SectionI.GeometricProps.A, 1);
            Assert.Equal(40, SectionI.GeometricProps.Xmax, 1);
            Assert.Equal(-40, SectionI.GeometricProps.Xmin, 1);
            Assert.Equal(284000, SectionI.GeometricProps.Sx, 1);
            Assert.Equal(0, SectionI.GeometricProps.Sy, 1);
            Assert.Equal(26186666.7, SectionI.GeometricProps.Ix, 1);
            Assert.Equal(1266666.7, SectionI.GeometricProps.Iy, 1);
            Assert.Equal(0, SectionI.GeometricProps.Ixy, 1);
            Assert.Equal(0, SectionI.GeometricProps.Xg, 1);
            Assert.Equal(64.55, SectionI.GeometricProps.Yg, 0);
            Assert.Equal(7855757.58, SectionI.GeometricProps.Ixg, 0);
            Assert.Equal(1266666.7, SectionI.GeometricProps.Iyg, 0);
            Assert.Equal(0, SectionI.GeometricProps.Ixyg, 1);
            Assert.Equal(-64.55, SectionI.GeometricProps.Y1, 0);
            Assert.Equal(55.45, SectionI.GeometricProps.Y2, 0);
            Assert.Equal(-121708.92, SectionI.GeometricProps.W1, 0);
            Assert.Equal(141661.20, SectionI.GeometricProps.W2, 0);
            Assert.Equal(120, SectionI.GeometricProps.Height, 1);
        }
    }
}
