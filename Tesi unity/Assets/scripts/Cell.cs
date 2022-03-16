using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Cell : MonoBehaviour
{

    private int x;
    private int y;
    SpriteRenderer sprrenderer;
    CellGraph graph;
    float maxfertility;
    float fertility;
    private Civ civ;
    private bool occupied;
    public enum celltypes
    {
        water,
        grassland,
        mountain,
        desert
    }
    private celltypes type;

    public void CellSetup (int x, int y, CellGraph graph)
    {
        this.x=x;
        this.y=y;
        sprrenderer=GetComponent<SpriteRenderer>();
        this.graph=graph;
        type=celltypes.water;
    }

    public void OnMouseDown()
    {
        if (type==celltypes.water && graph.terraformer.GetTerType()==TerraformController.terraformtype.grassland)
        {
            Landification(graph.terraformer.terraform_radius);
        }
        else if (type==celltypes.grassland && graph.terraformer.GetTerType()==TerraformController.terraformtype.desert)
        {
            Desertification(graph.terraformer.terraform_radius);
        }
        else if (type==celltypes.grassland && graph.terraformer.GetTerType()==TerraformController.terraformtype.mountain)
        {
            Mountainification(graph.terraformer.terraform_radius);
        }
	}
    public void Landification(int perc)
    {
        //starting at 100% with the first cell, the probability of turning into a land tile decrese by a fixed amount each jump
        //recursive
        if (perc>0)
        {
            int range = Random.Range(0,graph.terraformer.terraform_radius);
            if (perc>range)
            {
                sprrenderer.color=Color.green;
                type=celltypes.grassland;
                graph.AddGrass(this);
                SetMaxFert(1.5f);
                foreach (Cell cell in graph.GetNeighbour(this))
                {
                    cell.Landification(perc-1);
                }
            }
        }
        
    }
    public void Mountainification(int perc)
    {
        if (perc>0)
        {
            int range = Random.Range(0,graph.terraformer.terraform_radius);
            Debug.Log("Mountification: rolled -> " + range + "/" + perc);
            if (perc>range)
            {
                sprrenderer.color=Color.grey;
                type=celltypes.mountain;
                graph.AddMountain(this);
                SetMaxFert(0.5f);
                int randomneighbour = Random.Range(0,graph.GrassNeighbour(this).Count);
                Cell cell=graph.GrassNeighbour(this)[randomneighbour];
                cell.Mountainification(perc-1);
            }
        }
    }
    public void Desertification(int perc)
    {
        if (perc>0)
        {
            int range = Random.Range(0,graph.terraformer.terraform_radius);
            if (perc>range)
            {
                sprrenderer.color=Color.yellow;
                type=celltypes.desert;
                graph.AddDesert(this);
                SetMaxFert(0.1f);
                foreach (Cell cell in graph.GrassNeighbour(this))
                {
                    cell.Desertification(perc-1);
                }
            }
        }
    }




//____________VARIABLES______________________________________

    public void SetMaxFert(float fert)
    {
        maxfertility=fert;
        fertility=fert;
    }
    public void ChangeFert(float fert)
    {
        fertility+=fert;
    }
    public float GetFertility()
    {
        return fertility;
    }
    public int Getx()
    {
        return this.x;
    }
    public int Gety()
    {
        return this.y;
    }
    public celltypes GetCellType()
    {
        return type;
    }
    public void SetCiv(Civ civ)
    {
        this.civ=civ;
    }
    public Civ GetCiv()
    {
        if (civ!=null)
        {
            return civ;
        }
        return null;
    }
    public CellGraph GetGraph()
    {
        return graph;
    }







//____________DEBUG FUNCTIONS________________________________

    private void Print()
    {
		Debug.Log(x + " " + y);
    }

    private void CheckLandneighbour()
    {
        foreach (Cell cell in graph.LandNeighbour(this))
        {
            cell.sprrenderer.color=Color.black;
        }
    }
    private void CheckGrassNeighbour()
    {
        foreach (Cell cell in graph.GrassNeighbour(this))
        {
            cell.sprrenderer.color=Color.black;
        }
    }
    private void CheckMountainNeighbour()
    {
        foreach (Cell cell in graph.MountainNeighbour(this))
        {
            cell.sprrenderer.color=Color.black;
        }
    }
    private void CheckNeighbour()
    {
        foreach (Cell cell in graph.GetNeighbour(this))
        {
            cell.sprrenderer.color=Color.black;
        }
    }

}
