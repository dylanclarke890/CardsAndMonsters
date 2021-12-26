namespace CardsAndMonsters.Models
{
    public class MonsterTurnState
    {
        public Monster Monster { get; set; }

        public bool AbleToSwitch { get; set; }

        public int TimesAttacked { get; set; }

        public bool Destroyed { get; set; }
    }
}
