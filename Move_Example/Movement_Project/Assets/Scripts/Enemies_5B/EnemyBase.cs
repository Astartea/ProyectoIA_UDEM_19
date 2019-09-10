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

    AIDestinationSetter destinationSetter;
    AILerp ctrlMovement;

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
        //se inicializa el enemigo
        curHealth = maxHealth;
    }

    public virtual void BehaviourEnemy()
    {

    }

    public void SensorsDetection()
    {

    }

    public void ReciveDamage(int totalDamage)
    {
        curHealth -= (totalDamage - defense);
    }
}
