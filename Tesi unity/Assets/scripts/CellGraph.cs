using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGraph : MonoBehaviour
{
    private Cell[,] graph;
    private List<Cell> grasslands;
    private List<Cell> mountains;
    private List<Cell> deserts;
    public TerraformController terraformer;

    void Start()
    {
        graph = new Cell[20,10];
        grasslands = new List<Cell>();
        mountains = new List<Cell>();
        deserts = new List<Cell>();
    }
    public void addCell( GameObject cell, int x, int y)
    {
        graph[x,y]=cell.GetComponent<Cell>();
        graph[x,y].CellSetup(x,y,this);
    }

//________NEIGHBOURS OF CELLS__________________________________
    public List<Cell> GetNeighbour(Cell cell)
    {
        int cellx=cell.Getx();
        int celly=cell.Gety();
        List<Cell> cells = new List<Cell>();
        if (cellx%2!=0) //per le celle con x dispari
        {
            if (cellx>0)
            {
                cells.Add(graph[cellx-1,celly]);
                if (celly<9)
                {
                    cells.Add(graph[cellx-1,celly+1]);
                }
            }
            if (cellx<19)
            {
                cells.Add(graph[cellx+1,celly]);
                if (celly<9)
                {
                    cells.Add(graph[cellx+1,celly+1]);
                }
            }
        }
        else //per le celle con x pari
        {
            if (cellx>0)
            {
                cells.Add(graph[cellx-1,celly]);
                if (celly>0)
                {
                    cells.Add(graph[cellx-1,celly-1]);
                }
            }
            if (cellx<19)
            {
                cells.Add(graph[cellx+1,celly]);
                if (celly>0)
                {
                    cells.Add(graph[cellx+1,celly-1]);
                }
            }
        }
        if (celly<9)
        {
            cells.Add(graph[cellx,celly+1]);
        }
        if (celly>0)
        {
            cells.Add(graph[cellx,celly-1]);
        }
        
        return cells;
    }
    public List<Cell> LandNeighbour(Cell cell)
    {
        List<Cell> cells = new List<Cell>();
        foreach (Cell cell2 in GetNeighbour(cell))
        {
            if (cell2.GetCellType()!=Cell.celltypes.water)
            {
                cells.Add(cell2);
            }
        }
        return cells;
    }
    public List<Cell> GrassNeighbour(Cell cell)
    {
        List<Cell> cells = new List<Cell>();
        foreach (Cell cell2 in GetNeighbour(cell))
        {
            if (cell2.GetCellType()==Cell.celltypes.grassland)
            {
                cells.Add(cell2);
            }
        }
        return cells;
    }
    public List<Cell> MountainNeighbour(Cell cell)
    {
        List<Cell> cells = new List<Cell>();
        foreach (Cell cell2 in GetNeighbour(cell))
        {
            if (cell2.GetCellType()==Cell.celltypes.mountain)
            {
                cells.Add(cell2);
            }
        }
        return cells;
    }
    public List<Cell> DesertNeighbour(Cell cell)
    {
        List<Cell> cells = new List<Cell>();
        foreach (Cell cell2 in GetNeighbour(cell))
        {
            if (cell2.GetCellType()==Cell.celltypes.desert)
            {
                cells.Add(cell2);
            }
        }
        return cells;
    }
//_____________________________________________________________
    public void AddGrass (Cell cell)
    {
        RemoveDesert(cell);
        RemoveMountain(cell);
        grasslands.Add(cell);
    }
    public void AddMountain (Cell cell)
    {
        RemoveDesert(cell);
        RemoveGrass(cell);
        mountains.Add(cell);
    }
    public void AddDesert (Cell cell)
    {
        RemoveGrass(cell);
        RemoveMountain(cell);
        deserts.Add(cell);
    }
    public void RemoveGrass (Cell cell)
    {
        if (grasslands.Contains(cell))
        {
            grasslands.Remove(cell);
        }        
    }
    public void RemoveMountain (Cell cell)
    {
        if (mountains.Contains(cell))
        {
            mountains.Remove(cell);
        } 
    }
    public void RemoveDesert (Cell cell)
    {
        if (deserts.Contains(cell))
        {
            deserts.Remove(cell);
        } 
    }

}
