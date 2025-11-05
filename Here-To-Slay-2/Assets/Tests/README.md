# GameState Unit Tests

This folder contains comprehensive unit tests for the `GameState` class using NUnit.

## Project Structure

```
Assets/Tests/
├── TestFactory/
│   └── TestCardFactory.cs       # Factory for creating test cards
├── GameStateTests/
│   └── GameStateTests.cs        # Comprehensive GameState tests
├── Tests.asmdef                 # Assembly definition for tests
└── README.md                    # This file
```

## Running Tests

### In Unity Editor
1. Open the **Test Runner** window: `Window → Testing → Test Runner`
2. Select the **EditMode** tab
3. Click **Run All** to execute all tests

### From Command Line
```bash
# Run all tests
Unity -runTests -testPlatform editmode -projectPath <path-to-project>

# Run specific test
Unity -runTests -testPlatform editmode -testFilter "GameStateTests.GameStateTests.TestName" -projectPath <path-to-project>
```

## Test Coverage

### Card Management (8 tests)
- Adding single and multiple cards
- Retrieving cards by ID
- Filtering cards by deck
- Getting top card from draw deck

### Game Phase & Player Management (4 tests)
- Game initialization
- Current phase retrieval
- Player switching
- Turn ending with action reset

### Action Management (5 tests)
- Action retrieval and usage
- Multiple action usage
- Exception handling for insufficient actions

### Card Movement (3 tests)
- Playing hero cards from hand
- Drawing from draw deck
- Turn-based draw restrictions

### Card Selectability (7 tests)
- Hand and field card selectability
- Opponent card visibility
- Phase-specific selectability rules
- Getting selectable card IDs

### Deck Limits (6 tests)
- Identifying decks with limits
- Verifying limit status for all deck types

**Total: 33 tests**

## Test Factory Usage

The `TestCardFactory` provides convenient methods for creating test cards:

```csharp
// Create a single hero card with defaults
var hero = TestCardFactory.CreateHeroCard();

// Create with specific properties
var customHero = TestCardFactory.CreateHeroCard(
    cardId: 42,
    name: "CustomHero",
    deck: Deck.Player1Hand,
    minRoll: 7
);

// Create multiple cards
var heroes = TestCardFactory.CreateMultipleHeroCards(5, Deck.DrawDeck);

// Create a full draw deck
var drawDeck = TestCardFactory.CreateDrawDeck(30);
```

## Key Features

- **No Unity Runtime Required**: Tests run in edit mode without launching the game
- **Isolated Tests**: Each test is independent and can run in any order
- **Comprehensive Coverage**: Tests cover happy paths, edge cases, and error conditions
- **Factory Pattern**: Reusable test card creation with sensible defaults
- **Well-Documented**: Clear test names and comments explaining each test

## Adding New Tests

1. Add test methods to the appropriate test class or create a new fixture
2. Use the `TestCardFactory` for card creation
3. Follow the **Arrange-Act-Assert** pattern
4. Use meaningful test names: `MethodName_Scenario_ExpectedResult()`

Example:
```csharp
[Test]
public void YourMethod_YourScenario_ExpectedResult()
{
    // Arrange
    var card = TestCardFactory.CreateHeroCard();
    _gameState.AddCard(card);

    // Act
    var result = _gameState.GetCard(card.CardId);

    // Assert
    Assert.That(result, Is.EqualTo(card));
}
```

## Dependencies

- **NUnit**: Testing framework (included with Unity Test Framework)
- **Unity Test Framework**: Test runner and utilities

## Notes

- Tests use auto-incrementing card IDs (starting at 1000) to avoid conflicts
- Call `TestCardFactory.ResetIdCounter()` in `[SetUp]` to reset IDs for each test
- All tests run without needing a Unity scene or game manager

## Troubleshooting

**Tests not showing up in Test Runner:**
- Verify `Tests.asmdef` exists and is properly configured
- Ensure test files are in `Assets/Tests/` folder
- Check that test class has `[TestFixture]` attribute
- Verify test methods have `[Test]` attribute

**Compilation errors:**
- Check that all necessary namespaces are imported
- Verify card class constructors match factory calls
- Ensure enums and types are properly referenced


