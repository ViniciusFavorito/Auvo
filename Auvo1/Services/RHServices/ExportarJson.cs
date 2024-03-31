using Auvo1.Models;
using System.Text.Json;

namespace Auvo1.Services.RHServices;

public class ExportarJson
{
    public async Task Exportar(List<DepartamentoModel> departamentos, string arquivoSaida)
    {
        try
        {
            // Serializar os departamentos para JSON
            string json = JsonSerializer.Serialize(departamentos, new JsonSerializerOptions
            {
                WriteIndented = true 
            });

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
