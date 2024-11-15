using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class PlayerCombat : MonoBehaviour
{
    PlayerSO playerBase;

    List<AtacSO> atacs;
    public event Action<AtacSO> onAttack;
    Animator animator;


    //hacer una funcion que se suscriba que sea rebre mal;


    private void Awake()
    {
        this.atacs = playerBase.listaAtaques;
        this.animator = GetComponent<Animator>();
    }

    public void atacar1()
    {
        onAttack.Invoke(atacs.ElementAt(0));
        animator.Play("atac2");
    }

    public void atacar2()
    {
        onAttack.Invoke(atacs.ElementAt(1));
        animator.Play("atac2");

    }

    public void atacar3()
    {
        onAttack.Invoke(atacs.ElementAt(2));
        animator.Play("atac2");
    }

    public void atacar4()
    {
        onAttack.Invoke(atacs.ElementAt(3));
        animator.Play("atac2");
    }

    private void Update()
    {
        
    }
}
