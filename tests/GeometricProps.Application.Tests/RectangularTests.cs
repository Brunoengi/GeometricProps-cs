using Xunit;
using GeometricProps.Domain.Interfaces;
using GeometricProps.Domain;
using GeometricProps.Domain.Geometry;
using GeometricProps.Domain.Enums;
using System.Collections.Generic;
using Xunit;
using Moq;
using GeometricProps.Application.UseCases.Sections;

public class RectangularTests
{
    [Fact]
    public void Constructor_AssignsPropertiesCorrectly()
    {
        // Arrange
        var bwMock = new Mock<IDistance>();
        bwMock.Setup(b => b.Value).Returns(20);

        var hMock = new Mock<IDistance>();
        hMock.Setup(h => h.Value).Returns(60);

        var props = new Dictionary<RectangularProperty, IDistance>
        {
            { RectangularProperty.bw, bwMock.Object },
            { RectangularProperty.h, hMock.Object }
        };

        // Act
        var rect = new Rectangular(props);

        // Assert
        Assert.Equal(20, rect.bw.Value);
        Assert.Equal(60, rect.h.Value);

        // Verifica os pontos
        Assert.Equal(5, rect.points.Count); // inclui ponto inicial repetido

        Assert.Equal(0, rect.points[0].X);
        Assert.Equal(0, rect.points[0].Y);

        Assert.Equal(20, rect.points[1].X);
        Assert.Equal(0, rect.points[1].Y);

        Assert.Equal(20, rect.points[2].X);
        Assert.Equal(60, rect.points[2].Y);

        Assert.Equal(0, rect.points[3].X);
        Assert.Equal(60, rect.points[3].Y);

        Assert.Equal(0, rect.points[4].X);
        Assert.Equal(0, rect.points[4].Y);

        // Verifica que geometricProps foi criado
        Assert.NotNull(rect.geometricProps);

        //Verifica as propriedades geométricas
        Assert.Equal(1200, rect.geometricProps.A);
        Assert.Equal(160000, rect.geometricProps.Iy);
        Assert.Equal(20, rect.geometricProps.Xmax);
        Assert.Equal(0, rect.geometricProps.Xmin);
        Assert.Equal(36000, rect.geometricProps.Sx);
        Assert.Equal(12000, rect.geometricProps.Sy);
        Assert.Equal(1440000, rect.geometricProps.Ix);
        Assert.Equal(160000, rect.geometricProps.Iy);
        Assert.Equal(360000, rect.geometricProps.Ixy);
        Assert.Equal(10, rect.geometricProps.Xg);
        Assert.Equal(30, rect.geometricProps.Yg);
        Assert.Equal(360000, rect.geometricProps.Ixg);
        Assert.Equal(40000, rect.geometricProps.Iyg);
        Assert.Equal(0, rect.geometricProps.Ixyg);
        Assert.Equal(-30, rect.geometricProps.Y1);
        Assert.Equal(30, rect.geometricProps.Y2);
        Assert.Equal(-12000, rect.geometricProps.W1);
        Assert.Equal(12000, rect.geometricProps.W2);
        Assert.Equal(60, rect.geometricProps.Height);
    }
}