using CardsAndMonsters.Core.Exceptions;
using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Features.TurnPhase;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Enums;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace CardsAndMonsters.Features.Tests.TurnPhase
{
    public class PhaseServiceTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<IDuelLogService> _mockDuelLogService;

        public PhaseServiceTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockDuelLogService = _mockRepository.Create<IDuelLogService>();
        }

        private PhaseService CreateService()
        {
            return new PhaseService(_mockDuelLogService.Object);
        }

        [Fact]
        public async Task EnterPhase_ChangesCurrentTurnPhaseCorrectly()
        {
            // Arrange
            var service = CreateService();
            Phase phase = Phase.Main;
            Board board = new() { CurrentTurn = new() { Duelist = new(), Phase = Phase.Standby } };

            _mockDuelLogService.Setup(dls => dls.AddNewEventLog(Event.PhaseChange, board.CurrentTurn.Duelist));

            // Act
            await service.EnterPhase(phase, board);

            // Assert
            Assert.Equal(Phase.Main, board.CurrentTurn.Phase);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task EnterPhase_NullBoard_ThrowsGameArgumentException()
        {
            // Arrange
            var service = CreateService();
            Phase phase = Phase.Main;
            Board board = null;

            // Act
            async Task act() => await service.EnterPhase(phase, board);

            // Assert
            await Assert.ThrowsAsync<GameArgumentException<Board>>(act);

            _mockRepository.VerifyAll();
        }
    }
}
