# Quick Start Guide - Mercy AI Court

## ⚡ Getting Started (5 minutes)

### Step 1: Open in Unity
1. Open Unity Hub
2. Add the project from this directory
3. Use Unity version **6000.3.6f1** (or compatible)
4. Wait for Unity to import assets

### Step 2: Setup the Scene
1. In Unity Editor, go to menu: **Tools > Mercy AI Court > Setup Scene**
2. This automatically creates all UI elements and game objects
3. Save the scene (Ctrl+S / Cmd+S)

### Step 3: Play!
1. Press the **Play** button in Unity
2. Click "START INTERROGATION"
3. Watch the timer count down and guilt percentage
4. Press **T** to open the Testing Panel

---

## 🎮 Testing the Game

### Testing Panel Controls (Press T)
- **Guilt +/-**: Adjust guilt by ±5%
- **Time +/-**: Add/subtract 60 game seconds
- **Win**: Set guilt to 90% (instant victory)
- **Lose**: Set time to 0 with high guilt (instant defeat)
- **Reset**: Restart the game
- **Pause**: Toggle pause state

### What to Test
1. ✅ Timer counts down smoothly from 90:00
2. ✅ Guilt starts at 98% (red color)
3. ✅ Decrease guilt below 92% → Victory screen appears
4. ✅ Let timer reach 0:00 with guilt ≥ 92% → Game Over screen
5. ✅ Guilt meter fills proportionally
6. ✅ Colors change: Red (90%+), Yellow (50-89%), Green (<50%)

---

## 📖 Interactive Demo (No Unity Required)

Open `UI_MOCKUP.html` in your web browser to see a working demo of the UI:
- All screens (menu, gameplay, victory, game over)
- Working timer countdown
- Functional testing panel
- Color-coded guilt display

This is a perfect preview before opening in Unity!

---

## 🏗️ What's Implemented

### ✅ Core Systems (All Complete)
- **TimerSystem**: 90 game minutes = 5 real minutes
- **GuiltSystem**: 0-100% with color coding
- **GameStateManager**: Menu → Interrogation → Victory/GameOver
- **GameManager**: Singleton coordinating all systems
- **UIManager**: Event-driven display updates
- **TestingPanel**: Debug tools for testing

### ✅ Win/Lose Conditions
- **Win**: Guilt drops below 92% at any time
- **Lose**: Timer reaches 0:00 while guilt ≥ 92%

### ✅ Visual Design
- Black background (#000000)
- White text for readability
- Red/Yellow/Green guilt indicators
- Minimalist, futuristic aesthetic

---

## 📁 Project Structure

```
Assets/
  Scripts/
    Managers/
      GameManager.cs          ← Central coordinator (Singleton)
    Systems/
      TimerSystem.cs          ← Countdown logic
      GuiltSystem.cs          ← Guilt tracking
      GameStateManager.cs     ← State machine
    UI/
      UIManager.cs            ← Display updates
      TestingPanel.cs         ← Debug panel
      SceneSetup.cs           ← Auto-setup tool
    Tests/
      CoreSystemsTest.cs      ← Unit tests
```

---

## 🐛 Troubleshooting

### "Setup Scene" menu item not appearing
- Make sure all scripts have compiled (check Console for errors)
- Reimport all scripts: Right-click Scripts folder → Reimport

### Missing TextMeshPro resources
- Go to: Window > TextMeshPro > Import TMP Essential Resources
- Click "Import"

### UI elements not linked
- Run the Setup Scene tool again
- Or manually assign references in Inspector on UIManager and TestingPanel

### Timer not counting down
- Make sure you clicked "START INTERROGATION"
- Check that the game is in Play mode
- Verify GameManager exists in the scene hierarchy

---

## 🎯 Next Steps

### Ready for Phase 2!
The core foundation is complete. Future phases can add:
- **Dialogue System**: Choices that affect guilt
- **Evidence Viewer**: Documents and clues
- **AI Personality**: Mercy's responses and behavior

### How to Extend
All systems use events, so adding new features is easy:
```csharp
// Subscribe to guilt changes
GameManager.Instance.GuiltSystem.OnGuiltChanged += MyDialogueResponse;

// Modify guilt based on player choices
GameManager.Instance.GuiltSystem.DecreaseGuilt(5f);
```

---

## 📚 Documentation

- **README.md**: Full setup instructions
- **IMPLEMENTATION.md**: Technical architecture details
- **UI_MOCKUP.html**: Interactive browser demo
- **Code Comments**: All classes have XML documentation

---

## ✨ Features Highlights

1. **Event-Driven Architecture**: Systems communicate via events (loose coupling)
2. **Modular Design**: Each system is independent
3. **Testing-Friendly**: Comprehensive testing panel
4. **Well-Documented**: Comments, README, implementation guide
5. **Production-Ready**: Follows Unity best practices

---

## 🎉 You're All Set!

Run the Setup Scene tool, press Play, and enjoy testing your Mercy AI Court foundation!

For questions or issues, check IMPLEMENTATION.md for detailed technical information.
