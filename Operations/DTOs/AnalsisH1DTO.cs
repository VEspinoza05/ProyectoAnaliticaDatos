public class AnalisisH1ResultDto
{
    public double CoeficienteModulo { get; set; }
    public double CoeficienteDias { get; set; }
    public double Intercepto { get; set; }
    public string Conclusion { get; set; }
    
    // Gráfico 1: Curva Sigmoide (Puntos ordenados X, Y)
    public List<SigmoidPointDto> CurvaSigmoide { get; set; } = new List<SigmoidPointDto>();
    
    // Gráfico 2: Barras Apiladas 100% (Estructura de conteos de negocio)
    public BarrasApiladasDto BarrasApiladas100 { get; set; }
}

public class SigmoidPointDto
{
    public double Dia { get; set; }         // Eje X (1 al 7)
    public double Probabilidad { get; set; } // Eje Y (0.0 a 1.0)
}

public class BarrasApiladasDto
{
    // Grupo: Completó módulo en Semana 1 (SÍ)
    public int SiCompletó_Exito { get; set; }
    public int SiCompletó_Abandono { get; set; }

    // Grupo: No completó módulo en Semana 1 (NO)
    public int NoCompletó_Exito { get; set; }
    public int NoCompletó_Abandono { get; set; }
}