# Algoritmo do Banqueiro (SO)

Implementação multithread do algoritmo do banqueiro (Banker's Algorithm), com 5 clientes e 3 tipos de recursos, usando C# e `Thread`.

## Integrante(s)

- Lucca Casarim Moreira - 877538
- Italo Henrique Beraldo - 877488

## Requisitos

- .NET SDK 8.0 ou superior

## Compilação

No diretório do projeto:

```bash
dotnet build
```

## Execução

Passe os recursos disponíveis na linha de comando (3 valores):

```bash
dotnet run -- 10 5 7
```

Exemplo:

- Recurso A: 10 instâncias
- Recurso B: 5 instâncias
- Recurso C: 7 instâncias

## Estruturas de dados

- `disponivel[m]`: recursos disponíveis
- `maximo[n,m]`: demanda máxima de cada cliente
- `alocacao[n,m]`: recursos alocados
- `necessidade[n,m]`: necessidade restante (`maximo - alocacao`)

Constantes usadas no código:

- `NUMERO_DE_CLIENTES = 5`
- `NUMERO_DE_RECURSOS = 3`

Funções principais:

- `SolicitarRecursos(int cliente, int[] solicitacao)`
- `LiberarRecursos(int cliente, int[] liberacao)`
- `EstadoSeguro()`

## Concorrência e sincronização

- Cada cliente executa em uma thread.
- As operações de requisição/liberação usam `lock (bloqueio)` para evitar condição de corrida.
- Uma requisição só é concedida se o sistema permanecer em estado seguro (`EstadoSeguro()`).

## Arquivo principal

- `Program.cs`

## Organização do repositório

- `Program.cs`: implementação do algoritmo do banqueiro.
- `README.md`: instruções de compilação e execução.


