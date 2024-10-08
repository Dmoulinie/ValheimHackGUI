﻿using UnityEngine;
using System.Collections.Generic;
using ValheimHack;
using System.Security.Policy;
using System;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Linq;

namespace ValheimHackGUI
{
	class Hacks : MonoBehaviour
	{

        //Button Skin
        GUIStyle customButtonStyleFly = new GUIStyle();
        GUIStyle customButtonStyleGodMode = new GUIStyle();
        GUIStyle customButtonStyleGhostMode = new GUIStyle();
        GUIStyle customButtonStyleStamina = new GUIStyle();
        GUIStyle customButtonStyleStaminaOthers = new GUIStyle();
        public bool isButtonFlyPressed = false;
		public bool isButtonGodModePressed = false;
		public bool isButtonGhostModePressed = false;
        //stamina utiliser variables en dessous

        public bool noLocalPlayerFound = false;


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
        public bool isGodMode = false;
        public bool isGhostMode = false;

        public bool infiniteStamina = false;
		public bool infiniteStaminaOthers = false;

        // ESP General
        ESP esp = new ESP();

        // ESP characters 
        public bool EspCharacters = false;
        public bool charactersName = true;
        public bool charactersLines = false;
        public bool charactersDistance = false;
        public bool charactersHealth = false;
        float characterDrawRange = 200f;


        //ESP players
        public bool EspPlayers = false;
        public bool playersName = true;
        public bool playersLines = false;
        public bool playersDistance = false;
        public bool playersHealth = true;
        public bool playersPvp = false;
        float playersDrawRange = 200f;



        //Teleport
        Teleport teleport = new Teleport();
        int chosenPlayer1 = 0;
        int chosenPlayer2 = 0;
        string locationName = "";
        string location_position_x = "0";
        string location_position_y = "0";
        string location_position_z = "0";

        //Misc
        public bool helpCommands = true;




        // Main Menu
        public bool cheatMenu = false;
        public float width = 400f;
        public float height = 300f;

        // Main Menu Tabs
        public float mainTabsButtonsCount = 4f;
        public bool wallhackTab = true;
        public bool playerhacksTab = false;
        public bool teleportTab = false;
        public bool miscTab = false;

        // Teleport Sub Tabs
        public float subTabsButtonsCount = 4f;
        public bool teleportToPlayerTab = true;
        public bool teleportToMeTab = false;
        public bool teleportPlayerToPlayerTab = false;
        public bool teleportToLocationTab = false;

        public void OnGUI()
		{
            GUI.Label(new Rect(0, 0, 200, 20), "Pano Menu Unreleased"); // Watermark

            if (cheatMenu)
            {
                mainMenu();
            }

            if (helpCommands)
            {
                DrawCommands();
            }
			if (EspCharacters)
			{
				foreach (Character character in Character.GetAllCharacters())
				{
                    if (character.IsPlayer() && !EspPlayers) // Check si ESP joueur activé
                    {
                        continue;
                    }
                    if (!character.IsPlayer() && EspCharacters)
                    {
                        if (character.IsTamed())
                        {
                            esp.DrawESPCharacters(character, Color.cyan, charactersDistance, charactersName, charactersLines, charactersHealth, characterDrawRange); //carrée bleu boar 2 etoiles
                        }
                        else if (character.GetLevel() == 3) // mob 2 étoiles
                        {
                            esp.DrawESPCharacters(character, Color.blue, charactersDistance, charactersName, charactersLines, charactersHealth, characterDrawRange); //carrée bleu boar 2 etoiles
                        }
                        else
                        {
                            esp.DrawESPCharacters(character, Color.red, charactersDistance, charactersName, charactersLines, charactersHealth, characterDrawRange); //carrée rouge
                        }
                    }

				}
			}


            if (EspPlayers && Player.GetAllPlayers().Count > 1) // Bug when no players found (in menu) (count > 1 (myself) )// In logs
            {
                foreach (Player player in Player.GetAllPlayers())
                {
                    if (player == Player.m_localPlayer || player == null)
                    {
                        continue;
                    }

                    if (player.IsPVPEnabled() && playersPvp)
                    {
                        esp.DrawESPPlayer(player, new Color(1,0.65f,0), playersName, playersDistance, playersLines, playersHealth, playersDrawRange); //carrée orange
                    } else
                    {
                        esp.DrawESPPlayer(player, Color.green, playersName, playersDistance, playersLines, playersHealth, playersDrawRange); //carrée vert
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
                charactersLines = GUI.Toggle(new Rect(5, firstOption + 60, 200, 15), charactersLines, "Lines");
                charactersHealth = GUI.Toggle(new Rect(5, firstOption + 90, 200, 20), charactersHealth, "Health");
                charactersDistance = GUI.Toggle(new Rect(5, firstOption + 120, 200, 15), charactersDistance, "Distance");
                GUI.Label(new Rect(5, firstOption + 145, 200, 25),"Distance : " + (int)characterDrawRange + "m");
                characterDrawRange = GUI.HorizontalSlider(new Rect(5, firstOption + 170, 190, 15), characterDrawRange, 15, 200);

                //Joueurs
                GUI.Box(new Rect(200, 50, 200, 250), "Joueurs");
                firstOption = 70f;
                EspPlayers = GUI.Toggle(new Rect(205, firstOption, 200, 15), EspPlayers, "Activate");
                playersName = GUI.Toggle(new Rect(205, firstOption + 30, 200, 15), playersName, "Name");
                playersLines = GUI.Toggle(new Rect(205, firstOption + 60, 200, 15), playersLines, "Lines");
                playersHealth = GUI.Toggle(new Rect(205, firstOption + 90, 200, 20), playersHealth, "Health");
                playersPvp = GUI.Toggle(new Rect(205, firstOption + 120, 200, 20), playersPvp, "Check Pvp");
                playersDistance = GUI.Toggle(new Rect(205, firstOption + 150 , 200, 15), playersDistance, "Distance");
                GUI.Label(new Rect(205, firstOption + 175, 200, 25), "Distance : " + (int)playersDrawRange + "m");
                playersDrawRange = GUI.HorizontalSlider(new Rect(205, firstOption + 200, 190, 15), playersDrawRange, 15, 200);  
            }

            if (playerhacksTab)
            {   
                float firstOption = 70f;
                GUI.Box(new Rect(000, 50, 200, 250), "Player");
                isDebugFlying = GUI.Toggle(new Rect(5, firstOption, 200, 20), isDebugFlying, "Fly");
                isGodMode = GUI.Toggle(new Rect(5, firstOption + 30, 200, 20), isGodMode, "God Mode");
                isGhostMode = GUI.Toggle(new Rect(5, firstOption + 60, 200, 20), isGhostMode, "Ghost");
                infiniteStamina = GUI.Toggle(new Rect(5, firstOption + 90, 200, 20), infiniteStamina, "Infinite stamina");
                GUI.Box(new Rect(200, 50, 200, 250), "Others");
                firstOption = 70f;
                infiniteStaminaOthers = GUI.Toggle(new Rect(205, firstOption, 200, 20), infiniteStaminaOthers, "Infinite Stamina Others");


            }
            if (teleportTab)
            {
                if (GUI.Button(new Rect(0,55, width / subTabsButtonsCount,30), "TP to player"))
                {
                    teleportToPlayerTab = true;
                    disableAllSubTabsExcept("teleportToPlayerTab");
                }

                if (GUI.Button(new Rect(100, 55, width / subTabsButtonsCount, 30), "TP to me"))
                {
                    teleportToMeTab = true;
                    disableAllSubTabsExcept("teleportToMeTab");
                }

                if (GUI.Button(new Rect(200, 55, width / subTabsButtonsCount, 30), "TP P2P"))
                {
                    teleportPlayerToPlayerTab = true;
                    disableAllSubTabsExcept("teleportPlayerToPlayerTab");
                }

                if (GUI.Button(new Rect(300, 55, width / subTabsButtonsCount, 30), "TP Location"))
                {
                    teleportToLocationTab = true;
                    disableAllSubTabsExcept("teleportToLocationTab");
                }

                // Drawing what's inside the subtabs of the teleport Tab

                if (teleportToPlayerTab) // if ram abuse make a refresh list button
                {
                    GUI.Label(new Rect(85, 85, 400, 20), "Teleport me to selected player in range"); // Title

                    List<Player> allPlayersInRange = Player.GetAllPlayers();
                    Dictionary<int, long> allPlayersDict = new Dictionary<int, long>();
                    List<string> allPlayersName = new List<string>();

                    foreach (Player player in allPlayersInRange)
                    {
                        if (player == Player.m_localPlayer)
                        {
                            continue;
                        }
                        allPlayersDict.Add(allPlayersDict.Count, player.GetPlayerID());
                        allPlayersName.Add(player.GetHoverName());
                    }

                    if (allPlayersName.Count == 0)
                    {
                        GUI.Label(new Rect(180f, 150f, 200, 20), "No players in range");
                    }   else
                    {


                        chosenPlayer1 = GUI.SelectionGrid(new Rect(5f, 105f, 195f, 190f), chosenPlayer1, allPlayersName.ToArray(), 1); // Toarray() = List<string> -> string[] for GUI.SelectionGrid

                        GUI.Label(new Rect(260, 150, 150, 30), "Selected Player : ");
                        GUI.Label(new Rect(270, 170, 150, 30), allPlayersName[chosenPlayer1]);

                        if (GUI.Button(new Rect(250, 250, 100, 30), "Teleport"))
                        {
                            Player localplayer = Player.m_localPlayer;
                            long targetPlayerId = 0;
                            allPlayersDict.TryGetValue(chosenPlayer1, out targetPlayerId);
                            Player targetPlayer = Player.GetPlayer(targetPlayerId);
                            teleport.TeleportToPlayer(targetPlayer);
                        }
                    }

                }

                if (teleportToMeTab)
                {
                    GUI.Label(new Rect(85, 85, 400, 20), "Teleport selected player to me"); // Title

                    List<Player> allPlayersInRange = Player.GetAllPlayers();
                    Dictionary<int, long> allPlayersDict = new Dictionary<int, long>();
                    List<string> allPlayersName = new List<string>();

                    foreach (Player player in allPlayersInRange)
                    {
                        if (player == Player.m_localPlayer)
                        {
                            continue;
                        }
                        allPlayersDict.Add(allPlayersDict.Count, player.GetPlayerID());
                        allPlayersName.Add(player.GetHoverName());
                    }
                    if (allPlayersName.Count == 0)
                    {
                        GUI.Label(new Rect(180f, 150f, 200, 20), "No players in range");
                    }
                    else
                    {

                        chosenPlayer1 = GUI.SelectionGrid(new Rect(5f, 105f, 195f, 190), chosenPlayer1, allPlayersName.ToArray(), 1); // Toarray() = List<string> -> string[] for GUI.SelectionGrid

                        GUI.Label(new Rect(260, 150, 150, 30), "Selected Player : ");
                        GUI.Label(new Rect(270, 170, 150, 30), allPlayersName[chosenPlayer1]);

                        if (GUI.Button(new Rect(250, 250, 100, 30), "Teleport"))
                        {
                            Player localplayer = Player.m_localPlayer;
                            long targetPlayerId = 0;
                            allPlayersDict.TryGetValue(chosenPlayer1, out targetPlayerId);
                            Player targetPlayer = Player.GetPlayer(targetPlayerId);
                            teleport.TeleportToMe(targetPlayer);
                        }
                    }
                }

                if (teleportPlayerToPlayerTab)
                {
                    GUI.Label(new Rect(85, 85, 400, 20), "Teleport selected player to another player");

                    List<Player> allPlayersInRange = Player.GetAllPlayers();
                    Dictionary<int, long> allPlayersDict = new Dictionary<int, long>();
                    List<string> allPlayersName = new List<string>();

                    foreach (Player player in allPlayersInRange)
                    {
                        if (player == Player.m_localPlayer)
                        {
                            continue;
                        }
                        allPlayersDict.Add(allPlayersDict.Count, player.GetPlayerID());
                        allPlayersName.Add(player.GetHoverName());
                    }
                    if (allPlayersName.Count == 0)
                    {
                        GUI.Label(new Rect(180f, 150f, 200, 20), "No players in range");
                    }
                    else
                    {
                        GUI.Label(new Rect(80f, 105f, 195f, 100f), "Player 1");
                        GUI.Label(new Rect(280f, 105f, 195f, 100f), "Player 2");
                        chosenPlayer1 = GUI.SelectionGrid(new Rect(5f, 135f, 195f, 100f), chosenPlayer1, allPlayersName.ToArray(), 1); // Toarray() = List<string> -> string[] for GUI.SelectionGrid
                        chosenPlayer2 = GUI.SelectionGrid(new Rect(200f, 135f, 195f, 100f), chosenPlayer2, allPlayersName.ToArray(), 1); // Toarray() = List<string> -> string[] for GUI.SelectionGrid

                        GUI.Label(new Rect(25, 260, 200, 30), allPlayersName[chosenPlayer1] + " to " + allPlayersName[chosenPlayer2]);

                        if (GUI.Button(new Rect(250, 260, 100, 30), "Teleport"))
                        {
                            Player localplayer = Player.m_localPlayer;
                            long targetPlayerId1 = 0;
                            long targetPlayerId2 = 0;
                            allPlayersDict.TryGetValue(chosenPlayer1, out targetPlayerId1);
                            allPlayersDict.TryGetValue(chosenPlayer2, out targetPlayerId2);
                            Player playertoTeleport = Player.GetPlayer(targetPlayerId1);
                            Player targetPlayer = Player.GetPlayer(targetPlayerId2);
                            teleport.TeleportPlayerToPlayer(playertoTeleport, targetPlayer);
                        }

                    }
                }

                if (teleportToLocationTab)
                {
                    GUI.Label(new Rect(25f, 85f, 175f, 20f), "Nom à enregistrer :");
                    locationName = GUI.TextField(new Rect(5f, 105f, 170f, 20f), locationName, 20);
                    GUI.Label(new Rect(30f,140f,185f,20f), "Position");
                    if (GUI.Button(new Rect(80f,140f,95f, 20f),"Refresh"))
                    {
                        Player localplayer = Player.m_localPlayer;
                        if (localplayer != null)
                        {
                            location_position_x = localplayer.transform.position.x.ToString();
                            location_position_y = localplayer.transform.position.y.ToString();
                            location_position_z = localplayer.transform.position.z.ToString();
                        }
                    }
                    GUI.Label(new Rect(5f, 170f, 25f, 20f), " X : ");
                    location_position_x = GUI.TextField(new Rect(35f, 170f, 140f, 20f), location_position_x, 15);
                    GUI.Label(new Rect(5f, 195f, 25f, 20f), " Y : ");
                    location_position_y = GUI.TextField(new Rect(35f, 195f, 140f, 20f), location_position_y, 15);
                    GUI.Label(new Rect(5f, 220f, 25f, 20f), " Z : ");
                    location_position_z = GUI.TextField(new Rect(35f, 220f, 140f, 20f), location_position_z, 15);
                    if (GUI.Button(new Rect(50, 260, 100, 30), "Save position"))
                    {
                        teleport.SaveLocation(locationName);
                        locationName = "";
                    }

                    //Todo selectionGRID scrollable 
                    if (GUI.Button(new Rect(230, 260, 120, 30), "Teleport to position"))
                    {
                        teleport.TeleportToLocation(teleport.GetAllLocations()[0]);
                    }
                }


            }
            if (miscTab)
            {
                float firstOption = 70f;
                helpCommands = GUI.Toggle(new Rect(5, firstOption, 200, 20), helpCommands, "Help keys");
                
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

        public void disableAllSubTabsExcept(string tab)
        {
            switch (tab)
            {
                case "teleportToPlayerTab":
                    teleportToMeTab = false;
                    teleportPlayerToPlayerTab = false;
                    teleportToLocationTab = false;
                    break;
                case "teleportToMeTab":
                    teleportToPlayerTab = false;
                    teleportPlayerToPlayerTab = false;
                    teleportToLocationTab = false;
                    break;
                case "teleportPlayerToPlayerTab":
                    teleportToMeTab = false;
                    teleportToPlayerTab = false;
                    teleportToLocationTab = false;
                    break;
                case "teleportToLocationTab":
                    teleportToMeTab = false;
                    teleportPlayerToPlayerTab = false;
                    teleportToPlayerTab = false;
                    break;
            }
        }



        public void Start()
        {
            //make_button_style(customButtonStyleStamina, new Color(0.8f, 0f, 0f, 0.5f)); // set stamina button red
            //make_button_style(customButtonStyleStaminaOthers, new Color(0.8f, 0f, 0f, 0.5f)); // set stamina button red
            //make_button_style(customButtonStyleFly, new Color(0.8f, 0f, 0f, 0.5f)); // set stamina button red
            //make_button_style(customButtonStyleGodMode, new Color(0.8f, 0f, 0f, 0.5f)); // set stamina button red
            //make_button_style(customButtonStyleGhostMode, new Color(0.8f, 0f, 0f, 0.5f)); // set stamina button red

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
            isDebugFlying = !isDebugFlying;
        }

        public void toggleGodMode()
        {
            isGodMode = !isGodMode;
        }

        public void toggleGhostMode()
        {
            isGhostMode = !isGhostMode;
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
			Toggles();
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



        public void Toggles()
		{
            Player localplayer = Player.m_localPlayer;
            if (localplayer == null && !noLocalPlayerFound)
            {
                noLocalPlayerFound = true;
                make_button_style(customButtonStyleFly, new Color(0.8f, 0f, 0f, 0.5f)); // set fly button red
                make_button_style(customButtonStyleGodMode, new Color(0.8f, 0f, 0f, 0.5f)); // set god button red
                make_button_style(customButtonStyleGhostMode, new Color(0.8f, 0f, 0f, 0.5f)); // set ghost button red
                make_button_style(customButtonStyleStamina, new Color(0.8f, 0f, 0f, 0.5f)); // set stamina button red
                make_button_style(customButtonStyleStaminaOthers, new Color(0.8f, 0f, 0f, 0.5f));// set staminaOthers button red
                return;
            }
            noLocalPlayerFound = false;
            if (isButtonFlyPressed || isDebugFlying)
            {
                playerHacks.setDebugFly(true);
                if (!buttonColorFly) // if button red
                {
                    make_button_style(customButtonStyleFly, new Color(0f, 0.8f, 0f, 0.5f)); // set fly button green
                    buttonColorFly = true;
                }
            }
            else
            {
                playerHacks.setDebugFly(false);
                if (buttonColorFly) // if button green
                {
                    make_button_style(customButtonStyleFly, new Color(0.8f, 0f, 0f, 0.5f)); // set fly button red
                    buttonColorFly = false;

                }
            }


            if (isButtonGodModePressed || isGodMode)
            {
                playerHacks.setGodMode(true);
                if (!buttonColorGod) // if button red
                {
                    make_button_style(customButtonStyleGodMode, new Color(0f, 0.8f, 0f, 0.5f)); // set god button green
                    buttonColorGod = true;

                }
            }
            else
            {
                playerHacks.setGodMode(false);
                if (buttonColorGod) // if button green
                {
                    make_button_style(customButtonStyleGodMode, new Color(0.8f, 0f, 0f, 0.5f)); // set god button red
                    buttonColorGod = false;
                }
            }

            if (isButtonGhostModePressed || isGhostMode)
            {
                playerHacks.setGhostMode(true);
                if (!buttonColorGhost) // if button red
                {

                    make_button_style(customButtonStyleGhostMode, new Color(0f, 0.8f, 0f, 0.5f)); // set ghost button green
                    buttonColorGhost = true;
                }
            }
            else
            {
                playerHacks.setGhostMode(false);
                if (buttonColorGhost) // if button green
                {
                    make_button_style(customButtonStyleGhostMode, new Color(0.8f, 0f, 0f, 0.5f)); // set ghost button red
                    buttonColorGhost = false;
                }
            }

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
            isDebugFlying = false;
            isButtonFlyPressed = false;
            isGodMode = false;
            isGhostMode = false;
            isButtonGodModePressed = false;
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
