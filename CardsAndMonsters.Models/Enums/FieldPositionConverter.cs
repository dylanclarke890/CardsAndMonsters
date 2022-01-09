namespace CardsAndMonsters.Models.Enums
{
    public static class FieldPositionConverter
    {
        public static string MonsterPositionToString(FieldPosition fieldPosition)
        {
            return fieldPosition switch
            {
                FieldPosition.HorizontalUp => "Defense",
                FieldPosition.HorizontalDown => "Set",
                FieldPosition.VerticalUp => "Attack",
                _ => null,
            };
        }
        public static string CardPositionToString(FieldPosition fieldPosition)
        {
            return fieldPosition switch
            {
                FieldPosition.VerticalUp => "Face up",
                FieldPosition.VerticalDown => "Set",
                _ => null,
            };
        }
    }
}
