using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    private Cell cell;
    private List<Popolation> pop;
    private List<Popolation> exodus;
    private bool deficit;
    public float food;


    public void Found(Cell cell, List<Popolation> pop, Civ civ)
    {
        this.cell=cell;
        this.pop=pop;
        exodus=new List<Popolation>();
        deficit=false;
        this.cell.SetCiv(civ);
    }
    public void ChangeCell (Cell cell)
    {
        this.cell=cell;
        transform.position=cell.transform.position;
    }

    public void AddPop(Popolation civilian)
    {
        if (pop.Count<100)
        {
            pop.Add(civilian);
        }else
        {
            exodus.Add(civilian);
        }
        
    }

    public void KillPop (int j) //kill "i" civilian in the list
    {
        int random;
        for (int i = 0; i < j; i++)
        {
            random = Random.Range(0,pop.Count);
            Debug.Log("kill " + j + " out of " + pop.Count);
            pop.RemoveAt(random);
        }
    }
    public void KillOld (int j) //kill the first "i" elements
    {
        for (int i = 0; i < j; i++)
        {
            pop.RemoveAt(0);
        }
    }

    public void NewGen() //make 2 by 2 procreation
    {
        int npop = pop.Count;
        for (int i = 0; i < npop; i+=2)
        {
            NewPop(pop[i],pop[i+1]);
        }
    }

    public void NewPop(Popolation pop1, Popolation pop2) //every child have a 33% of beeing the exact avarage of its parents or have an ofset in both stats
    {
        int mutation= Random.Range(0,3);
        Popolation civilian;
        switch (mutation)
        {
            case 0:
                civilian = new Popolation(Avg(pop1.GetFoodProd(),pop2.GetFoodProd()), Avg(pop1.GetFight(),pop2.GetFight()));
                AddPop(civilian);
                break;
            case 1:
                civilian = new Popolation(Avg(pop1.GetFoodProd(),pop2.GetFoodProd())+0.1f, Avg(pop1.GetFight(),pop2.GetFight())-0.1f);
                AddPop(civilian);
                break;
            case 2:
                civilian = new Popolation(Avg(pop1.GetFoodProd(),pop2.GetFoodProd())-0.1f, Avg(pop1.GetFight(),pop2.GetFight())+0.1f);
                AddPop(civilian);
                break;
        }
    }
    public void ProduceFood()
    {
        //Debug.Log("      CITY FOOD PRODUCTION:");
        foreach (Popolation civilian in pop)
        {
            food+=civilian.Produce(cell.GetFertility());
        }
    }

    public void ConsumeFood()
    {
        food-=pop.Count*1.4f;
        if (food<0)
        {
            deficit=true;
            Debug.Log("CITY IN DEFICIT, food ==> " + food);
        }
    }
    public float GetFood()
    {
        return food;
    }
    public void SetFood(float food)
    {
        this.food=food;
    }
    public List<Popolation> GetPops()
    {
        return pop;
    }
    public List<Popolation> GetExodus()
    {
        return exodus;
    }
    public int MissingTo1H()
    {
        return 100-pop.Count;
    }
    public Cell GetCell()
    {
        return cell;
    }





















    private float Avg(float a, float b)
    {
        return (a+b)/2f;
    }
}
