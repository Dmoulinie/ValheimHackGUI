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


        //Lines
        public bool linesMobs = false;
        public bool linesPlayers = false;


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


        // ESP General
        ESP esp = new ESP();

        // ESP characters 
        public bool EspCharacters = false;
        public bool charactersName = false;
        public bool charactersLines = false;
        public bool charactersDistance = false;
        public bool charactersHealth = false;



        //ESP players
		public bool EspPlayers = false;

        // Main Menu
        public bool cheatMenu = false;

        // Main Menu Tabs
        public bool wallhackTab = true;
        public bool playerhacksTab = false;
        public bool teleportTab = false;
        public bool miscTab = false;

        public void OnGUI()
		{
            GUI.Label(new Rect(0, 0, 110, 20), "Pano Menu"); // Watermark

            if (cheatMenu)
            {
                mainMenu();
            }

            DrawCommands();
			if (EspCharacters)
			{
				List<Character> allCharacters = Character.GetAllCharacters();
				foreach (Character character in allCharacters)
				{
                    if (character.IsPlayer() && !EspPlayers) // Check si ESP joueur activé
                    {
                        continue;
                    }
                    if (!character.IsPlayer() && EspCharacters)
                    {
                        esp.DrawESPCharacters(character, Color.red, charactersDistance, charactersName, charactersLines, charactersHealth); //carrée rouge
                    }

				}
			}


            if (EspPlayers)
            {
                List<Player> allPlayers = Player.GetAllPlayers();
                foreach (Player player in allPlayers)
                {
                    if (player == Player.m_localPlayer)
                    {
                        continue;
                    }
                    if (player.IsPVPEnabled())
                    {
                        esp.DrawESPPlayer(player, new Color(1,0.65f,0)); //carrée orange
                    } else
                    {
                        esp.DrawESPPlayer(player, Color.green); //carrée vert
                    }

                }
            }
		}

        public void mainMenu()
        {
            GUI.BeginGroup(new Rect(300, 400, 400, 300)); // x = largeur écran /2 - moitié fenetre | y = hauteur écran /2 - moitié fenetre = placement au centre
            // Draw a box in the new coordinate space defined by the BeginGroup.
            // Notice how (0,0) has now been moved on-screen
            GUI.Box(new Rect(0, 0, 400, 300), "Pano Cheat");



            //---------------------TABS---------------------//
            if (GUI.Button(new Rect(000, 20, 100, 30), "Wallhack"))
            {
                wallhackTab = true;
                disableAllTabsExcept("wallhackTab");
            }
            if(GUI.Button(new Rect(100, 20, 100, 30), "Player Hacks"))
            {
                playerhacksTab = true;
                disableAllTabsExcept("playerhacksTab");

            }
            if (GUI.Button(new Rect(200, 20, 100, 30), "Teleport"))
            {
                teleportTab = true;
                disableAllTabsExcept("teleportTab");
            }
            if(GUI.Button(new Rect(300, 20, 100, 30), "Miscellaneous"))
            {
                miscTab = true;
                disableAllTabsExcept("miscTab");

            }


            if (wallhackTab)
            {
                //Monstres  ---- add 30 to "y" rect for each option
                GUI.Box(new Rect(000, 50, 200, 250), "Monstres");
                float firstOption = 70f;
                EspCharacters = GUI.Toggle(new Rect(5, firstOption, 200, 15), EspCharacters, "Activate"); 
                charactersName = GUI.Toggle(new Rect(5, firstOption + 30, 200, 15),charactersName,"Name");
                charactersDistance = GUI.Toggle(new Rect(5, firstOption + 60, 200, 15), charactersDistance, "Distance");
                charactersLines = GUI.Toggle(new Rect(5, firstOption + 90, 200, 15), charactersLines, "Lines");
                charactersHealth = GUI.Toggle(new Rect(5, firstOption + 120, 200, 15), charactersHealth, "Health (WIP)");


                //Joueurs
                GUI.Box(new Rect(200, 50, 200, 250), "Joueurs");
            }

            // We need to match all BeginGroup calls with an EndGroup
            GUI.EndGroup();
        }

        public void disableAllTabsExcept(string tab)
        {
            switch (tab)
            {
                case "wallhackTab":
                    playerhacksTab = false;
                    teleportTab = false;
                    miscTab = false;
                    break;
                case "playerhacksTab":
                    wallhackTab = false;
                    teleportTab = false;
                    miscTab = false;
                    break;
                case "teleportTab":
                    wallhackTab = false;
                    playerhacksTab = false;
                    miscTab = false;
                    break;
                case "miscTab":
                    wallhackTab = false;
                    playerhacksTab = false;
                    teleportTab = false;
                    break;
  
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
			if (GUI.Button(new Rect(buttonX, 430, buttonWidth, buttonHeight), flyKey.ToString() + " : Fly", customButtonStyleFly))
			{
                toggleFly();
            }

            if (GUI.Button(new Rect(buttonX, 490, buttonWidth, buttonHeight), godKey.ToString() + " : God", customButtonStyleGodMode))
            {
                toggleGodMode();
            }
            if (GUI.Button(new Rect(buttonX, 550, buttonWidth, buttonHeight), ghostKey.ToString() + " : Ghost", customButtonStyleGhostMode))
            {
                toggleGhostMode();
            }
            //TODO ajouter no placement cost sur F6 et décaler les autres

            if (GUI.Button(new Rect(buttonX, 610, buttonWidth, buttonHeight), staminaKey.ToString() + " : Stamina", customButtonStyleStamina))
            {
                toggleInfiniteStamina();
            }

            if (GUI.Button(new Rect(buttonX, 670, buttonWidth, buttonHeight), staminaOthersKey.ToString() + " : StaminaAll", customButtonStyleStaminaOthers))
            {
                toggleInfiniteStaminaOthers();
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

        public void toggleMenu()
        {
            cheatMenu = !cheatMenu;
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
            if (Input.GetKeyDown(KeyCode.Insert))
            {
                toggleMenu();
            }

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
            // color buttons reset TODO
            make_button_style(customButtonStyleStamina, new Color(0.8f, 0f, 0f, 0.5f)); // set stamina button red
            make_button_style(customButtonStyleStaminaOthers, new Color(0.8f, 0f, 0f, 0.5f));// set staminaOthers button red
            make_button_style(customButtonStyleFly, new Color(0.8f, 0f, 0f, 0.5f)); // set fly button red
            make_button_style(customButtonStyleGodMode, new Color(0.8f, 0f, 0f, 0.5f)); // set god button red
            make_button_style(customButtonStyleGhostMode, new Color(0.8f, 0f, 0f, 0.5f)); // set ghost button red



        }
    }
}
