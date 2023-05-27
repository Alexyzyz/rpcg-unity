using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : ICard
{

    private string _title = "Scratch";
    private string _description = "Deal 2 damage.";
    private int _cost = 1;

    public string Title
    {
        get { return _title; }
    }

    public string Description
    {
        get { return _description; }
    }

    public int Cost
    {
        get { return _cost; }
    }

    public void OnPlayed()
    {

    }

}
