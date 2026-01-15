using System;
using System.Collections.Generic;
using GeometricProps.Domain.Interfaces;
using GeometricProps.Domain.Geometry;

namespace GeometricProps.Domain
{
    /// <summary>
    /// Calcula propriedades geométricas de uma figura plana (2D) definida por um contorno poligonal.
    ///
    /// As integrais são calculadas via uma formulação discreta baseada no Teorema de Green,
    /// percorrendo os segmentos consecutivos do vetor de pontos.
    ///
    /// Observações importantes:
    /// - Espera-se que <paramref name="vector"/> represente o contorno em ordem (horária ou anti-horária).
    /// - O algoritmo soma contribuições por aresta (p0 -> p1). Se o polígono não estiver “fechado”
    ///   (último ponto != primeiro), o fechamento depende do chamador (este código NÃO adiciona a aresta final).
    /// - O sinal de algumas grandezas (principalmente A e momentos) depende da orientação do contorno.
    /// </summary>
    public sealed class GeometricProps2D : IGeometricProps
    {
        // ===== Propriedades integrais em relação à origem (0,0) =====

        /// <summary>Área algébrica (pode ser negativa dependendo da orientação do contorno).</summary>
        private double _A = 0;

        /// <summary>Momento estático em relação ao eixo X (primeiro momento) em relação à origem.</summary>
        private double _Sx = 0;

        /// <summary>Momento estático em relação ao eixo Y (primeiro momento) em relação à origem.</summary>
        private double _Sy = 0;

        /// <summary>Segundo momento de área em relação ao eixo X na origem.</summary>
        private double _Ix = 0;

        /// <summary>Segundo momento de área em relação ao eixo Y na origem.</summary>
        private double _Iy = 0;

        /// <summary>Produto de inércia em relação à origem.</summary>
        private double _Ixy = 0;

        // ===== Propriedades no centroide (eixos centroidais paralelos) =====
        private double _Ixg;
        private double _Iyg;
        private double _Ixyg;

        // ===== Distâncias do centroide às fibras extremas (vertical) e módulos resistentes =====

        /// <summary>
        /// Distância do centroide até a fibra extrema inferior (em Ymin).
        /// Observação: este código aplica uma correção de sinal ao final (ver <see cref="SumSignCorrection"/>).
        /// </summary>
        private double _Y1;

        /// <summary>Distância do centroide até a fibra extrema superior (em Ymax).</summary>
        private double _Y2;

        /// <summary>Módulo resistente em relação a X para a fibra inferior: W1 = Ixg / Y1.</summary>
        private double _W1;

        /// <summary>Módulo resistente em relação a X para a fibra superior: W2 = Ixg / Y2.</summary>
        private double _W2;

        // ===== Extremos do envelope do contorno =====
        private double? _Xmax;
        private double? _Ymax;
        private double? _Xmin;
        private double? _Ymin;

        // ===== Centroide =====
        private double _Xg;
        private double _Yg;

        // ===== Dimensões globais do envelope (bounding box) =====
        private double _height;
        private double _base;

        /// <summary>
        /// Cria um calculador de propriedades geométricas para um contorno 2D.
        /// </summary>
        /// <param name="vector">
        /// Lista ordenada de pontos do contorno.
        /// Cada par consecutivo de pontos define um segmento usado no somatório.
        /// </param>
        /// <exception cref="ArgumentNullException">Se <paramref name="vector"/> for null.</exception>
        /// <exception cref="ArgumentException">Se <paramref name="vector"/> tiver menos de 2 pontos.</exception>
        public GeometricProps2D(IReadOnlyList<IBidimensionalPoint> vector)
        {
            if (vector == null) throw new ArgumentNullException(nameof(vector));
            if (vector.Count < 2)
                throw new ArgumentException(
                    "Vector must contain at least 2 points.",
                    nameof(vector)
                );

            // 1) Integrais por arestas: percorre segmentos (p0 -> p1)
            //    e acumula área, momentos estáticos e inércias na origem.
            for (int i = 0; i < vector.Count - 1; i++)
            {
                var p0 = vector[i];
                var p1 = vector[i + 1];

                // Representação do segmento na forma usada pela fórmula do Teorema de Green.
                // Espera-se que GreenTheoremLine exponha:
                // - X0, Y0: ponto inicial
                // - Dx, Dy: deltas (X1-X0, Y1-Y0)
                var line = new GreenTheoremLine(p0.X, p1.X, p0.Y, p1.Y);

                CalculateArea(line);
                CalculateSx(line);
                CalculateSy(line);
                CalculateIx(line);
                CalculateIy(line);
                CalculateIxy(line);
            }

            // 2) Centroide (Xg, Yg) a partir dos primeiros momentos e área
            CalculateXg();
            CalculateYg();

            // 3) Inércias centroidais via Teorema dos Eixos Paralelos
            CalculateIxg();
            CalculateIyg();
            CalculateIxyg();

            // 4) Extremos do contorno para envelope (min/max)
            for (int j = 0; j < vector.Count; j++)
            {
                CalculateXMax(vector[j].X);
                CalculateXMin(vector[j].X);
                CalculateYMax(vector[j].Y);
                CalculateYMin(vector[j].Y);
            }

            // 5) Dimensões globais do envelope e grandezas derivadas
            CalculateHeight();
            CalculateBase();
            CalculateY1();
            CalculateY2();
            CalculateW1();
            CalculateW2();

            // 6) Ajuste de sinal adotado pela biblioteca para Y1/W1
            SumSignCorrection();
        }

        /// <summary>
        /// Acumula área algébrica usando a formulação discreta (Teorema de Green).
        /// </summary>
        private void CalculateArea(GreenTheoremLine line)
        {
            _A += (line.X0 + line.Dx / 2.0) * line.Dy;
        }

        /// <summary>
        /// Acumula o primeiro momento Sx (momento estático em relação ao eixo X).
        /// Usado para calcular \( Y_g = S_x / A \).
        /// </summary>
        private void CalculateSx(GreenTheoremLine line)
        {
            _Sx +=
                (
                    line.X0 * (line.Y0 + line.Dy / 2.0)
                    + line.Dx * (line.Y0 / 2.0 + line.Dy / 3.0)
                ) * line.Dy;
        }

        /// <summary>
        /// Acumula o primeiro momento Sy (momento estático em relação ao eixo Y).
        /// Usado para calcular \( X_g = S_y / A \).
        /// </summary>
        private void CalculateSy(GreenTheoremLine line)
        {
            _Sy +=
                (line.X0 * (line.X0 + line.Dx) + Math.Pow(line.Dx, 2) / 3.0)
                * line.Dy
                / 2.0;
        }

        /// <summary>
        /// Acumula o segundo momento de área Ix em relação ao eixo X na origem.
        /// </summary>
        private void CalculateIx(GreenTheoremLine line)
        {
            _Ix +=
                (
                    line.X0
                        * (
                            line.Y0 * (line.Dy + line.Y0)
                            + Math.Pow(line.Dy, 2) / 3.0
                        )
                    + line.Dx
                        * (
                            line.Y0 * (line.Y0 / 2.0 + 2.0 * line.Dy / 3.0)
                            + Math.Pow(line.Dy, 2) / 4.0
                        )
                ) * line.Dy;
        }

        /// <summary>
        /// Acumula o segundo momento de área Iy em relação ao eixo Y na origem.
        /// </summary>
        private void CalculateIy(GreenTheoremLine line)
        {
            _Iy +=
                (
                    Math.Pow(line.Dx, 3) / 4.0
                    + line.X0
                        * (
                            Math.Pow(line.Dx, 2)
                            + line.X0 * (3.0 * line.Dx / 2.0 + line.X0)
                        )
                ) * line.Dy
                / 3.0;
        }

        /// <summary>
        /// Acumula o produto de inércia Ixy em relação à origem.
        /// </summary>
        private void CalculateIxy(GreenTheoremLine line)
        {
            _Ixy +=
                (
                    line.X0
                        * (
                            line.X0 * (line.Y0 + line.Dy / 2.0)
                            + line.Dx * (line.Y0 + 2.0 * line.Dy / 3.0)
                        )
                    + Math.Pow(line.Dx, 2) * (line.Y0 / 3.0 + line.Dy / 4.0)
                ) * line.Dy
                / 2.0;
        }

        /// <summary>
        /// Calcula a coordenada X do centroide: \( X_g = S_y / A \).
        /// </summary>
        /// <remarks>
        /// Se A for 0 (contorno degenerado), isso gera divisão por zero.
        /// </remarks>
        private void CalculateXg()
        {
            _Xg = _Sy / _A;
        }

        /// <summary>
        /// Calcula a coordenada Y do centroide: \( Y_g = S_x / A \).
        /// </summary>
        /// <remarks>
        /// Se A for 0 (contorno degenerado), isso gera divisão por zero.
        /// </remarks>
        private void CalculateYg()
        {
            _Yg = _Sx / _A;
        }

        /// <summary>
        /// Calcula o segundo momento centroidal Ixg pelo Teorema dos Eixos Paralelos:
        /// \( I_{xg} = I_x - A \cdot Y_g^2 \).
        /// </summary>
        private void CalculateIxg()
        {
            _Ixg = _Ix - Math.Pow(_Yg, 2) * _A;
        }

        /// <summary>
        /// Calcula o segundo momento centroidal Iyg pelo Teorema dos Eixos Paralelos:
        /// \( I_{yg} = I_y - A \cdot X_g^2 \).
        /// </summary>
        private void CalculateIyg()
        {
            _Iyg = _Iy - Math.Pow(_Xg, 2) * _A;
        }

        /// <summary>
        /// Calcula o produto de inércia centroidal:
        /// \( I_{xyg} = I_{xy} - A \cdot X_g \cdot Y_g \).
        /// </summary>
        private void CalculateIxyg()
        {
            _Ixyg = _Ixy - _Xg * _Yg * _A;
        }

        /// <summary>
        /// Calcula a distância do centroide até Ymin.
        /// </summary>
        private void CalculateY1()
        {
            _Y1 = Math.Abs(_Yg - Ymin);
        }

        /// <summary>
        /// Calcula a distância do centroide até Ymax.
        /// </summary>
        private void CalculateY2()
        {
            _Y2 = Math.Abs(Ymax - _Yg);
        }

        /// <summary>
        /// Calcula o módulo resistente inferior: W1 = Ixg / Y1.
        /// </summary>
        private void CalculateW1()
        {
            _W1 = _Ixg / _Y1;
        }

        /// <summary>
        /// Calcula o módulo resistente superior: W2 = Ixg / Y2.
        /// </summary>
        private void CalculateW2()
        {
            _W2 = _Ixg / _Y2;
        }

        /// <summary>
        /// Calcula a altura do envelope: |Ymax - Ymin|.
        /// </summary>
        private void CalculateHeight()
        {
            _height = Math.Abs(Ymax - Ymin);
        }

        /// <summary>
        /// Calcula a base (largura) do envelope: |Xmax - Xmin|.
        /// </summary>
        private void CalculateBase()
        {
            _base = Math.Abs(Xmax - Xmin);
        }

        /// <summary>
        /// Atualiza Xmax incrementalmente.
        /// </summary>
        private void CalculateXMax(double x)
        {
            if (!_Xmax.HasValue || x >= _Xmax.Value) _Xmax = x;
        }

        /// <summary>
        /// Atualiza Xmin incrementalmente.
        /// </summary>
        private void CalculateXMin(double x)
        {
            if (!_Xmin.HasValue || x <= _Xmin.Value) _Xmin = x;
        }

        /// <summary>
        /// Atualiza Ymax incrementalmente.
        /// </summary>
        private void CalculateYMax(double y)
        {
            if (!_Ymax.HasValue || y >= _Ymax.Value) _Ymax = y;
        }

        /// <summary>
        /// Atualiza Ymin incrementalmente.
        /// </summary>
        private void CalculateYMin(double y)
        {
            if (!_Ymin.HasValue || y <= _Ymin.Value) _Ymin = y;
        }

        /// <summary>
        /// Correção de sinal adotada para Y1 e W1.
        /// 
        /// Parece ser uma convenção interna para que:
        /// - Y1 represente a distância até a fibra inferior com sinal negativo,
        ///   enquanto Y2 permanece positiva.
        /// - W1 acompanhe o mesmo sinal.
        /// </summary>
        private void SumSignCorrection()
        {
            _Y1 *= -1;
            _W1 *= -1;
        }

        // ===== API pública (somente leitura) =====

        /// <summary>Área algébrica do contorno.</summary>
        public double A => _A;

        /// <summary>Momento estático em relação a X (na origem).</summary>
        public double Sx => _Sx;

        /// <summary>Momento estático em relação a Y (na origem).</summary>
        public double Sy => _Sy;

        /// <summary>Inércia em X (na origem).</summary>
        public double Ix => _Ix;

        /// <summary>Inércia em Y (na origem).</summary>
        public double Iy => _Iy;

        /// <summary>Produto de inércia (na origem).</summary>
        public double Ixy => _Ixy;

        /// <summary>X máximo do envelope do contorno.</summary>
        public double Xmax => _Xmax ?? throw new InvalidOperationException("Xmax not calculated.");

        /// <summary>X mínimo do envelope do contorno.</summary>
        public double Xmin => _Xmin ?? throw new InvalidOperationException("Xmin not calculated.");

        /// <summary>Y máximo do envelope do contorno.</summary>
        public double Ymax => _Ymax ?? throw new InvalidOperationException("Ymax not calculated.");

        /// <summary>Y mínimo do envelope do contorno.</summary>
        public double Ymin => _Ymin ?? throw new InvalidOperationException("Ymin not calculated.");

        /// <summary>Coordenada X do centroide.</summary>
        public double Xg => _Xg;

        /// <summary>Coordenada Y do centroide.</summary>
        public double Yg => _Yg;

        /// <summary>Inércia em X no centroide.</summary>
        public double Ixg => _Ixg;

        /// <summary>Inércia em Y no centroide.</summary>
        public double Iyg => _Iyg;

        /// <summary>Produto de inércia no centroide.</summary>
        public double Ixyg => _Ixyg;

        /// <summary>Distância (com convenção de sinal interna) do centroide até Ymin.</summary>
        public double Y1 => _Y1;

        /// <summary>Distância do centroide até Ymax.</summary>
        public double Y2 => _Y2;

        /// <summary>Módulo resistente associado a Y1.</summary>
        public double W1 => _W1;

        /// <summary>Módulo resistente associado a Y2.</summary>
        public double W2 => _W2;

        /// <summary>Altura do envelope do contorno: |Ymax - Ymin|.</summary>
        public double Height => _height;

        /// <summary>Base do envelope do contorno: |Xmax - Xmin|.</summary>
        public double Base => _base;
    }
}