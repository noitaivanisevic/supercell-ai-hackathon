# RPG Adventure Game - Unity Project

## Project Overview

A 2D RPG adventure game built in Unity featuring character selection, exploration across multiple town and dungeon areas, with a focus on world navigation and character customization.

## Key Features

### Character System
- **6 Playable Characters**: Fighter, Knight, Thief, Beast Class, Vampire, and Archer
- **Unique Stats**: Each character has different HP, Attack, Defense, and Speed stats
- **Persistent Selection**: Character choice is saved using PlayerPrefs and persists across game sessions
- **Dynamic Sprite Loading**: Character sprites automatically apply to all player instances across different areas

### World Exploration
- **Town Areas**: 3 explorable town zones (Town1, Town2, Town3) with Town Map navigation
- **Dungeon System**: 3 dangerous dungeons (Dungeon1, Dungeon2, Dungeon3) with Dungeon Map
- **Area Transitions**: Seamless transitions between areas using trigger zones
- **Camera System**: Dynamic camera that follows the player with smooth movement and configurable bounds

### Visual Systems
- **Character Introduction**: Animated character showcase with fade effects and mist particles
- **Background Switching**: Dynamic backgrounds change based on current game state
- **UI Elements**: Polished user interface with animated buttons and visual feedback
- **Camera Effects**: Smooth camera following with configurable bounds for each area

## Technical Architecture

### Core Manager Systems

#### CharacterManager
- Singleton pattern for global character data
- Manages character selection, stats, and sprite references
- Handles save/load functionality using PlayerPrefs
- Provides health management (TakeDamage, Heal, RestoreHealth)
- Broadcasts sprite updates to all active ApplyCharacterSkin scripts

#### GameStateManager
- Central state machine controlling game flow
- 9 game states: TownMap, Town1-3, DungeonMap, Dungeon1-3, Battle
- Manages scene object activation/deactivation
- Controls background sprite switching
- Handles camera positioning per state

### Component Scripts

#### ApplyCharacterSkin
- Applies selected character sprite to player GameObjects
- Handles character-specific scaling (Fighter vs other classes)
- Adjusts BoxCollider2D to match sprite size
- Compensates for parent object scaling
- Automatically reapplies skin when areas activate

#### PlayerMovement2D
- Rigidbody2D-based movement system
- WASD/Arrow key controls
- Optional horizontal-only movement mode
- State-aware (only moves in exploration states)
- Normalized diagonal movement

#### CameraFollowPlayer
- Smooth camera tracking with lerp
- State-specific player targeting
- Configurable camera bounds (min/max X/Y)
- Works with all 8 exploration areas

### Utility Scripts

#### AreaTransition
- Trigger-based area switching
- Configurable spawn positions
- State transition with cooldown prevention

#### ResetPositionOnEnable
- Resets player position when area activates
- Ensures consistent spawn points

#### CharacterSwitcher
- Character showcase animation system
- Sequential fade in/out with mist effects
- Group display (all 6 characters together)
- Smooth transitions with easing

#### BossDoor
- Triggers special boss area entrance
- Earthquake effect with camera shake
- Background sprite swapping (closed → open door)
- Player movement during cutscene
- Disables player scripts during sequence

## Character Stats

| Character | HP | ATK | SPD | DEF |
|-----------|-----|-----|-----|-----|
| Fighter | 100 | 15 | 2 | 5 |
| Knight | 120 | 18 | 2 | 8 |
| Thief | 80 | 20 | 4 | 3 |
| Beast Class | 150 | 12 | 3 | 6 |
| Vampire | 90 | 22 | 3 | 4 |
| Archer | 75 | 19 | 3 | 4 |

## Controls

### Exploration
- **WASD / Arrow Keys**: Move character
- **Walk into triggers**: Transition between areas

### Debug Controls
- **P Key**: Print current PlayerPrefs data (CharacterManager)
- **R Key**: Force refresh all character sprites (CharacterManager)

## Project Structure

```
Assets/
├── Scripts/
│   ├── Managers/
│   │   ├── CharacterManager.cs
│   │   └── GameStateManager.cs
│   ├── Player/
│   │   ├── ApplyCharacterSkin.cs
│   │   ├── PlayerMovement2D.cs
│   │   └── CharacterMovement.cs
│   ├── UI/
│   │   ├── CharacterSelectionManager.cs
│   │   ├── CharacterSelectionUI.cs
│   │   ├── CharacterCard.cs
│   │   └── ButtonPulse.cs
│   ├── World/
│   │   ├── AreaTransition.cs
│   │   ├── AutoExitArea.cs
│   │   ├── ResetPositionOnEnable.cs
│   │   └── BossDoor.cs
│   ├── Camera/
│   │   ├── CameraFollowPlayer.cs
│   │   └── CameraShake.cs
│   └── Utilities/
│       ├── CharacterSwitcher.cs
│       ├── ClassIntroController.cs
│       └── DungeonGenerator.cs
├── Scenes/
│   ├── MainMenu.unity
│   ├── InitialScene.unity (Character Selection)
│   └── FirstMap.unity (Main Game)
└── Sprites/
    ├── Characters/
    ├── Backgrounds/
    └── UI/
```

## Setup Instructions

### Prerequisites
- Unity 2021.3 or later
- TextMeshPro package

### Initial Setup
1. Open project in Unity
2. Ensure all sprite references are assigned in CharacterManager Inspector
3. Set up GameStateManager with all area GameObjects and backgrounds
4. Configure camera bounds for each area in CameraFollowPlayer

### Scene Setup

#### Character Selection Scene
- CharacterSelectionManager GameObject with script
- Character card UI elements
- Continue button with ButtonPulse effect
- ClassIntroController for animated intro

#### Main Game Scene
- GameStateManager with all area references
- CharacterManager (DontDestroyOnLoad)
- Player GameObjects for each area with:
  - ApplyCharacterSkin
  - PlayerMovement2D
  - Rigidbody2D (Kinematic)
  - BoxCollider2D
  - SpriteRenderer
- Area transition triggers with AreaTransition script
- Background SpriteRenderer for dynamic backgrounds

## Common Issues & Solutions

### Character Sprite Not Applying
- Ensure CharacterManager exists and has DontDestroyOnLoad
- Check that sprites are assigned in CharacterManager Inspector
- Verify ApplyCharacterSkin is on player GameObject
- Press R key to force refresh sprites

### Wrong Character Showing
- Press P key to check saved PlayerPrefs data
- Clear PlayerPrefs: `PlayerPrefs.DeleteAll()` in Unity Console
- Verify CharacterManager resets values in Awake()

### Camera Not Following
- Check CameraFollowPlayer has correct player references for each state
- Ensure player transforms are assigned in Inspector
- Verify GameStateManager is switching states correctly

### Area Transitions Not Working
- Verify AreaTransition components have proper BoxCollider2D (Is Trigger = true)
- Check that player has "Player" tag assigned
- Ensure target area is correctly set in AreaTransition inspector

## Debug Features

### Built-in Testing
- **P Key**: View saved character data
- **R Key**: Refresh character sprites

### Console Logging
- Extensive debug logs track:
  - Character selection and loading
  - State transitions
  - Sprite application
  - Area transitions

## Screenshots

Example structure:

### Character Selection
![Character Selection Screen](Images/characters.png)
*Choose from 6 unique character classes*

### Character Introduction
![Character Showcase](Images/Title.png)
*Animated character introduction sequence*

### Town Exploration
![Town Area](Images/City_map.png)

*Explore peaceful town areas*

### Town Map
![Town Map 1](Images/panorama_city_zone_1.jpg)
![Town Map 2](Images/panorama_city_zone_3.jpg)
*Navigate between different town locations*

### Dungeon Exploration
![Dungeon Area](Images/dark_dungeon_map.jpg)
*Venture into dangerous dungeons*

### Dungeon Map
![Dungeon Map 1](Images/panorama_dungeon_zone_1.jpg)
![Dungeon Map 2](Images/panorama_dungeon_zone_3.jpg)
*Navigate between different Dungeon locations*

### Boss Door
![Boss Entrance](Images/panorama_dungeon_zone_2.jpg)
*Dramatic boss area entrance with earthquake effects*


## Future Improvements

### Planned Features
- [ ] Combat system with turn-based battles
- [ ] Enemy encounters and AI
- [ ] Inventory system with items and equipment
- [ ] Quest/mission system
- [ ] Save/Load game state (beyond character selection)
- [ ] Experience and leveling system
- [ ] Sound effects and background music
- [ ] More dungeon areas and puzzles
- [ ] Interactive objects and collectibles

### Technical Improvements
- [ ] Scene additive loading for smoother transitions
- [ ] Scriptable Objects for character data
- [ ] Event system to reduce coupling between managers
- [ ] Input system package integration
- [ ] Mobile touch controls
- [ ] Minimap system

## Contributing

If you'd like to contribute to this project:
1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## License

*MIT license*

## Credits

### Development Team
- *Kosta Ljubisavljević*
- *Ricardo Rino de Sousa*
- *Noah Ivanisevic*

---

**Version**: 1.0  
**Last Updated**: 2025  
**Unity Version**: 2021.3+  
**Platform**: PC (Windows/Mac/Linux)

---

## Contact

For questions or support:
- Email: [noitaivanisevic49@gmail.com](mailto:noitaivanisevic49@gmail.com)
- GitHub: [View Repository](https://github.com/noitaivanisevic/supercell-ai-hackathon)
- Portfolio: [View my Portfolio]()