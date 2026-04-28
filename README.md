# Algoritmo do Banqueiro em C#

## Descrição

Este projeto implementa o Algoritmo do Banqueiro utilizando programação concorrente em C#.

O sistema simula múltiplos clientes solicitando e liberando recursos de forma concorrente, garantindo que o sistema permaneça em estado seguro e evitando deadlocks.

---

# Objetivos

- Implementar o Algoritmo do Banqueiro;
- Utilizar Threads;
- Utilizar Mutex para exclusão mútua;
- Evitar condições de corrida;
- Evitar deadlocks;
- Simular gerenciamento de recursos em sistemas operacionais.

---

# Tecnologias Utilizadas

- C#
- .NET 6+
- Threads
- Mutex

---

# Estruturas Utilizadas

O algoritmo utiliza as seguintes estruturas:

## Available
Quantidade disponível de cada recurso.

## Maximum
Demanda máxima de recursos de cada cliente.

## Allocation
Quantidade de recursos atualmente alocados.

## Need
Quantidade de recursos que cada cliente ainda necessita.

A relação entre as estruturas é:

```text
Need = Maximum - Allocation
```

---

# Funcionalidades

- Solicitação de recursos;
- Liberação de recursos;
- Verificação de estado seguro;
- Controle de concorrência;
- Prevenção de deadlock;
- Execução multithreaded.

---

# Como Executar

## 1. Clone o repositório

---

## 2. Entre na pasta do projeto

---

## 3. Execute o projeto

```bash
dotnet run 10 5 7
```

Onde:

- 10 = quantidade do recurso 1
- 5 = quantidade do recurso 2
- 7 = quantidade do recurso 3

---

# Exemplo de Execução

```text
=== MATRIZ MAXIMUM ===
7 5 3
3 2 2
9 0 2
2 2 2
4 3 3

Cliente 1 solicitando recursos...
Solicitação do cliente 1 APROVADA.

========== ESTADO ATUAL ==========
Available: 7 3 5
```

---

# Estrutura do Projeto

```text
BankerAlgorithm/
│
├── Program.cs
├── README.md
├── Trapalho Pratico 1.csproj
```

---

# Compilação

Caso deseje apenas compilar:

```bash
dotnet build
```

---

# Conceitos Aplicados

- Sistemas Operacionais
- Threads
- Mutex
- Concorrência
- Deadlock
- Algoritmo do Banqueiro

---

# Grupo: 

Miguel de Paula Goncalves e Thiago Grandim das Merces

---

# Referências

SILBERSCHATZ, Abraham; GALVIN, Peter B.; GAGNE, Greg.

Fundamentos de Sistemas Operacionais. 9. ed.
Rio de Janeiro: LTC, 2015.
