using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeartsController : MonoBehaviour
{
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    CloneManager cloneManager;
    // Start is called before the first frame update
    void Start()
    {
        cloneManager = FindFirstObjectByType<CloneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        int currentHealth = cloneManager.currentlyControlledPlayer.health;
        int currentMaxHealth = cloneManager.currentlyControlledPlayer.maxHealth;
        Image[] hearts = GetComponentsInChildren<Image>(true);

        //apply max health (show/disable)
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].gameObject.SetActive(currentMaxHealth > i * 2);
        }
        //set each one to be either full or empty
        for (int i = 0; i < hearts.Length; i++)
        {
            Image heart = hearts[i];
            if((i+1)*2 <= currentHealth)
            {
                heart.sprite = fullHeart;
            }
            else
            {
                heart.sprite = emptyHeart;
            }
        }
        //if health is even, set the boundary one to be half.
        if(currentHealth % 2 == 1)
        {
            hearts[Mathf.FloorToInt(currentHealth/2)].sprite = halfHeart;
        }

    }
}
