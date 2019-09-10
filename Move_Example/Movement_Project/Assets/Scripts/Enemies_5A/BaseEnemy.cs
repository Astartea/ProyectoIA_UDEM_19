using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BaseEnemy : MonoBehaviour, IDamage
{
    //Los diferentes estados de los enemigos
    public enum StateEnemy
    {
        Walk, Attack, Dead,
    }
    [Header("Status enemy")]
    public StateEnemy curState; //El estado actual del enemigo

    [Header("Basic Elements")]
    [Range(0, 1000)]
    public int maxHealth;       //El valor maximo de vida
    [HideInInspector]
    public int curHealth;       //El valor actual de vida
    [Range(1, 500)]
    public int attackPower;     //Poder de ataque del enemigo
    [Range(1, 100)]
    public int defense;         //Cantidad de defensa
    [Range(1, 50)]
    public int moveSpeed;       //Velocidad de movimiento

    [Tooltip("Distancia minima a la que debe acercarse el enemmigo a su actual target")]
    [Range(1, 100)]
    public float minDistance;

    [HideInInspector]
    public GameObject curTarget;          //El objetivo actual del enemigo

    [HideInInspector]
    public AIDestinationSetter destination;    //El lugar a donde se debe mover el enemigo
    [HideInInspector]
    public AILerp ctrlMovement;            //Control de movimiento en el A*

    [HideInInspector]
    public Rigidbody rdb3D;

    [Header("Sensors")]
    public LayerMask playerMask;    //El layer del jugador
    public Transform posSensorView; //Posicion del sensor de deteccion
    public Collider[] hitCollider; //Arreglo de collisionadores para saber que detectamos
    [Range(0.5f, 50f)]
    public float radiusDetection;   //Radio de deteccion

    // Start is called before the first frame update
    void Start()
    {
        InitEnemy(); //Inicializa el enemigo al inicio del juego
    }

    //Cada que se reutilice el enemigo, hay que resetearlo con
    //este metodo
    public virtual void InitEnemy()
    {
        rdb3D = GetComponent<Rigidbody>(); //Obten el rigidBody
        hitCollider = null;

        ctrlMovement = GetComponent<AILerp>();
        ctrlMovement.speed = moveSpeed;
        curTarget = GameObject.FindGameObjectWithTag("Player");

        destination = GetComponent<AIDestinationSetter>();
        destination.target = curTarget.transform;

        //Resetea la vida
        curHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (curState != StateEnemy.Dead)
        {
            BehaviourEnemy();
        }
    }

    /// <summary>
    /// This function is called every fixed framerate frame, 
    /// if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        if (curState != StateEnemy.Dead)
        {
            SensorsDetection();
        }
    }

    //Los sensores especificos del enemigo
    public virtual void SensorsDetection()
    {
        hitCollider = Physics.OverlapSphere(
        posSensorView.position, radiusDetection, playerMask);

        int i = 0;
        while (i < hitCollider.Length)
        {
            if (hitCollider[i].gameObject.name == "Player_Cam_Game")
            {
                Debug.LogError("DEtecte al jugador");
                curTarget = hitCollider[i].gameObject; //Asignar el objeto jugador
                curState = StateEnemy.Attack;
                destination.target = curTarget.transform;
            }
            i++;
        }

        if (hitCollider.Length < 1)
        {
            curState = StateEnemy.Walk;
        }
    }

    //El comportamiento especifico de cada enemigo
    public virtual void BehaviourEnemy()
    {
        if (curState == StateEnemy.Attack)
        {
            float actualDistance = Vector3.Distance(transform.position, curTarget.transform.position);

            if (actualDistance <= minDistance)
            {

                AttackMode();
            }
        }
    }

    public virtual void AttackMode()
    {

    }

    public virtual void TakeDamage(int totalDamage)
    {
        //Si el enemigo esta muerto
        if (curState == StateEnemy.Dead)
            return; //No hagas nada, termina la funcion aqui

        //Restar el total de daño a la vida actual
        curHealth -= (totalDamage -= defense);

        //CAMBIAR COLORCITO DEL ENEMIGO

        //Cual es la cantidad de vida del enemigo
        if (curHealth < 1)
        {
            //Si ya no tiene, esta muerto
            curState = StateEnemy.Dead;
            //ACTIVAR ANIMACION DE MUERTE
        }

        //QUE TIPO DE PROYECTIL O ARMA NOS ESTA DAÑADO

    }

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(posSensorView.position, radiusDetection);
    }
}
