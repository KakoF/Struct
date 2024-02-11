# Levantar estrutura e rodar o projeto

### Infrastrutura

O projeto fornece um arquivo composer com a infrastrutura, apenas é necessário ter docker instalado e executar o comando:

```sh
docker composer up -d
```
### API (atlas-metrics-configuration-api)

Todas variáveis de ambiente locais pra desenvolvimento estão servidas. Apenas rodar o projeto localmente

  
  

# Padrões (Sugestão, nada foi escrito em pedra)

  

### Cacheamento

Uma interface para os métodos similares entre memória e cache foram injetadas em ambas classes concretas. Qualquer nova feature de método deve ser implementada em ambas e todos os

métodos que já foram implementados apenas dependem de invocação. Então qualquer método/processo/resultado que será necessário ser cacheado, depende apenas de encapsular sua execução no cacheamento.

Segue exemplo fictício:

  
### Consumo de clients

Um abstração de client foi criado pra mitigar novas implementações de consumos simples dos verbos:

- GET
- POST
- PUT
- DELETE

  

Qualquer novo client que será necessário ser consumido, não precisa de implementações, apenas para casos de extrema singularidade ou especificação. Segue exemplo de um novo client que precisou ser feita integração:

  #### Program:
```csharp
services.AddScoped<IChuckClient, ChuckClient>();
services.AddHttpClient("ChuckClient", config =>
{
	config.BaseAddress = new Uri(configuration["Clients:Chuck:BasePath"]);
});
```
  
  
  #### Interface:
  ```csharp
public interface IChuckClient : IClientMethods
{
}
```
  
#### Classe Concreta:
 ```csharp
public class ChuckClient : ClientAbstract, IChuckClient

{

	protected override string _nameClient => "ChuckClient";
	public ChuckClient(IHttpClientFactory clientFactory, ILogger<ChuckClient> logger, INotificationHandler<Notification> notification) : base(clientFactory, logger, notification)
	{

	}
	protected override string ParseMessageError(string error)
	{
		return error;
	}
}
```
  
  
#### Service:
 ```csharp
public class ChuckService : IChuckService
{
	private readonly IChuckClient _client;
    public ChuckService(IChuckClient client)
    {
	    _client = client;
    }
    public async Task<ChuckNorrisModel> GetRandomJokeAsync()
    {
	    return await _client.GetAsync<ChuckNorrisModel>("jokes/random");
    }
}
```
Todo esse escopo garante os métodos para consumo da aplicação e requisitando o recurso

### Padrões de retorno de erro
Contamos também com 3 tipos de erros:
- FluentValidation
- Notification
- DomainException

#### FluentValidation
Fluent Validation segue a implementação de própria lib:
https://fluentvalidation.net/

Mas temos um tratamento de retorno, que seguirá nesse padrão:
```json
{
  "statusCode": 400,
  "message": "Erro em processar os dados de entrada da requisição",
  "errors": [
    "Nome deve ter ao menos 5 caracteres",
    "Descrição deve ter ao menos 5 caracteres"
  ]
}
```


#### Notification  
Segue um escopo de não lançar Exceptions na aplicação
```csharp
 _notification.AddNotification(404, "Regra não encontrada", $"Regra {id} não encontrada na base");
 ```
Ele segue um padrão onde é possível manipular o status code, message e uma lista de erros. Exemplo do retorno:
```json
{
  "StatusCode": 404,
  "Message": "Algo não encontrado",
  "Errors": [
    "Algo sobre id 213 não encontrado na base"
  ]
}
```

#### DomainException
É uma extensão de Execption que permite manipular o status code, segue exemplo:
```csharp
 throw new DomainException("Regra não encontrada", 404);
 ```
 Retorno:
 ```json
{
  "StatusCode": 404,
  "Message": "Algo não encontrado"
}
```

#### Corpo nos retornos de erro
Todos retornos acima levam um body com a mesma proposta, onde **sempre teremos status code e message**, e uma **possível lista de erros** 
Todo erro não tratato tem o retorno:
 ```json
{
  "StatusCode": 500,
  "Message": "Internal Server Error"
}
```
