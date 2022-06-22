using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyController : MonoBehaviour
{
    [Header("Partes del menu para activar y desactivar")]
    public GameObject door;
    public GameObject upgradesMenuOpen;
    public GameObject upgradesMenuClosed;
    public GameObject telefonoDescolgado;
    public GameObject telefonoColgado;
    public GameObject bloquearTodo;

    [Header("Main Parameters")]
    //public int nMouse = 5;
    public int exito = 0;
    public int calidad = 0;       //calidad = exito + ambienceLevel + musicLevel + foodLevel + confortLevel + bonusFamoso;
    public int bonusFamoso = 0;
    public Text textTickets;
    public Text textMoney;
    public Text textTimer;
    public Text textAforo;
    public Text textExito;
    private int precio_entrada = 0;
    private int money = 0;
    public bool gameOn;

    [Header("Upgrades")]// parametros
    public GameObject confort; // gameObjects que tienen las imagenes que representan las mejoras (sofa, globos, etc), referenciados aqui para cambiar las imagenes
    public GameObject ambience;
    public GameObject music;
    public GameObject food;
    public Image confortLevelImage; // imagen al lado del icono de mejora que muestra el nivel de la mejora (LVL 1, LVL 2, LVL 3)
    public Image ambienceLevelImage;
    public Image musicLevelImage;
    public Image foodLevelImage;
    public UI_HoldButton confortLevelUpgradeButton; // botones de comprar mejora referenciados aqui para cambiarles el sprite a MAX y deshabilitarlos al llegar a nivel 3
    public UI_HoldButton ambienceLevelUpgradeButton;
    public UI_HoldButton musicLevelUpgradeButton;
    public UI_HoldButton foodLevelUpgradeButton;
    private int confortLevel = 1; 
    private int ambienceLevel = 1;
    private int musicLevel = 1;
    private int foodLevel = 1;



    [Header("Sprites")]
    public Sprite[] attributeSprites;
    public Sprite[] levelTextSprites;
    public Sprite maxLevelReachedSprite;
    public Animator mouseAnimator;

    [Header("Fonts")]
    public Font[] fonts; // arrastrar aqui todas las fuentes del proyecto

    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Audio Clips")]
    public AudioClip normalButtonAudioClip;
    public AudioClip upgradeButtonAudioClip;
    public AudioClip descolgarTelefonoAudioClip;
    public AudioClip colgarTelefonoAudioClip;
    public AudioClip[] picarNumerosTelefono;
    public AudioClip comunicandoAudioClip;
    public AudioClip mouseTalkingAudioClip;

    [Header("Audio Clips")]
    public GameObject levelLoader;

    // Start is called before the first frame update
    void Start()
    {
        gameOn = true;
        fixFonts();

        InvokeRepeating("SumarNumeroDeRatones", 5.0f, 5.0f);
        InvokeRepeating("triggersPorRaton", 3.0f, 3.0f);

        coroutineVienenRatones = StartCoroutine(vienenRatones());



        //ratones_en_la_fiesta.Add(new Raton(3));
        //ratones_en_la_fiesta.Add(new Raton(3));
        //ratones_en_la_fiesta.Add(new Raton(3)); //QUITAR LUEGO!!!!
    }

    private static readonly int _CONFLICTIVO = 1;
    private static readonly int _GLOTON = 3;
    public int getNumeroDeRatonesPorTipo (int tipo)
    {
        int total = 0;
        foreach (Raton raton in ratones_en_la_fiesta)
        {
            if (tipo.Equals(_CONFLICTIVO) && raton.esConflictivo()) total++;
            else if (tipo.Equals(_GLOTON) && raton.esGloton()) total++;
        }
        return total;
    }

    public bool triggerConflicto = false;

    //public IEnumerator triggersPorRaton()
    public void triggersPorRaton()
    {
        //while (gameOn)
        //{
            //yield return new WaitForSeconds(3);

            // posibilidad de que haya una pelea
            if (!triggerConflicto && getAforo()>2) triggerConflicto = (Random.Range(0, 20) > 20 - getNumeroDeRatonesPorTipo(_CONFLICTIVO)) ; // llama a la policia para echar dos ratones conflictivos aleatorios
            Debug.Log("probabilidad de tener conflicto: si num aleatorio del 0 al 20 es mayor que " + (20 - getNumeroDeRatonesPorTipo(_CONFLICTIVO)));

            // posibilidad de que te rompan muebles
            //int minimoRomperMuebles = 19 - getNumeroDeRatonesPorTipo(_DESTRUCTIVO) - (int)(getAforo() * 0.3f);
            int minimoRomperMuebles = 20;
            if (getAforo() > 20) minimoRomperMuebles -= (int)(getAforo() * 0.3f);
            if (triggerConflicto) minimoRomperMuebles -= 2;
            bool triggerRomperMuebles = (Random.Range(0, 20) > minimoRomperMuebles) ; // comodidad o musica vuelve a nivel 1

            // posibilidad de que se terminen la comida
            bool triggerTerminarseComida = (Random.Range(0, 20) > 19 - getNumeroDeRatonesPorTipo(_GLOTON) - (int)(getAforo() * 0.3f)) ; // comida vuelve a nivel 1
        //}
    }
    
    ArrayList ratones_en_la_fiesta = new ArrayList();
    int plus_de_calidad = 0;

    Raton raton_en_la_puerta;

    public AudioClip[] knockAudioClips;

    public void playKnockSound ()
    {
        if (raton_en_la_puerta!=null) playSound(knockAudioClips[raton_en_la_puerta.audioClipNumberForKnock]);
    }

    // "parametros" que en realidad son getters porque son resultados de calculos que se hacen con otros parametros

    public int getAforo()
    {
        return ratones_en_la_fiesta.Count;
    }

    public int getCalidad()
    {
        int calidad = exito +
            ((confortLevel-1) * 10) +
            ambienceLevel-1 +
            musicLevel-1 +
            foodLevel-1 +
            plus_de_calidad +
            getBonusFamosos();
        return calidad;
    }

    public int getBonusFamosos()
    {
        int bonus = 0;
        foreach (Raton raton in ratones_en_la_fiesta)
        {
            bonus += raton.getCalidad();
        }
        return bonus;
    }

    public GameObject mouseAtTheDoor;

    public IEnumerator vienenRatones()
    {
        while (gameOn)
        {
            int segundos = precio_entrada / 10 - exito / 10;
            if (segundos < 6) segundos = 6;

            yield return new WaitForSeconds(segundos); // el tiempo que tarda el siguiente raton en venir
            
            raton_en_la_puerta = seleccionarRatonAlAzar();
            Debug.Log("aqui viene " + raton_en_la_puerta.aestring());

            cambiarSpritesRatonEnLaPuerta();

            if (raton_en_la_puerta.especial == 1 && getAforo() > 2)
            {
                mouseAnimator.SetBool("cat", true);
            }
            else mouseAnimator.SetBool("cat", false);

            mouseAnimator.SetInteger("ani", 1);
            spritesRatonPorDentroDeLaCasa(false);

            yield return new WaitForSeconds(8); // el tiempo que tarda el raton en irse si no le abres

            raton_en_la_puerta = null;
            mouseAnimator.SetInteger("ani", 3);
        }
    }

    string raton_al_que_has_llamado = "";

    public Raton seleccionarRatonAlAzar()
    {
        Raton rat = new Raton();
        if (raton_al_que_has_llamado.Equals(""))
        {
            rat.calidad = (int)(Random.Range(0, getCalidad()) * 0.1f);
        }
        else
        {
            switch (raton_al_que_has_llamado)
            {
                case "The Mouse": rat.especial = 2; rat.calidad = 30;  break;
                case "Mouse Pitt": rat.especial = 3; rat.calidad = 40; break;
                case "LifeMau5": rat.especial = 4; rat.calidad = 50; break;
                case "Mousillex": rat.especial = 5; rat.calidad = 60; break;
                case "Police": rat.especial = 6; break;
            }
            raton_al_que_has_llamado = "";
        }
        return rat;
    }

    public Sprite[] spritesBody;
    public Sprite[] spritesEye;
    public Sprite[] spritesHat;
    public Sprite[] spritesTorso;
    public Sprite spriteConflictivo;

    public void cambiarSpritesRatonEnLaPuerta()
    {
        SpriteRenderer body = mouseAtTheDoor.transform.Find("sprite_body").GetComponent<SpriteRenderer>();
        SpriteRenderer eye = mouseAtTheDoor.transform.Find("sprite_eye").GetComponent<SpriteRenderer>();
        SpriteRenderer hat = mouseAtTheDoor.transform.Find("sprite_hat").GetComponent<SpriteRenderer>();
        SpriteRenderer torso = mouseAtTheDoor.transform.Find("sprite_torso").GetComponent<SpriteRenderer>();

        SpriteRenderer conflictivo = mouseAtTheDoor.transform.Find("sprite3_conflictivo").GetComponent<SpriteRenderer>();

        body.sprite = null;
        eye.sprite = null;
        hat.sprite = null;
        torso.sprite = null;
        conflictivo.sprite = null;

        if (raton_en_la_puerta.especial == 0) // Es un raton normal
        {
            mouseAtTheDoor.transform.Find("sprite_gato").gameObject.SetActive(false);

            body.sprite = spritesBody[raton_en_la_puerta.body];
            eye.sprite = spritesEye[raton_en_la_puerta.eye];
            hat.sprite = spritesHat[raton_en_la_puerta.hat];
            torso.sprite = spritesTorso[raton_en_la_puerta.torso];

            if (raton_en_la_puerta.esConflictivo()) conflictivo.sprite = spriteConflictivo;
            else conflictivo.sprite = null;
        }
        else if (raton_en_la_puerta.especial==1) // Es el gato
        {
            mouseAtTheDoor.transform.Find("sprite_gato").gameObject.SetActive(true);
        }
        else if (raton_en_la_puerta.especial == 2) // 2 = TheMouse,
        {
        }
        else if (raton_en_la_puerta.especial == 3)// 3 = MousePitt
        {
        }
        else if (raton_en_la_puerta.especial == 4)// 4 = LifeMau,
        {
        }
        else if (raton_en_la_puerta.especial == 5)// 5 = Mousillex,
        {
        }
        else if (raton_en_la_puerta.especial == 6)// 6 = Police
        {
            body.sprite = spritesBody[0];
            hat.sprite = policeHatSprite;
        }
    }
    public Sprite policeHatSprite;


    // Update is called once per frame
    void Update()
    {
        if (gameOn)
        {
            float t = GameManager.time = GameManager.time - Time.deltaTime;
            string minutes = ((int)t / 60).ToString("00");
            string seconds = ((int)t % 60).ToString("00");

            textTimer.text = minutes + ":" + seconds;
            if (t <= 0)
            {
                Invoke("FinishGame", 2.0f);
                gameOn = false;
            }

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
            {
                pausem();
            }
        }
    }

    public GameObject pauseMenu;

    public void pausem ()
    {
        if (pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
       
    }

    void FinishGame()
    {
        GameManager.dinero = money;
        GameManager.exito = exito;
        GameManager.numeroDeRatones = getAforo();
        //GameManager.numeroDeRatonesFamosos = getAforo(); //falta controlar ratones famosos ---> no hace falta. Los famosos haran que tu fiesta sea mejor y por tanto influyen directamente en el exito. No hace falta ponerlos a parte.
        //GameManager.calidad= exito + ambienceLevel + musicLevel + foodLevel + confortLevel + bonusFamoso;
        GameManager.calidad = getCalidad();
        levelLoader.gameObject.GetComponent<LevelLoader>().GameOver();
    }

    int getCalidadRatones ()
    {
        int calidadTotal = 0;
        foreach (Raton raton in ratones_en_la_fiesta)
        {
            calidadTotal += raton.getCalidad();
        }
        return calidadTotal;
    }

    void SumarNumeroDeRatones()
    {
        if (gameOn)
        {
            int diferencia_de_exito = 0;
            diferencia_de_exito += getCalidadRatones();
            diferencia_de_exito -= 5;
            if (triggerConflicto) diferencia_de_exito -= 10;

            exito += diferencia_de_exito;

            string textForExitoText = "";

            if (diferencia_de_exito >= 0) textForExitoText = "+" + (diferencia_de_exito);
            else                          textForExitoText = ""  + (diferencia_de_exito);

            if (!triggerConflicto) textForExitoText += " Success!";
            else                   textForExitoText += " CONFLICT!!";

            textExito.text = textForExitoText;

            textExito.gameObject.GetComponent<Animator>().SetInteger("ExitoText", 1);
            // MOSTRAR + X EXITO EN UNA ESQUINA

            if (exito < 0) FinishGame();
        }
    }


    public Sprite catHand;
    public Sprite catHandAndMice;

    public void openDoor ()
    {
        if (gameOn && doorAnimator.GetCurrentAnimatorStateInfo(0).IsName("doorClosed"))
        {
            door.gameObject.GetComponent<Animator>().SetInteger("animation", 1);
            //if (raton_en_la_puerta!= null && raton_en_la_puerta.especial!=6 && raton_en_la_puerta.especial !=1) Invoke("closeDoor", 2f);
            if (raton_en_la_puerta == null || raton_en_la_puerta.especial != 6) Invoke("closeDoor", 2f);

            if (raton_en_la_puerta != null && (mouseAnimator.GetCurrentAnimatorStateInfo(0).IsName("mouseComeToKnock")
                || mouseAnimator.GetCurrentAnimatorStateInfo(0).IsName("catCome")))
            {
                StopCoroutine(coroutineVienenRatones);

                if (raton_en_la_puerta.especial == 0) // Es un raton normal
                {
                    money += precio_entrada;
                    textMoney.text = money.ToString();

                    mouseAnimator.SetInteger("ani", 2);
                    Invoke("ponerRatonPorEncima", 0.5f);
                    ratones_en_la_fiesta.Add(raton_en_la_puerta);
                    if (raton_en_la_puerta.traeComida())
                    {
                        plus_de_calidad += 30;
                        
                        textExito.text = "+30 FOOD BONUS";

                        textExito.gameObject.GetComponent<Animator>().SetInteger("ExitoText", 1);

                        Invoke("bajar_plus_de_calidad",20f);
                    }
                    raton_en_la_puerta = null;
                    Invoke("startTheCoroutineVienenRatones", 5f);
                }
                else if (raton_en_la_puerta.especial == 1) // Es el gato
                {
                    raton_en_la_puerta = null;

                    mouseAtTheDoor.transform.Find("sprite_gato").gameObject.SetActive(false);
                    mouseAtTheDoor.transform.Find("sprite_gato (1)").gameObject.SetActive(true);
                    mouseAnimator.SetInteger("ani", 2);
                    Invoke("ponerRatonPorEncima", 0.5f);

                    ratones_en_la_fiesta.Remove(Random.Range(0, ratones_en_la_fiesta.Count));
                    ratones_en_la_fiesta.Remove(Random.Range(0, ratones_en_la_fiesta.Count));
                    ratones_en_la_fiesta.Remove(Random.Range(0, ratones_en_la_fiesta.Count));

                    textAforo.text = getAforo()+"/20";

                    //Invoke("closeDoor", 2f);
                    Invoke("startTheCoroutineVienenRatones", 5f);
                }
                else if (raton_en_la_puerta.especial == 2) // 2 = TheMouse,
                {
                }
                else if (raton_en_la_puerta.especial == 3)// 3 = MousePitt
                {
                }
                else if (raton_en_la_puerta.especial == 4)// 4 = LifeMau,
                {
                }
                else if (raton_en_la_puerta.especial == 5)// 5 = Mousillex,
                {
                }
                else if (raton_en_la_puerta.especial == 6)// 6 = Police
                {
                    raton_en_la_puerta = null;

                    if (triggerConflicto)
                    {
                        StopCoroutine(coroutineVienenRatones);
                        coroutinePoesia = StartCoroutine(laPoesiaSeLlevaDos());
                    }
                    else
                    {
                        // reproducir audio de policia enfadado?
                        money -= 500;
                        if (money < 0) money = 0; // te pone multa
                        textMoney.text = money+"";

                        mouseAnimator.SetInteger("ani", 5);

                        Invoke("startTheCoroutineVienenRatones", 5f);
                        Invoke("closeDoor", 2f);
                    }
                }

            }
        }
    }

    void bajar_plus_de_calidad ()
    {
        plus_de_calidad -= 30;
    }

    Coroutine coroutinePoesia;

    IEnumerator laPoesiaSeLlevaDos ()
    {
        triggerConflicto = false;

        mouseAnimator.SetInteger("ani", 2); // entra la poesia
        Invoke("ponerRatonPorEncima", 0.5f);

        Debug.Log("poesia entra! esperando 5f segundos...");
        yield return new WaitForSeconds(5f);

        // se va un raton
        foreach (Raton raton in ratones_en_la_fiesta)
        {
            if (raton.esConflictivo())
            {
                raton_en_la_puerta = raton;
                cambiarSpritesRatonEnLaPuerta();
                ratones_en_la_fiesta.Remove(raton);
                break;
            }
        }
        mouseAnimator.SetInteger("ani", 4);
        yield return new WaitForSeconds(0.5f);
        mouseAnimator.SetInteger("ani", -1);
        Debug.Log("se va OTRO raton! esperando 8f segundos...");
        yield return new WaitForSeconds(4f);
        //mouseAnimator.SetInteger("ani", 3);
        //spritesRatonPorDentroDeLaCasa(false);
        //yield return new WaitForSeconds(5f);
        spritesRatonPorDentroDeLaCasa(true);

        // se va otro raton
        foreach (Raton raton in ratones_en_la_fiesta)
        {
            if (raton.esConflictivo())
            {
                raton_en_la_puerta = raton;
                cambiarSpritesRatonEnLaPuerta();
                ratones_en_la_fiesta.Remove(raton);
                break;
            }
        }
        mouseAnimator.SetInteger("ani", 4);
        yield return new WaitForSeconds(0.5f);
        mouseAnimator.SetInteger("ani", -1);
        Debug.Log("se va OTRO raton! esperando 8f segundos...");
        yield return new WaitForSeconds(4f);
        //mouseAnimator.SetInteger("ani", 3);
        //spritesRatonPorDentroDeLaCasa(false);
        //yield return new WaitForSeconds(5f);
        spritesRatonPorDentroDeLaCasa(true);

        raton_en_la_puerta = new Raton("Police", 6); // se va la poesia
        cambiarSpritesRatonEnLaPuerta();
        mouseAnimator.SetInteger("ani", 4);
        yield return new WaitForSeconds(0.5f);
        mouseAnimator.SetInteger("ani", -1);
        Debug.Log("se va la poesia! esperando 8f segundos...");
        yield return new WaitForSeconds(4f);
        //mouseAnimator.SetInteger("ani", 3);
        //spritesRatonPorDentroDeLaCasa(false);
        //yield return new WaitForSeconds(1f);
        spritesRatonPorDentroDeLaCasa(true);

        Invoke("closeDoor", 1f);
        Invoke("startTheCoroutineVienenRatones", 5f);
        

    }

    void ponerRatonPorEncima ()
    {
        spritesRatonPorDentroDeLaCasa(true);
    }

    public void spritesRatonPorDentroDeLaCasa (bool b)
    {
        //if (!hayUnGatoAhoraMismo) añadir despues
        //{
            int sorting = 20;
            if (!b) sorting = -19;

            int i = 0;

            foreach (SpriteRenderer sprite in mouseAtTheDoor.GetComponentsInChildren<SpriteRenderer>())
            {
                sprite.sortingOrder = sorting + i;
                i++;
            }
        //}
    }

    Coroutine coroutineVienenRatones;

    public void startTheCoroutineVienenRatones ()
    {
        coroutineVienenRatones = StartCoroutine(vienenRatones());
    }

    void closeDoor()
    {
        if (gameOn)
        {
            textAforo.text = ratones_en_la_fiesta.Count + "/20";
            door.gameObject.GetComponent<Animator>().SetInteger("animation", -1);
        }
    }

    public GameObject[] whenLookingThroughPeephole;
    public GameObject[] whenNOTlookingThroughPeephole;

    public Animator doorAnimator;

    public void look()
    {
        if (doorAnimator.GetCurrentAnimatorStateInfo(0).IsName("doorClosed"))
        {
            foreach (GameObject go in whenNOTlookingThroughPeephole) go.SetActive(false);
            foreach (GameObject go in whenLookingThroughPeephole) go.SetActive(true);
        }
    }

    public void stopLooking()
    {
        foreach (GameObject go in whenNOTlookingThroughPeephole) go.SetActive(true);
        foreach (GameObject go in whenLookingThroughPeephole) go.SetActive(false);
    }



    public Sprite button300coins;
    public Sprite button300coinsPulsado;

    public void lvlUpConfort()
    {
        if (gameOn)
        {
            if (confortLevel == 1 && money >= 150)
            {
                confortLevel++;
                confortLevelImage.sprite = levelTextSprites[1];
                money = money - 150;
                textMoney.text = money.ToString();
                confort.gameObject.GetComponent<Image>().sprite = attributeSprites[1];
                //GameManager.calidad = GameManager.calidad++; //Hay que decidir cuantos puntos te da subir cada cosa al final de la partida --> la calidad se calcula a partir de las mejoras y de otras cosas. No es un parametro, es un metodo. getCalidad()
                confortLevelUpgradeButton.despulsado = button300coins;
                confortLevelUpgradeButton.pulsado = button300coinsPulsado;
                playSound(upgradeButtonAudioClip);
            }
            else if (confortLevel == 2 && money >= 300)
            {
                confortLevel++;
                confortLevelImage.sprite = levelTextSprites[2];
                money = money - 300;
                textMoney.text = money.ToString();
                confort.gameObject.GetComponent<Image>().sprite = attributeSprites[2];
                //GameManager.calidad = GameManager.calidad++;
                confortLevelUpgradeButton.enableButton = false; // inutilizar boton de mejora
                confortLevelUpgradeButton.despulsado = maxLevelReachedSprite; // sustituir boton por "MAX"
                playSound(upgradeButtonAudioClip);
            }
        }
    }

    public void lvlUpAmbience()
    {
        if (gameOn)
        {
            if (ambienceLevel == 1 && money >= 150)
            {
                ambienceLevel++;
                ambienceLevelImage.sprite = levelTextSprites[1];
                money = money - 150;
                textMoney.text = money.ToString();
                ambience.gameObject.GetComponent<Image>().sprite = attributeSprites[4];
                //GameManager.calidad = GameManager.calidad++;
                ambienceLevelUpgradeButton.despulsado = button300coins;
                ambienceLevelUpgradeButton.pulsado = button300coinsPulsado;
                playSound(upgradeButtonAudioClip);
            }
            else if (ambienceLevel == 2 && money >= 300)
            {
                ambienceLevel++;
                ambienceLevelImage.sprite = levelTextSprites[2];
                money = money - 300;
                textMoney.text = money.ToString();
                ambience.gameObject.GetComponent<Image>().sprite = attributeSprites[5];
                //GameManager.calidad = GameManager.calidad++;
                ambienceLevelUpgradeButton.enableButton = false; // inutilizar boton de mejora
                ambienceLevelUpgradeButton.despulsado = maxLevelReachedSprite; // sustituir boton por "MAX"
                playSound(upgradeButtonAudioClip);
            }
        }
    }

    public void lvlUpMusic()
    {
        if (gameOn)
        {
            if (musicLevel == 1 && money >= 150)
            {
                musicLevel++;
                musicLevelImage.sprite = levelTextSprites[1];
                money = money - 150;
                textMoney.text = money.ToString();
                music.gameObject.GetComponent<Image>().sprite = attributeSprites[7];
                //GameManager.calidad = GameManager.calidad++;
                musicLevelUpgradeButton.despulsado = button300coins;
                musicLevelUpgradeButton.pulsado = button300coinsPulsado;
                playSound(upgradeButtonAudioClip);
            }
            else if (musicLevel == 2 && money >= 300)
            {
                musicLevel++;
                musicLevelImage.sprite = levelTextSprites[2];
                money = money - 300;
                textMoney.text = money.ToString();
                music.gameObject.GetComponent<Image>().sprite = attributeSprites[8];
                //GameManager.calidad = GameManager.calidad++;
                musicLevelUpgradeButton.enableButton = false; // inutilizar boton de mejora
                musicLevelUpgradeButton.despulsado = maxLevelReachedSprite; // sustituir boton por "MAX"
                playSound(upgradeButtonAudioClip);
            }
        }
    }

    public void lvlUpFood()
    {
        if (gameOn)
        {
            if (foodLevel == 1 && money >= 150)
            {
                foodLevel++;
                foodLevelImage.sprite = levelTextSprites[1];
                money = money - 150;
                textMoney.text = money.ToString();
                food.gameObject.GetComponent<Image>().sprite = attributeSprites[10];
                //GameManager.calidad = GameManager.calidad++;
                foodLevelUpgradeButton.despulsado = button300coins;
                foodLevelUpgradeButton.pulsado = button300coinsPulsado;
                playSound(upgradeButtonAudioClip);
            }
            else if (foodLevel == 2 && money >= 300)
            {
                foodLevel++;
                foodLevelImage.sprite = levelTextSprites[2];
                money = money - 300;
                textMoney.text = money.ToString();
                food.gameObject.GetComponent<Image>().sprite = attributeSprites[11];
                //GameManager.calidad = GameManager.calidad++;
                foodLevelUpgradeButton.enableButton = false; // inutilizar boton de mejora
                foodLevelUpgradeButton.despulsado = maxLevelReachedSprite; // sustituir boton por "MAX"
                playSound(upgradeButtonAudioClip);
            }
        }

    }

    public void phone (string mouseToCall)
    {
        if (gameOn)
        {
            phoneCallCoroutine = StartCoroutine(makePhoneCall(mouseToCall));
        }
    }

    public IEnumerator makePhoneCall (string mouseToCall)
    {
        if (gameOn)
        {
            bloquearTodo.SetActive(true);
            playSound(picarNumerosTelefono[Random.Range(0, 3)]);

            yield return new WaitForSeconds(2.5f);

            playSound(comunicandoAudioClip);

            yield return new WaitForSeconds(3);

            playSound(descolgarTelefonoAudioClip);

            yield return new WaitForSeconds(1);

            if (mouseToCall.Equals("The Mouse"))
            {
                if (getCalidad() > 20)
                {
                    raton_al_que_has_llamado = mouseToCall;
                }
            }
            else if (mouseToCall.Equals("Mouse Pitt"))
            {
                if (getCalidad() > 30)
                {
                    raton_al_que_has_llamado = mouseToCall;
                }
            }
            else if (mouseToCall.Equals("LifeMau5"))
            {
                if (getCalidad() > 40)
                {
                    raton_al_que_has_llamado = mouseToCall;
                }
            }
            else if (mouseToCall.Equals("Mousillex"))
            {
                if (getCalidad() > 50)
                {
                    raton_al_que_has_llamado = mouseToCall;
                }
            }
            else if (mouseToCall.Equals("Police"))
            {
                raton_al_que_has_llamado = mouseToCall;
            }

            bloquearTodo.SetActive(false);
        }
    }

    public void upgradesMenu_show()
    {
        if (gameOn)
        {
            upgradesMenuOpen.SetActive(true);
            upgradesMenuClosed.SetActive(false);
        }
    }

    public void upgradesMenu_hide()
    {
        if (gameOn)
        {
            upgradesMenuOpen.SetActive(false);
            upgradesMenuClosed.SetActive(true);
        }
    }

    public void tickets_increase()
    {
        if (gameOn)
        {
            precio_entrada += 10;
            textTickets.text = precio_entrada.ToString();
        }
    }

    public void tickets_decrease()
    {
        if (gameOn)
        {
            if(precio_entrada > 0)
            {
                precio_entrada -= 10;
                textTickets.text = precio_entrada.ToString();
            }
        }
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

    Coroutine phoneCallCoroutine;

    public void descolgarTelefono()
    {
        if (gameOn)
        {
            telefonoDescolgado.SetActive(true);
            telefonoColgado.SetActive(false);
            playSound(descolgarTelefonoAudioClip);
        }
    }

    public void colgarTelefono()
    {
        if (gameOn)
        {
            if (phoneCallCoroutine!=null) StopCoroutine(phoneCallCoroutine);
            bloquearTodo.SetActive(false);
            telefonoDescolgado.SetActive(false);
            telefonoColgado.SetActive(true);
            playSound(colgarTelefonoAudioClip);
        }
    }




    public void playSound (AudioClip audioClip)
    {
        if (gameOn)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}
