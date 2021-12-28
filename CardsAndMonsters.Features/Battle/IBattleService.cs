using CardsAndMonsters.Models;

namespace CardsAndMonsters.Features.Battle
{
    public interface IBattleService
    {
        BattleInfo CurrentBattle { get; set; }
        bool DeclaringAttack { get; set; }

        void Attack(BattleInfo battleInfo);
        void AttackTarget(BattleInfo info);
        void DeclareAttack(BattleInfo info);
        void ClearCurrentBattle();
    }
}