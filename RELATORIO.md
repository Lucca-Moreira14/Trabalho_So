# Relatorio - Trabalho Pratico: Algoritmo do Banqueiro

## Integrantes

- Lucca Casarim Moreira - 877538
- Italo Henrique Beraldo - 877488

## Link do repositorio publico

- Inserir link: https://github.com/usuario/repositorio

## Introducao

Este trabalho apresenta uma implementacao multithread do Algoritmo do Banqueiro, utilizado em sistemas operacionais para evitar deadlocks por meio da verificacao de estado seguro antes da concessao de recursos.

O problema considera varios clientes concorrentes solicitando e liberando recursos de diferentes tipos. O objetivo principal e garantir que o sistema nunca entre em um estado inseguro.

## Desenvolvimento

A implementacao foi feita em C# com uso de threads da biblioteca `System.Threading`.

Foram utilizadas as estruturas classicas do algoritmo:

- `available`: quantidade disponivel de cada tipo de recurso.
- `maximum`: demanda maxima de cada cliente.
- `allocation`: quantidade atualmente alocada a cada cliente.
- `need`: necessidade restante de cada cliente (`maximum - allocation`).

As funcoes principais sao:

- `RequestResources(int customer, int[] request)`: valida e tenta conceder a requisicao; somente confirma se o estado continuar seguro.
- `ReleaseResources(int customer, int[] release)`: libera recursos previamente alocados ao cliente.
- `IsSafe()`: executa o algoritmo de seguranca para verificar se existe sequencia segura de execucao.

Para evitar condicoes de corrida, o acesso aos dados compartilhados foi protegido com `lock (mutex)` nas operacoes de requisicao e liberacao.

## Resultados

Nos testes realizados com diferentes entradas de recursos (exemplo: `10 5 7`), o programa:

- criou 5 threads de clientes em execucao concorrente;
- concedeu requisicoes quando o estado permaneceu seguro;
- negou requisicoes que poderiam levar a estado inseguro;
- manteve consistencia dos vetores e matrizes de controle durante a execucao.

Dessa forma, a simulacao demonstrou o comportamento esperado do Algoritmo do Banqueiro em ambiente concorrente.

## Conclusao

O trabalho permitiu aplicar conceitos importantes de Sistemas Operacionais: concorrencia, exclusao mutua e prevencao de deadlocks.

A verificacao de seguranca antes da alocacao mostrou-se essencial para manter o sistema em estado consistente e seguro. A organizacao do codigo e da documentacao no repositorio facilita compilacao, execucao e avaliacao da solucao.

