using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField]private Slider hpBar;

    private float maxHp = 100;
    private float curHp = 100;

    private void Awake()
    {
    }
    void Start()
    {

        hpBar.value = curHp / maxHp;
    }

    void Update()
    {
        updateHp();
    }

    private void updateHp()
    {
        hpBar.value = curHp / maxHp;
    }
}
