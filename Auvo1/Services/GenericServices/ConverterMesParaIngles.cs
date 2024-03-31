namespace Auvo1.Services.GenericServices;

public class ConverterMesParaIngles
{
    public string Converter(string mes)
    {
        Dictionary<string, string> meses = new Dictionary<string, string>()
        {
            {"Janeiro", "January"},
            {"Fevereiro", "February"},
            {"Março", "March"},
            {"Abril", "April"},
            {"Maio", "May"},
            {"Junho", "June"},
            {"Julho", "July"},
            {"Agosto", "August"},
            {"Setembro", "September"},
            {"Outubro", "October"},
            {"Novembro", "November"},
            {"Dezembro", "December"}
        };

        if (meses.ContainsKey(mes))
        {
            return meses[mes];
        }
        else
        {
            return mes;
        }
    }
}
