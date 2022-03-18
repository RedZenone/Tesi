using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popolation : MonoBehaviour
{
    public enum Gene
    {
        farmer,
        nomad,
        soldier
    }
    private Gene behave;
    private float food_production;  //based on the land fertility produces food for the city
    private float fight;
    private float land_explot;      //increse food production at the expens of the land fertility for the nex generation

    public Popolation(float food_production, float fight)
    {
        this.food_production=food_production;
        this.fight=fight;
    }

    public float Produce(float fertility)
    {
        
        float food = food_production*food_production*fertility;
        //Debug.Log("      POP FOOD PRODUCTION: " + food);
        return food;
    }
    public float Fight()
    {
        return fight*fight;
    }
    public float GetFoodProd()
    {
        return food_production;
    }
    public float GetFight()
    {
        return fight;
    }
}
