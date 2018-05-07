using Microsoft.Extensions.DependencyInjection;
using mp.ce.fdid.Data.Repositories;
using mp.ce.fdid.Domain.Interfaces;

namespace mp.ce.fdid.IoC
{
    public static class NativeInjectorConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IBancoRepository, BancoRepository>();
            services.AddScoped<IInstituicaoRepository, InstituicaoRepository>();
            services.AddScoped<ICidadeRepository, CidadeRepository>();
            services.AddScoped<IEstadoRepository, EstadoRepository>();
            services.AddScoped<IProjetoRepository, ProjetoRepository>();
            services.AddScoped<IArquivoRepository, ArquivoRepository>();
            services.AddScoped<IPrazoRepository, PrazoRepository>();
        }

    }
}
