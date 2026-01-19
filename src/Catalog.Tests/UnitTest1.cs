using Catalog.Application;
using Catalog.Core.Dtos;
using Catalog.Core.Models;
using Catalog.Infrastructure.Persistence;
using FIAP.Cloud.Games.Orchestration.Events;
using MassTransit;
using Moq;

namespace Catalog.Tests
{
    public class UnitTest1
    {
        private readonly Mock<IRepository> _repositoryMock;
        private readonly Mock<IPublishEndpoint> _publishEndpointMock;
        private readonly GameServices _service;

        public UnitTest1()
        {
            _repositoryMock = new Mock<IRepository>();
            _publishEndpointMock = new Mock<IPublishEndpoint>();

            _service = new GameServices(_repositoryMock.Object, _publishEndpointMock.Object);
        }

        [Fact]
        public async Task CreateAsync_QuandoCriadoComSucesso_DeveRetornarSucesso()
        {
            var name = "Hades";
            var input = new GameInputDto
            {
                Name = name,
                Price = 99.99m,
                Description = "",
            };

            _repositoryMock
                .Setup(r => r.GetQuery<Game>())
                .Returns(Enumerable.Empty<Game>().AsQueryable());

            _repositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<Game>()))
                .ReturnsAsync(true);

            var result = await _service.CreateAsync(input);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(name, result.Value.Name);
        }

        [Fact]
        public async Task CreateAsync_QuandoJogoJaExiste_DeveRetornarFalha()
        {
            var input = new GameInputDto
            {
                Name = "God of War",
                Price = 199.99m,
                Description = "",
            };

            var jogos = new List<Game>
            {
                new() {
                    Name = "God of War",
                    Price = 199.99m,
                    Active = true
                }
            }.AsQueryable();

            _repositoryMock
                .Setup(r => r.GetQuery<Game>())
                .Returns(jogos);

            var resultado = await _service.CreateAsync(input);

            Assert.False(resultado.IsSuccess);
            Assert.Equal("Jogo já cadastrado.", resultado.Message);

            _repositoryMock.Verify(r => r.CreateAsync(It.IsAny<Game>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_QuandoRepositorioNaoConsegueCriar_DeveRetornarFalha()
        {
            var input = new GameInputDto
            {
                Name = "Elden Ring",
                Price = 299.99m,
                Description = "",
            };

            _repositoryMock
                .Setup(r => r.GetQuery<Game>())
                .Returns(Enumerable.Empty<Game>().AsQueryable());

            _repositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<Game>()))
                .ReturnsAsync(false);

            var resultado = await _service.CreateAsync(input);

            Assert.False(resultado.IsSuccess);
            Assert.Equal("Não foi possível criar o jogo.", resultado.Message);
        }

        [Fact]
        public async Task UpdateAsync_QuandoJogoNaoEncontrado_DeveRetornarFalha()
        {
            var id = 10;
            var dto = new GameUpdateDto();

            _repositoryMock
                .Setup(r => r.GetGameByIdAsync(id))
                .ReturnsAsync((Game?)null);

            var resultado = await _service.UpdateAsync(id, dto);

            Assert.False(resultado.IsSuccess);
            Assert.Equal($"Jogo com ID {id} não foi encontrado.", resultado.Message);

            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Game>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_QuandoRepositorioFalhaAoAtualizar_DeveRetornarFalha()
        {
            var id = 1;

            var game = new Game
            {
                Id = id,
                Name = "Hades",
                Description = "Rogue-like",
                Price = 99.99m,
                Active = true
            };

            var dto = new GameUpdateDto { Name = "Hades Updated" };

            _repositoryMock
                .Setup(r => r.GetGameByIdAsync(id))
                .ReturnsAsync(game);

            _repositoryMock
                .Setup(r => r.UpdateAsync(game))
                .ReturnsAsync(false);

            var resultado = await _service.UpdateAsync(id, dto);

            Assert.False(resultado.IsSuccess);
            Assert.Equal("Não foi possível atualizar o jogo.", resultado.Message);
        }

        [Fact]
        public async Task UpdateAsync_QuandoAtualizadoComSucesso_DeveRetornarSucesso()
        {
            var id = 1;

            var game = new Game
            {
                Id = id,
                Name = "Hades",
                Description = "Rogue-like",
                Price = 99.99m,
                Active = true
            };

            var dto = new GameUpdateDto
            {
                Name = "Hades II",
                Price = 149.99m,
                Active = false
            };

            _repositoryMock
                .Setup(r => r.GetGameByIdAsync(id))
                .ReturnsAsync(game);

            _repositoryMock
                .Setup(r => r.UpdateAsync(game))
                .ReturnsAsync(true);

            var resultado = await _service.UpdateAsync(id, dto);

            Assert.True(resultado.IsSuccess);
            Assert.NotNull(resultado.Value);
            Assert.Equal("Hades II", resultado.Value.Name);
            Assert.Equal(149.99m, resultado.Value.Price);
            Assert.False(resultado.Value.Active);
        }

        [Fact]
        public async Task BuyGameAsync_QuandoJogoNaoEncontrado_DeveRetornarFalha()
        {
            var request = new BuyRequest(1, 10);

            _repositoryMock
                .Setup(r => r.GetGameByIdAsync(request.GameId))
                .ReturnsAsync((Game?)null);

            var resultado = await _service.BuyGameAsync(request);

            Assert.False(resultado.IsSuccess);
            Assert.Equal($"Jogo com ID {request.GameId} não foi encontrado.", resultado.Message);

            _publishEndpointMock.Verify(p => p.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task BuyGameAsync_QuandoUsuarioNaoEncontrado_DeveRetornarFalha()
        {
            var request = new BuyRequest(99, 1);

            var game = new Game
            {
                Id = 1,
                Name = "Hades",
                Price = 99.99m,
                Active = true
            };

            _repositoryMock
                .Setup(r => r.GetGameByIdAsync(request.GameId))
                .ReturnsAsync(game);

            _repositoryMock
                .Setup(r => r.GetQuery<Member>())
                .Returns(new List<Member>().AsQueryable());

            var resultado = await _service.BuyGameAsync(request);

            Assert.False(resultado.IsSuccess);
            Assert.Equal($"Usuário com ID {request.GameId} não foi encontrado.", resultado.Message);

            _publishEndpointMock.Verify(p => p.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task BuyGameAsync_QuandoCompraIniciadaComSucesso_DeveRetornarSucesso()
        {
            var request = new BuyRequest(10, 1);

            var game = new Game
            {
                Id = 1,
                Name = "Hades",
                Price = 99.99m,
                Active = true
            };

            var member = new Member
            {
                Id = 10,
                Name = "usuario@teste.com"
            };

            _repositoryMock
                .Setup(r => r.GetGameByIdAsync(request.GameId))
                .ReturnsAsync(game);

            _repositoryMock
                .Setup(r => r.GetQuery<Member>())
                .Returns(new List<Member> { member }.AsQueryable());

            var resultado = await _service.BuyGameAsync(request);

            Assert.True(resultado.IsSuccess);
            Assert.Equal($"Compra iniciada para o jogo {game.Name}!", resultado.Message);
            Assert.NotNull(resultado.Value);
            Assert.Equal(game.Name, resultado.Value.Name);

            _publishEndpointMock.Verify(
                p => p.Publish(It.Is<OrderPlacedEvent>(e => e.UserId == request.UserId && e.Id == game.Id),
                               It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Fact]
        public async Task DisableGameAsync_QuandoJogoNaoEncontrado_DeveRetornarFalha()
        {
            var id = 10;

            _repositoryMock
                .Setup(r => r.GetGameByIdAsync(id))
                .ReturnsAsync((Game?)null);

            var resultado = await _service.DisableGameAsync(id);

            Assert.False(resultado.IsSuccess);
            Assert.Equal($"Jogo com ID {id} não foi encontrado.", resultado.Message);

            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Game>()), Times.Never);
        }

        [Fact]
        public async Task DisableGameAsync_QuandoJogoDesativadoComSucesso_DeveRetornarSucesso()
        {
            var id = 1;

            var game = new Game
            {
                Id = id,
                Name = "Hades",
                Price = 99.99m,
                Active = true
            };

            _repositoryMock
                .Setup(r => r.GetGameByIdAsync(id))
                .ReturnsAsync(game);

            _repositoryMock
                .Setup(r => r.UpdateAsync(game))
                .ReturnsAsync(true);

            var resultado = await _service.DisableGameAsync(id);

            Assert.True(resultado.IsSuccess);
            Assert.NotNull(resultado.Value);
            Assert.False(resultado.Value.Active);
            Assert.Equal(game.Name, resultado.Value.Name);

            _repositoryMock.Verify(r => r.UpdateAsync(game), Times.Once);
        }

    }
}
