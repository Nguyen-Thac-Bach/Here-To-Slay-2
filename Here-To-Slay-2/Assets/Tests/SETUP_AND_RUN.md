# Running Tests - Quick Setup Guide

## Prerequisites

✅ Unity Test Framework (installed by default in Unity 2022+)  
✅ NUnit framework (included with UTF)  
✅ Code organized into assemblies (see ASSEMBLY_STRUCTURE.md)

## Step-by-Step Setup

### 1. **Verify Assembly Structure**
Check that these files exist:
- ✅ Assets/Scripts/Core.asmdef
- ✅ Assets/Scripts/Util/GameServices.asmdef
- ✅ Assets/Scripts/ViewModel/ViewModel.asmdef
- ✅ Assets/Scripts/View/View.asmdef
- ✅ Assets/Scripts/App/App.asmdef
- ✅ Assets/Tests/Tests.asmdef

### 2. **Force Unity to Recognize Changes**
In the Unity Editor:
1. Go to **Assets → Reimport All** (or press Ctrl+R)
2. Wait for compilation to finish
3. Check the Console for errors

### 3. **Open Test Runner**
1. Click **Window → Testing → Test Runner**
2. A new panel should appear

### 4. **Switch to EditMode Tab**
- Look for tabs at the top of the Test Runner window
- Click **EditMode** (not PlayMode)

### 5. **Run Tests**
- Click **Run All** button to run all tests
- Or click the play icon next to individual tests

## What You Should See

✅ Tests appear in the list under `Tests.GameStateTests`  
✅ Green checkmarks for passing tests  
✅ Detailed output in the Console

## Expected Test Output

```
GameStateTests.Constructor_InitializesEmptyCardList ... PASSED
GameStateTests.AddCard_AddsCardToCardsList ... PASSED
GameStateTests.GetCard_ReturnsCorrectCardById ... PASSED
...
[33 out of 33 tests passed]
```

## Common Issues and Solutions

### Issue: Tests don't appear in Test Runner
**Solution:**
1. Check that Tests.asmdef exists and is properly formatted
2. Verify test files are inside Assets/Tests/ folder
3. Ensure test class has `[TestFixture]` attribute
4. Run Assets → Reimport All

### Issue: "Components namespace not found"
**Solution:**
1. Verify Core.asmdef exists at Assets/Scripts/Core.asmdef
2. Check that Tests.asmdef has `"references": ["Core"]`
3. Reimport all assets

### Issue: Tests run but fail with null reference errors
**Solution:**
1. Check TestCardFactory.ResetIdCounter() is called in [SetUp]
2. Verify draw deck has enough cards in tests (usually 10+ for StartGame)
3. Check that test factories are creating proper card objects

### Issue: "Assembly reference not found"
**Solution:**
1. Check all .asmdef files are valid JSON
2. Verify reference names match exactly: "Core", "GameServices", etc.
3. Reimport all assets: Assets → Reimport All

## Running Tests from Command Line

### Run all tests:
```bash
Unity -runTests -testPlatform editmode -projectPath "<path-to-project>"
```

### Run specific test class:
```bash
Unity -runTests -testPlatform editmode -testFilter "GameStateTests" -projectPath "<path-to-project>"
```

### Run specific test:
```bash
Unity -runTests -testPlatform editmode -testFilter "GameStateTests.Constructor_InitializesEmptyCardList" -projectPath "<path-to-project>"
```

## Writing New Tests

1. Add test method to `GameStateTests.cs` or create new test class
2. Use `[TestFixture]` on class, `[Test]` on methods
3. Use TestCardFactory for creating test cards
4. Follow Arrange-Act-Assert pattern

Example:
```csharp
[Test]
public void MyTest_Scenario_ExpectedResult()
{
    // Arrange
    var card = TestCardFactory.CreateHeroCard();
    
    // Act
    var result = _gameState.GetCard(card.CardId);
    
    // Assert
    Assert.That(result, Is.NotNull);
}
```

## Next Steps

1. ✅ Run tests to verify setup works
2. ✅ Add more tests for uncovered scenarios
3. ✅ Integrate tests into CI/CD pipeline
4. ✅ Check test coverage with code coverage tools

## Documentation

For detailed assembly information, see **ASSEMBLY_STRUCTURE.md**  
For test details, see **Assets/Tests/README.md**


