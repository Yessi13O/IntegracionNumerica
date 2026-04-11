namespace IntegracionNumerica
{
    class Program
    {
        // ── Método 1: Punto Medio ──────────────────────────────────────────────
        static double PuntoMedio(Func<double, double> f, double a, double b, int n)
        {
            double h = (b - a) / n;
            double suma = 0;
            for (int i = 0; i < n; i++)
                suma += f(a + (i + 0.5) * h);
            return suma * h;
        }

        // ── Método 2: Trapecio ─────────────────────────────────────────────────
        static double Trapecio(Func<double, double> f, double a, double b, int n)
        {
            double h = (b - a) / n;
            double suma = f(a) + f(b);
            for (int i = 1; i < n; i++)
                suma += 2 * f(a + i * h);
            return suma * h / 2;
        }

        // ── Método 3: Simpson 1/3 ──────────────────────────────────────────────
        static double Simpson(Func<double, double> f, double a, double b, int n)
        {
            if (n % 2 != 0) n++; // Simpson requiere n par
            double h = (b - a) / n;
            double suma = f(a) + f(b);
            for (int i = 1; i < n; i++)
                suma += (i % 2 == 0 ? 2 : 4) * f(a + i * h);
            return suma * h / 3;
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║         INTEGRACIÓN NUMÉRICA — ITM                              ║");
            Console.WriteLine("║   Punto Medio  |  Trapecio  |  Simpson 1/3                     ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            // ── Definición de los 4 casos ──────────────────────────────────────
            var casos = new (string Nombre, Func<double, double> F, double A, double B, int N)[]
            {
                ("∫₂⁴ dx/ln(x)",          x => 1.0 / Math.Log(x),                   2, 4, 26 ),
                ("∫₁² ln(x)dx/(x+2)",     x => Math.Log(x) / (x + 2),               1, 2, 40 ),
                ("∫₀¹ dx/√(1+x³)",        x => 1.0 / Math.Sqrt(1 + x * x * x),      0, 1, 68 ),
                ("∫₀¹ e^(-√x) dx",        x => Math.Exp(-Math.Sqrt(x)),              0, 1, 100),
            };

            // ── Encabezado de la tabla ─────────────────────────────────────────
            string sep = new string('─', 78);
            Console.WriteLine(sep);
            Console.WriteLine($"{"Integral",-22} {"n",5}  {"Punto Medio",16}  {"Trapecio",16}  {"Simpson 1/3",16}");
            Console.WriteLine(sep);

            foreach (var c in casos)
            {
                double pm = PuntoMedio(c.F, c.A, c.B, c.N);
                double tr = Trapecio(c.F, c.A, c.B, c.N);
                double si = Simpson(c.F, c.A, c.B, c.N);

                Console.WriteLine($"{c.Nombre,-22} {c.N,5}  {pm,16:F10}  {tr,16:F10}  {si,16:F10}");
            }

            Console.WriteLine(sep);
            Console.WriteLine();

            // ── Tabla de diferencias (precisión y convergencia) ────────────────
            Console.WriteLine("DIFERENCIAS ENTRE MÉTODOS (|A - B|)");
            Console.WriteLine(sep);
            Console.WriteLine($"{"Integral",-22} {"n",5}  {"|Simp-Trap|",16}  {"|Simp-Medio|",16}  {"|Trap-Medio|",16}");
            Console.WriteLine(sep);

            foreach (var c in casos)
            {
                double pm = PuntoMedio(c.F, c.A, c.B, c.N);
                double tr = Trapecio(c.F, c.A, c.B, c.N);
                double si = Simpson(c.F, c.A, c.B, c.N);

                Console.WriteLine($"{c.Nombre,-22} {c.N,5}  {Math.Abs(si - tr),16:E4}  {Math.Abs(si - pm),16:E4}  {Math.Abs(tr - pm),16:E4}");
            }

            Console.WriteLine(sep);
            Console.WriteLine();
            Console.WriteLine("Nota: Simpson 1/3 es el más preciso (error O(h⁴) vs O(h²) de los otros dos).");
            Console.WriteLine("      Cuando |Simpson - Trapecio| → 0, el resultado numérico se estabilizó.");
            Console.WriteLine();
            Console.Write("Presiona cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}
