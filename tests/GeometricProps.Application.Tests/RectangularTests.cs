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
        Assert.Equal(5, rect.Points.Count); // inclui ponto inicial repetido

        Assert.Equal(0, rect.Points[0].X);
        Assert.Equal(0, rect.Points[0].Y);

        Assert.Equal(20, rect.Points[1].X);
        Assert.Equal(0, rect.Points[1].Y);

        Assert.Equal(20, rect.Points[2].X);
        Assert.Equal(60, rect.Points[2].Y);

        Assert.Equal(0, rect.Points[3].X);
        Assert.Equal(60, rect.Points[3].Y);

        Assert.Equal(0, rect.Points[4].X);
        Assert.Equal(0, rect.Points[4].Y);

        // Verifica que geometricProps foi criado
        Assert.NotNull(rect.GeometricProps);

        //Verifica as propriedades geométricas
        Assert.Equal(1200, rect.GeometricProps.A);
        Assert.Equal(160000, rect.GeometricProps.Iy);
        Assert.Equal(20, rect.GeometricProps.Xmax);
        Assert.Equal(0, rect.GeometricProps.Xmin);
        Assert.Equal(36000, rect.GeometricProps.Sx);
        Assert.Equal(12000, rect.GeometricProps.Sy);
        Assert.Equal(1440000, rect.GeometricProps.Ix);
        Assert.Equal(160000, rect.GeometricProps.Iy);
        Assert.Equal(360000, rect.GeometricProps.Ixy);
        Assert.Equal(10, rect.GeometricProps.Xg);
        Assert.Equal(30, rect.GeometricProps.Yg);
        Assert.Equal(360000, rect.GeometricProps.Ixg);
        Assert.Equal(40000, rect.GeometricProps.Iyg);
        Assert.Equal(0, rect.GeometricProps.Ixyg);
        Assert.Equal(-30, rect.GeometricProps.Y1);
        Assert.Equal(30, rect.GeometricProps.Y2);
        Assert.Equal(-12000, rect.GeometricProps.W1);
        Assert.Equal(12000, rect.GeometricProps.W2);
        Assert.Equal(60, rect.GeometricProps.Height);
    }
}