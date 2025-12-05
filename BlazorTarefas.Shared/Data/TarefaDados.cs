using BlazorTarefas.Shared.Entities;

namespace BlazorTarefas.Shared.Data;

public static class TarefaDados
{
    public static List<Tarefa> ObterTarefas() => new()
    {
        new Tarefa {
            ID = Guid.NewGuid(),
            Descricao = "Estudar Blazor",
            Concluida = false,
            DataCriacao = DateTime.Now.AddDays(-2)
        },
        new Tarefa {
            ID = Guid.NewGuid(),
            Descricao = "Fazer compras",
            Concluida = true,
            DataCriacao = DateTime.Now.AddDays(-5)
        },
        new Tarefa {
            ID = Guid.NewGuid(),
            Descricao = "Ler um livro",
            Concluida = false,
            DataCriacao = DateTime.Now.AddDays(-1)
        },
        new Tarefa {
            ID = Guid.NewGuid(),
            Descricao = "Praticar consultas LINQ",
            Concluida = true,
            DataCriacao = DateTime.Now.AddDays(-1)
        },
    };
}
