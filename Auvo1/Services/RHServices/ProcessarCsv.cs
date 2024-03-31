using Auvo1.Models;
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using Auvo1.Services.GenericServices;

namespace Auvo1.Services.RHServices;

public class ProcessarCsv
{
    public async Task<List<DepartamentoModel>> ProcessarArquivosCSV(string pasta)
    {
        List<DepartamentoModel> departamentos = new List<DepartamentoModel>();

        try
        {
            string[] arquivosCSV = Directory.GetFiles(pasta, "*.csv");

            var tasks = arquivosCSV.Select(arquivo => Task.Run(() => ProcessarArquivoCSVAsync(arquivo, departamentos))).ToList();

            await Task.WhenAll(tasks);

        }
        catch (Exception ex)
        {
            Console.WriteLine("Ocorreu um erro ao processar os arquivos CSV: " + ex.Message);
        }

        return departamentos;
    }


    public async Task ProcessarArquivoCSVAsync(string arquivo, List<DepartamentoModel> departamentos)
    {
        try {

            // Extrair informações do nome do arquivo
            string[] partesNomeArquivo = Path.GetFileNameWithoutExtension(arquivo).Split('-');
            if (partesNomeArquivo.Length < 3)
            {
                Console.WriteLine($"O nome do arquivo \"{arquivo}\" não está no formato esperado.");
                throw new ArgumentException($"O nome do arquivo \"{arquivo}\" não está no formato esperado.");
            }

            string nomeDepartamento = partesNomeArquivo[0];
            string mesVigencia = partesNomeArquivo[1];
            string anoVigencia = partesNomeArquivo[2];

            // Verificar se o departamento já existe na lista de departamentos
            var departamento = departamentos.FirstOrDefault(d => d.Nome == nomeDepartamento);
            if (departamento == null)
            {
                departamento = new DepartamentoModel { Nome = nomeDepartamento, MesVigencia = mesVigencia, AnoVigencia = anoVigencia };
            }

            // Ler o arquivo CSV e associar os funcionários ao departamento
            using (var reader = new StreamReader(arquivo))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.Context.RegisterClassMap<FuncionarioMap>(); 

                Dictionary<int, List<FuncionarioCsv>> funcionariosInfo = new Dictionary<int, List<FuncionarioCsv>>();
                decimal departamentoTotalPagar = 0;
                decimal departamentoTotalDescontos = 0;
                decimal departamentoTotalExtras = 0;

                while (csv.Read())
                {
                    var funcionarioInfoCsv = csv.GetRecord<FuncionarioCsv>();

                    if (!funcionariosInfo.ContainsKey(funcionarioInfoCsv.Codigo))
                    {
                        funcionariosInfo.Add(funcionarioInfoCsv.Codigo, new List<FuncionarioCsv>());
                    }

                    funcionariosInfo[funcionarioInfoCsv.Codigo].Add(funcionarioInfoCsv);

                }
                foreach (var funcionario in funcionariosInfo)
                {
                    var qtdRegistros = funcionario.Value.Count;
                    var funcionarioNome = string.Empty;
                    var funcionarioCodigo = 0;
                    decimal totalReceber = 0;
                    decimal valorDiarioPadrao = 0;
                    var horasExtras = 0;
                    var horasDebito = 0;
                    int diasTrabalhado = 0;
                    var horasTrabalhadas = 0;
                    decimal totalReceberDia = 0;
                    InfoGanhoDiarioModel infoGanhoDiario;
                    List<DateTime> diasTrabalhados = new List<DateTime>();
                    List<DateTime> diasUteis = await new CalcularDiasUteis().Calcular(mesVigencia, anoVigencia);

                    for (int i = 0; i < qtdRegistros; i++)
                    {
                        funcionarioNome = funcionario.Value[i].Nome;
                        funcionarioCodigo = funcionario.Value[i].Codigo;

                        horasTrabalhadas = await new CalcularInfoFuncionario().ObterHorasTrabalhadas(funcionario.Value[i].Entrada, funcionario.Value[i].Saida, funcionario.Value[i].Almoco);

                        infoGanhoDiario = await new CalcularInfoFuncionario().ObterGanhoDiario(horasTrabalhadas, funcionario.Value[i].ValorHora);

                        if (infoGanhoDiario.HorasExtras < 0)
                        {
                            horasDebito += infoGanhoDiario.HorasExtras;
                        }
                        else
                        {
                            horasExtras += infoGanhoDiario.HorasExtras;
                        }

                        //Monta uma lista com os dias trabalhados para ser comparada e identificar os dias extras e/o faltantes
                        diasTrabalhados.Add(funcionario.Value[i].Data);

                        //Aqui já está descontando ou adicionando os valores de hora extra ou desconto
                        totalReceberDia = infoGanhoDiario.ValorDiario;

                        //A cada dia trabalhado soma o valor recebido aqui
                        totalReceber += totalReceberDia;

                        //Armazena/Conta os dias trabalhados
                        diasTrabalhado++;

                        //Somente armazena em uma variável o valor da diária padrão 
                        valorDiarioPadrao = infoGanhoDiario.ValorDiarioPadrão;
                    }

                    //Verifica diferença dos dias trabalhados com as faltas
                    List<DateTime> diasExtrasTrabalhados = diasTrabalhados.Except(diasUteis).ToList();
                    List<DateTime> diasFaltados = diasUteis.Except(diasTrabalhados).ToList();
                    decimal valorExtra = 0;
                    decimal valorDebitado = 0;

                    if (diasExtrasTrabalhados.Count > 0)
                    {
                        valorExtra = diasExtrasTrabalhados.Count * valorDiarioPadrao;
                        totalReceber += valorExtra;
                    }

                    if (diasFaltados.Count > 0)
                    {
                        valorDebitado = diasFaltados.Count * valorDiarioPadrao;
                        totalReceber -= valorDebitado;
                    }

                    var funcionarioJson = new FuncionarioJson
                    {
                        Nome = funcionarioNome,
                        Codigo = funcionarioCodigo,
                        TotalReceber = Math.Round(totalReceber, 2),
                        HorasExtras = horasExtras,
                        HorasDebito = horasDebito,
                        DiasFalta = diasFaltados.Count,
                        DiasExtras = diasExtrasTrabalhados.Count,
                        DiasTrabalhados = diasTrabalhado
                    };

                    departamentoTotalPagar += totalReceber;
                    departamentoTotalDescontos += valorDebitado;
                    departamentoTotalExtras += valorExtra;

                    departamento.Funcionarios.Add(funcionarioJson);

                }
                departamento.TotalPagar = Math.Round(departamentoTotalPagar, 2);
                departamento.TotalDescontos = Math.Round(departamentoTotalDescontos, 2); ;
                departamento.TotalExtras = Math.Round(departamentoTotalExtras, 2); ;

                departamentos.Add(departamento);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine("Ocorreu um erro ao processar os arquivos CSV: " + ex.Message);
        }
    }
}
