# Mercy AI Court - Setup Instructions

## Overview
This repository contains the core systems for the Mercy AI Court game - a narrative interrogation game where players must reduce their guilt percentage before time runs out.

## Unity Setup

### Prerequisites
- Unity 6000.3.6f1 or compatible version
- TextMeshPro package (included with Unity 6)

### Scene Setup

1. **Open the project in Unity Editor**
2. **Run the Scene Setup Tool**:
   - Go to `Tools > Mercy AI Court > Setup Scene` in the Unity menu
   - This will automatically create all UI elements and configure the scene

3. **Save the scene**:
   - File > Save Scene
   - The scene will be ready to play

### Alternative Manual Setup
If you prefer to set up the scene manually or the automatic setup doesn't work:

1. Create a new scene or open `Assets/Scenes/SampleScene.unity`
2. Add the following GameObjects:
   - **GameManager** (Add component: `GameManager.cs`)
   - **Canvas** (UI > Canvas)
     - Set Canvas Scaler to "Scale With Screen Size"
     - Reference Resolution: 1920x1080
   - **Main Camera**
     - Set Clear Flags to "Solid Color"
     - Background Color: Black
     - Camera Type: Orthographic

3. Set up the UI hierarchy under Canvas (see detailed UI structure in SceneSetup.cs)

## Game Systems

### Core Systems
- **TimerSystem**: Manages countdown timer (90 game minutes = 5 real minutes)
- **GuiltSystem**: Tracks guilt percentage (starts at 98%, win condition < 92%)
- **GameStateManager**: Handles game states (MainMenu, Interrogation, Victory, GameOver)
- **GameManager**: Singleton that coordinates all systems

### Win/Lose Conditions
- **Win**: Reduce guilt below 92% at any time during the 90 minutes
- **Lose**: Timer reaches 0:00 while guilt is still >= 92%

## Testing

### Testing Panel
Press `T` key during gameplay to open the testing panel with the following controls:
- **Guilt +/-**: Adjust guilt by ±5%
- **Time +/-**: Add/subtract 60 game seconds (1 game minute)
- **Win**: Instantly set guilt to 90% (below win threshold)
- **Lose**: Set guilt to 95% and expire the timer
- **Reset**: Restart the game
- **Pause**: Toggle pause state

### Playing the Game
1. Press Play in Unity Editor
2. Click "START INTERROGATION" on the main menu
3. Watch the timer count down and guilt percentage
4. Use the testing panel (T key) to test various scenarios

## File Structure
```
Assets/
├── Scenes/
│   └── SampleScene.unity
└── Scripts/
    ├── Managers/
    │   └── GameManager.cs          # Central singleton manager
    ├── Systems/
    │   ├── TimerSystem.cs          # Timer countdown logic
    │   ├── GuiltSystem.cs          # Guilt tracking and color coding
    │   └── GameStateManager.cs     # State machine
    └── UI/
        ├── UIManager.cs             # UI display updates
        ├── TestingPanel.cs          # Debug panel for testing
        └── SceneSetup.cs            # Editor tool for scene setup
```

## Visual Design
- **Background**: Pure black (#000000)
- **Primary Text**: White (#FFFFFF)
- **Guilt Colors**:
  - Red: 90%+ (high guilt)
  - Yellow: 50-89% (medium guilt)
  - Green: <50% (low guilt)
- **UI Style**: Minimalist, clean, futuristic

## Next Steps
This implementation covers Phase 1: Core Foundation Systems. Future phases will add:
- Phase 2: Dialogue system, evidence viewer, AI personality
- Phase 3: Multiple scenarios, plot twists, save system

## Troubleshooting

### Scene Setup Tool Not Found
- Make sure all scripts are compiled (check Console for errors)
- The menu item appears under `Tools > Mercy AI Court > Setup Scene`

### Missing References
- If UI elements are not linked properly, use the Inspector to manually assign references in UIManager and TestingPanel components

### TextMeshPro Missing
- Import TextMeshPro from Window > TextMeshPro > Import TMP Essential Resources
