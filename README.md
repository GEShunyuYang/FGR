# FGR

FGR indicates course: Fundamentals of Games Research, Development and Management in University of Warwick. Game Title: Reaching the IcePeak.

This is a Unity turn-based card battle Game. The project combines grid-based tactical movement, deck construction, card effects, enemy AI, stylized rendering, procedural environment placement, weather effects, and runtime UI/tutorial systems.

## Setup Instructions

### For Directly Game Play

1. Enter Github Release page. 
2. Download ReachingIcePeak_1.0.0
3. Unzip in your computer.
4. Double click the .exe file in folder.

### For Unity project

1. Install Unity `2022.3.62f2`.
2. Clone or download this repository.
3. Open the project folder in Unity Hub:
   ```text
   E:/unity/Projects/FGR
   ```
4. Let Unity restore packages from `Packages/manifest.json`.
5. Open the start scene:
   ```text
   Assets/Scenes/GameStart.unity
   ```
6. Confirm the build scenes are enabled in this order:
   ```text
   Assets/Scenes/GameStart.unity
   Assets/Scenes/TeachingScene.unity
   Assets/Scenes/BattleScene1.unity
   Assets/Scenes/BattleScene2.unity
   Assets/Scenes/BattleScene3.unity
   Assets/Scenes/Transition.unity
   Assets/Scenes/GameEnd.unity
   ```
7. Press Play from `GameStart.unity`.

To create a build, open:

```text
File -> Build Profiles
```

Select the target platform, then build with `GameStart` as the first scene.

## Dependencies

Main Unity package dependencies:

- Unity `2022.3.62f2`
- Universal Render Pipeline `14.0.12`
- Shader Graph `14.0.12`
- TextMeshPro `3.0.9`
- uGUI `1.0.0`
- Input System `1.14.0`
- ProBuilder `5.2.4`
- Post Processing `3.4.0`
- Timeline `1.7.7`
- Visual Scripting `1.9.4`

The project also uses built-in Unity modules for animation, audio, physics, particles, UI, terrain, video, and related runtime systems.

## External Assets And Credits

External or imported assets are stored mainly under:

```text
Assets/Import
Assets/Audio
```

Current imported asset categories include:

- Character, monster, chicken, weapon, tree, grass, stone, fence, wall, road, and terrain meshes.
- Character and monster animation clips, including humanoid-style attack, idle, walk, damage, and death animations.
- Stylized vegetation, stone, snow, water, weapon, UI, and card textures.
- Background music and scene audio files under `Assets/Audio`.

External Assets Source:

Clean Settings UI: https://assetstore.unity.com/packages/tools/gui/clean-settings-ui-65588  
AQUIS - Water Toon Shader (Orto & Perspective): https://assetstore.unity.com/packages/vfx/shaders/aquis-water-toon-shader-orto-perspective-297566  
Nature Pack - Low Poly Trees & Bushes: https://assetstore.unity.com/packages/3d/vegetation/nature-pack-low-poly-trees-bushes-210184  
Snowy Low-Poly Trees: https://assetstore.unity.com/packages/3d/vegetation/trees/snowy-low-poly-trees-76796  
Slavic Medieval Village Free – Modular Environment Kit: https://assetstore.unity.com/packages/3d/environments/fantasy/slavic-medieval-village-free-modular-environment-kit-167010  
Mountain - Stylized Fantasy Environment: https://assetstore.unity.com/packages/3d/environments/landscapes/mountain-stylized-fantasy-environment-307488  
Low Poly Stones: https://assetstore.unity.com/packages/3d/props/low-poly-stones-298380  
Health System For Dummies: https://assetstore.unity.com/packages/tools/utilities/health-system-for-dummies-215755  
Animals FREE - Animated Low Poly 3D Models: https://assetstore.unity.com/packages/3d/characters/animals/animals-free-animated-low-poly-3d-models-260727  
Devil animated character: https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/devil-animated-character-60777  
Fantasy Monster Evolution Pack 40-41–42 – Game Ready Creatures | PixeliusVita: https://assetstore.unity.com/packages/3d/characters/creatures/fantasy-monster-evolution-pack-40-41-42-game-ready-creatures-pix-382222  
3D Fantasy Wood/Beginner Fantasy Weapon: https://assetstore.unity.com/packages/3d/props/weapons/3d-fantasy-wood-beginner-fantasy-weapon-278607  
Fantasy character: https://assetstore.unity.com/packages/3d/characters/fantasy-character-363582  
Low Poly Environment - Nature Free - LOWPOLY MEDIEVAL FANTASY SERIES: https://assetstore.unity.com/packages/3d/environments/low-poly-environment-nature-free-lowpoly-medieval-fantasy-series-187052  

Some Animations are from Mixamo

Some textures are from ChatGPT-5.5

Audio:

RanceX OST - 作戦フェイズ  
RanceX OST - 首都  
RanceX OST - 魔物界  
RanceX OST - Unique Battle  
RanceX OST - 決戦  
RanceX OST - the end  
```
