using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieUI : MonoBehaviour
{
    [SerializeField] Zombie zombie;
    [SerializeField] Slider health;
    void Start()
    {
        zombie.onHealthChange += UpdateSlider;
        health.maxValue = zombie.health;
    }
    private void UpdateSlider()
    {
        health.value = zombie.health;
    }
    
    void Update()
    {
        transform.rotation = Quaternion.identity;
    }
}
