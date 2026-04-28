using System;
using System.Threading;

class Banker
{
    private const int NUMBER_OF_CUSTOMERS = 5;
    private static int NUMBER_OF_RESOURCES;

    private static int[] available;
    private static int[,] maximum;
    private static int[,] allocation;
    private static int[,] need;

    private static Mutex mutex = new Mutex();
    private static Random random = new Random();

    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Uso: dotnet run <rec1> <rec2> <rec3> ...");
            return;
        }

        NUMBER_OF_RESOURCES = args.Length;

        available = new int[NUMBER_OF_RESOURCES];

        for (int i = 0; i < NUMBER_OF_RESOURCES; i++)
        {
            available[i] = int.Parse(args[i]);
        }

        maximum = new int[NUMBER_OF_CUSTOMERS, NUMBER_OF_RESOURCES];
        allocation = new int[NUMBER_OF_CUSTOMERS, NUMBER_OF_RESOURCES];
        need = new int[NUMBER_OF_CUSTOMERS, NUMBER_OF_RESOURCES];

        InicializarClientes();

        Thread[] clientes = new Thread[NUMBER_OF_CUSTOMERS];

        for (int i = 0; i < NUMBER_OF_CUSTOMERS; i++)
        {
            int id = i;
            clientes[i] = new Thread(() => Cliente(id));
            clientes[i].Start();
        }

        foreach (Thread t in clientes)
        {
            t.Join();
        }
    }

    static void InicializarClientes()
    {
        Console.WriteLine("=== MATRIZ MAXIMUM ===");

        for (int i = 0; i < NUMBER_OF_CUSTOMERS; i++)
        {
            for (int j = 0; j < NUMBER_OF_RESOURCES; j++)
            {
                maximum[i, j] = random.Next(1, available[j] + 1);
                need[i, j] = maximum[i, j];

                Console.Write(maximum[i, j] + " ");
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }

    static void Cliente(int customerNum)
    {
        for (int k = 0; k < 5; k++)
        {
            Thread.Sleep(random.Next(500, 1500));

            int[] request = new int[NUMBER_OF_RESOURCES];

            for (int i = 0; i < NUMBER_OF_RESOURCES; i++)
            {
                request[i] = random.Next(need[customerNum, i] + 1);
            }

            Console.WriteLine($"\nCliente {customerNum} solicitando recursos...");

            MostrarVetor("Request", request);

            if (RequestResources(customerNum, request) == 0)
            {
                Console.WriteLine($"Solicitação do cliente {customerNum} APROVADA.");

                Thread.Sleep(random.Next(500, 1500));

                int[] release = new int[NUMBER_OF_RESOURCES];

                for (int i = 0; i < NUMBER_OF_RESOURCES; i++)
                {
                    release[i] = allocation[customerNum, i];
                }

                ReleaseResources(customerNum, release);

                Console.WriteLine($"Cliente {customerNum} liberou recursos.");
            }
            else
            {
                Console.WriteLine($"Solicitação do cliente {customerNum} NEGADA.");
            }
        }
    }

    static int RequestResources(int customerNum, int[] request)
    {
        mutex.WaitOne();

        try
        {
            for (int i = 0; i < NUMBER_OF_RESOURCES; i++)
            {
                if (request[i] > need[customerNum, i])
                {
                    return -1;
                }

                if (request[i] > available[i])
                {
                    return -1;
                }
            }

            // Simula alocação
            for (int i = 0; i < NUMBER_OF_RESOURCES; i++)
            {
                available[i] -= request[i];
                allocation[customerNum, i] += request[i];
                need[customerNum, i] -= request[i];
            }

            if (IsSafe())
            {
                MostrarEstado();
                return 0;
            }
            else
            {
                // Desfaz alocação
                for (int i = 0; i < NUMBER_OF_RESOURCES; i++)
                {
                    available[i] += request[i];
                    allocation[customerNum, i] -= request[i];
                    need[customerNum, i] += request[i];
                }

                return -1;
            }
        }
        finally
        {
            mutex.ReleaseMutex();
        }
    }

    static int ReleaseResources(int customerNum, int[] release)
    {
        mutex.WaitOne();

        try
        {
            for (int i = 0; i < NUMBER_OF_RESOURCES; i++)
            {
                available[i] += release[i];
                allocation[customerNum, i] -= release[i];
                need[customerNum, i] += release[i];
            }

            MostrarEstado();

            return 0;
        }
        finally
        {
            mutex.ReleaseMutex();
        }
    }

    static bool IsSafe()
    {
        int[] work = new int[NUMBER_OF_RESOURCES];
        bool[] finish = new bool[NUMBER_OF_CUSTOMERS];

        for (int i = 0; i < NUMBER_OF_RESOURCES; i++)
        {
            work[i] = available[i];
        }

        bool encontrou;

        do
        {
            encontrou = false;

            for (int i = 0; i < NUMBER_OF_CUSTOMERS; i++)
            {
                if (!finish[i])
                {
                    bool possivel = true;

                    for (int j = 0; j < NUMBER_OF_RESOURCES; j++)
                    {
                        if (need[i, j] > work[j])
                        {
                            possivel = false;
                            break;
                        }
                    }

                    if (possivel)
                    {
                        for (int j = 0; j < NUMBER_OF_RESOURCES; j++)
                        {
                            work[j] += allocation[i, j];
                        }

                        finish[i] = true;
                        encontrou = true;
                    }
                }
            }

        } while (encontrou);

        for (int i = 0; i < NUMBER_OF_CUSTOMERS; i++)
        {
            if (!finish[i])
            {
                return false;
            }
        }

        return true;
    }

    static void MostrarEstado()
    {
        Console.WriteLine("\n========== ESTADO ATUAL ==========");

        Console.Write("\nAvailable: ");

        for (int i = 0; i < NUMBER_OF_RESOURCES; i++)
        {
            Console.Write(available[i] + " ");
        }

        Console.WriteLine("\n\nAllocation:");

        for (int i = 0; i < NUMBER_OF_CUSTOMERS; i++)
        {
            for (int j = 0; j < NUMBER_OF_RESOURCES; j++)
            {
                Console.Write(allocation[i, j] + " ");
            }

            Console.WriteLine();
        }

        Console.WriteLine("\nNeed:");

        for (int i = 0; i < NUMBER_OF_CUSTOMERS; i++)
        {
            for (int j = 0; j < NUMBER_OF_RESOURCES; j++)
            {
                Console.Write(need[i, j] + " ");
            }

            Console.WriteLine();
        }

        Console.WriteLine("\n==================================\n");
    }

    static void MostrarVetor(string nome, int[] vetor)
    {
        Console.Write(nome + ": ");

        for (int i = 0; i < vetor.Length; i++)
        {
            Console.Write(vetor[i] + " ");
        }

        Console.WriteLine();
    }
}
