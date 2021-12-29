﻿using CardsAndMonsters.Features.Battle;
using CardsAndMonsters.Features.GameOver;
using CardsAndMonsters.Features.Logging;
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
            services.AddScoped<IBattleService, BattleService>();
            services.AddScoped<IDuelLogService, DuelLogService>();
            services.AddScoped<IFakeOpponentService, FakeOpponentService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IGameOverService, GameOverService>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<IPhaseService, PhaseService>();
            services.AddScoped<ITurnService, TurnService>();

            return services;
        }
    }
}
