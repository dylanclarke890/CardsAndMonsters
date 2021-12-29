﻿using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Enums;
using System;
using System.Linq;

namespace CardsAndMonsters.Features.Position
{
    public class PositionService : IPositionService
    {
        public FieldPosition NewPosition(FieldPosition old)
        {
            return old switch
            {
                FieldPosition.HorizontalUp => FieldPosition.VerticalUp,
                FieldPosition.HorizontalDown => FieldPosition.VerticalUp,
                FieldPosition.VerticalUp => FieldPosition.HorizontalUp,
                FieldPosition.VerticalDown => FieldPosition.VerticalUp,
                _ => throw new IndexOutOfRangeException("Couldn't work out the new position")
            };
        }

        public void PositionSwitched(Monster monster, Board board)
        {
            var currentMonster = board.PlayerField.Monsters.FirstOrDefault(m => m.Equals(monster));
            board.PlayerField.Monsters[board.PlayerField.Monsters.IndexOf(currentMonster)] = monster;
            board.CurrentTurn.MonsterState[monster.Id].AbleToSwitch = false;
        }
    }
}
