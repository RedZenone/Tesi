using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Civ : MonoBehaviour
{
    public float food_surplus;
    public int popcount;
    private List<City> cities;
    private List<GameObject> cities_objects;
    private List<City> cities_deficit;
    private List<City> tagged_for_destruction;
    private List<Cell> borderingtiles;
    public GameObject city_prefab;
    private GenerationController generationController;

    public void Founding(Cell cell, Color mycolor)
    {
        generationController= GameObject.FindWithTag("manager").GetComponent<GenerationController>();
        generationController.AddCiv(this);
        cities= new List<City>();
        cities_deficit=new List<City>();
        cities_objects = new List<GameObject>();
        borderingtiles = new List<Cell>();
        tagged_for_destruction = new List<City>();
        List<Popolation> pops = new List<Popolation>();
        for (int i = 0; i < 10; i++)
        {
            Popolation pop = new Popolation(1.5f,1.5f);
            pops.Add(pop);
        }
        AddCity(cell, pops, mycolor);
        popcount=10;
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
        Debug.Log("CIVILIZATION FOOD PRODUCTION:");
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
        if (food_surplus>0)
        {
            int deficitcount= cities_deficit.Count;
            for (int i = 0; i < deficitcount; i++)
            {
                if (food_surplus> Mathf.Abs(cities_deficit[i].GetFood()))
                {
                    food_surplus-=Mathf.Abs(cities_deficit[i].GetFood());
                    cities_deficit.RemoveAt(i);                
                }else
                {
                    cities_deficit[i].SetFood(cities_deficit[i].GetFood()-food_surplus);
                }
            }
        }
        
        /*foreach (City city in cities_deficit) //try to safe the cities that are in deficit of food with the excess from the others
        {
            if (food_surplus> city.GetFood())
            {
                food_surplus-=city.GetFood();
                cities_deficit.Remove(city);                
            }else
            {
                city.SetFood(city.GetFood()-food_surplus);
            }
        }*/
    }
    public void KillStarved()
    {
        foreach (City city in cities_deficit)
        {
            city.KillPop(Mathf.RoundToInt(Mathf.Abs(city.GetFood())/2.5f)); //each pop consume 2.5 unit of food
            Debug.Log("CIV " + Mathf.RoundToInt(Mathf.Abs(city.GetFood())/2.5f) + " DIED");
            if (city.GetPops().Count<6 && !tagged_for_destruction.Contains(city))
            {
                tagged_for_destruction.Add(city);
            }
        }
        int tagged= tagged_for_destruction.Count;
        for (int i = 0; i < tagged; i++)
        {
            DestroyCity(tagged_for_destruction[0]);
        }
        
    }
    public void KillOld()
    {
        foreach (City city in cities)
        {
            city.KillOld(Mathf.RoundToInt(city.GetPops().Count/10f)); //each genetarion 10% of the total popolation dies of "old age"
            Debug.Log("CIV " + Mathf.RoundToInt(city.GetPops().Count/10f) + " DIED");
            if (city.GetPops().Count<6)
            {
                tagged_for_destruction.Add(city);
            }
        }
        int tagged= tagged_for_destruction.Count;
        for (int i = 0; i < tagged; i++)
        {
            DestroyCity(tagged_for_destruction[0]);
        }
    }
    public void DestroyCity(City city) 
    {
        Debug.Log("CITY LOST!!!!!!");
        if (cities.Contains(city))
        {
            cities.Remove(city);
            city.GetCell().SetCiv(null);
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

    private void UpdateBoreder(Cell cell)
    {

    }

    //DA TOGLIERE, E' SOLO PER DEBUGGARE
    void Update()
    {
        popcount=0;
        foreach (City city in cities)
        {
            popcount+=city.GetPops().Count;
        }
    }

    public void CleanupFood()
    {
        food_surplus=0;
        foreach (City city in cities)
        {
            city.SetFood(0);
        }
    }
    public List<City> GetCity()
    {
        return cities;
    }
}
