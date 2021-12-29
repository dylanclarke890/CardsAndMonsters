namespace CardsAndMonsters.Models.Enums
{
    public enum Event
    {
        GameStarted = -1,
        TurnChange = 0, 
        PhaseChange = 1, 
        DrawCard = 2, 
        MonsterPositionChange = 3, 
        PlayMonster = 4,
        AttackDeclared = 5,
        MonsterDestroyed = 6,
        DamageTaken = 7,
        GameEnded = 99,
    }
}
