using Auvo1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Auvo1.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    public async Task<ActionResult> ProcessarArquivos(string pasta)
    {
        RHController rhController = new RHController();
        var departamentos = await rhController.ProcessarArquivosCSV(pasta);

        string arquivoSaida = ("C:\\Arquivo a Importar\\Saida.json"); // Caminho e nome do arquivo de saída
        await rhController.GerarSaidaJSON(departamentos, arquivoSaida);

        //Redireciona para a página de resultados onde no decorrer dos meus testes ficava mais fácil eu ver quais arquivos foram processados, então resolvi deixar.
        return View("Resultado", departamentos);
    }

    

}
