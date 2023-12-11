
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    int counter = 0;
    void Update()
    {
        counter++;
        if(counter < 400) return; // wait 400 frames before allowing quit.

        if(Input.anyKeyDown)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
