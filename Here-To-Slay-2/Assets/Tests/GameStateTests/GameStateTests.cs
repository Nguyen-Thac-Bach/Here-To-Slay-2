using NUnit.Framework;
using Components;
using Components.Enums;
using Model;
using System.Collections.Generic;
using Tests.TestFactory;

namespace Tests.GameStateTests
{
    [TestFixture]
    public class GameStateTests
    {
        private GameState _gameState;

        [SetUp]
        public void Setup()
        {
            _gameState = new GameState();
            TestCardFactory.ResetIdCounter();
        }

        #region Constructor and Initialization Tests

        [Test]
        public void Constructor_InitializesEmptyCardList()
        {
            // Assert
            Assert.That(_gameState.GetCards(), Is.Empty);
        }

        #endregion

        #region Card Management Tests

        [Test]
        public void AddCard_AddsCardToCardsList()
        {
            // Arrange
            var card = TestCardFactory.CreateHeroCard();

            // Act
            _gameState.AddCard(card);

            // Assert
            Assert.That(_gameState.GetCards(), Has.Count.EqualTo(1));
            Assert.That(_gameState.GetCards(), Contains.Item(card));
        }

        [Test]
        public void AddCard_MultipleCards_AllAdded()
        {
            // Arrange
            var cards = TestCardFactory.CreateMultipleHeroCards(5);

            // Act
            foreach (var card in cards)
            {
                _gameState.AddCard(card);
            }

            // Assert
            Assert.That(_gameState.GetCards(), Has.Count.EqualTo(5));
        }

        [Test]
        public void GetCard_ReturnsCorrectCardById()
        {
            // Arrange
            var card = TestCardFactory.CreateHeroCard(cardId: 42);
            _gameState.AddCard(card);

            // Act
            var retrievedCard = _gameState.GetCard(42);

            // Assert
            Assert.That(retrievedCard, Is.EqualTo(card));
            Assert.That(retrievedCard.CardId, Is.EqualTo(42));
        }

        [Test]
        public void GetCard_NonExistentCardId_ReturnsNull()
        {
            // Act
            var retrievedCard = _gameState.GetCard(999);

            // Assert
            Assert.That(retrievedCard, Is.Null);
        }

        [Test]
        public void GetCardsFromDeck_ReturnsOnlyCardsFromSpecifiedDeck()
        {
            // Arrange
            var drawDeckCard = TestCardFactory.CreateHeroCard(deck: Deck.DrawDeck);
            var player1HandCard = TestCardFactory.CreateHeroCard(deck: Deck.Player1Hand);
            var player2HandCard = TestCardFactory.CreateHeroCard(deck: Deck.Player2Hand);

            _gameState.AddCard(drawDeckCard);
            _gameState.AddCard(player1HandCard);
            _gameState.AddCard(player2HandCard);

            // Act
            var drawDeckCards = _gameState.GetCardsFromDeck(Deck.DrawDeck);
            var player1Cards = _gameState.GetCardsFromDeck(Deck.Player1Hand);

            // Assert
            Assert.That(drawDeckCards, Has.Count.EqualTo(1));
            Assert.That(drawDeckCards, Contains.Item(drawDeckCard));
            Assert.That(player1Cards, Has.Count.EqualTo(1));
            Assert.That(player1Cards, Contains.Item(player1HandCard));
        }

        [Test]
        public void GetTopCardFromDrawDeck_ReturnsFirstCardInDrawDeck()
        {
            // Arrange
            var cards = TestCardFactory.CreateMultipleHeroCards(3, Deck.DrawDeck);
            foreach (var card in cards)
            {
                _gameState.AddCard(card);
            }

            // Act
            var topCard = _gameState.GetTopCardFromDrawDeck();

            // Assert
            Assert.That(topCard, Is.EqualTo(cards[0]));
        }

        #endregion

        #region Game Phase Management Tests

        [Test]
        public void StartGame_SetsInitialGameState()
        {
            // Arrange
            var drawDeckCards = TestCardFactory.CreateDrawDeck(10);
            foreach (var card in drawDeckCards)
            {
                _gameState.AddCard(card);
            }

            // Act
            _gameState.StartGame();

            // Assert
            Assert.That(_gameState.GetCurrentPlayer(), Is.EqualTo(Player.Player1));
            Assert.That(_gameState.GetCurrentPhase(), Is.EqualTo(GamePhase.ChoosingAction));
            Assert.That(_gameState.GetRemainingActions(), Is.EqualTo(3));
        }

        [Test]
        public void GetCurrentPhase_ReturnsCurrentGamePhase()
        {
            // Arrange
            var drawDeckCards = TestCardFactory.CreateDrawDeck(10);
            foreach (var card in drawDeckCards)
            {
                _gameState.AddCard(card);
            }
            _gameState.StartGame();

            // Act & Assert
            Assert.That(_gameState.GetCurrentPhase(), Is.EqualTo(GamePhase.ChoosingAction));
        }

        #endregion

        #region Player Management Tests

        [Test]
        public void GetCurrentPlayer_AfterStartGame_ReturnsPlayer1()
        {
            // Arrange
            var drawDeckCards = TestCardFactory.CreateDrawDeck(10);
            foreach (var card in drawDeckCards)
            {
                _gameState.AddCard(card);
            }

            // Act
            _gameState.StartGame();

            // Assert
            Assert.That(_gameState.GetCurrentPlayer(), Is.EqualTo(Player.Player1));
        }

        [Test]
        public void EndTurn_SwitchesPlayerAndResetsActions()
        {
            // Arrange
            var drawDeckCards = TestCardFactory.CreateDrawDeck(10);
            foreach (var card in drawDeckCards)
            {
                _gameState.AddCard(card);
            }
            _gameState.StartGame();
            _gameState.UseAction(2); // Use 2 actions, leaving 1

            // Act
            _gameState.EndTurn();

            // Assert
            Assert.That(_gameState.GetCurrentPlayer(), Is.EqualTo(Player.Player2));
            Assert.That(_gameState.GetRemainingActions(), Is.EqualTo(3)); // Reset to max
        }

        [Test]
        public void EndTurn_SwitchesBackAndForth()
        {
            // Arrange
            var drawDeckCards = TestCardFactory.CreateDrawDeck(10);
            foreach (var card in drawDeckCards)
            {
                _gameState.AddCard(card);
            }
            _gameState.StartGame();

            // Act
            _gameState.EndTurn();
            Player secondTurnPlayer = _gameState.GetCurrentPlayer();
            _gameState.EndTurn();
            Player thirdTurnPlayer = _gameState.GetCurrentPlayer();

            // Assert
            Assert.That(secondTurnPlayer, Is.EqualTo(Player.Player2));
            Assert.That(thirdTurnPlayer, Is.EqualTo(Player.Player1));
        }

        #endregion

        #region Action Management Tests

        [Test]
        public void GetRemainingActions_ReturnsCorrectAmount()
        {
            // Arrange
            var drawDeckCards = TestCardFactory.CreateDrawDeck(10);
            foreach (var card in drawDeckCards)
            {
                _gameState.AddCard(card);
            }
            _gameState.StartGame();

            // Assert
            Assert.That(_gameState.GetRemainingActions(), Is.EqualTo(3));
        }

        [Test]
        public void UseAction_DecreasesRemainingActions()
        {
            // Arrange
            var drawDeckCards = TestCardFactory.CreateDrawDeck(10);
            foreach (var card in drawDeckCards)
            {
                _gameState.AddCard(card);
            }
            _gameState.StartGame();

            // Act
            _gameState.UseAction(1);

            // Assert
            Assert.That(_gameState.GetRemainingActions(), Is.EqualTo(2));
        }

        [Test]
        public void UseAction_MultipleUses_DecreasesCorrectly()
        {
            // Arrange
            var drawDeckCards = TestCardFactory.CreateDrawDeck(10);
            foreach (var card in drawDeckCards)
            {
                _gameState.AddCard(card);
            }
            _gameState.StartGame();

            // Act
            _gameState.UseAction(1);
            _gameState.UseAction(1);
            _gameState.UseAction(1);

            // Assert
            Assert.That(_gameState.GetRemainingActions(), Is.EqualTo(0));
        }

        [Test]
        public void UseAction_ThrowsException_WhenNotEnoughActions()
        {
            // Arrange
            var drawDeckCards = TestCardFactory.CreateDrawDeck(10);
            foreach (var card in drawDeckCards)
            {
                _gameState.AddCard(card);
            }
            _gameState.StartGame();

            // Act & Assert
            Assert.Throws<System.Exception>(() =>
            {
                _gameState.UseAction(4); // More than available
            });
        }

        [Test]
        public void UseAction_PartialOverdraft_ThrowsException()
        {
            // Arrange
            var drawDeckCards = TestCardFactory.CreateDrawDeck(10);
            foreach (var card in drawDeckCards)
            {
                _gameState.AddCard(card);
            }
            _gameState.StartGame();
            _gameState.UseAction(2); // 1 action left

            // Act & Assert
            Assert.Throws<System.Exception>(() =>
            {
                _gameState.UseAction(2); // Trying to use 2 when only 1 available
            });
        }

        #endregion

        #region Card Movement Tests

        [Test]
        public void PlayHeroCardFromHand_MovesCardToField()
        {
            // Arrange
            var hero = TestCardFactory.CreateHeroCard(deck: Deck.Player1Hand);
            _gameState.AddCard(hero);

            var drawDeckCards = TestCardFactory.CreateDrawDeck(5);
            foreach (var card in drawDeckCards)
            {
                _gameState.AddCard(card);
            }

            _gameState.StartGame();

            // Act
            _gameState.PlayHeroCardFromHand(hero.CardId, Player.Player1);

            // Assert
            Assert.That(hero.Deck, Is.EqualTo(Deck.Player1Field));
        }

        [Test]
        public void PlayHeroCardFromHand_WithWrongPlayer_DoesNotMove()
        {
            // Arrange
            var hero = TestCardFactory.CreateHeroCard(deck: Deck.Player1Hand);
            _gameState.AddCard(hero);

            var drawDeckCards = TestCardFactory.CreateDrawDeck(5);
            foreach (var card in drawDeckCards)
            {
                _gameState.AddCard(card);
            }

            _gameState.StartGame();

            // Act
            _gameState.PlayHeroCardFromHand(hero.CardId, Player.Player2); // Wrong player

            // Assert
            Assert.That(hero.Deck, Is.EqualTo(Deck.Player1Hand)); // Card should not move
        }

        [Test]
        public void PlayHeroCardFromHand_WithNonHeroCard_DoesNotMove()
        {
            // Arrange
            var baseCard = TestCardFactory.CreateBaseCard(deck: Deck.Player1Hand);
            _gameState.AddCard(baseCard);

            var drawDeckCards = TestCardFactory.CreateDrawDeck(5);
            foreach (var card in drawDeckCards)
            {
                _gameState.AddCard(card);
            }

            _gameState.StartGame();

            // Act
            _gameState.PlayHeroCardFromHand(baseCard.CardId, Player.Player1);

            // Assert
            Assert.That(baseCard.Deck, Is.EqualTo(Deck.Player1Hand)); // Card should not move
        }

        [Test]
        public void DrawFromDrawDeck_MovesCardToHand()
        {
            // Arrange
            var drawDeckCards = TestCardFactory.CreateDrawDeck(10);
            foreach (var card in drawDeckCards)
            {
                _gameState.AddCard(card);
            }

            _gameState.StartGame();
            var topCard = _gameState.GetTopCardFromDrawDeck();

            // Act
            _gameState.DrawFromDrawDeck(Player.Player1, outOfOwnTurnAllowed: true);

            // Assert
            Assert.That(topCard.Deck, Is.EqualTo(Deck.Player1Hand));
        }

        [Test]
        public void DrawFromDrawDeck_DuringOtherPlayersTurn_FailsWithoutFlag()
        {
            // Arrange
            var drawDeckCards = TestCardFactory.CreateDrawDeck(10);
            foreach (var card in drawDeckCards)
            {
                _gameState.AddCard(card);
            }

            _gameState.StartGame();
            _gameState.EndTurn(); // Switch to Player2's turn
            var topCard = _gameState.GetTopCardFromDrawDeck();
            var initialDeck = topCard.Deck;

            // Act
            _gameState.DrawFromDrawDeck(Player.Player1, outOfOwnTurnAllowed: false);

            // Assert
            Assert.That(topCard.Deck, Is.EqualTo(initialDeck)); // Card should not have moved
        }

        #endregion

        #region Card Selectability Tests

        [Test]
        public void IsCardSelectable_HandCard_DuringChoosingActionPhase_ReturnsTrue()
        {
            // Arrange
            var card = TestCardFactory.CreateHeroCard(deck: Deck.Player1Hand);
            _gameState.AddCard(card);

            var drawDeckCards = TestCardFactory.CreateDrawDeck(5);
            foreach (var c in drawDeckCards)
            {
                _gameState.AddCard(c);
            }

            _gameState.StartGame();

            // Act
            bool isSelectable = _gameState.IsCardSelectable(card.CardId, GamePhase.ChoosingAction, Player.Player1);

            // Assert
            Assert.That(isSelectable, Is.True);
        }

        [Test]
        public void IsCardSelectable_FieldCard_DuringChoosingActionPhase_ReturnsTrue()
        {
            // Arrange
            var card = TestCardFactory.CreateHeroCard(deck: Deck.Player1Field);
            _gameState.AddCard(card);

            var drawDeckCards = TestCardFactory.CreateDrawDeck(5);
            foreach (var c in drawDeckCards)
            {
                _gameState.AddCard(c);
            }

            _gameState.StartGame();

            // Act
            bool isSelectable = _gameState.IsCardSelectable(card.CardId, GamePhase.ChoosingAction, Player.Player1);

            // Assert
            Assert.That(isSelectable, Is.True);
        }

        [Test]
        public void IsCardSelectable_OpponentHandCard_ReturnsTrue()
        {
            // Arrange
            var card = TestCardFactory.CreateHeroCard(deck: Deck.Player2Hand);
            _gameState.AddCard(card);

            var drawDeckCards = TestCardFactory.CreateDrawDeck(5);
            foreach (var c in drawDeckCards)
            {
                _gameState.AddCard(c);
            }

            _gameState.StartGame();

            // Act
            bool isSelectable = _gameState.IsCardSelectable(card.CardId, GamePhase.ChoosingAction, Player.Player2);

            // Assert
            Assert.That(isSelectable, Is.True);
        }

        [Test]
        public void IsCardSelectable_WrongPlayerCard_ReturnsFalse()
        {
            // Arrange
            var card = TestCardFactory.CreateHeroCard(deck: Deck.Player2Hand);
            _gameState.AddCard(card);

            var drawDeckCards = TestCardFactory.CreateDrawDeck(5);
            foreach (var c in drawDeckCards)
            {
                _gameState.AddCard(c);
            }

            _gameState.StartGame();

            // Act
            bool isSelectable = _gameState.IsCardSelectable(card.CardId, GamePhase.ChoosingAction, Player.Player1);

            // Assert
            Assert.That(isSelectable, Is.False);
        }

        [Test]
        public void IsCardSelectable_AttackableMonsterCard_ReturnsTrue()
        {
            // Arrange
            var card = TestCardFactory.CreateHeroCard(deck: Deck.AttackableMonsters);
            _gameState.AddCard(card);

            var drawDeckCards = TestCardFactory.CreateDrawDeck(5);
            foreach (var c in drawDeckCards)
            {
                _gameState.AddCard(c);
            }

            _gameState.StartGame();

            // Act
            bool isSelectable = _gameState.IsCardSelectable(card.CardId, GamePhase.ChoosingAction, Player.Player1);

            // Assert
            Assert.That(isSelectable, Is.True);
        }

        [Test]
        public void IsCardSelectable_DrawDeckCard_ReturnsTrue()
        {
            // Arrange
            var card = TestCardFactory.CreateHeroCard(deck: Deck.DrawDeck);
            _gameState.AddCard(card);

            var drawDeckCards = TestCardFactory.CreateDrawDeck(5);
            foreach (var c in drawDeckCards)
            {
                _gameState.AddCard(c);
            }

            _gameState.StartGame();

            // Act
            bool isSelectable = _gameState.IsCardSelectable(card.CardId, GamePhase.ChoosingAction, Player.Player1);

            // Assert
            Assert.That(isSelectable, Is.True);
        }

        [Test]
        public void IsCardSelectable_SlainMonstersCard_ReturnsFalse()
        {
            // Arrange
            var card = TestCardFactory.CreateHeroCard(deck: Deck.Player1SlainMonsters);
            _gameState.AddCard(card);

            var drawDeckCards = TestCardFactory.CreateDrawDeck(5);
            foreach (var c in drawDeckCards)
            {
                _gameState.AddCard(c);
            }

            _gameState.StartGame();

            // Act
            bool isSelectable = _gameState.IsCardSelectable(card.CardId, GamePhase.ChoosingAction, Player.Player1);

            // Assert
            Assert.That(isSelectable, Is.False);
        }

        [Test]
        public void GetSelectableCardIds_ReturnsOnlySelectableCards()
        {
            // Arrange
            var hand1 = TestCardFactory.CreateHeroCard(cardId: 1, deck: Deck.Player1Hand);
            var field1 = TestCardFactory.CreateHeroCard(cardId: 2, deck: Deck.Player1Field);
            var slained = TestCardFactory.CreateHeroCard(cardId: 3, deck: Deck.Player1SlainMonsters);
            var opponent = TestCardFactory.CreateHeroCard(cardId: 4, deck: Deck.Player2Hand);

            _gameState.AddCard(hand1);
            _gameState.AddCard(field1);
            _gameState.AddCard(slained);
            _gameState.AddCard(opponent);

            var drawDeckCards = TestCardFactory.CreateDrawDeck(5);
            foreach (var c in drawDeckCards)
            {
                _gameState.AddCard(c);
            }

            _gameState.StartGame();

            // Act
            var selectableIds = _gameState.GetSelectableCardIds(GamePhase.ChoosingAction, Player.Player1);

            // Assert
            Assert.That(selectableIds, Contains.Item(1));
            Assert.That(selectableIds, Contains.Item(2));
            Assert.That(selectableIds, Does.Not.ContainValue(3)); // Slained should not be selectable
            Assert.That(selectableIds, Does.Not.ContainValue(4)); // Opponent's hand should not be selectable
        }

        #endregion

        #region Deck Limit Tests

        [Test]
        public void IsDeckWithLimit_HandDecks_ReturnsTrue()
        {
            // Assert
            Assert.That(_gameState.IsDeckWithLimit(Deck.Player1Hand), Is.True);
            Assert.That(_gameState.IsDeckWithLimit(Deck.Player2Hand), Is.True);
        }

        [Test]
        public void IsDeckWithLimit_FieldDecks_ReturnsTrue()
        {
            // Assert
            Assert.That(_gameState.IsDeckWithLimit(Deck.Player1Field), Is.True);
            Assert.That(_gameState.IsDeckWithLimit(Deck.Player2Field), Is.True);
        }

        [Test]
        public void IsDeckWithLimit_SlainMonstersDecks_ReturnsTrue()
        {
            // Assert
            Assert.That(_gameState.IsDeckWithLimit(Deck.Player1SlainMonsters), Is.True);
            Assert.That(_gameState.IsDeckWithLimit(Deck.Player2SlainMonsters), Is.True);
        }

        [Test]
        public void IsDeckWithLimit_AttackableMonsters_ReturnsTrue()
        {
            // Assert
            Assert.That(_gameState.IsDeckWithLimit(Deck.AttackableMonsters), Is.True);
        }

        [Test]
        public void IsDeckWithLimit_DrawDeck_ReturnsFalse()
        {
            // Assert
            Assert.That(_gameState.IsDeckWithLimit(Deck.DrawDeck), Is.False);
        }

        [Test]
        public void IsDeckWithLimit_DiscardDeck_ReturnsFalse()
        {
            // Assert
            Assert.That(_gameState.IsDeckWithLimit(Deck.DiscardDeck), Is.False);
        }

        #endregion
    }
}

