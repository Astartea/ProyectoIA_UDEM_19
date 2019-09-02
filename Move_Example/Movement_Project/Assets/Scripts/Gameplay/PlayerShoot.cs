using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShoot : MonoBehaviour
{
    public GameObject mira;             //La mirilla en la UI
    public GameObject currentWeapon;    //Lugar en donde estara el arma, debe ser un objeto vacio
    public GameObject lookHereWeapon;   //Hacia donde tiene que ver el arma

    Camera theCamera;

    [Header("Max and Min values for the mouse position on screen")]
    public float maxX;
    public float minX;
    public float maxY;
    public float minY;


    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        theCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //Para evitar que la mira salga de la posicion en pantalla
        if(theCamera.ScreenToViewportPoint(Input.mousePosition).x > minX &&
           theCamera.ScreenToViewportPoint(Input.mousePosition).x < maxX &&
           theCamera.ScreenToViewportPoint(Input.mousePosition).y > minY &&
           theCamera.ScreenToViewportPoint(Input.mousePosition).y < maxY)
           {
               mira.transform.position = Input.mousePosition;
           }

           //Asignamos la posicion del objeto vacio (LookHereWeapon)
           // en base a la posicion del mouse en pantalla convitiendo las coordenadas a mundo
           lookHereWeapon.transform.position = theCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 7.6f));

           //Hay que asignar al objeto donde esta ubicada el arma, hacia donde debe mirar
           currentWeapon.transform.LookAt(lookHereWeapon.transform.position);

           //Crear un raycast cuando se presione el boton izquierdo del mouse
           if(Input.GetMouseButtonDown(0))
           {
               if(Physics.Raycast(theCamera.ScreenPointToRay(Input.mousePosition), out hit, 20f))
               {
                   Debug.LogError("Le dispare a: "+hit.collider.name);
               }
           }
    }
}
