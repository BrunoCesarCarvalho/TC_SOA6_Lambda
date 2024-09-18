using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using Amazon.Lambda.APIGatewayEvents;
using System.Text.Json;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWSLambdaAuthenticatorClient;

public class Function
{
    private ServiceCollection _serviceCollection;

    public Function()
    {
        ConfigureServices();
    }


    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {

        request.PathParameters.TryGetValue("input", out var body);
        var clientDTO = new
        {
            Cpf = "99999999999",
            DataNascimento = "28/06/1988",
            Email = "ricardosn87@hotmail.com",
            Id = 19,
            Nome = "Ricardo Nogueira teste action 2",
            Texto = body
        };
        return new APIGatewayHttpApiV2ProxyResponse
        {
            Body = JsonSerializer.Serialize(clientDTO),
            StatusCode = 200
        };

      

        //if (string.IsNullOrEmpty(body))
        //{
        //    return new APIGatewayHttpApiV2ProxyResponse
        //    {
        //        Body = JsonSerializer.Serialize("CPF input is missing or empty."),
        //        StatusCode = 400,
        //    };
        //}

        //if (!ValidarCPF(body))
        //{
        //    return new APIGatewayHttpApiV2ProxyResponse
        //    {
        //        Body = JsonSerializer.Serialize("Provided CPF invalid."),
        //        StatusCode = 400,
        //    };
        //}


        //using (ServiceProvider serviceProvider = _serviceCollection.BuildServiceProvider())
        //{
        //    // entry to run app.
        //    var clienteUserCase = serviceProvider.GetService<IClienteUserCase>();
        //    var response = await clienteUserCase.GetByCpfAsync(body);


        //    if (response == null)
        //    {
        //        return new APIGatewayHttpApiV2ProxyResponse
        //        {
        //            Body = JsonSerializer.Serialize("NotFound: No data found for the provided CPF."),
        //            StatusCode = 404,
        //        }; ;
        //    }

        //    return new APIGatewayHttpApiV2ProxyResponse
        //    {
        //        Body = JsonSerializer.Serialize(response),
        //        StatusCode = 200,
        //    };
        //}
    }

    private void ConfigureServices()
    {
       
        //var connectionString = Environment.GetEnvironmentVariable("StringConnection");
        //var list = Environment.GetEnvironmentVariables();

        //_serviceCollection = new ServiceCollection();
        //_serviceCollection.AddDbContext<DataBaseContext>(options =>
        //       options.UseSqlServer("Data Source=127.0.0.1,32000;Initial Catalog=TechChallengeFIAP;User ID=sa;Password=YourStrong@Passw0rd;Integrated Security=False;Encrypt=False"));

        //_serviceCollection.AddScoped<IClienteUserCase, ClienteUserCase>();
        //_serviceCollection.AddScoped<IClienteRepository, ClienteRepository>();
    }

    public static bool ValidarCPF(string cpf)
    {
        // Remove caracteres especiais como pontos e traços
        cpf = Regex.Replace(cpf, @"[^\d]", "");

        // Verifica se o CPF tem 11 dígitos
        if (cpf.Length != 11)
            return false;

        // Verifica se todos os dígitos são iguais, o que seria um CPF inválido (ex: 111.111.111-11)
        if (cpf.Distinct().Count() == 1)
            return false;

        // Calcula o primeiro dígito verificador
        int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int soma = 0;
        for (int i = 0; i < 9; i++)
        {
            soma += int.Parse(cpf[i].ToString()) * multiplicador1[i];
        }
        int resto = soma % 11;
        int digito1 = resto < 2 ? 0 : 11 - resto;

        // Calcula o segundo dígito verificador
        int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        soma = 0;
        for (int i = 0; i < 10; i++)
        {
            soma += int.Parse(cpf[i].ToString()) * multiplicador2[i];
        }
        resto = soma % 11;
        int digito2 = resto < 2 ? 0 : 11 - resto;

        // Verifica se os dígitos calculados são iguais aos fornecidos no CPF
        return cpf.EndsWith($"{digito1}{digito2}");
    }
}
