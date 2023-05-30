using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilList
{

	public static T SelectRandom<T>(this List<T> list)
	{
		if (list == null) return default;
		if (list.Count == 0) return default;

		return list[Random.Range(0, list.Count)];
	}
	
}
