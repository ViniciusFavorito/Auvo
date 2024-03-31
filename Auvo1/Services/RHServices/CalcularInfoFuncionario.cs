using Auvo1.Models;
using System.Globalization;

namespace Auvo1.Services.RHServices;

public class CalcularInfoFuncionario
{
    public async Task<int> ObterHorasTrabalhadas(TimeSpan horaEntrada, TimeSpan horaSaida, string horaAlmoco)
    {
        return await Task.Run(() => {
            var arrayStringHoraAlmoco = horaAlmoco.Split('-');
            TimeSpan inicioAlmoco = TimeSpan.Parse(arrayStringHoraAlmoco[0]);
            TimeSpan fimAlmoco = TimeSpan.Parse(arrayStringHoraAlmoco[1]);

            var horasTrabalhadas = (horaSaida - horaEntrada) - (fimAlmoco - inicioAlmoco);

            return (int)horasTrabalhadas.TotalHours;
        });
    }

    public async Task<InfoGanhoDiarioModel> ObterGanhoDiario(int horasTrabalhadas, string stringValorHora)
    {
        var horasExtras = await ObterHorasExtras(horasTrabalhadas);

        return await Task.Run(() => {
            var arrayStringValorHora = stringValorHora.Split("R$");
            string valorHoraString = arrayStringValorHora[1].Trim().Replace(",", ".").Replace(" ", "");
            decimal valorHora;

            if (!decimal.TryParse(valorHoraString.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out valorHora))
            {
                // Se der algum erro...
                throw new ArgumentException("Valor inválido na conversão do Valor Hora");
            }

            decimal ganhoDiarioPadrao = valorHora * horasTrabalhadas;

            decimal ganhoDiario = ganhoDiarioPadrao + (horasExtras);

            var infoGanhoDiario = new InfoGanhoDiarioModel
            {
                ValorDiario = ganhoDiario,
                ValorDiarioPadrão = ganhoDiarioPadrao,
                HorasExtras = horasExtras,
            };

            return infoGanhoDiario;
        });
    }

    //Pode retornar tanto hora extra feita quanto hora faltante caso o valor seja negativo
    public async Task<int> ObterHorasExtras(int horasTrabalhadas)
    {
        return await Task.Run(() => {
            var horasExtras = horasTrabalhadas - 8;

            return horasExtras;
        });
    }
}
