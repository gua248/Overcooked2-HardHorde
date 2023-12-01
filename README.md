# Overcooked! 2 - HardHorde MOD

Make Horde Great Again!

## Installation

1. Install [BepInEx 5 (x86)](https://github.com/BepInEx/BepInEx/releases) for the game
2. Copy `bin/Release/OC2HardHorde.dll` to the game's `BepInEx/plugins/` folder

> ### Compiling
>
> You may compile the MOD yourself. The following dependencies need to be copied into `lib/` directory: 
>
> - In the game's `Overcooked2_Data/Managed/` directory `Assembly-CSharp.dll`, `UnityEngine.dll`, `UnityEngine.CoreModule.dll`, `UnityEngine.UI.dll`.
> - In `BepInEx/core/` directory `0Harmony.dll`, `BepInEx.dll`, `BepInEx.Harmony.dll`.



## How to use

- Enable in the game main menu Settings - MODs (disabled by default).

- Only requires the host to enable to take effect (including arcade mode).

- Modifications:
  > - All enemies eat one extra order (2 orders for zombie breads and chilis, 3 orders for apples).
  > - At the end of each wave, start the next wave without waiting for all enemies to be defeated.
  > - The menu generation rule changes from independent random to normal level's generation rule.

