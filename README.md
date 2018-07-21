# Project_Night

## Outline
```
Move player to safe house. During the way, there are some monster that approach to player for attack. Player can use lantern to protect. If you find treasurebox, you can get some item that help you.
```
---
## Term
+ 2018.02.03~ (~ing)
---
## Member
+ Park Sangjoon
+ Park Yehun
---
## Tool
+ Unity
+ Blender
---
## Update Note
>+ ## __2018.02.07__
>    ```
>    > Create Team repository
>    ```
>+ ## __2018.02.13__
>    ```
>    > Create Terrain Map(Tree, Ground, Sky etc)
>    > Create Object(House, Lantern)
>    > Create Basic UI
>    ```
>+ ## __2018.02.20__
>    ```
>    > Create Inventory Trigger
>    > Make Objects(TreasureBox, Battery, Potion)
>    > Object Random Initiate
>    ```
>+ ## __2018.02.27__
>   ```
>   > [ Player patch ]
>    + Player Weapon(Lantern Light) patch
>       - when monster get in the Lantern light, it will be taken DOT
>
>   > [ Monster patch ]
>    + Create in random range(Player).
>    + Attack Player periodically
>
>   > [ UI & System patch ]
>    + Synchronize game data with UI(HP, Lantern)
>
>   > [ Terrain patch ]
>    + Make terrains(Stage2, 3)
>
>   > [ Scene patch ]
>    + Make Scene - Intro, Clear, GameOver.
>   ```
>+ ## __2018.03.06__
>   ```
>   > [ Scene patch ]
>    + Syschronize each scenes - Intro -> Main -> Clear or GameOver
>
>   > [ Player patch ]
>    + Apply environment effect
>       - Decline player HP periodically because of body temperature. It is very cold in night forest.
>   ```
>+ ## __2018.03.20__
>   ```
>   > [ Monster Patch ]
>    + Add Unity Navmesh for tracking down player
>   ```
>+ ## __2018.04.03__
>   ```
>   > [ Object Patch ]
>    + Add Object - StreetLight
>    + Include Trigger(StreetLight)
>       - Switch On/Off
>   ```
>+ ## __2018.05.15__
>   ```
>   > [ Player Patch ]
>    + Modify Script
>       - Player Controller(about move)
>
>   > [ Monster Patch ]
>    + Modify Script
>       - Attack judgment
>       - Monster respawn
>   ```
>+ ## __2018.06.05__
>   ```
>   > [ Object Patch ]
>    + StreetLight - Add Flickering
>       - According to battery amount, the light will flicker.
>   ```
>+ ## __2018.06.12__
>   ```
>   > [ UI Patch ]
>    + Apply animation, highlight etc.
>   ```
>+ ## __2018.06.26__
>   ```
>   > [ Object Patch ]
>    < TreasureBox >
>     + Modify script
>     + Add Animation
>       - Box Open, Close
>    < Lantern >
>     + Modify script
>
>   > [ System Patch ]
>     + Apply Inventory system (main inventory, treasurebox, lantern)
>   ```
>+ ## __2018.07.03__
>   > __[ Object Patch ]__
>   ```
>   < TreasureBox >
>    + Modify script
>   ```
>   > __[System Patch]__
>   ```
>   < Inventory >
>    + Optimize script
>    + Synchronize with treasurebox object in real time
>    + Synchronize with lantern object in real time
>   ```
>+ ## __2018.07.10__
>   > __[ System Patch ]__
>    ```
>    < Common >
>    + Apply Sound System.
>
>    < TreasureBox >
>    + Create Objects randomly in map
>
>    < Inventory >
>     + Optimize script
>   ```
>   > __[ UI Patch ]__
>   ```
>   < Button >
>    + Image change
>      - Start, Exit, Retry buttons
>   ```
>   > __[ Sound Patch ]__
>   ```
>    + Add Objects sounds - Box, TreasureBox, Lantern, StreetLight, Monster etc.
>    + Add Background sounds - Intro, Clear, Gameover Scenes
>   ```