using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CuentaAtras : MonoBehaviour
{

    public GameObject canvas;
    public GameObject cancion;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
    }



    void ChangeNumero()
    {
        if(this.gameObject.GetComponent<Text>().text=="3")
        this.gameObject.GetComponent<Text>().text = "2";
        else if(this.gameObject.GetComponent<Text>().text == "2")
        {
            this.gameObject.GetComponent<Text>().text = "1";
        }
        else
        {
            this.gameObject.GetComponent<Animator>().SetInteger("Cambio", 1);
            this.gameObject.GetComponent<Text>().text = "LETS GET THIS\nPARTY STARTED!";
            cancion.gameObject.GetComponent<AudioSource>().Play();
            Time.timeScale = 1;

           
            Invoke("DestroyThisAndStart", 1.0f);
        }
    }
    void DestroyThisAndStart()
    {
        canvas.gameObject.GetComponent<PartyController>().gameOn = true;
        Destroy(this.gameObject);
    }

}
