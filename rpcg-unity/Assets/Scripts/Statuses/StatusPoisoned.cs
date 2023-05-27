using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusPoisoned : IStatus
{

    private string _title;
    private string _description;
    private int _stackCount;

    public string Title
    {
        get { return _title; }
    }

    public string Description
    {
        get { return _description; }
    }

    public int StackCount
    {
        get { return _stackCount; }
    }

}
