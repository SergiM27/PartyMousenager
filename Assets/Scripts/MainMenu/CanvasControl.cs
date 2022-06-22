using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasControl : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject credits;
    public GameObject howToPlay;
    public AudioSource audioSource;

    [Header("Fonts")]
    public Font[] fonts; // arrastrar aqui todas las fuentes del proyecto

    private void Start()
    {
        fixFonts();
    }

    void hideMenu()
    {
        ButtonPressed();
        credits.GetComponent<Animator>().SetInteger("Credits", 2);
    }

    public void ShowHowToPlayMenu()
    {
        ButtonPressed();
        howToPlay.SetActive(true);
        howToPlay.GetComponent<Animator>().SetInteger("HowToPlay", 1);
    }


    public void HideHowToPlayMenu()
    {
        ButtonPressed();
        howToPlay.GetComponent<Animator>().SetInteger("HowToPlay", 2);
    }



    public void hideCredits()
    {
        ButtonPressed();
        credits.SetActive(false);
    }

    public void showMainMenu()
    {
        ButtonPressed();
        hideMenu();
        mainMenu.SetActive(true);
    }

    public void showCredits()
    {
        ButtonPressed();
        credits.SetActive(true);
        credits.GetComponent<Animator>().SetInteger("Credits", 1);
    }

    public void closeGame()
    {
        ButtonPressed();
        Application.Quit();
    }

    public void ButtonPressed()
    {
        audioSource.Play();
    }

    // metodo para arreglar las fuentes de texto de pixel para que se vean bien
    void fixFonts()
    {
        //https://youtu.be/ccYJOT7bUUY?t=187
        foreach (Font font in fonts)
        {
            font.material.mainTexture.filterMode = FilterMode.Point;
        }
        /*
         * Para que las fuentes se vean totalmente bien:
         * Clica en la fuente y en sus propiedades: Rendering Mode = Hinted Raster, Character=Unicode
         * Pasa las fuentes por el script (arrastralas desde el proyecto hasta su hueco en el PartyController del Canvas)
         * En el Rect Transform, ponles escala 1, 1, 1 (ESTANDO FUERA DE LA JERARQUIA)
         * En el Rect Transform, ponles Width y Height los pixeles que tenga.
         * Unos tamaños de fuente se ven bien , otros no.Tamaño recomendado: 16
         * Ponle de material spritepixelsnap 
         */
    }
}
