# Quick Unity Lesson
Manual incase you wanna learn specifics: [LINK](https://docs.unity3d.com/Manual/index.html)

## Unity Concepts

- Games are composed of **GameObjects** in a **Scene**, rendered with a **Camera**.

- All **GameObjects** have **Components**, which change how the GameObject looks, moves around, and affects anything.

- The **Camera** is a **Component**

- Almost all **GameObjects** have a **Transform** Component that contains the GameObject's 3D Position, and Rotation.

## Scripting

- You can make scripts that take player input and make GameObjects do stuff.

- C# is incredibly similar to Java, and by extension C++.

- Object Oriented Programming is really helpful here, especially because we are dealing with actual **GameObjects**

- Inherit other classes by replacing the MonoBehaviour with your class name.
    - Access the parent with the `base` keyword.

            base.OnDestroy();

- One class per '*.cs' file. Please.

- Naming convention: camelCase for variables, CamelCaps for functions.
    - No Hungarian Notation, like for example `m_XXXXXX`

- Public variables can be edited in the Inspector, while Private variables are hidden.
    - Unhide Private variables by starting the declaration with `[SerializeField]`

- Public variables can be accessed from another in-game object by getting the GameObject of that specific object and its relevant Components.

- Common Acessors of GameObjects and Components:

    ```cs
        GameObject enemy = GameObject.FindObjectOfType<Enemy>(); //try not to use this alot
        SpriteRenderer enemySprite = enemy.GetComponent<SpriteRenderer>();
    ```

- GameObjects can be variables that are assigned in the Inspector.

- `Start()` is called on the first frame, and `Update()` is called every frame.
    - Use `FixedUpdate()` for anything to do with physics.
    - There are other special-use-case start and update functions.

- Code structure is usually this:
    ```cs
        using System.Collections; // these are for math libraries... 

        using System.Collections.Generic; // ...data structures, and other stuff.

        using UnityEngine; //this is what allows you to use GameObject objects and other unity-specific stuff. 

        public class Player : MonoBehaviour
        {


            // Initialize your variables here.
            // You may initialize them in here or in the inspector.
            [SerializeField] int PrivateInspectorVariable = 10;

            int PrivateVariableA = 3; // By default, variables are private.

            private int PrivateVariableB = 4; // but just clear it up if its private or public.

            public int VeryPublicVariable = 1024;



            // Start is called before the first frame update
            void Start()
            {
                // Assign and initialize variables that can vary on start.
            }


            // Update is called once per frame
            void Update()
            {
                // Update your values here and call any functions.
                PrivateVariableA += 1;
                PrivateVariableB -= 1;

                DoSomethingRepetitive(PrivateInspectorVariable)
            }


            // You can add custom functions
            void DoSomethingRepetitive(int value)
            {
                
            }

        }
    ```

- Destroy objects and unload them from memory with the `Destroy(gameObj, afterSecs)` function.

- Because framerates vary across platforms and computer hardware, multiply variables with `Time.deltaTime` in order to make them move smoothly.

- `enums` allow you to declare specifically-named states of a variable.

- Rotation is done with 4D Quaternions, which are super-duper-complex thingys that you dont need to worry about.
    - There are functions that can convert between Euler (360 deg) and Quaternion in the `Quaternion` class.

- Debug your scripts with the `Debug` class and log to the console with either `Debug.Log("Message")` and its variants, 
    - ...Or quickly log to the console with `print("Message")`

- Get input with the `Input` class. For example: `Input.GetKeyDown(KeyCode.Z);`
    - `GetKey/Button()` returns `true` if it is pushed.
    - `GetKey/ButtonDown()` returns `true` only on the frame it was *pressed*.
    - `GetKey/ButtonUp()` returns `true` only on the frame it was *released*.

## File Management

- File name convention: CamelCaps.xxx or CamelCaps_###.xxx for similar files.

- GameObjects can be put into a file as a **Prefab** by dragging the GameObject in the "Heirarchy" Tab into the "Project" Tab.
    - This allows for easy placement of duplicate GameObjects.
    - You can also store prefabs as a GameObject variable in your script, and create them with the `Instantiate(gameObj, position, rotation)` function.

- You can load Assets in-code using the `AssetDatabase.LoadAssetAtPath(path, type)` function, along with any string manipulation shenanigans that make it more useful.