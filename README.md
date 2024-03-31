<h1>Teste prático Auvo</h1>

Este projeto foi desenvolvido para processar arquivos CSV contendo informações sobre os funcionários de diferentes departamentos, calcular métricas relacionadas ao pagamento de salários e outras estatísticas, e gerar um arquivo de saída no formato JSON com os resultados calculados.

<h4>Tecnologias Utilizadas</h4>
Linguagem de Programação: C#
Framework: .NET (ASP.NET MVC)
Biblioteca para leitura de arquivos CSV: CsvHelper

<h4>Funcionalidades Implementadas</h4>
Leitura de Arquivos CSV: O sistema lê todos os arquivos CSV de uma pasta designada, contendo informações sobre os funcionários de diferentes departamentos.

Processamento dos Dados: Os dados dos arquivos CSV são processados para calcular métricas como o valor total a pagar a cada departamento, valor total de descontos, horas extras, dias extras, etc.

Geração de Saída em JSON: Com base nos cálculos realizados, o sistema gera um arquivo de saída no formato JSON contendo informações detalhadas sobre cada departamento e seus respectivos funcionários.

Cálculos Específicos: Foram implementados métodos para calcular total a receber de cada funcionário, horas extras, horas debitadas, dias de falta, dias extras e dias trabalhados.

<h4>Estrutura do Projeto</h4>
O projeto segue a arquitetura MVC (Model-View-Controller) com uma pasta adicional chamada "Services", que guarda as regras de negócio e os serviços executados no código.

Model: Responsável apenas por definir os modelos de dados, como FuncionarioModel e DepartamentoModel, que encapsulam as informações relevantes para o sistema.

View: Encarregada de apresentar os dados ao usuário por meio de páginas como Home e About.

Controller: Gerencia o fluxo de informações entre a interface do usuário e o modelo de dados. No RHController, as ações ProcessarArquivosCSV e GerarSaidaJSON coordenam o processamento dos arquivos CSV e a geração do arquivo de saída JSON, respectivamente. Essas ações fazem uso dos serviços definidos na pasta "Services" para realizar as operações específicas, mantendo a lógica de negócio separada das responsabilidades do controlador.

<h4>Fluxo de Funcionamento</h4>
O usuário fornece o caminho da pasta contendo os arquivos CSV.

O sistema lê todos os arquivos CSV na pasta especificada.

Os dados dos arquivos são processados para calcular métricas específicas.

As informações processadas são agregadas e organizadas de acordo com os departamentos e funcionários correspondentes.

Um arquivo JSON é gerado contendo os resultados calculados e estruturado conforme os requisitos especificados.

<h4>Execução do Projeto</h4>
Para executar o projeto:

Abra a solução no Visual Studio.

Compile e execute o projeto.

Acesse a página inicial do aplicativo e forneça o caminho da pasta contendo os arquivos CSV.

Aguarde o processamento dos arquivos e a geração do arquivo de saída JSON.
