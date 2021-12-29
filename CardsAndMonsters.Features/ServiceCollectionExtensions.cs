using CardsAndMonsters.Features.Battle;
using CardsAndMonsters.Features.GameOver;
using CardsAndMonsters.Features.Opponent;
using CardsAndMonsters.Features.Position;
using CardsAndMonsters.Features.Turn;
using CardsAndMonsters.Features.TurnPhase;
using Microsoft.Extensions.DependencyInjection;

namespace CardsAndMonsters.Features
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFeatures(this IServiceCollection services)
        {
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<IPhaseService, PhaseService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IGameOverService, GameOverService>();
            services.AddScoped<IBattleService, BattleService>();
            services.AddScoped<ITurnService, TurnService>();
            services.AddScoped<IFakeOpponentService, FakeOpponentService>();

            return services;
        }
    }
}
