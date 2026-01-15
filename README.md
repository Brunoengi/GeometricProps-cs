# GeometricProps

Um projeto em C# que calcula propriedades geométricas de figuras planas (2D) definidas por contornos poligonais. Utiliza o Teorema de Green para computar integrais discretas de características geométricas.

## 📋 Visão Geral

A solução é composta por dois projetos principais:

- **GeometricProps.Domain**: Contém a lógica de cálculo de propriedades geométricas
- **GeometricProps.Application**: Fornece casos de uso específicos para diferentes tipos de geometria (ex: retângulos)

## 🏗️ Arquitetura

```
GeometricProps/
├── src/
│   ├── GeometricProps.Domain/
│   │   ├── GeometricProps.cs (GeometricProps2D)
│   │   ├── Geometry/
│   │   │   └── BidimensionalPoint.cs
│   │   ├── Interfaces/
│   │   │   ├── IGeometricProps.cs
│   │   │   ├── IBidimensionalPoint.cs
│   │   │   └── IUnits.cs
│   │   └── Enums/
│   │       └── RectangularProperty.cs
│   └── GeometricProps.Application/
│       └── UseCases/
│           └── Sections/
│               ├── Rectangular.cs
│               ├── T/
│               │   ├── TWithoutCorbel.cs
│               │   └── TTriangularCorbel.cs
│               └── I/
│                   ├── IWithoutCorbel.cs
│                   └── ITriangularCorbel.cs
└── tests/
    └── GeometricProps.Application.Tests/
```

## 🔑 Classes Principais

### GeometricProps2D

A classe `GeometricProps2D` implementa `IGeometricProps` e é responsável por calcular todas as propriedades geométricas de um contorno 2D.

**Construtor:**
```csharp
public GeometricProps2D(IReadOnlyList<IBidimensionalPoint> vector)
```

Recebe uma lista ordenada de pontos que definem o contorno de uma figura plana. Cada par consecutivo de pontos representa um segmento usado nos cálculos.

**Propriedades Disponíveis:**

#### Propriedades na Origem (0,0):
- **A**: Área algébrica do contorno
- **Sx**: Momento estático em relação ao eixo X
- **Sy**: Momento estático em relação ao eixo Y
- **Ix**: Segundo momento de área em relação ao eixo X
- **Iy**: Segundo momento de área em relação ao eixo Y
- **Ixy**: Produto de inércia

#### Propriedades no Centroide (Eixos Centroidais):
- **Ixg**: Segundo momento de área em relação ao eixo X centroidal
- **Iyg**: Segundo momento de área em relação ao eixo Y centroidal
- **Ixyg**: Produto de inércia centroidal

#### Dimensões e Módulos Resistentes:
- **Y1**: Distância do centroide até a fibra extrema inferior
- **Y2**: Distância do centroide até a fibra extrema superior
- **W1**: Módulo resistente para a fibra inferior (W1 = Ixg / Y1)
- **W2**: Módulo resistente para a fibra superior (W2 = Ixg / Y2)

#### Envelope do Contorno (Bounding Box):
- **Xmin**: Coordenada X mínima
- **Xmax**: Coordenada X máxima
- **Ymin**: Coordenada Y mínima
- **Ymax**: Coordenada Y máxima
- **Height**: Altura total (Ymax - Ymin)
- **Base**: Largura total (Xmax - Xmin)

#### Centroide:
- **Xg**: Coordenada X do centroide
- **Yg**: Coordenada Y do centroide

### Seções Padronizadas (Use Cases)

O projeto fornece implementações prontas para seções transversais comuns no namespace `GeometricProps.Application.UseCases.Sections`.

#### 1. Retangular (`Rectangular`)

**Construtor:**
```csharp
public Rectangular(Dictionary<RectangularProperty, IDistance> props)
```

**Parâmetros (`RectangularProperty`):**
- `bw`: Largura (base)
- `h`: Altura

#### 2. Seção T (`TWithoutCorbel`)

Representa uma seção em forma de T.

**Construtor:**
```csharp
public TWithoutCorbel(Dictionary<TWithoutCorbelProperty, IDistance> props)
```

**Parâmetros (`TWithoutCorbelProperty`):**
- `bf`: Largura da mesa (flange)
- `hf`: Altura da mesa
- `bw`: Largura da alma (web)
- `h`: Altura total

#### 3. Seção I (`IWithoutCorbel`)

Representa uma seção em forma de I (pode ser assimétrica).

**Construtor:**
```csharp
public IWithoutCorbel(Dictionary<IWithoutCorbelProperty, IDistance> props)
```

**Parâmetros (`IWithoutCorbelProperty`):**
- `bf`: Largura da mesa superior
- `hf`: Altura da mesa superior
- `bw`: Largura da alma
- `bi`: Largura da mesa inferior
- `hi`: Altura da mesa inferior
- `h`: Altura total

## 💡 Exemplos de Uso

### Exemplo 1: Calcular Propriedades de um Retângulo

```csharp
using GeometricProps.Application.UseCases.Sections;
using GeometricProps.Domain.Enums;
using GeometricProps.Domain.Interfaces;

// Preparar propriedades com dicionário
var props = new Dictionary<RectangularProperty, IDistance>
{
    { RectangularProperty.bw, new Distance(20) },   // largura de 20 cm
    { RectangularProperty.h, new Distance(60) }    // altura de 60 cm
};

// Criar o retângulo
var rectangle = new Rectangular(props);

// Acessar propriedades geométricas
double area = rectangle.geometricProps.A;                    // 1200
double momentoInerciaX = rectangle.geometricProps.Ix;        // 1440000
double momentoInerciaY = rectangle.geometricProps.Iy;        // 160000
double centroideX = rectangle.geometricProps.Xg;             // 10
double centroideY = rectangle.geometricProps.Yg;             // 30
double momentoInerciaXg = rectangle.geometricProps.Ixg;      // 360000
double momentoInerciaYg = rectangle.geometricProps.Iyg;      // 40000

Console.WriteLine($"Área: {area}");
Console.WriteLine($"Centroide: ({centroideX}, {centroideY})");
Console.WriteLine($"Ix: {momentoInerciaX}, Iy: {momentoInerciaY}");
```

### Exemplo 2: Calcular Propriedades de um Polígono Arbitrário

```csharp
using GeometricProps.Domain;
using GeometricProps.Domain.Geometry;

// Criar uma lista de pontos que define o contorno
var points = new List<IBidimensionalPoint>
{
    new BidimensionalPoint(0, 0),
    new BidimensionalPoint(10, 0),
    new BidimensionalPoint(10, 5),
    new BidimensionalPoint(0, 5),
    new BidimensionalPoint(0, 0)  // Fechar o polígono repetindo o primeiro ponto
};

// Criar a instância de GeometricProps2D
var geometricProps = new GeometricProps2D(points);

// Acessar as propriedades
Console.WriteLine($"Área: {geometricProps.A}");
Console.WriteLine($"Momento estático Sx: {geometricProps.Sx}");
Console.WriteLine($"Momento estático Sy: {geometricProps.Sy}");
Console.WriteLine($"Centroide: ({geometricProps.Xg}, {geometricProps.Yg})");
Console.WriteLine($"Altura: {geometricProps.Height}");
Console.WriteLine($"Base: {geometricProps.Base}");
```

### Exemplo 3: Acessar Propriedades Centroidais

```csharp
// Usando um polígono já criado
var props = geometricProps;

// Propriedades em relação aos eixos centroidais
double ixCentroid = props.Ixg;   // Momento de inércia X em relação ao centroide
double iyCentroid = props.Iyg;   // Momento de inércia Y em relação ao centroide
double ixyProduct = props.Ixyg;  // Produto de inércia centroidal

// Módulos resistentes
double w1 = props.W1;  // Módulo resistente para fibra inferior
double w2 = props.W2;  // Módulo resistente para fibra superior

Console.WriteLine($"Ixg: {ixCentroid}, Iyg: {iyCentroid}");
Console.WriteLine($"Módulos resistentes: W1={w1}, W2={w2}");
```

## 📐 Conceitos Matemáticos

O cálculo das propriedades geométricas é baseado no **Teorema de Green**, que permite calcular integrais sobre uma região através de integrais sobre seu contorno.

Para um polígono fechado, as propriedades são calculadas como:

- **Área**: $A = \frac{1}{2} \sum_{i=0}^{n-1} (x_i y_{i+1} - x_{i+1} y_i)$

- **Momento Estático**: $S_x = \frac{1}{6} \sum_{i=0}^{n-1} (x_i y_{i+1} - x_{i+1} y_i)(y_i + y_{i+1})$

- **Segundo Momento**: $I_x = \frac{1}{12} \sum_{i=0}^{n-1} (x_i y_{i+1} - x_{i+1} y_i)(y_i^2 + y_i y_{i+1} + y_{i+1}^2)$

- **Centroide**: $\bar{x} = \frac{S_y}{A}$, $\bar{y} = \frac{S_x}{A}$

## ⚠️ Observações Importantes

1. **Orientação do Contorno**: O sinal das propriedades (especialmente A e momentos) depende da orientação do contorno (horária ou anti-horária).

2. **Fechamento do Polígono**: Espera-se que o vetor de pontos represente o contorno em ordem. Se o polígono não estiver explicitamente fechado (último ponto ≠ primeiro), o fechamento depende do chamador.

3. **Requisito Mínimo**: O vetor deve conter pelo menos 2 pontos.

4. **Precisão**: Os cálculos são realizados em ponto flutuante (double), adequado para a maioria das aplicações em engenharia.

## 🧪 Testes

O projeto inclui testes de integração em `GeometricProps.Application.Tests`:

```csharp
[Fact]
public void Constructor_AssignsPropertiesCorrectly()
{
    // Teste de um retângulo 20 x 60
    var bwMock = new Mock<IDistance>();
    bwMock.Setup(b => b.Value).Returns(20);
    
    var hMock = new Mock<IDistance>();
    hMock.Setup(h => h.Value).Returns(60);
    
    var props = new Dictionary<RectangularProperty, IDistance>
    {
        { RectangularProperty.bw, bwMock.Object },
        { RectangularProperty.h, hMock.Object }
    };
    
    var rect = new Rectangular(props);
    
    // Verificações
    Assert.Equal(1200, rect.geometricProps.A);
    Assert.Equal(10, rect.geometricProps.Xg);
    Assert.Equal(30, rect.geometricProps.Yg);
}
```

Execute os testes com:
```bash
dotnet test
```

## 📦 Estrutura de Interfaces

### IGeometricProps
Define contrato para propriedades geométricas:
```csharp
public interface IGeometricProps
{
    double A { get; }        // Área
    double Sx { get; }       // Momento estático X
    double Sy { get; }       // Momento estático Y
    double Ix { get; }       // Momento de inércia X
    double Iy { get; }       // Momento de inércia Y
    double Ixy { get; }      // Produto de inércia
    double Ixg { get; }      // Momento de inércia X centroidal
    double Iyg { get; }      // Momento de inércia Y centroidal
    double Ixyg { get; }     // Produto de inércia centroidal
    double Y1 { get; }       // Distância fibra inferior
    double Y2 { get; }       // Distância fibra superior
    double W1 { get; }       // Módulo resistente inferior
    double W2 { get; }       // Módulo resistente superior
    double Xmax { get; }     // X máximo
    double Ymax { get; }     // Y máximo
    double Xmin { get; }     // X mínimo
    double Ymin { get; }     // Y mínimo
    double Xg { get; }       // Centroide X
    double Yg { get; }       // Centroide Y
    double Height { get; }   // Altura
    double Base { get; }     // Base
}
```

### IBidimensionalPoint
Representa um ponto em 2D:
```csharp
public interface IBidimensionalPoint
{
    double X { get; }
    double Y { get; }
}
```

### IDistance
Representa uma dimensão com unidade:
```csharp
public interface IDistance
{
    float Value { get; }
    string Unit => "cm";
}
```

## 🚀 Como Compilar e Executar

### Requisitos
- .NET 10.0 ou superior

### Compilação
```bash
dotnet build
```

### Testes
```bash
dotnet test
```

### Restaurar Dependências
```bash
dotnet restore
```

## 📝 Licença

Este projeto é fornecido como está, sem garantias implícitas ou explícitas.

## 🤝 Contribuindo

Contribuições são bem-vindas! Para contribuir:

1. Fork o repositório
2. Crie uma branch para sua feature
3. Commit suas mudanças
4. Push para a branch
5. Abra um Pull Request

## 📞 Contato

Para dúvidas ou sugestões, entre em contato com o mantenedor do projeto.