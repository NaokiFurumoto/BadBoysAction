﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDontDestroy : MonoBehaviour
{
	public bool DontDestroyEnabled = true;

	void Start()
	{
		if (DontDestroyEnabled)
		{
			DontDestroyOnLoad(this);
		}
	}
}
