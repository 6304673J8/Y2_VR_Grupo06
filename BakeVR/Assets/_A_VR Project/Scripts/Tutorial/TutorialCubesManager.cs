using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCubesManager : MonoBehaviour
{
    public bool cleanedTable;
    private int fellCube;
    private int totalCubes;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<Collider>().gameObject.CompareTag("MainSolution"))
        {
            totalCubes++;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.GetComponent<Collider>().gameObject.CompareTag("MainSolution"))
        {
            fellCube++;
        }
        if (fellCube == totalCubes)
        {
            cleanedTable = true;
        }
    }

    public void SetCleanedTable()
    {
        if (!cleanedTable)
        {
            cleanedTable = true;
        }
    }
}
