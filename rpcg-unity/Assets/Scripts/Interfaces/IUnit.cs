using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{

    public int HP { get; set; }
    public int HPmax { get; set; }
    public List<IStatus> StatusList { get; set; }

}
