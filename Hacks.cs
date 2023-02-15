using UnityEngine;
using System.Collections.Generic;
using ValheimHack;
using System.Security.Policy;

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

        // couleurs on/off bouttons -> fix memory leak
        public bool buttonColorStamina = false; // true = green | false = red
        public bool buttonColorStaminaOthers = false;
        public bool buttonColorFly = false;
        public bool buttonColorGod = false;
        public bool buttonColorGhost = false;


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
            make_button_style(customButtonStyleStamina, new Color(0.8f, 0f, 0f, 0.5f)); // set stamina button red
            make_button_style(customButtonStyleStaminaOthers, new Color(0.8f, 0f, 0f, 0.5f)); // set stamina button red
            make_button_style(customButtonStyleFly, new Color(0.8f, 0f, 0f, 0.5f)); // set stamina button red
            make_button_style(customButtonStyleGodMode, new Color(0.8f, 0f, 0f, 0.5f)); // set stamina button red
            make_button_style(customButtonStyleGhostMode, new Color(0.8f, 0f, 0f, 0.5f)); // set stamina button red

        }

        public void DrawCommands()
		{
            
            float buttonWidth = 120f;
			float buttonHeight = 30f;
			float buttonX = 15f; // 5 -> décalage de la boite + 10 décalage intérieur
            GUI.Box(new Rect(5, 400, 140, 386), "Les touches");
			if (GUI.Button(new Rect(buttonX, 430, buttonWidth, buttonHeight), flyKey.ToString() +" : Fly", customButtonStyleFly))
			{
                toggleFly();
            }

            if (GUI.Button(new Rect(buttonX, 490, buttonWidth, buttonHeight), "F3 : God", customButtonStyleGodMode))
            {
                toggleGodMode();
            }
            if (GUI.Button(new Rect(buttonX, 550, buttonWidth, buttonHeight), "F4 : Ghost", customButtonStyleGhostMode))
            {
                toggleGhostMode();
            }
            //TODO ajouter no placement cost sur F6 et décaler les autres

            if (GUI.Button(new Rect(buttonX, 610, buttonWidth, buttonHeight), "F6 : Stamina", customButtonStyleStamina))
            {
                toggleInfiniteStamina();
            }

            if (GUI.Button(new Rect(buttonX, 670, buttonWidth, buttonHeight), "F7 : StaminaAll", customButtonStyleStaminaOthers))
            {
                toggleInfiniteStaminaOthers();
            }
        }

        public void DrawESP(Character character)
		{
            List<string> mobsToDraw = new List<string>();
            //mobsToDraw.Add("$enemy_greydwarf");
            //if (!mobsToDraw.Contains(character.m_name))
            //{
            //    return;
            //}
            Player localplayer = Player.m_localPlayer;
            Vector3 localplayerPosition = localplayer.transform.position;

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
            int distanceToMob = (int)Vector3.Distance(pivotPos, localplayerPosition);

            Vector3 w2s_footpos = Camera.main.WorldToScreenPoint(playerFootPos);
			Vector3 w2s_headpos = Camera.main.WorldToScreenPoint(playerHeadPos);
			if (w2s_footpos.z > 5f && lastPosition != pivotPos)
			{


                string nameMob = character.name.Replace("(Clone)", "");
                    
				DrawBoxESP(w2s_footpos, w2s_headpos, Color.red, distanceToMob.ToString(), nameMob, true); //TODO passer en argument le nom du mob et définir sa width et height dans DrawBoxESP
				lastPosition = pivotPos;

            }

        }

		public void DrawBoxESP(Vector3 footpos, Vector3 headpos, Color color, string distanceToMob, string nameMob,bool lines)
		{
			float height = footpos.y - headpos.y;
			float widthOffset = 2f;
			float width = height / widthOffset;
            Render.DrawBox(footpos.x - (width / 2), (float)Screen.height - headpos.y - height, width, height , color, 2f);
            Render.DrawString(new Vector2(headpos.x, (float)Screen.height - headpos.y + 10f), distanceToMob.ToString() + "m", true); // Distance to mob
            Render.DrawString(new Vector2(headpos.x, (float)Screen.height - headpos.y -20f),  nameMob, true); // Name of mob
            if (lines)
			{
				Vector2 screenCenter = new Vector2((float)Screen.width / 2, (float)Screen.height / 2);
				Vector2 playerPosition = new Vector2(footpos.x, (float)Screen.height - headpos.y);


                //Debug lines
                Vector2 playerPositionhead = new Vector2(footpos.x, (float)Screen.height - headpos.y);
                Vector2 playerPositionFoot = new Vector2(footpos.x, (float)Screen.height - footpos.y);
				Render.DrawLine(screenCenter, playerPosition, color, 2f);
				//Render.DrawLine(playerPositionhead, playerPositionFoot, Color.green, 2f);
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
                if(!buttonColorStamina) // if button red
                { 
                    make_button_style(customButtonStyleStamina, new Color(0f, 0.8f, 0f, 0.5f)); // set stamina button green
                    buttonColorStamina = true;
                } 
			} else
            {
                if (buttonColorStamina) // if button green
                { 
                    make_button_style(customButtonStyleStamina, new Color(0.8f, 0f, 0f, 0.5f)); // set stamina button red
                    buttonColorStamina = false;
                } 
            }
			if (infiniteStaminaOthers)
			{
				playerHacks.infiniteStaminaOthers();
                if (!buttonColorStaminaOthers) // if button red
                {
                    make_button_style(customButtonStyleStaminaOthers, new Color(0f, 0.8f, 0f, 0.5f)); // set staminaOthers button green
                    buttonColorStaminaOthers = true;
                }
            } else
            {
                if (buttonColorStaminaOthers) // if button green
                {
                    make_button_style(customButtonStyleStaminaOthers, new Color(0.8f, 0f, 0f, 0.5f));// set staminaOthers button red
                    buttonColorStaminaOthers = false;
                }
            }


            if (isButtonFlyPressed)
            {
                if (!buttonColorFly) // if button red
                {
                    make_button_style(customButtonStyleFly, new Color(0f, 0.8f, 0f, 0.5f)); // set fly button green
                    buttonColorFly = true;
                }
            }
            else
            {
                if (buttonColorFly) // if button green
                {
                    make_button_style(customButtonStyleFly, new Color(0.8f, 0f, 0f, 0.5f)); // set fly button red
                    buttonColorFly = false;

                }
            }

            if (isButtonGodModePressed) 
            {
                if (!buttonColorGod) // if button red
                {
                    make_button_style(customButtonStyleGodMode, new Color(0f, 0.8f, 0f, 0.5f)); // set god button green
                    buttonColorGod = true;

                }
            }
            else                       
            {
                if (buttonColorGod) // if button green
                {
                    make_button_style(customButtonStyleGodMode, new Color(0.8f, 0f, 0f, 0.5f)); // set god button red
                    buttonColorGod = false;
                }
            }

            if (isButtonGhostModePressed) 
            {
                if (!buttonColorGhost) // if button red
                {

                    make_button_style(customButtonStyleGhostMode, new Color(0f, 0.8f, 0f, 0.5f)); // set ghost button green
                    buttonColorGhost = true;
                }
            }
            else 
            {
                if (buttonColorGhost) // if button green
                {
                    make_button_style(customButtonStyleGhostMode, new Color(0.8f, 0f, 0f, 0.5f)); // set ghost button red
                    buttonColorGhost = false;

                }
            }
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
            //Make all buttons colors red
            buttonColorStamina = false; 
            buttonColorStaminaOthers = false;
            buttonColorFly = false;
            buttonColorGod = false;
            buttonColorGhost = false;

    }
    }
}
