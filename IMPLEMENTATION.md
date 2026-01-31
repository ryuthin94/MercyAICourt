# Implementation Details - Mercy AI Court Core Systems

## Architecture Overview

The Mercy AI Court game uses a modular architecture with clear separation of concerns:

### System Architecture

```
GameManager (Singleton)
    ├── TimerSystem
    ├── GuiltSystem
    └── GameStateManager

UIManager
    ├── Subscribes to all system events
    └── Updates UI based on system state

TestingPanel
    └── Provides debug controls for testing
```

## Core Systems

### 1. TimerSystem (`Assets/Scripts/Systems/TimerSystem.cs`)

**Responsibilities:**
- Count down from 90 in-game minutes to 0:00
- Map real-world time to game time (1 real second = 18 game seconds)
- Support pause/resume functionality
- Trigger events when timer expires

**Key Features:**
- Event-driven: `OnTimerExpired`, `OnTimerUpdated`
- Supports time manipulation for testing
- Formatted display output (MM:SS)

**Technical Details:**
- Initial time: 5400 seconds (90 minutes)
- Conversion rate: 18 game seconds per real second
- This means 5 real minutes = 300 real seconds × 18 = 5400 game seconds = 90 game minutes

### 2. GuiltSystem (`Assets/Scripts/Systems/GuiltSystem.cs`)

**Responsibilities:**
- Track player guilt percentage (0-100%)
- Start at 98% guilt
- Determine victory condition (guilt < 92%)
- Provide color coding for UI

**Key Features:**
- Event-driven: `OnGuiltChanged`
- Clamped values (0-100%)
- Color coding logic:
  - Red (Color.red): guilt >= 90%
  - Yellow (Color.yellow): 50% <= guilt < 90%
  - Green (Color.green): guilt < 50%
- Victory condition check: guilt < 92%

**Methods:**
- `SetGuilt(percentage)`: Set guilt to specific value
- `IncreaseGuilt(amount)`: Add to current guilt
- `DecreaseGuilt(amount)`: Subtract from current guilt
- `GetGuiltColor()`: Returns appropriate color for current guilt
- `GetNormalizedGuilt()`: Returns 0-1 value for UI bars
- `IsVictoryConditionMet()`: Check if player has won

### 3. GameStateManager (`Assets/Scripts/Systems/GameStateManager.cs`)

**Responsibilities:**
- Manage game state transitions
- Track current game state
- Notify listeners of state changes

**States:**
- `MainMenu`: Initial state, shows menu
- `Interrogation`: Active gameplay state
- `Victory`: Player won (guilt < 92%)
- `GameOver`: Player lost (timer expired with guilt >= 92%)

**Key Features:**
- Event-driven: `OnStateChanged`
- State validation (prevents duplicate state changes)
- Helper methods: `IsPlaying()`, `IsGameOver()`

### 4. GameManager (`Assets/Scripts/Managers/GameManager.cs`)

**Responsibilities:**
- Central coordinator for all systems
- Singleton pattern for global access
- Handle game lifecycle (start, restart, pause)
- Connect system events to game logic

**Key Features:**
- Singleton implementation with DontDestroyOnLoad
- Initializes all core systems
- Subscribes to system events:
  - Timer expiration → Check lose condition
  - Guilt changes → Check win condition
- Update loop: Updates TimerSystem when in Interrogation state

**Public Methods:**
- `StartGame()`: Reset systems and start interrogation
- `RestartGame()`: Restart from beginning
- `PauseGame()`: Pause timer
- `ResumeGame()`: Resume timer
- `TogglePause()`: Toggle pause state
- `Victory()`: Transition to victory state
- `GameOver()`: Transition to game over state
- `ReturnToMainMenu()`: Go back to main menu

### 5. UIManager (`Assets/Scripts/UI/UIManager.cs`)

**Responsibilities:**
- Display all UI elements
- Update displays based on system events
- Handle UI button callbacks
- Manage screen transitions

**UI Elements:**
- Title text: "MERCY AI COURT SYSTEM"
- Timer display: Shows formatted time (MM:SS)
- Guilt percentage: Shows guilt with color coding
- Guilt meter: Visual bar representation
- Screen panels: MainMenu, Interrogation, Victory, GameOver

**Event Subscriptions:**
- `TimerSystem.OnTimerUpdated` → Update timer display
- `GuiltSystem.OnGuiltChanged` → Update guilt display and meter
- `GameStateManager.OnStateChanged` → Switch between screens

### 6. TestingPanel (`Assets/Scripts/UI/TestingPanel.cs`)

**Responsibilities:**
- Provide debug controls for testing
- Toggle visibility with 'T' key
- Display current game state

**Controls:**
- Guilt +/-: Adjust guilt by ±5%
- Time +/-: Add/subtract 60 game seconds
- Win: Set conditions for instant victory
- Lose: Set conditions for instant defeat
- Reset: Restart game
- Pause: Toggle pause state

**Status Display:**
- Current state
- Current guilt percentage
- Current time remaining
- Pause status

## Win/Lose Logic

### Win Condition
The player wins when guilt drops below 92% at ANY time during the game:

```csharp
// Triggered whenever guilt changes
private void OnGuiltChanged(float newGuilt)
{
    if (stateManager.IsPlaying() && guiltSystem.IsVictoryConditionMet())
    {
        Victory(); // guilt < 92%
    }
}
```

### Lose Condition
The player loses when the timer reaches 0:00 while guilt is still >= 92%:

```csharp
// Triggered when timer expires
private void OnTimerExpired()
{
    if (!guiltSystem.IsVictoryConditionMet())
    {
        GameOver(); // guilt >= 92%
    }
}
```

## Scene Setup

### Automatic Setup
Run `Tools > Mercy AI Court > Setup Scene` in Unity Editor to automatically create:
- GameManager GameObject
- Canvas with proper scaling
- All UI panels and elements
- Camera with black background
- Testing panel
- All component references and connections

### Manual Setup
If automatic setup fails, follow the structure in `SceneSetup.cs` to manually create:
1. Canvas (ScreenSpace-Overlay, 1920x1080 reference)
2. Background (black)
3. Four panels: MainMenu, Interrogation, Victory, GameOver
4. UI elements in each panel
5. Testing panel
6. Component references

## Event Flow

### Game Start Flow
```
Player clicks "START INTERROGATION"
    → UIManager.OnStartGameButton()
    → GameManager.StartGame()
    → TimerSystem.ResetTimer()
    → GuiltSystem.ResetGuilt()
    → GameStateManager.ChangeState(Interrogation)
    → UIManager updates to show interrogation panel
```

### Victory Flow
```
Guilt decreases below 92%
    → GuiltSystem.OnGuiltChanged event
    → GameManager.OnGuiltChanged()
    → Check IsVictoryConditionMet()
    → GameManager.Victory()
    → GameStateManager.ChangeState(Victory)
    → UIManager shows victory screen
```

### Defeat Flow
```
Timer reaches 0:00
    → TimerSystem.OnTimerExpired event
    → GameManager.OnTimerExpired()
    → Check if guilt >= 92%
    → GameManager.GameOver()
    → GameStateManager.ChangeState(GameOver)
    → UIManager shows game over screen
```

## Testing Strategy

### Unit Testing
`CoreSystemsTest.cs` provides basic unit tests for core logic:
- Timer countdown and formatting
- Guilt percentage and color coding
- State transitions
- Win/lose condition logic

Run tests in Unity Test Runner or adapt for standalone testing.

### Integration Testing
Use the Testing Panel (T key) to verify:
- Timer countdown is smooth and accurate
- Guilt changes update UI immediately
- Win condition triggers at guilt < 92%
- Lose condition triggers at timer = 0:00 with guilt >= 92%
- State transitions work correctly
- Pause functionality works
- Reset functionality works

### Visual Testing
Verify:
- Black background (#000000)
- White text is readable
- Guilt colors change correctly:
  - Red at 90%+
  - Yellow at 50-89%
  - Green below 50%
- Guilt meter fills proportionally
- All screens display correctly
- UI scales properly on different resolutions

## Extension Points

The current implementation is designed to be extended in future phases:

### Phase 2 Extensions
- **Dialogue System**: Add dialogue choices that affect guilt
  - Use `GuiltSystem.IncreaseGuilt()` and `DecreaseGuilt()`
  - Integrate with existing pause system during dialogue

- **Evidence Viewer**: Display evidence that impacts case
  - Can affect guilt when viewed
  - May add time penalties/bonuses

- **AI Personality**: Mercy AI responses and behavior
  - React to guilt levels
  - Dynamic dialogue based on time remaining

### Phase 3 Extensions
- **Multiple Scenarios**: Different cases and storylines
  - Each scenario can have different initial guilt
  - Different time limits
  - Different victory thresholds

- **Save System**: Save/load game progress
  - Save current state, timer, and guilt
  - Load previous games

- **Plot Twists**: Mid-game events that change parameters
  - Sudden guilt changes
  - Time extensions or reductions
  - State-based events

## Performance Considerations

- Event-based architecture minimizes Update() calls
- Only GameManager has Update() to tick TimerSystem
- UI updates only when values change (event-driven)
- No expensive operations in Update loops
- Singleton pattern prevents duplicate managers

## Code Style

All code follows these conventions:
- C# naming conventions (PascalCase for public, camelCase for private)
- XML documentation comments for classes and public methods
- Clear namespace organization
- Event-driven communication between systems
- Dependency injection where appropriate
- Single Responsibility Principle for each system

## Future Improvements

Potential enhancements for future phases:
- Add audio system (tick sounds, warning sounds)
- Add visual effects (screen shake, guilt meter animations)
- Add particle effects for victory/defeat
- Implement screen transitions/fades
- Add settings panel (volume, difficulty)
- Implement analytics/statistics tracking
- Add achievements system
- Implement difficulty levels (different time/guilt thresholds)
