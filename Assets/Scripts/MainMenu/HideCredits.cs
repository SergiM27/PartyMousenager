using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCredits : MonoBehaviour
{
    public GameObject mainMenu;
    public void hideCredits()
    {
        this.gameObject.SetActive(false);
    }

    public void hideMainMenu()
    {
        mainMenu.SetActive(false);
    }


}
