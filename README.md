# Algoritmo do Banqueiro (SO)

Implementacao multithread do algoritmo do banqueiro (Banker's Algorithm), com 5 clientes e 3 tipos de recursos, usando C# e `Thread`.

## Integrante(s)

- Nome 1 - Lucca Casarim Moreira - 877538
- Nome 2 - Italo Henrique Beraldo -- 877488

## Requisitos

- .NET SDK 8.0 ou superior

## Compilacao

No diretorio do projeto:

```bash
dotnet build
```

## Execucao

Passe os recursos disponiveis na linha de comando (3 valores):

```bash
dotnet run -- 10 5 7
```

Exemplo:

- Recurso A: 10 instancias
- Recurso B: 5 instancias
- Recurso C: 7 instancias

## Estruturas de dados

- `available[m]`: recursos disponiveis
- `maximum[n,m]`: demanda maxima de cada cliente
- `allocation[n,m]`: recursos alocados
- `need[n,m]`: necessidade restante (`maximum - allocation`)

## Concorrencia e sincronizacao

- Cada cliente executa em uma thread.
- As operacoes de requisicao/liberacao usam `lock (mutex)` para evitar condicao de corrida.
- Uma requisicao so e concedida se o sistema permanecer em estado seguro (`IsSafe()`).

## Arquivo principal

- `Program.cs`

## Organizacao do repositorio

- `Program.cs`: implementacao do algoritmo do banqueiro.
- `README.md`: instrucoes de compilacao e execucao.
- `RELATORIO.md`: modelo base para gerar o relatorio em PDF.

## Checklist de entrega

- Repositorio publico no GitHub com codigo e README.
- Relatorio em PDF com: introducao, desenvolvimento, resultados e conclusao.
- Link do repositorio incluido no relatorio.
- Em caso de dupla, ambos devem enviar no Canvas.
