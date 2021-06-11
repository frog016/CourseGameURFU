# Arena Battle  
## Little Description
This 2d game is a coursework for 1 course in programming at UrFU, written in C#. The essence of the game is an attempt to survive in the battle in the arena with various opponents, bots.  
A collision is implemented in the game, those one object cannot pass through another. There is also a small artificial intelligence for opponents and a combat system.

There are 3 classes in the game, each of which has 2 skills:
1. Stealthy and fast Rogue
2. Strong and warlike Swordsman
3. Tough and impenetrable Guard
## Features of classes  
### Rogue
Rogue has little health, armor and attack range, as well as low weapon damage, but he is the fastest.
1. The first skill of the Rogue, this is not very strong, but a quick blow with daggers at the enemy.
2. The second skill is stronger, but has a cooldown. Its peculiarity lies in the fact that when it hits her back, the damage from it increases 3 times!!!  
### Swordsman
The Swordsman has the highest damage, but is also the slowest. He also has medium health, armor and a high attack range.
1. With the first skill, the swordsman deals normal weapon damage.
2. The second skill enrages him, doubling his damage dealt for 10 seconds.
### Guard
The Guard has the highest amount of armor and health, which justifies its name, having an average attack speed, damage and range.
1. The first skill also deals normal damage.
2. The second ability increases the armor value by 2 times for 5 seconds, which makes it impossible to kill him except for the swordsman.
## Gameplay description
First, the player must select a class in the menu for which he will fight, after which he appears in the arena. Then he must defeat all enemies, they remain the only one in the arena to win. If the player dies, he loses. In either of these two cases, the game ends, with an appropriate result. After it, the player can go to the menu to play again, or exit.
### Control
The player can control the character with the following keyboard keys:
#### Walk keys
1. W - go up.
2. S - go down.
3. A - go left.
4. D - go right.
#### Skill keys
1. 1 - use first skill.
2. 2 - use second skill.
#### Control keys
1. Escape - back to menu.
