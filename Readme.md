# UdpRule

#### UdpRule é uma maneira super simples de construir clientes e servidores usando o protocolo UDP em C#.

UdpRule envia qualquer tipo de objeto, se queres enviar uma class personalizado do cliente para servidor ou do servidor para o cliente, podes fazer com UdpRule, na pasta 'Test' tem alguns exemplos como podes utilizar como do lado do 'servidor' como do lado do 'cliente'

## Requerimentos

### .NET 6.0


## Classes
### ‼️ Servidor e o Cliente teem de ser do mesmo tipo ‼️
#### Exemplo

Aqui eu quero que o servidor e o cliente comuniquem com tipo 'int' 

Ficheiro1.cs
``` csharp
Server<int> server = new Server<int>;
```

Ficheiro2.cs
``` csharp
Client<int> client = new Client<int>;
```

## Handlers

Server.cs
``` csharp
// Quando o servidor recebe alguma mensagem do cliente
EventHandler<object>    DataReceivedEvent;
// Quando recebe algum dado do cliente
EventHandler<object>    DataSendEvent;
// Quando um cliente e adicionado a lista de clientes do servidor
EventHandler<object>    ClientConnectEvent;
// Quando um cliente demora a comunicar com o servidor
EventHandler<object>    ClientDisconectEvent;
// Qualquer erro que pode ocorrer durante o seu processo
EventHandler<Exception> ExceptionEvent;           
```

Client.cs
``` csharp
// Quando envia um dado para o servidor
EventHandler<object>    DataReceivedEvent; 
// Quando recebe algum dado do servidor
EventHandler<object>    DataSendEvent;
// Quando o cliente envia algum dado para o servidor mas o servidor esta offline
EventHandler            ServerDownEvent;
// Qualquer erro que pode ocorrer durante o seu processo
EventHandler<Exception> ExceptionEvent;
```

## Construtores

Server.cs
``` csharp
// Porta do servidor
new Server<object>(int port);
```

Client.cs
``` csharp
new Client<object>();
```

## Propriedades

Server.cs
``` csharp
server.Start();
```

Client.cs
``` csharp
 // IP e Porta do servidor
client.Connect(string ip, int port);
 // Object tem que ser do tipo que foi iniciado no construtor da class
client.SendData(object anyClass);
```
