using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Shape[] m_allShapes;

    Shape GetRandomShape()
    {
        int i = Random.Range(0, m_allShapes.Length);
        if (!m_allShapes[i])
        {
            Debug.Log("WARNING! Invalid shape!");
            return null;
        }

        return m_allShapes[i];
    }

    public Shape SpawnShape()
    {
        Shape shape = null;
        shape = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as Shape;

        if (!shape)
        {
            Debug.LogWarning("WARNING! Invalid shape in spawner!");
            return null;
        }

        return shape;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
