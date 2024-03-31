using Auvo1.Models;
using System.Text.Json;

namespace Auvo1.Services.RHServices;

public class ExportarJson
{
    public async Task Exportar(List<DepartamentoModel> departamentos, string arquivoSaida)
    {
        try
        {
            // Configurar as opções de serialização
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            // Serializar os departamentos para JSON
            string json = JsonSerializer.Serialize(departamentos, options);

            // Escrever o JSON no arquivo de saída
            await File.WriteAllTextAsync(arquivoSaida, json);

            Console.WriteLine("Saída JSON gerada com sucesso em: " + arquivoSaida);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ocorreu um erro ao gerar a saída JSON: " + ex.Message);
        }
    }

}
