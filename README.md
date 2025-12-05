# Blazor Tarefas

Aplicação de exemplo em Blazor (NET 9) para gerenciar e visualizar tarefas.

## Projetos
- `BlazorTarefas` (Server/Host): aplicação Blazor que hospeda componentes e páginas.
- `BlazorTarefas.Client` (Client): componentes/páginas adicionais do cliente.
- `BlazorTarefas.Shared` (Shared): modelos e dados compartilhados (`Tarefa`, `TarefaDados`).

## Pré-requisitos
- SDK do .NET 9 instalado.
- IDE compatível (Visual Studio 2022+, VS Code, Rider).

## Como executar
1. Restaure e compile:
   - `dotnet build`
2. Execute o projeto host:
   - `dotnet run --project BlazorTarefas/BlazorTarefas/BlazorTarefas.csproj`
3. Acesse no navegador a URL exibida no console (ex.: `https://localhost:xxxx`).

## Funcionalidades
- Página `Home` com navegação básica.
- Página `Tarefas`: lista e manipula tarefas com base em `TarefaDados`.
- Página `Weather` (exemplo) e `Error`.
- Layout com `NavMenu` e `MainLayout`.

## Estrutura relevante
- `BlazorTarefas/Components/Pages`: páginas do app (`Home`, `Tarefas`, `Weather`, `Error`).
- `BlazorTarefas/Components/Layout`: layout e navegação (`MainLayout`, `NavMenu`).
- `BlazorTarefas.Shared/Entities/Tarefa.cs`: entidade de tarefa.
- `BlazorTarefas.Shared/Data/TarefaDados.cs`: fonte de dados das tarefas.




