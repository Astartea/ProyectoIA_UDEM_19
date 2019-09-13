using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyBase : MonoBehaviour, ITakeDamage
{
    public enum EnemyStatus
    {
        MOVE, ATTACK, DEAD,
    }
    [Header("Status Enemy")]
    public EnemyStatus curState;

    [Header("Basic Elements")] 
    public int maxHealth;       //El maximo de vida del enemigo
    [HideInInspector]
    public int curHealth;   //Vida actual del enemigo
    [Range(1, 1000)]
    public int attackPower; //Poder de ataque
    [Range(1, 500)]
    public int defense;     //Defensa del enemigo
    [Range(1, 50)]
    public int moveSpeed;   //Velocidad de movimiento

    [HideInInspector]
    public GameObject curTarget;   //El objetivo actual del enemigo

    [HideInInspector]
    public AIDestinationSetter destinationSetter;
    [HideInInspector]
    public AILerp ctrlMovement;

    Rigidbody rbd3D;

    [Header("Sensors")]
    public LayerMask playerMask;    //Layer del jugador para la deteccion
    //Para detectar al jugador
    public Transform posSensorViewPlayer;
    public Collider[] hitCollider;
    [Range(2, 50)]
    public float radiusDetection;

    // Start is called before the first frame update
    void Start()
    {
        InitEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void InitEnemy()
    {
        rbd3D = GetComponent<Rigidbody>();
        hitCollider = null;

        ctrlMovement = GetComponent<AILerp>();
        ctrlMovement.speed = moveSpeed;
        curTarget = GameObject.FindGameObjectWithTag("Player");

        destinationSetter = GetComponent<AIDestinationSetter>();
        destinationSetter.target = curTarget.transform;

        curHealth = maxHealth;
    }

    public virtual void BehaviourEnemy()
    {

    }

    public void SensorsDetection()
    {
        hitCollider = Physics.OverlapSphere
        (posSensorViewPlayer.position, radiusDetection, playerMask);
    
        int i = 0;
        while(i < hitCollider.Length)
        {
            if(hitCollider[i].gameObject.name == "Player_Cam_Game")
            {
                curTarget = hitCollider[i].gameObject;
                curState = EnemyStatus.ATTACK;
                destinationSetter.target = curTarget.transform;
            }
            
            i++;
        }
    }

    public void ReciveDamage(int totalDamage)
    {
        if(curState == EnemyStatus.DEAD)
        {
            return;
        }

        curHealth -= (totalDamage - defense);

        if(curHealth < 1)
        {
            curState = EnemyStatus.DEAD;
        }
    }
}
