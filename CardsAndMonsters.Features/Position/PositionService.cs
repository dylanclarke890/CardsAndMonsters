using CardsAndMonsters.Core.Exceptions;
using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Enums;
using System.Linq;

namespace CardsAndMonsters.Features.Position
{
    public class PositionService : IPositionService
    {
        private readonly IDuelLogService _duelLogService;

        public PositionService(IDuelLogService duelLogService)
        {
            _duelLogService = duelLogService;
        }

        public FieldPosition NewPosition(FieldPosition old)
        {
            return old switch
            {
                FieldPosition.HorizontalUp => FieldPosition.VerticalUp,
                FieldPosition.HorizontalDown => FieldPosition.VerticalUp,
                FieldPosition.VerticalUp => FieldPosition.HorizontalUp,
                FieldPosition.VerticalDown => FieldPosition.VerticalUp,
                _ => throw new GameArgumentException<BaseCard>(nameof(old), old)
            };
        }

        public void PositionSwitched(Monster monster, Board board)
        {
            if (monster == null)
            {
                throw new GameArgumentException<Monster>(nameof(monster), monster);
            }
            if (board == null)
            {
                throw new GameArgumentException<Monster>(nameof(board), board);
            }

            var currentMonster = board.PlayerField.Monsters.FirstOrDefault(m => m.Equals(monster));
            if (currentMonster == null)
            {
                throw new GameArgumentException<Monster>(nameof(currentMonster), currentMonster);
            }

            board.PlayerField.Monsters[board.PlayerField.Monsters.IndexOf(currentMonster)] = monster;
            board.CurrentTurn.MonsterState[monster.Id].AbleToSwitch = false;

            _duelLogService.AddNewEventLog(Event.MonsterPositionChange, board.Player);
        }
    }
}
