using Auvo1.Models;
using Auvo1.Services.RHServices;
using Microsoft.AspNetCore.Mvc;


public class RHController : Controller
{
    private ProcessarCsv _processarCsv = new ProcessarCsv();
    private ExportarJson _exportarJson = new ExportarJson();

    public async Task<List<DepartamentoModel>> ProcessarArquivosCSV(string pasta)
    {
        var departamentos = await _processarCsv.ProcessarArquivosCSV(pasta);
        return departamentos;
    }

    public async Task<IActionResult> GerarSaidaJSON(List<DepartamentoModel> departamentos, string arquivoSaida)
    {
        await _exportarJson.Exportar(departamentos, arquivoSaida);
        return Ok();
    }
}
