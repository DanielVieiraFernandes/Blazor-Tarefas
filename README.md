# Blazor Tarefas

Aplicação de estudo construída com Blazor (.NET 9) para praticar desde conceitos introdutórios de desenvolvimento web até recursos mais avançados e específicos do Blazor, como modos de renderização, pré-renderização, streaming e persistência de estado.

O foco do projeto é servir como laboratório para entender **como o Blazor funciona por baixo dos panos**, misturando:

- Fundamentos de um framework web (rotas, layouts, navegação, componentes).
- Conceitos exclusivos do Blazor (SSR, Interactive Server, WebAssembly, Interactive Auto, `RendererInfo`, `NavLink`, `LayoutComponentBase`).
- Recursos mais avançados (streaming `[StreamRendering]`, `PersistentComponentState`, múltiplas rotas, parâmetros de rota, renderização híbrida Server + WebAssembly).

---

## Estrutura da solução

A solução é dividida em três projetos:

- `BlazorTarefas` (Server/Host)
  - Aplicação host ASP.NET Core/Blazor.
  - Configura os modos de renderização:
    - `.AddInteractiveServerComponents()`
    - `.AddInteractiveWebAssemblyComponents()`
  - Mapeia os componentes Razor:
    - `app.MapRazorComponents<App>()`
      - `.AddInteractiveServerRenderMode()`
      - `.AddInteractiveWebAssemblyRenderMode()`
      - `.AddAdditionalAssemblies(typeof(BlazorTarefas.Client._Imports).Assembly);`

- `BlazorTarefas.Client` (WebAssembly)
  - Host do Blazor WebAssembly.
  - Demonstra o modo `InteractiveWebAssembly` em páginas como `ResumoTarefas`.
  - `Program.cs` usa `WebAssemblyHostBuilder.CreateDefault(args)` para criar e rodar o host WebAssembly.

- `BlazorTarefas.Shared` (Shared)
  - Código compartilhado entre Server e Client.
  - Entidades e dados de exemplo:
    - `Entities/Tarefa.cs`: modelo de tarefa.
    - `Data/TarefaDados.cs`: fonte de dados em memória com exemplos de tarefas.

---

## Páginas e exemplos de aprendizado

### Layout e navegação

- `Components/Layout/MainLayout.razor`
  - Implementa um layout padrão herdando de `LayoutComponentBase`.
  - Usa `@Body` para renderizar o conteúdo das páginas filhas.
  - Inclui o container de erro padrão do Blazor (`#blazor-error-ui`).

- `Components/Layout/NavMenu.razor`
  - Demonstra o uso do componente `NavLink`:
    - Aplicação automática da classe CSS `active`.
    - Navegação client-side integrada ao roteador do Blazor.
    - Uso da propriedade `Match` (`NavLinkMatch.All` e `NavLinkMatch.Prefix`).
  - Links de navegação para:
    - `Home (SSR)`
    - `Counter (Auto)`
    - `Contador (Auto)`
    - `Weather (Stream)`
    - `Weather Persist (Server)`
    - `Tempo (Stream)` (`/weather-dupla-execucao`)
    - `Tarefas (Server)`
    - `Pendentes` (`/tarefas/pendentes`)
    - `Concluídas` (`/tarefas/concluidas`)
    - `Resumo (Wasm)` (`/tarefas-resumo`)

### Tarefas (lista e resumo)

- `Components/Pages/Tarefas.razor`
  - Rotas:
    - `@page "/tarefas"`
    - `@page "/tarefas/{status}"`
    - `@page "/lista"`
  - `@rendermode InteractiveServer`
  - Conceitos:
    - Leitura de parâmetro de rota via `[Parameter] public string? Status { get; set; }`.
    - Filtro de tarefas com base no status:
      - `concluidas`
      - `pendentes`
      - todos (padrão)
    - Uso de `TarefaDados.ObterTarefas()` (dados em memória).
    - Simulação de carregamento com `Task.Delay(2000)`.
    - Renderização condicional:
      - Sem dados, carregando, lista preenchida.
    - Manipulação de eventos:
      - Remoção de itens (`RemoverTarefa(Guid id)`).
      - Alternância de conclusão (`AlternarConclusao(Guid id)`).
    - Navegação programática com `NavigationManager`:
      - `Nav.NavigateTo("/tarefas-resumo")`.

- `BlazorTarefas.Client/Pages/ResumoTarefas.razor`
  - `@page "/tarefas-resumo"`
  - `@layout LayoutSimples`
  - `@rendermode InteractiveWebAssembly`
  - Conceitos:
    - Execução 100% no cliente (WebAssembly).
    - Leitura de dados compartilhados (`TarefaDados`) no lado client.
    - Uso intenso de LINQ para resumir as tarefas:
      - Total, concluidas, pendentes.
      - Primeira e última data de criação.
    - Diagnóstico de renderização com `RendererInfo` e `AssignedRenderMode`.

### Renderização, pré-renderização e modos interativos

- `Components/Pages/Weather.razor`
  - `@page "/weather"`
  - `@attribute [StreamRendering]`
  - Demonstra:
    - Streaming de conteúdo: primeira parte da página é renderizada antes da conclusão de `OnInitializedAsync`.
    - Diagnóstico de renderização com `RendererInfo`:
      - `RendererInfo.Name` (Static / Server / WebAssembly).
      - `RendererInfo.IsInteractive`.
      - `AssignedRenderMode`.
    - Carregamento assíncrono dos dados (`Task.Delay(500)`).

- `Components/Pages/Previsao.razor`
  - `@page "/weather-dupla-execucao"`
  - `@attribute [StreamRendering]`
  - `@rendermode InteractiveServer`
  - Demonstra:
    - Execução dupla de `OnInitializedAsync` (pré-renderização + fase interativa).
    - Logs no console (`Console.WriteLine`) para inspecionar:
      - Quantidade de chamadas (`initializedCallCount`).
      - `RendererInfo.Name` e `RendererInfo.IsInteractive`.

- `Components/Pages/WheatherPersistent.razor`
  - `@page "/weather-persistent"`
  - `@rendermode @(new InteractiveServerRenderMode(prerender: true))`
  - `@inject PersistentComponentState ApplicationState`
  - Implementa:
    - Persistência de estado entre a pré-renderização e a fase interativa:
      - Uso de `PersistentComponentState`.
      - Registro de callback com `ApplicationState.RegisterOnPersisting(PersistirDados)`.
      - Leitura/gravação em JSON:
        - `TryTakeFromJson<WeatherForecast[]>("weatherData", out forecasts)`
        - `PersistAsJson("weatherData", forecasts)`.
    - Evita recarregar dados na segunda execução de `OnInitializedAsync`.
    - Implementação de `IDisposable` para descartar a inscrição (`PersistingComponentStateSubscription`).

- `BlazorTarefas.Client/Pages/Contador.razor`
  - `@page "/contador"`
  - `@rendermode InteractiveAuto`
  - Demonstra:
    - Detecção de pré-renderização vs estado interativo:
      - `RendererInfo.IsInteractive == false` → pré-renderização.
      - `RendererInfo.IsInteractive == true` → componente interativo.
    - Habilitação/desabilitação de botão com base na interatividade.

### Estrutura do HTML principal

- `Components/App.razor`
  - HTML completo (`<!DOCTYPE html> ...`).
  - Demonstra:
    - Uso de `<base href="/" />`.
    - Inclusão de CSS estático via `Assets[...]`.
    - `ImportMap` integrado para Blazor WebAssembly.
    - `HeadOutlet` para conteúdo dinâmico no `<head>`.
    - Bootstrap Icons via CDN.
    - `Routes` e script `_framework/blazor.web.js`.

---

## Conceitos de Blazor trabalhados

- Componentes Razor e `@page`.
- Layouts (`LayoutComponentBase`, `@Body`).
- Roteamento:
  - Múltiplas rotas por componente.
  - Parâmetros de rota (`/tarefas/{status}`).
- Navegação:
  - `NavLink` (menu).
  - `NavigationManager` (navegação programática).
- Modos de renderização:
  - SSR (Static).
  - `InteractiveServer`.
  - `InteractiveWebAssembly`.
  - `InteractiveAuto`.
  - Diagnóstico com `RendererInfo` e `AssignedRenderMode`.
- Streaming e pré-renderização:
  - `[StreamRendering]`.
  - Detecção de interatividade.
  - Execução dupla de `OnInitializedAsync`.
- Persistência de estado:
  - `PersistentComponentState`.
  - `RegisterOnPersisting`, `PersistAsJson`, `TryTakeFromJson`.
- Compartilhamento de código entre Server e WebAssembly:
  - Projeto `Shared` com entidades e dados.

---

## Pré-requisitos

- SDK do .NET 9 instalado.
- IDE compatível:
  - Visual Studio 2022+ (com suporte a .NET 9 e Blazor).
  - VS Code com extensões C#.
  - Rider ou equivalente.

---

## Como executar o projeto

### Via CLI

1. Restaurar e compilar a solução:

   ```bash
   dotnet build
   ```

2. Executar o projeto host (Server):

   ```bash
   dotnet run --project BlazorTarefas/BlazorTarefas/BlazorTarefas.csproj
   ```

3. Acessar a URL exibida no console (ex.: `https://localhost:xxxx`).

### Via Visual Studio

1. Abrir a solução `BlazorTarefas.sln`.
2. Definir o projeto `BlazorTarefas` como projeto de inicialização.
3. Executar (F5 ou Ctrl+F5).

