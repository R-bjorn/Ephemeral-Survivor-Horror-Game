# COMP-4990 Project Group 19 --> Survival Horror Video Game: "Ephemeral"
## Introduction
> This is the repo for the **`survival horror video game`** we're building for the class COMP-4990 : Project Management.
> 
> This project was carried out through two terms. 

## Game Specification 
### Game Genre:
> Action-Adventure Survivor Horror

### Game Mode: 
> Single Player or Multiplayer

### Game Design and Development:
> This game was developed in the Unity game engine, and using C# programming. It was designed for PC platforms.

### Gameplay:
> The initial plan was for the game to be divided into three different levels, each level with its own unique environment, enemies, and challenges. In the end, we decided to limit it to 1 tutorial level and 1 main level for the alpha version of the game.
> The game is meant to be playable both as a single player and in multiplayer mode.
> - Player has to explore the game world, collecting power crystals while avoiding demon enemies.
> - In the tutorial, the player must collect power crystals, avoid demon minions, and find Artificer to complete the level.
> - In single-player mode, the player must collect enough power crystals and then find the Scourge to win the game.
> - In multiplayer mode, the players must collect enough power crystals between them, and then find and defeat the Scourge's three Generals
> - In addition to the main enemies, there are a variety of minion enemies in each level, each with unique strengths and weaknesses.
> - There are also items that can be collected by players to give them single-use weapons or temporary power-ups.

## Storyline for the game. 
### Story Modes:

> - `Single-player mode`: The realm of Perpetua has been forcefully enslaved by the elemental demon Scourge. Artificer, last of Perpetua's old rulers, has summoned the Champion to liberate his world. The Champion must gain strength by gathering power crystals until they can successfully face the Scourge and take it down - but they must be careful to avoid Scourge's minions if they don't want to become enslaved as well.

> - `Multiplayer mode`: The realm of Perpetua has been forcefully enslaved by the elemental demon Scourge. Artificer, last of Perpetua's old rulers, has summoned the 3 Champions to liberate his world. The Champions must gain strength by gathering power crystals until they can successfully face the Scourge's Generals and take them down - but they must be careful to avoid Scourge's minions until then if they don't want to become enslaved as well.

> - A player can connect with other players online to play this game in a multiplayer mode where they can make a server with other users to develop a fun experience for the game.  

<details><summary>The Top-down view of the storyline</summary>
 
![IMG_0097](https://user-images.githubusercontent.com/81584201/206886670-326b15bd-a2bf-4cb2-9183-719de824adc4.jpg)
</details>

## Designing and Development
### Environment:
> The game world will be a post-apocalyptic environment with a dark and eerie atmosphere. The environment will be filled with abandoned buildings, ruins, and hazardous areas. The environment will be designed to create a sense of danger and tension.

> - The game world will feature different environments in each level, including urban, rural, and industrial settings.
> - Each environment will be designed to immerse the player in the post-apocalyptic world, with destructed buildings, abandoned vehicles, and other debris.
> - Each level will have unique challenges and obstacles that the player must overcome, including puzzles and traps.

### Audio Design:

> - The game will feature high-quality sound effects and background music that will enhance the player's immersion in the game world.
> - The sound effects will be designed to complement the game's visuals and create a realistic post-apocalyptic atmosphere.
> - The background music will be used to build tension and create suspense during gameplay.

### UI for this game. 
> This is the main menu/UI for the game demo which includes different buttons to direct player to different scenes within the game. When a player starts the game, they will be directed to the main menu and then they can decide what to do next. 

![image](https://user-images.githubusercontent.com/81584201/219817702-02ac0674-f359-4846-8aee-5c6fc8773406.png)

#### Main Menu has 4 buttons:
> **Start Game** : This button directs player to a demo scene where player can explore the demo world that I have designed with multiple assets, audios, buildings & structures and natural phenomenon such as rain, dust, falling leaves from the tree, fire, sunrise, weather and fog.
>
> **Multiplayer** : This button is for players who wants to play the game with other players over the network. Upon clicking this button, player gets directed to a waiting room scene where the player gets connected to different player playing on the network. 
> 
> **Options** : This button will direct the player to move to a different UI scene where player can get the knowledge of how to move the character and other options that are connected to the game and player. 
> 
> **Exit** : Clicking exit quits the game.

### Levels: (Not fully designed yet)
> - Level 1: Urban setting with abandoned buildings, cars, and other debris. Player has to collect the first piece of the antidote from a research center.
> - Level 2: Rural setting with forests and farmland. Player has to collect the second piece of the antidote from a farm where the virus originated.
> - Level 3: Industrial setting with factories and warehouses. Player has to collect the final piece of the antidote from a secret lab hidden within a factory.

#### Procedural Rules for Generating Levels:
> - Each level will be designed to be challenging and immersive, with unique objectives and environments.
> - The levels will be designed to encourage exploration and experimentation.
> - The levels will be generated procedurally, ensuring that each playthrough is unique and unpredictable.
> - The difficulty of the game will increase as the player progresses through each level, with tougher enemies and more complex puzzles to solve.
