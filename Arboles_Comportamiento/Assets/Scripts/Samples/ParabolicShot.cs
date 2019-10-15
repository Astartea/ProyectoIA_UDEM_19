using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicShot : MonoBehaviour
{
    public Color m_evaluating;
    public Color m_succeeded;
    public Color m_failed;

    public Sequence m_rootNode;
    List<Node> rootChildren;

    public bool canLaunch = false;
    public GameObject target;
    public Transform launchPoint;

    public LayerMask targetMask;
    public Transform posSensor;
    public float radiusSensor = 1f;

    public ActionNode m_node2A; //Check distance
    public ActionNode m_node2B;//Define Velocity
    public ActionNode m_node2C;//Result

    public GameObject m_rootNodeBox;
    public GameObject m_node2aBox;
    public GameObject m_node2bBox;
    public GameObject m_node2cBox;


    // Use this for initialization
    void Start()
    {
        m_node2A = new ActionNode(DefineDistance);

        m_node2B = new ActionNode(DefineVelocityOnX_OnY);

        m_node2C = new ActionNode(ResultParabolicShot);

        List<Node> rootChildren = new List<Node>();
        rootChildren.Add(m_node2A);
        rootChildren.Add(m_node2B);
        rootChildren.Add(m_node2C);

        m_rootNode = new Sequence(rootChildren);
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        Collider[] hitColliders = Physics.OverlapSphere(posSensor.position, radiusSensor, targetMask);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders.Length > 0)
            {
                canLaunch = true;

                if (m_rootNode.nodeState == NodeStates.SUCCESS)
                {
                    Debug.Log("Lanza proyectil");
                    launchPoint.rotation = Quaternion.LookRotation(result);
                    //Debug.LogError("Rotación: "+result);
                    Debug.LogError("Angulo: " + launchPoint.eulerAngles);
                }

                
            }
            i++;
        }

        if (hitColliders.Length == 0)
        {
            canLaunch = false;
        }
        m_rootNode.Evaluate();
		UpdateBoxes();
    }

    Vector3 distance;
    Vector3 distanceXZ;

    float d_Y, d_XZ;

    NodeStates DefineDistance()
    {
        if (!canLaunch)
        {
            return NodeStates.FAILURE;
        }
        else
        {
            //Definiendo la distancia respecto al target
            distance = target.transform.position - launchPoint.position;
            distanceXZ = distance;
            distanceXZ.y = 0f;

            d_Y = distance.y;
            d_XZ = distanceXZ.magnitude;

            return NodeStates.SUCCESS;
        }
    }

    float Vxz, Vy;
    float time = 1;
    NodeStates DefineVelocityOnX_OnY()
    {
        if (!canLaunch)
        {
            return NodeStates.FAILURE;
        }
        else
        {
            /*
            d_XZ = Vx * t
            Vx = d_XZ / t
         	*/
            Vxz = d_XZ / time;
            Debug.LogError("Velocidad en X: " + Vxz);

            /*
            y = Vyo * t - 1/2 * g * t^2
            y + 1/2 * g * t^2 = Vyo * t
            
            Vyo = (y + 1/2 * g * t^2)/ t
            Vyo = y / t + 1/2 * g * t
         	*/
            Vy = d_Y / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;
            Debug.LogError("Velocidad en Y: " + Vy);
            return NodeStates.SUCCESS;
        }
    }

    Vector3 result;

    NodeStates ResultParabolicShot()
    {
        if (!canLaunch)
        {
            return NodeStates.FAILURE;
        }
        else
        {
            result = distanceXZ.normalized;
            result *= Vxz;
            result.y = Vy;

            return NodeStates.SUCCESS;
        }
    }

    private void UpdateBoxes()
    {
        /** Update root node box */
        if (m_rootNode.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(m_rootNodeBox);
        }
        else if (m_rootNode.nodeState == NodeStates.FAILURE)
        {
            SetFailed(m_rootNodeBox);
        }

        /** Update 2A node box */
        if (m_node2A.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(m_node2aBox);
        }
        else if (m_node2A.nodeState == NodeStates.FAILURE)
        {
            SetFailed(m_node2aBox);
        }

        /** Update 2B node box */
        if (m_node2B.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(m_node2bBox);
        }
        else if (m_node2B.nodeState == NodeStates.FAILURE)
        {
            SetFailed(m_node2bBox);
        }

        /** Update 2C node box */
        if (m_node2C.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(m_node2cBox);
        }
        else if (m_node2C.nodeState == NodeStates.FAILURE)
        {
            SetFailed(m_node2cBox);
        }
    }

    private void SetEvaluating(GameObject box)
    {
        box.GetComponent<Renderer>().material.color = m_evaluating;
    }

    private void SetSucceeded(GameObject box)
    {
        box.GetComponent<Renderer>().material.color = m_succeeded;
    }

    private void SetFailed(GameObject box)
    {
        box.GetComponent<Renderer>().material.color = m_failed;
    }

    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(posSensor.position, radiusSensor);
    }

}
