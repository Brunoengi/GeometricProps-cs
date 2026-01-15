using GeometricProps.Application.UseCases.Sections.T;
using GeometricProps.Domain.Enums;
using GeometricProps.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeometricProps.Application.Tests.T
{
    public class TWithoutCorbelTests
    {
        [Fact]
        public void Constructor_AssignsPropertiesCorrectly()
        {
            // Arrange
            var bfMock = new Mock<IDistance>();
            bfMock.Setup(bf => bf.Value).Returns(60);

            var hfMock = new Mock<IDistance>();
            hfMock.Setup(hf => hf.Value).Returns(9);

            var bwMock = new Mock<IDistance>();
            bwMock.Setup(bw => bw.Value).Returns(12);

            var hMock = new Mock<IDistance>();
            hMock.Setup(h => h.Value).Returns(40);

            var props = new Dictionary<TWithoutCorbelProperty, IDistance>
        {
            { TWithoutCorbelProperty.bf, bfMock.Object },
            { TWithoutCorbelProperty.hf, hfMock.Object },
            { TWithoutCorbelProperty.bw, bwMock.Object },
            { TWithoutCorbelProperty.h, hMock.Object }
        };

            // Act
            var T = new TWithoutCorbel(props);

            // Assert
            Assert.Equal(12, T.bw.Value);
            Assert.Equal(40, T.h.Value);
            Assert.Equal(60, T.bf.Value);
            Assert.Equal(9, T.hf.Value);

            // Verifica os pontos
            Assert.Equal(9, T.Points.Count);

            // Verifica que geometricProps foi criado
            Assert.NotNull(T.GeometricProps);

            //Verifica as propriedades geométricas
            Assert.Equal(912, T.GeometricProps.A, 1);
            Assert.Equal(30, T.GeometricProps.Xmax, 1);
            Assert.Equal(-30, T.GeometricProps.Xmin, 1);
            Assert.Equal(24936, T.GeometricProps.Sx, 1);
            Assert.Equal(0, T.GeometricProps.Sy, 1);
            Assert.Equal(803344, T.GeometricProps.Ix, 1);
            Assert.Equal(166464, T.GeometricProps.Iy, 1);
            Assert.Equal(0, T.GeometricProps.Ixy, 1);
            Assert.Equal(0, T.GeometricProps.Xg, 1);
            Assert.Equal(27.34, T.GeometricProps.Yg, 1);
            Assert.Equal(121541.26, T.GeometricProps.Ixg, 0);
            Assert.Equal(166464, T.GeometricProps.Iyg, 0);
            Assert.Equal(0, T.GeometricProps.Ixyg, 1);
            Assert.Equal(-27.34, T.GeometricProps.Y1, 1);
            Assert.Equal(12.66, T.GeometricProps.Y2, 1);
            Assert.Equal(-4445.21, T.GeometricProps.W1, 0);
            Assert.Equal(9602.21, T.GeometricProps.W2, 0);
            Assert.Equal(40, T.GeometricProps.Height, 1);
        }
    }
}
