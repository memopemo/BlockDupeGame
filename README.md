# Pitch
You play as a box character that can duplicate themselves and throw those duplicates, and that new duplicate you threw is now being controlled by you, and the old one dies and can be picked up and thrown.

You collect powerups by exploration and beating bosses.

There are enemies and obstacles that hurt you, and if you take too much damage, you have to restart at a save point and try again.

# Powerups
These are collected by the player after progressing.

## Duplication
Allows the player to duplicate.

## Metal Slam
This allows you to make your next duplicate made of metal, which can conduct electricity and break breakable objects.

## Duplication Pack
This increases the amount of duplicates that can be in a room at once.

## Midair Duplication
This allows the player to make a duplicate while airborne, allowing the player to "fly" by repeatedly duplicating in midair and throwing those duplicates.

## Hold While Jumping
This allows the player to jump while holding an object
- This is sort of a "restriction" thats lifted from the player when obtained in order to create challenges that require stacking or something.

## Remote Duplication
This allows the player to set a point where the duplicate will spawn, move somewhere else and then release it. Examples of use-cases would be for retrying before knowing you take damage, going to press a button that locks you out when held down, meaning you have to be inside the space and outside of the space at the same time.

## Straight Shot
This allows the player to charge up and throw their duplicate in a straight line until they collide with a wall. When combined with the metal slam, it allows many blocks in a row to be broken all at once.

## Phantom Duplication
This allows the player to throw duplicates that can go through special walls temporarily. Once inside, they must go through the entire phantom block in order to appear on the other side.

## Water Bubble
This allows the player to move regularly in water. Otherwise, their jumping ability is limited.

## Sticky Duplicates
This allows the player to create dead duplicates (meaning they will never be alive) and throw them with the "Speed Throw" powerup to stick them to walls as platforms.

## Grapple Dupes
This allows the player to make a rope of clones that attach to grapple blocks. The length is determined by the number of clones the player can make, and by default goes straight up, but can be aimed diagonally.

## More???
Im not sure what other powers there can be. They must be both helpful to explore or keys to be used with obstacles, and be used for puzzle solving.

# Obstacles
These are parts of the level that the player must overcome, either now or at a later period when they have the neccesary powerups.

## Circle Zones
These turn the player into a circle, thus preventing them from stacking.

## Powerup Inhibitor Zones
These prohibit the player from using certain powerups, including duplicating. These can inhibit multiple powerups, and powerups that are disabled are displayed on the HUD.

## Phantom Blocks
These only allow phantom duplicates to pass through.

## Enemies
These damage the player, and their movement, size, and actions can be varying

## Harmful Geometry
Spikes, Lava, and other things that can hurt the player and stay still.

## Platforming
Simple jumping platforming that may or may not require abilities.

# Controls
The player can walk, jump, duplicate, lift objects, and throw.
| Button                   | Action                                                         |
|--------                  |----------------------                                          |
| A                        | Jump                                                           |
| B(Tap)                   | Duplicate/Throw/Lift                                           |
| B(Hold and Release)      | (Create Remote Point / Create Dupe At Point ) / Throw Straight |
| B(Hold Down and Release) | Create Metal Dupe or if not, regular B(Hold and Release)       |
| B(Hold Up and Release)   | Create Dupe Grapple (also works with diagonal up)              |
| Pause                    | Pause Game                                                     |
| Map                      | Shows the map                                                  |

Powers that are always on:
- Midair Duplication
- Hold While Jumping
- Phantom Duplication
    When throwing, there is a short period where the player can go through phantom blocks.
- Water Bubble
    Appears when in water
- Sticky Dupes
    If the player hits a sticky wall, give control back to the original clone that threw and stick the clone to the wall.
    OR (alternate implementation) if a raycast in the direction we throw will hit a sticky wall, allow the sticky clone to hit the wall and keep the clone alive.


# Attacking
The player can attack by throwing things, including their old clones.
The player can also stomp on enemies that are stompable.


# Health
The player can initially take 3 hits before dying. 


# Goodies
These are things the player is rewarded with, other than the way forward to the next item or boss at dead ends, or after completing a challenge.

## Save Station
These save your game, and are where you return to after you die.

## Health Refill Station
These refill all of your health.

## Dupe Packs
See [Dupe Packs](#duplication-pack)

## Extra Metal Dupes
These give you more ammunition to metal dupe.

## Extra Remote Dupes
These give you more ammunition to remote dupe.

## Health Packs
These give the player more health to withstand attacks.

# Setting
Abandoned Mars base

# Map Regions
These are the main map split into 5 areas with a common theme between individual rooms. Main method of travelling between them is a (???)


