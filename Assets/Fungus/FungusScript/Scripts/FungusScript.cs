﻿using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Fungus.Script
{

	public class FungusScript : MonoBehaviour 
	{
		[System.NonSerialized]
		public Sequence executingSequence;

		[System.NonSerialized]
		public FungusCommand copyCommand;

		[HideInInspector]
		public int selectedAddCommandIndex;

		[HideInInspector]
		public int selectedCommandCategoryIndex;

		[HideInInspector]
		public Vector2 scriptScrollPos;

		[HideInInspector]
		public Vector2 commandScrollPos;

		[HideInInspector]
		public float commandViewWidth = 300;

		public float stepTime;
		
		public Sequence startSequence;

		public Sequence selectedSequence;

		public FungusCommand selectedCommand;

		public bool startAutomatically = false;

		public List<FungusVariable> variables = new List<FungusVariable>();

		void Start()
		{
			if (startAutomatically)
			{
				Execute();
			}
		}

		public Sequence CreateSequence(Vector2 position)
		{
			GameObject go = new GameObject("Sequence");
			go.transform.parent = transform;
			go.transform.hideFlags = HideFlags.HideInHierarchy;
			Sequence s = go.AddComponent<Sequence>();
			s.nodeRect.x = position.x;
			s.nodeRect.y = position.y;
			return s;
		}

		public void Execute()
		{
			if (startSequence == null)
			{
				return;
			}

			ExecuteSequence(startSequence);
		}

		public void ExecuteSequence(Sequence sequence)
		{
			if (sequence == null)
			{
				return;
			}

			executingSequence = sequence;
			selectedSequence = sequence;
			sequence.ExecuteNextCommand();
		}

		public string GetUniqueVariableKey(string originalKey, FungusVariable ignoreVariable = null)
		{
			int suffix = 0;
			string baseKey = originalKey;

			// Only letters and digits allowed
			char[] arr = baseKey.Where(c => (char.IsLetterOrDigit(c) || c == '_')).ToArray(); 
			baseKey = new string(arr);

			// No leading digits allowed
			baseKey = baseKey.TrimStart('0','1','2','3','4','5','6','7','8','9');

			// No empty keys allowed
			if (baseKey.Length == 0)
			{
				baseKey = "Var";
			}

			string key = baseKey;
			while (true)
			{
				bool collision = false;
				foreach(FungusVariable variable in GetComponents<FungusVariable>())
				{
					if (variable == ignoreVariable ||
					    variable.key == null)
					{
						continue;
					}

					if (variable.key.Equals(key, StringComparison.CurrentCultureIgnoreCase))
					{
						collision = true;
						suffix++;
						key = baseKey + suffix;
					}
				}
				
				if (!collision)
				{
					return key;
				}
			}
		}
	}

}