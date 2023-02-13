using UnityEngine;
using System.Collections.Generic;
using ValheimHack;

namespace ValheimHackGUI
{
	class Hacks : MonoBehaviour
	{

        //Button Skin Test
        GUIStyle customButtonStyleFly = new GUIStyle();
        GUIStyle customButtonStyleGodMode = new GUIStyle();
        GUIStyle customButtonStyleGhostMode = new GUIStyle();
        GUIStyle customButtonStyleStamina = new GUIStyle();
        GUIStyle customButtonStyleStaminaOthers = new GUIStyle();
        public bool isButtonFlyPressed = false;
		public bool isButtonGodModePressed = false;
		public bool isButtonGhostModePressed = false;
        //stamina utiliser variables en dessous


        //Keycodes variables
        KeyCode flyKey = KeyCode.F1;
        KeyCode godKey = KeyCode.F3;
        KeyCode ghostKey = KeyCode.F4;
        KeyCode staminaKey = KeyCode.F6;
        KeyCode staminaOthersKey = KeyCode.F7;


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

        public void Start()
        {
        }

        public void DrawCommands()
		{
            
            float buttonWidth = 120f;
			float buttonHeight = 30f;
			float buttonX = 15f; // 5 -> décalage de la boite + 10 décalage intérieur
            GUI.Box(new Rect(5, 400, 140, 326), "Les touches");
			if (GUI.Button(new Rect(buttonX, 430, buttonWidth, buttonHeight), flyKey.ToString() +" : Fly", customButtonStyleFly))
			{
                toggleFly();
            }

            if (GUI.Button(new Rect(buttonX, 490, buttonWidth, buttonHeight), "F3: God", customButtonStyleGodMode))
            {
                toggleGodMode();
            }
            if (GUI.Button(new Rect(buttonX, 550, buttonWidth, buttonHeight), "F4: Ghost", customButtonStyleGhostMode))
            {
                toggleGhostMode();
            }

            if (GUI.Button(new Rect(buttonX, 610, buttonWidth, buttonHeight), "F6: Stamina", customButtonStyleStamina))
            {
                toggleInfiniteStamina();
            }

            if (GUI.Button(new Rect(buttonX, 670, buttonWidth, buttonHeight), "F7: StaminaAll", customButtonStyleStaminaOthers))
            {
                toggleInfiniteStaminaOthers();
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


        public void toggleFly()
        {
            if (!playerHacks.debugFly())
            {
                isButtonFlyPressed = false;
                return;
            }
            isButtonFlyPressed = true;

        }

        public void toggleGodMode()
        {
            if (!playerHacks.godMode())
            {
                isButtonGodModePressed = false;
                return;
            }
            isButtonGodModePressed = true;
        }

        public void toggleGhostMode()
        {
            if (!playerHacks.ghostMode())
            {
                isButtonGhostModePressed = false;
                return;
            }
            isButtonGhostModePressed = true;
        }

        public void toggleInfiniteStamina()
        {
            infiniteStamina = !infiniteStamina;
        }

        public void toggleInfiniteStaminaOthers()
        {
            infiniteStaminaOthers = !infiniteStaminaOthers;

        }

        public void Update()
		{
			Key_Handler();
			CheckToggles();
        }

		public void Key_Handler()
		{

			//² (petit 2)


            //F1
            if (Input.GetKeyDown(flyKey))
            {
                toggleFly();
            }

            // Skip F2 -> opens ingame interface

            //F3
            if (Input.GetKeyDown(ghostKey))
            {
                toggleGhostMode();
            }

            //F4
            if (Input.GetKeyDown(godKey))
            {
                toggleGodMode();
            }

            // Skip F5 -> opens ingame console

            //F6
            if (Input.GetKeyDown(staminaKey))
            {
                toggleInfiniteStamina();

            }

            //F7
            if (Input.GetKeyDown(staminaOthersKey))
            {
                toggleInfiniteStaminaOthers();

            }

            //F9
            if (Input.GetKeyDown(KeyCode.F9))
            {
                EspCharacters = !EspCharacters;
            }

			//F10
            if (Input.GetKeyDown(KeyCode.F10))
            {
                EspPlayers = !EspPlayers;
            }


			//Insert -> Draw help ou open menu a voir

			//Delete
            if (Input.GetKeyDown(KeyCode.Delete))
			{
				disableAllCheats();
            }
        }

		public void CheckToggles()
		{
			if (infiniteStamina)
			{
				playerHacks.infiniteStamina();
                make_button_style(customButtonStyleStamina, new Color(0f, 0.8f, 0f, 0.5f));
			} else
            {
                make_button_style(customButtonStyleStamina, new Color(0.8f, 0f, 0f, 0.5f));
            }
			if (infiniteStaminaOthers)
			{
				playerHacks.infiniteStaminaOthers();
                make_button_style(customButtonStyleStaminaOthers, new Color(0f, 0.8f, 0f, 0.5f));
            } else
            {
                make_button_style(customButtonStyleStaminaOthers, new Color(0.8f, 0f, 0f, 0.5f));
            }


            if (isButtonFlyPressed) { make_button_style(customButtonStyleFly, new Color(0f,0.8f,0f,0.5f)); }
            else                    { make_button_style(customButtonStyleFly, new Color(0.8f, 0f, 0f, 0.5f)); }

            if (isButtonGodModePressed) { make_button_style(customButtonStyleGodMode, new Color(0f, 0.8f, 0f, 0.5f)); }
            else                        { make_button_style(customButtonStyleGodMode, new Color(0.8f, 0f, 0f, 0.5f)); }

            if (isButtonGhostModePressed) { make_button_style(customButtonStyleGhostMode, new Color(0f, 0.8f, 0f, 0.5f)); }
            else { make_button_style(customButtonStyleGhostMode, new Color(0.8f, 0f, 0f, 0.5f)); }
        }

        //buttons save
        public void make_button_style2(GUIStyle style, Color color)
        {
            style.normal.background = new Texture2D(1, 1);
            Color[] pixels = style.normal.background.GetPixels();
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }
            style.normal.background.SetPixels(pixels);
            style.normal.background.Apply();
            style.normal.textColor = Color.white;
            style.alignment = TextAnchor.MiddleCenter;

        }

        //Buttons
        public void make_button_style(GUIStyle style, Color color)
        {

            // Create a custom Texture2D with black borders
            Texture2D blackBordersTexture = new Texture2D(200, 50);
            Color[] pixels = new Color[200 * 50];
            for (int x = 0; x < blackBordersTexture.width; x++)
            {
                for (int y = 0; y < blackBordersTexture.height; y++)
                {
                    if (x == 0 || x == blackBordersTexture.width - 1 || y == 0 || y == blackBordersTexture.height - 1)
                    {
                        pixels[x + y * blackBordersTexture.width] = Color.black;
                    }
                    else
                    {
                        pixels[x + y * blackBordersTexture.width] = color;
                    }
                }
            }
            blackBordersTexture.SetPixels(pixels);
            blackBordersTexture.Apply();

            style.normal.background = blackBordersTexture;
            style.normal.textColor = Color.white;
            style.fontSize = 16;
            style.alignment = TextAnchor.MiddleCenter;

        }


        public void disableAllCheats()
		{
			infiniteStamina = false;
			infiniteStaminaOthers = false;
			EspCharacters = false;
			EspPlayers = false;
            playerHacks.disableDebugFly();
            isButtonFlyPressed = false;
            playerHacks.disableGodMode();
            isButtonGodModePressed = false;
            playerHacks.disableGhostMode();
            isButtonGhostModePressed = false;

        }
    }
}
