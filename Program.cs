using System;
using System.Threading;

class Banqueiro
{
    const int NUMERO_DE_CLIENTES = 5;
    const int NUMERO_DE_RECURSOS = 3;

    static int[] disponivel = new int[NUMERO_DE_RECURSOS];
    static int[,] maximo = new int[NUMERO_DE_CLIENTES, NUMERO_DE_RECURSOS];
    static int[,] alocacao = new int[NUMERO_DE_CLIENTES, NUMERO_DE_RECURSOS];
    static int[,] necessidade = new int[NUMERO_DE_CLIENTES, NUMERO_DE_RECURSOS];

    static readonly object bloqueio = new object();

    static readonly ThreadLocal<Random> aleatorioThread = new ThreadLocal<Random>(
        () => new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))
    );

    static bool EstadoSeguro()
    {
        int[] trabalho = new int[NUMERO_DE_RECURSOS];
        bool[] finalizado = new bool[NUMERO_DE_CLIENTES];

        for (int i = 0; i < NUMERO_DE_RECURSOS; i++)
            trabalho[i] = disponivel[i];

        int contador = 0;

        while (contador < NUMERO_DE_CLIENTES)
        {
            bool encontrado = false;

            for (int i = 0; i < NUMERO_DE_CLIENTES; i++)
            {
                if (!finalizado[i])
                {
                    int j;
                    for (j = 0; j < NUMERO_DE_RECURSOS; j++)
                    {
                        if (necessidade[i, j] > trabalho[j])
                            break;
                    }

                    if (j == NUMERO_DE_RECURSOS)
                    {
                        for (int k = 0; k < NUMERO_DE_RECURSOS; k++)
                            trabalho[k] += alocacao[i, k];

                        finalizado[i] = true;
                        encontrado = true;
                        contador++;
                    }
                }
            }

            if (!encontrado)
                return false;
        }

        return true;
    }

    static int SolicitarRecursos(int cliente, int[] solicitacao)
    {
        lock (bloqueio)
        {
            for (int i = 0; i < NUMERO_DE_RECURSOS; i++)
            {
                if (solicitacao[i] < 0 || solicitacao[i] > necessidade[cliente, i] || solicitacao[i] > disponivel[i])
                    return -1;
            }

            for (int i = 0; i < NUMERO_DE_RECURSOS; i++)
            {
                disponivel[i]           -= solicitacao[i];
                alocacao[cliente, i]    += solicitacao[i];
                necessidade[cliente, i] -= solicitacao[i];
            }

            if (!EstadoSeguro())
            {
                for (int i = 0; i < NUMERO_DE_RECURSOS; i++)
                {
                    disponivel[i]           += solicitacao[i];
                    alocacao[cliente, i]    -= solicitacao[i];
                    necessidade[cliente, i] += solicitacao[i];
                }

                return -1;
            }

            return 0;
        }
    }

    static int LiberarRecursos(int cliente, int[] liberacao)
    {
        lock (bloqueio)
        {
            for (int i = 0; i < NUMERO_DE_RECURSOS; i++)
            {
                if (liberacao[i] < 0 || liberacao[i] > alocacao[cliente, i])
                    return -1;
            }

            for (int i = 0; i < NUMERO_DE_RECURSOS; i++)
            {
                disponivel[i]           += liberacao[i];
                alocacao[cliente, i]    -= liberacao[i];
                necessidade[cliente, i] += liberacao[i];
            }

            return 0;
        }
    }

    static void Cliente(object? obj)
    {
        int id = (int)obj!;

        while (true)
        {
            int[] solicitacao = new int[NUMERO_DE_RECURSOS];
            lock (bloqueio)
            {
                for (int i = 0; i < NUMERO_DE_RECURSOS; i++)
                    solicitacao[i] = aleatorioThread.Value!.Next(necessidade[id, i] + 1);
            }

            if (SolicitarRecursos(id, solicitacao) == 0)
                Console.WriteLine($"Cliente {id} conseguiu recursos: [{string.Join(", ", solicitacao)}]");
            else
                Console.WriteLine($"Cliente {id} teve solicitação negada: [{string.Join(", ", solicitacao)}]");

            Thread.Sleep(1000);

            int[] liberacao = new int[NUMERO_DE_RECURSOS];
            lock (bloqueio)
            {
                for (int i = 0; i < NUMERO_DE_RECURSOS; i++)
                    liberacao[i] = aleatorioThread.Value!.Next(alocacao[id, i] + 1);
            }

            if (LiberarRecursos(id, liberacao) == 0)
                Console.WriteLine($"Cliente {id} liberou recursos:    [{string.Join(", ", liberacao)}]");

            Thread.Sleep(1000);
        }
    }

    static void Main(string[] args)
    {
        if (args.Length != NUMERO_DE_RECURSOS)
        {
            Console.WriteLine($"Uso: dotnet run <r1> <r2> <r3>  (ex: dotnet run 10 5 7)");
            return;
        }

        for (int i = 0; i < NUMERO_DE_RECURSOS; i++)
        {
            if (!int.TryParse(args[i], out disponivel[i]) || disponivel[i] < 0)
            {
                Console.WriteLine("Erro: cada recurso deve ser um inteiro >= 0.");
                return;
            }
        }

        var aleatorioInicial = new Random();
        for (int i = 0; i < NUMERO_DE_CLIENTES; i++)
        {
            for (int j = 0; j < NUMERO_DE_RECURSOS; j++)
            {
                maximo[i, j]      = aleatorioInicial.Next(disponivel[j] + 1);
                alocacao[i, j]    = 0;
                necessidade[i, j] = maximo[i, j];
            }
        }

        Console.WriteLine("=== Algoritmo do Banqueiro ===");
        Console.WriteLine($"Recursos disponíveis: [{string.Join(", ", disponivel)}]");
        Console.WriteLine("Iniciando threads de clientes...\n");

        Thread[] threadsClientes = new Thread[NUMERO_DE_CLIENTES];

        for (int i = 0; i < NUMERO_DE_CLIENTES; i++)
        {
            threadsClientes[i] = new Thread(Cliente);
            threadsClientes[i].IsBackground = true;
            threadsClientes[i].Start(i);
        }

        foreach (var t in threadsClientes)
            t.Join();
    }
}