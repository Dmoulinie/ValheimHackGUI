using UnityEngine;
using System.Collections.Generic;

namespace ValheimHackGUI
{
	class Hacks : MonoBehaviour
	{
		public void OnGUI()
		{
			GUI.Label(new Rect(10f, 10f, 100f, 100f), "Woulah je veux du texte");
		}

		public void Start()
		{

		}

		public void Update()
		{
			Player player= GetComponent<Player>();
		}
	}
}
