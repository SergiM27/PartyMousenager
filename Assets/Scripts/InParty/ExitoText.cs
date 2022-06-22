using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitoText : MonoBehaviour
{

    public void ExitoTextDone()
    {
        this.gameObject.GetComponent<Animator>().SetInteger("ExitoText", 2);
    }

}
