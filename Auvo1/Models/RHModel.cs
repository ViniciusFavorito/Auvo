using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using System.Text.Json;

namespace Auvo1.Models;

public class DepartamentoModel
{
    public string Nome { get; set; } = string.Empty;
    public string MesVigencia { get; set; } = string.Empty;
    public string AnoVigencia { get; set; } = string.Empty;
    public decimal TotalPagar { get; set; }
    public decimal TotalDescontos { get; set; }
    public decimal TotalExtras { get; set; }
    public List<FuncionarioJsonModel> Funcionarios { get; set; } = new List<FuncionarioJsonModel>(); 
}
public class FuncionarioCsvModel
{
    public string Nome { get; set; } = string.Empty;
    public int Codigo { get; set; }
    public string ValorHora { get; set; } = string.Empty;
    public string Almoco { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public TimeSpan Entrada { get; set; }
    public TimeSpan Saida { get; set; }
}

public class FuncionarioJsonModel
{
    public string Nome { get; set; } = string.Empty;
    public int Codigo { get; set; }
    public decimal TotalReceber { get; set; }
    public decimal HorasExtras { get; set; }
    public decimal HorasDebito { get; set; }
    public int DiasFalta { get; set; }
    public int DiasExtras { get; set; }
    public int DiasTrabalhados { get; set; }
}

public class InfoGanhoDiarioModel
{
    public decimal ValorDiario { get; set; }
    public decimal ValorDiarioPadrão { get; set; }
    public int HorasExtras { get; set; }
}

public class FuncionarioMapModel : ClassMap<FuncionarioCsvModel>
{
    public FuncionarioMapModel()
    {
        Map(m => m.Codigo).Name("Código");
        Map(m => m.Nome).Name("Nome");
        Map(m => m.ValorHora).Name("Valor hora");
        Map(m => m.Data).Name("Data");
        Map(m => m.Entrada).Name("Entrada");
        Map(m => m.Saida).Name("Saída");
        Map(m => m.Almoco).Name("Almoço");
    }
}

