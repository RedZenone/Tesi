using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public GameObject myCell;
    public CellGraph graph;
    void Start()
    {
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 20; x++)
            {

                GameObject cell = Instantiate(myCell, transform);
                if (x % 2 == 0)
                {
                    cell.transform.position= new Vector3(x*0.75f, y*0.866f, 0);
                }
                else
                {
                    cell.transform.position= new Vector3(x*0.75f, y*0.866f + 0.433f, 0);
                }
                graph.addCell(cell,x,y);
            }
        }
    }

    void Update()
    {
        
    }
}
