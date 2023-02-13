using UnityEngine;
using System.Collections.Generic;
using PlayFab.DataModels;
using UnityEngine.UIElements;
using System.Reflection;
using ValheimHack;
using Steamworks;

namespace ValheimHackGUI
{
	class Hacks : MonoBehaviour
	{

		// Player Hacks
		PlayerHacks playerHacks = new PlayerHacks();
        public bool isDebugFlying = false;
		public bool infiniteStamina = false;
		public bool infiniteStaminaOthers = false;
        

        // ESP
        public bool EspCharacters = false;
		public bool EspPlayers = false;

        public void OnGUI()
		{
			
			DrawCommands();
			if (EspCharacters)
			{
				List<Character> allCharacters = Character.GetAllCharacters();
				foreach (Character character in allCharacters)
				{
					if (character.IsPlayer())
					{
						continue;
					}
					DrawESP(character);
				}

			}
		}

        public void DrawCommands()
		{
			//Rect rectangle
			//GUI.Label()
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
				DrawBoxESP(w2s_footpos, w2s_headpos, Color.red, true); //TODO passer en argument le nom du mob et définir sa width et height dans DrawBoxESP
				lastPosition = pivotPos;

            }

        }

		public void DrawBoxESP(Vector3 footpos, Vector3 headpos, Color color, bool lines)
		{
			float height = footpos.y - headpos.y;
			float widthOffset = 2f;
			float width = height / widthOffset;

			Render.DrawBox(footpos.x - (width / 2), (float)Screen.height - headpos.y - height, width, height, color, 2f);

			if (lines)
			{
				Vector2 screenCenter = new Vector2((float)Screen.width / 2, (float)Screen.height / 2);
				Vector2 playerPosition = new Vector2(footpos.x, (float)Screen.height - headpos.y);
				Render.DrawLine(screenCenter, playerPosition, color, 2f);
            }

        }

        public void Start()
		{

		}

		public void Update()
		{
			Debug.Log(Input.mousePosition);
			Key_Handler();

			CheckToggles();
        }

		public void Key_Handler()
		{
			if (Input.GetKeyDown(KeyCode.Quote))
			{
                playerHacks.debugFly();
			}

            if (Input.GetKeyDown(KeyCode.F1))
            {
                EspPlayers = !EspPlayers;
            }

			// Skip F2 -> opens ingame interface

            if (Input.GetKeyDown(KeyCode.F3))
            {
                EspCharacters = !EspCharacters;
            }

			if (Input.GetKeyDown(KeyCode.F4))
			{
				infiniteStamina = !infiniteStamina;
			}

            // Skip F5 -> opens ingame console

            if (Input.GetKeyDown(KeyCode.F6))
			{
				infiniteStaminaOthers = !infiniteStaminaOthers;
			}
        }

		public void CheckToggles()
		{
			if (infiniteStamina)
			{
				playerHacks.infiniteStamina();
			}
			if (infiniteStaminaOthers)
			{
				playerHacks.infiniteStaminaOthers();
			}
		}


		public void disableAllCheats()
		{
			infiniteStamina = false;
			infiniteStaminaOthers = false;
			EspCharacters = false;
			EspPlayers = false;
            playerHacks.disableDebugFly();
            playerHacks.disableGodMode();

        }
    }
}
