# AutoAim

This is a game mod developed for automatic aiming, using the Harmony library for method patching, enabling the player to automatically aim at the nearest enemy when pressing the left mouse button.

## Features

- Automatically detects and locks onto the nearest enemy
- Supports two methods for obtaining the enemy's head position
- Can fall back to aiming at the body center

## Technologies Used

- C#
- Harmony library for method patching
- Unity engine

## Main Classes and Methods

- `ModBehaviour`: Inherits from `Duckov.Modding.ModBehaviour`, responsible for initializing and unloading the mod.
- `AutoAimPatch`: Uses Harmony to patch the `InputManager.SetAimInputUsingMouse` method, implementing the auto aim logic.
- `GetEnemyHeadPosition`: Attempts to obtain the enemy's head position via the `HeadCollider` component or ray detection.

## Installation

1. Place the `AutoAim.dll` and any required dependencies into the game's plugins directory.
2. Ensure that the Harmony library is installed.
3. Launch the game; the mod will load automatically.

## Contributions

Code contributions and suggestions for improvements are welcome. Please follow the project's coding standards and submission guidelines.

## License

This project is licensed under the MIT License. For details, please see the LICENSE file located in the project root directory.