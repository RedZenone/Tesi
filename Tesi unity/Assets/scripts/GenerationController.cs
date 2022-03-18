using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationController : MonoBehaviour
{
    public float time;
    private List<Civ> civilizations;
    private List<Civ> tagged_for_destruction;
    public GameObject timer;
    private bool stop;
    void Start()
    {
        time=0f;
        civilizations= new List<Civ>();
        tagged_for_destruction= new List<Civ>();
        stop=true;
        timer.gameObject.GetComponent<Animator>().enabled = false;
    }
    void Update()
    {
        if (!stop)
        {
            time+= Time.deltaTime;
        }
        if (time>=6f)
        {
            NewGeneration();
            time=0;
        }
    }

    private void NewGeneration()
    {
        foreach (Civ civ in civilizations)
        {
            civ.ProduceFood();
            civ.KillStarved();
            civ.NewGen();
            civ.KillOld();
            civ.CleanupFood();
            if (civ.GetCity().Count==0)
            {
                tagged_for_destruction.Add(civ);
            }
        }
        if ( tagged_for_destruction.Count>0)
        {
            int tagged=tagged_for_destruction.Count;
            for (int i = 0; i < tagged; i++)
            {
                Civ lost = tagged_for_destruction[0];
                tagged_for_destruction.RemoveAt(0);
                civilizations.RemoveAt(0);
                Destroy(lost);
            }
        }
    }




    public void AddCiv(Civ civ)
    {
        civilizations.Add(civ);
    }
    public void RemoveCiv(Civ civ)
    {
        if (civilizations.Contains(civ))
        {
            civilizations.Remove(civ);
            Destroy(civ);
        }
    }
    public void StopTime()
    {
        if (stop)
        {
            timer.gameObject.GetComponent<Animator>().enabled = true;
            stop=false;
        }else
        {
            timer.gameObject.GetComponent<Animator>().enabled = false;
            stop=true;
        }
        
    }
}
