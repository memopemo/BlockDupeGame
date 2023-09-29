# Player Implementation
This is for describing how the player should be implemented. (Or just to keep track of it in my head)

# Powerup specification
What specifically each powerup does.

## Duplication
- Allows the player to duplicate.
- Is disabled if the player is in midair and does not have [Midair Duplication](#midair-duplication),

1. When the player holds down the B button, the character starts the process of creating a clone.

2. During the player holding the button, the character is in a struggling state and can still move while struggling. 

3. After the player releases the B button, the character is in a duplicating state and a new clone emerges on top of the character, with the character now holding the clone.

## Metal Slam
- This allows you to make your next duplicate made of metal, which can conduct electricity and break breakable objects.

1. When the player holds the B button to start the struggling state, the player may also hold the Down button aswell while in the struggling state at any time. 

2. During the time the down button is held, the character will be in a more struggling state to signify that the Down button does something.  

3. Once the player releases the B button while still holding the down button down, the character will enter a duplicating state like he would when cloning, except the clone is now made of metal.

## Duplication Pack
- This increases the amount of duplicates that can be in a room at once.

- This just increases the global counter for how many clones can be in a room at once. This number is saved in the save file.

## Midair Duplication
This allows the player to make a duplicate while airborne, allowing the player to "fly" by repeatedly duplicating in midair and throwing those duplicates.

- During normal [Duplication](#duplication), the player may not duplicate while airborne, however this powerup removes that limitation.

## Hold While Jumping
This allows the player to jump while holding an object

- While the character is holding something, normally, they cannot jump. This powerup removes that limitation. 

## Remote Duplication
This allows the player to set a point where the duplicate will spawn, move somewhere else and then release it.

- During normal [duplication](#duplication) while holding the B button down, if the B button is held for a length of time, while standing still, and has Remote Duplication, a graphic appears above the character's head to signify where the duplicate will spawn. 

- The character enters a new type of struggling state to indicate this once the player leaves within a few pixels of the point to indicate that the new clone will spawn at that location. 

- If the player goes back to the point, the character will go back to the regular duplicaton struggle state. This can happen infinitely many times.

- Once the player releases the button and is in the struggle state where the duplicate will spawn remotely, the character currently controlled by the player will die, the new duplicate spawns at the remote location, and control will be given to the new duplicate, and is alive.

- If the player is back where they were when they started the remote duplication, or did not move at all, then the character will instead create and carry the new duplicate, exactly similar to a normal duplication.

## Straight Shot
This allows the player to charge up and throw their duplicate in a straight line until they collide with a wall. When combined with the metal slam, it allows many blocks in a row to be broken all at once.

- When the player is carrying something, if they have the straight shot, upon holding the B button, the player will present a new struggle animation for throwing straight. During this time, the player can also hold down the up button, or diagonal left/right up. The player can also throw down if airborne.

- On released B button:
    - if holding up:
        - if also holding left or right:
            - throw straight diagonally left or right.
        - else:
            - throw straight up.
    - else if airborne and holding down:
        - throw straight down.
    - else
        - throw straight left or right.

- After picking the direction to throw based on input, disable gravity and have the thrown clone go in that direction. If they die or [stick](#sticky-duplicates), return control back to the original that threw the clone. Else, if they hit a regular, impassible wall, make the dead duplicate alive, and give control to them.

- As a failsafe, after a long enough time control will be given back to the original.
    
## Phantom Duplication
This allows the player to throw duplicates that can go through special walls temporarily. Once inside, they must go through the entire phantom block in order to appear on the other side.

- Upon throwing, they will enter a phantom thrown state where collision with anything marked as phantom is disabled. 

- While airborne, if the player is currently intersecting with collision marked as phantom, their velocity stays the same, and they are kept in the phantom thrown state until they exit.

- As a failsafe, touching a wall will immediately bounce them in the opposite direction they were thrown in, ensuring that they will not get stuck inside the phantom blocks.

## Water Bubble
This allows the player to move regularly in water. Otherwise, their jumping ability is limited.

- water without the water bubble slows down movement and gravity.

## Sticky Duplicates
This allows the player to create dead duplicates (meaning they will never be alive) and throw them with the "Speed Throw" powerup to stick them to walls as platforms.

- When thrown straight by [Straight Shot](#straight-shot), if they touch a wall that is marked sticky, the dead clone will disable all physics, become solid and remain attached to the object. Control will still be in the original that threw the clone. 

## Grapple Dupes
This allows the player to make a rope of clones that attach to grapple blocks. The length is determined by the number of clones the player can make, and by default goes straight up, but can be aimed diagonally.

- Upon holding up and / or left/right AND THEN pressing B,  the character will be in a state of throwing an extending a rope of clones in the direction held. During this, the player may not move.

- The number of duplicates in the rope is equal to the max number of clones - 1. 

- If the end of the rope attaches to a block before the max number of clones to make is reached, the player is put in a swing state, where their left and right inputs add or subtract their momentum swinging around the grapple block. This stays in effect as long as the player holds the B button.

- Upon releasing the B button while swinging, the player will be launched left or right depending on their momentum at the time of release. 

- If the end of the rope does NOT attach to any grapple block efore the max number of clones to make is reached, then it retracts and removes clones until theres none left, then finally giving control back to the player.


# Heirarchy of States

## Alive
- Throwing
- Dying
- Cloning Hold
- Cloning Release
- 

## Dead



