using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raton
{
    public int especial = 0; // 0=Raton, 1=Gato, 2=TheMouse, 3=MousePitt, 4=LifeMau, 5=Mousillex, 6=Police

    // PARAMETROS CARACTERISTICAS (causan triggers etc)
    public int calidad;
    bool trae_comida;
    bool conflictivo;
    bool gloton;

    // PARAMETROS APARIENCIA (unicamente para cumplir los requisitos de la fiesta)
    public int body;
    public int eye;
    public int hat;
    public int torso;

    public int audioClipNumberForKnock = 0; // 0=normal, 1=conflictivo, 2=calidad

    // GETTERS DE CARACTERISTICAS
    public int getCalidad() { return calidad; }
    public bool traeComida() { return trae_comida; }
    public bool esConflictivo() { return conflictivo; }
    public bool esGloton() { return gloton; }

    // CONSTRUCTOR - genera un raton con caracteristicas y apariencia aleatorias
    public Raton()
    {
        setRandomParams(-1);

        //BORRAR LUEGO:
        trae_comida = true;
        //----------------
    }
    
    public Raton(int caracteristica)
    {
        setRandomParams(caracteristica);
    }
    
    public Raton(string especial, int esp)
    {
        espespecial = esp;
        setRandomParams(-1);
    }

    int espespecial = -1;

    public void setRandomParams (int caracteristica)
    {
        body = Random.Range(0, 2);
        torso = Random.Range(0, 5);
        eye = Random.Range(0, 7);
        hat = Random.Range(0, 5);

        if (especial == 0) if (Random.Range(0, 5) == 5) especial = 1; // lo pondria a 15
            else if (espespecial != -1) especial = espespecial;

        if (especial == 0)
        {
            calidad = Random.Range(0, 3);
            if (calidad == 3) audioClipNumberForKnock = 2;

            if (caracteristica == -1) caracteristica = Random.Range(0, 8); // la mayoria de ratones no tendra ninguna caractersitica especial

            if (caracteristica == 1) trae_comida = true;
            else if (caracteristica == 3)
            {
                conflictivo = true;
                audioClipNumberForKnock = 1;
            }
            else if (caracteristica == 4)
            {
                gloton = true;
                body = 3;
                torso = 5;
            }

        }
    }
    
    public Raton (bool b)
    {
        setRandomParams(-1);
        calidad = 0;
    }

    public string aestring ()
    {
        return "calidad="+calidad+", comida="+trae_comida+", conflic="+conflictivo+", gloton="+gloton;
    }
}
