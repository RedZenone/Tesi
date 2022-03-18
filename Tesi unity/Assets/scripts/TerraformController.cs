using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerraformController : MonoBehaviour
{
    public enum terraformtype
    {
        grassland,
        mountain,
        desert,
        civ
    }
    private terraformtype current_terraform;
    public int terraform_radius;
    public Text radius_text;
    public Dropdown terraform_dropdown;
    public Slider terraform_slider;
    public GenerationController gen;
    void Start()
    {
        current_terraform=terraformtype.grassland;
        terraform_radius=5;
    }
    public void UpdateTerType ()
    {
        switch (terraform_dropdown.value)
        {
            case 0: current_terraform=terraformtype.grassland; break;
            case 1: current_terraform=terraformtype.mountain; break;
            case 2: current_terraform=terraformtype.desert; break;
            case 3: current_terraform=terraformtype.civ; break;
        }
        
    }
    public void UpdateTerRad () // update terraformer radius
    {
        terraform_radius=(int) terraform_slider.value;
        radius_text.text=""+terraform_radius;
    }

    public terraformtype GetTerType()
    {
        return current_terraform;
    }
}
