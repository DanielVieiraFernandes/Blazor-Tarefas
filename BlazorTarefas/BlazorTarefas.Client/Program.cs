using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
// Estou criando o Host do Blazor WebAssembly
// o AppSettings.json será carregado 
// e os serviços de injeção de dependência serão configurados automaticamente
//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Construindo e executando o Host do Blazor WebAssembly
await builder.Build().RunAsync();
