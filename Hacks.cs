using UnityEngine;
using System.Collections.Generic;
using PlayFab.DataModels;
using UnityEngine.UIElements;

namespace ValheimHackGUI
{
	class Hacks : MonoBehaviour
	{

		public bool isEspCharactersEnabled = false;
			
		public void OnGUI()
		{
			if (isEspCharactersEnabled)
			{
				List<Character> allCharacters = Character.GetAllCharacters();
				foreach (Character character in allCharacters)
				{
					if (!character.IsPlayer())
					{
                        DrawESP(character);
					}
				}

			}
		}

        public void DrawESP(Character character)
		{
			Vector3 lastPosition = Vector3.zero;
			Vector3 pivotPos = character.transform.position;
            Vector3 playerFootPos;
			playerFootPos.x = pivotPos.x;
			playerFootPos.y = pivotPos.y - 2f;
			playerFootPos.z = pivotPos.z;
            Vector3 playerHeadPos;
			playerHeadPos.x = pivotPos.x;
			playerHeadPos.y = pivotPos.y + 4f;
			playerHeadPos.z = pivotPos.z;


            Vector3 w2s_footpos = Camera.main.WorldToScreenPoint(playerFootPos);
			Vector3 w2s_headpos = Camera.main.WorldToScreenPoint(playerHeadPos);

			if (w2s_footpos.z > 5f && lastPosition != pivotPos)
			{
				DrawBoxESP(w2s_footpos, w2s_headpos, Color.red, true);
				lastPosition = pivotPos;

            }

        }

		public void DrawBoxESP(Vector3 footpos, Vector3 headpos, Color color, bool lines)
		{
			float height = footpos.y - headpos.y;
			float widthOffset = 2f;
			float width = height / widthOffset;

			Render.DrawBox(footpos.x - (width / 2), (float)Screen.height - footpos.y - height, width, height, color, 2f);

			if (lines)
			{
				Vector2 screenCenter = new Vector2((float)Screen.width / 2, (float)Screen.height / 2);
				Vector2 playerPosition = new Vector2(footpos.x, (float)Screen.height - footpos.y);
				Render.DrawLine(screenCenter, playerPosition, color, 2f);
            }

        }

        public void Start()
		{

		}

		public void Update()
		{
			if (Input.GetKeyUp(KeyCode.F1)) 
			{
				isEspCharactersEnabled = !isEspCharactersEnabled;
			}
		}
	}
}
