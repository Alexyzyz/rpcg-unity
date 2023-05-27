using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatus
{

    public string Title { get; }
    public string Description { get; }
    public int StackCount { get; }

}
