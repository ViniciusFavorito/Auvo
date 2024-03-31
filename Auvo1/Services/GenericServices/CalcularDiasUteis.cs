using Auvo1.Models;
using System.Globalization;

namespace Auvo1.Services.GenericServices;

public class DiaUtilModel
{
    public DateTime Data { get; set; }
}

public class CalcularDiasUteis
{
    public List<DiaUtilModel> DiasUteis { get; set; } = new List<DiaUtilModel>();

    public async Task<List<DateTime>> Calcular(string mesVigente, string anoVigente)
    {
        return await Task.Run(() =>
        {
            List<DateTime> diasUteis = new List<DateTime>();

            mesVigente = new ConverterMesParaIngles().Converter(mesVigente);

            string dataString = $"01-{mesVigente}-{anoVigente}";

            DateTime primeiroDiaDoMes = DateTime.ParseExact(dataString, "dd-MMMM-yyyy", CultureInfo.InvariantCulture);

            DateTime ultimoDiaDoMes = primeiroDiaDoMes.AddMonths(1).AddDays(-1);

            for (DateTime dia = primeiroDiaDoMes; dia <= ultimoDiaDoMes; dia = dia.AddDays(1))
            {
                if (dia.DayOfWeek != DayOfWeek.Saturday && dia.DayOfWeek != DayOfWeek.Sunday)
                {
                    diasUteis.Add(dia);
                }
            }

            return diasUteis;
        });
    }
}
