using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ResultController : MonoBehaviour
{

    public Text earnings;
    public Text mouse;
    public Text famousMouse;
    public Text exitoPoints;
    public Text calidadPoints;
    public Text finalScore;
    public int finalScoreInt;

    // Start is called before the first frame update
    void Start()
    {
        fixFonts();

        earnings.text = (GameManager.dinero).ToString();
        mouse.text = (GameManager.numeroDeRatones).ToString();
        famousMouse.text = (GameManager.numeroDeRatonesFamosos).ToString();
        calidadPoints.text = (GameManager.calidad).ToString();
        exitoPoints.text = (GameManager.exito).ToString();
        finalScoreInt = GameManager.dinero + GameManager.numeroDeRatones + GameManager.numeroDeRatonesFamosos + GameManager.exito + GameManager.dinero + GameManager.calidad;
        finalScore.text = (finalScoreInt).ToString();
    }

    [Header("Fonts")]
    public Font[] fonts; // arrastrar aqui todas las fuentes del proyecto

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
