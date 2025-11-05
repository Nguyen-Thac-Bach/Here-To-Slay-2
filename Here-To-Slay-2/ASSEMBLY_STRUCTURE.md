# Assembly Definition Structure

This document explains the assembly organization and dependency hierarchy.

## Assembly Layout

```
Assets/Scripts/
├── Core.asmdef                    ← Pure game logic (testable)
│   ├── Components/
│   │   ├── BaseCard.cs
│   │   ├── HeroCard.cs
│   │   ├── Effect.cs
│   │   ├── Enums/
│   │   │   ├── Deck.cs
│   │   │   ├── Player.cs
│   │   │   ├── GamePhase.cs
│   │   │   ├── CardType.cs
│   │   │   ├── HeroClass.cs
│   │   │   └── AtomicCardEffect.cs
│   │   └── CustomEventArgs/
│   │       ├── CardMovedEventArgs.cs
│   │       ├── PlayerChangedEventArgs.cs
│   │       ├── PhaseChangedEventArgs.cs
│   │       ├── ActionUsedEventArgs.cs
│   │       ├── TopCardInDrawDeckChangedEventArgs.cs
│   │       └── ... (other event args)
│   └── Model/
│       └── GameState.cs
│
├── Util/
│   ├── GameServices.asmdef        ← Services layer (references Core)
│   ├── HeroJSON.cs
│   ├── Heroes.cs
│   ├── DataPath.cs
│   └── RenameButton.cs
│
├── Model/
│   ├── Services/
│   │   └── CardJsonLoader.cs
│   └── CreateCardsFromJson.cs
│
├── ViewModel/
│   ├── ViewModel.asmdef           ← View model layer (references Core, GameServices)
│   └── GameViewModel.cs
│
├── View/
│   ├── View.asmdef                ← UI layer (references Core, ViewModel)
│   ├── GameView.cs
│   ├── GameStateUI.cs
│   ├── BaseCardUI.cs
│   ├── HeroCardUI.cs
│   ├── HeroCardClickManager.cs
│   ├── CardMoverView.cs
│   ├── CardMoveAnimation.cs
│   └── ... (other UI components)
│
└── App/
    ├── App.asmdef                 ← Coordinator (references all layers)
    └── App.cs
```

## Assembly Definitions

### **Core** (Assets/Scripts/Core.asmdef)
**Contents:** Components, Model, and CustomEventArgs  
**Dependencies:** None  
**Key files:** GameState.cs, BaseCard.cs, HeroCard.cs, all enums, event args

**Why:** Contains pure game logic with NO Unity dependencies. Makes these classes testable in edit mode.

### **GameServices** (Assets/Scripts/Util/GameServices.asmdef)
**Contents:** Util, Model.Services, Utility classes  
**Dependencies:** Core  
**Key files:** CardJsonLoader.cs, HeroJSON.cs, Heroes.cs

**Why:** Handles data loading and services. Depends on Core for game logic.

### **ViewModel** (Assets/Scripts/ViewModel/ViewModel.asmdef)
**Contents:** ViewModel layer  
**Dependencies:** Core, GameServices  
**Key files:** GameViewModel.cs

**Why:** Bridges Model and View. Can access Core logic and services.

### **View** (Assets/Scripts/View/View.asmdef)
**Contents:** All UI and display code  
**Dependencies:** Core, ViewModel  
**Key files:** GameView.cs, GameStateUI.cs, card UI classes

**Why:** UI layer that uses Core data and subscribes to ViewModel events.

### **App** (Assets/Scripts/App/App.asmdef)
**Contents:** App coordinator  
**Dependencies:** Core, GameServices, ViewModel, View  
**Key files:** App.cs

**Why:** MonoBehaviour that initializes and wires everything together.

### **Tests** (Assets/Tests/Tests.asmdef)
**Contents:** Unit tests  
**Dependencies:** Core ONLY  
**Key files:** GameStateTests.cs, TestCardFactory.cs

**Why:** Test assembly only references Core for pure logic testing. Cannot reference View or ViewModel to avoid UI dependencies.

## Dependency Graph

```
Tests ──────────┐
                │
            ┌───▼──────┐
            │   CORE   │
            └───┬──────┘
        ┌───────┼───────┐
        │               │
    ┌───▼─────┐     ┌──▼────────┐
    │ GameSrv │     │ ViewModel │
    └────┬────┘     └──┬────────┘
         │             │
         │         ┌───▼──┐
         └────┬────┤ View │
              │    └──────┘
         ┌────▼────┐
         │   App   │
         └─────────┘
```

**Key rules:**
- ✅ Core has NO dependencies
- ✅ Tests only reference Core
- ✅ Lower layers cannot reference higher layers
- ✅ Dependencies flow downward only (no circular references)

## Migration Notes

**Before:** All code in default Assembly-CSharp.dll  
**After:** Code organized into 6 focused assemblies

**Impact:**
- ✅ Tests can now run without launching game
- ✅ Clear separation of concerns
- ✅ Easier to refactor and maintain
- ✅ Faster compile times for individual layers
- ✅ Better IDE intellisense and navigation

## How to Add New Code

1. **Pure game logic** → Add to Components/ or Model/ → Part of **Core**
2. **Services (JSON loading, etc)** → Add to Util/ or Model/Services/ → Part of **GameServices**
3. **UI Components** → Add to View/ → Part of **View**
4. **View Models** → Add to ViewModel/ → Part of **ViewModel**
5. **Event handlers, data classes** → Add to Components/CustomEventArgs/ → Part of **Core**

## Troubleshooting

**Assembly conflicts or compilation errors:**
1. Restart Unity or reimport all assets: Assets → Reimport All
2. Check that you haven't created circular references
3. Verify the dependency chain matches the diagram above

**Tests still can't access code:**
1. Ensure code is in Core.asmdef folder structure
2. Verify Core.asmdef exists and is properly configured
3. Check that Tests.asmdef references "Core"


