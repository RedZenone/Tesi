using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Civ : MonoBehaviour
{
    private float food_surplus;
    private List<City> cities;
    private List<GameObject> cities_objects;
    private List<City> cities_deficit;
    private List<Cell> borderingtiles;
    public GameObject city_prefab;

    public void Founding(Cell cell, Color mycolor)
    {
        List<Popolation> pops = new List<Popolation>();
        for (int i = 0; i < 10; i++)
        {
            Popolation pop = new Popolation(1.5f,1.5f);
            pops.Add(pop);
        }
        AddCity(cell, pops, mycolor);
    }

    public void AddCity(Cell cell, List<Popolation> pops, Color mycolor)
    {
        GameObject city = Instantiate(city_prefab,transform);
        city.GetComponent<City>().Found(cell,pops,this);
        cities.Add(city.GetComponent<City>());
        cities_objects.Add(city);
        city.GetComponent<City>().GetComponent<SpriteRenderer>().color=mycolor;

        //take trak of every tile on the border of the civilization
        if (borderingtiles.Contains(city.GetComponent<City>().GetCell()))
        {
            borderingtiles.Remove(city.GetComponent<City>().GetCell());
        }
        foreach (Cell centercell in city.GetComponent<City>().GetCell().GetGraph().LandNeighbour(city.GetComponent<City>().GetCell()))
        {
            if (centercell.GetCiv()!=this)
            {
                borderingtiles.Add(centercell);
            }
        }
    }

    public void ProduceFood()
    {
        foreach (City city in cities) //produce food in every city and calculate hte excess
        {
            city.ProduceFood();
            city.ConsumeFood();
            if (city.GetFood()<0)
            {
                cities_deficit.Add(city);
            }else
            {
                food_surplus+=city.GetFood();
            }
        }
        foreach (City city in cities_deficit) //try to safe the cities that are in deficit of food with the excess from the others
        {
            if (food_surplus> city.GetFood())
            {
                food_surplus-=city.GetFood();
                cities_deficit.Remove(city);                
            }else
            {
                city.SetFood(city.GetFood()-food_surplus);
            }
        }
    }
    public void KillStarved()
    {
        foreach (City city in cities_deficit)
        {
            city.KillPop(Mathf.RoundToInt(city.GetFood()/2.5f)); //each pop consume 2.5 unit of food
            city.KillOld(Mathf.RoundToInt(city.GetPops().Count/10f)); //each genetarion 10% of the total popolation dies of "old age"
            if (city.GetPops().Count<6)
            {
                DestroyCity(city);
            }
        }
        
    }
    public void DestroyCity(City city) //TOGLIERE ANCHE LA REFERENCE ALLA CIV 
    {
        if (cities.Contains(city))
        {
            cities.Remove(city);
        }
        if (cities_deficit.Contains(city))
        {
            cities_deficit.Remove(city);            
        }
        Destroy(city); //PROBABILMENTE NON DISTRUGGE L'OGGETTO MA SOLO LA CITTA' NELLA LISTA
        //SOSTITUIRE LE ISTANZE DI city CON L'OGGETTO STESSO E REFENZIARE OVUNQUE CON GETCOMPONENT<CITY>
    }
    public void NewGen() //create new children for each 
    {
        foreach (City city in cities)
        {
            city.NewGen();
        }
        foreach (City city in cities)
        {
            if (city.GetExodus().Count>0)
            {
                while (city.GetExodus().Count>0)
                {
                    foreach (City city2 in cities)
                    {
                        if (city2.GetPops().Count<100 && city2.MissingTo1H()<city.GetExodus().Count)
                        {
                            for (int i = 0; i < city2.MissingTo1H(); i++)
                            {
                                city2.AddPop(city.GetExodus()[0]);
                                city.GetExodus().RemoveAt(0);
                            }
                        }
                    }
                }
            }
        }
    }
}
