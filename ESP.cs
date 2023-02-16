using UnityEngine;

namespace ValheimHackGUI
{
    internal class ESP
    {

        //-------------------------------------------------------Characters-------------------------------------------------------//
        public void DrawESPCharacters(Character character, Color colorBox, bool charactersDistance, bool charactersName, bool charactersLines, bool charactersHealth, float characterDrawRange)
        {

            //List<string> mobsToDraw = new List<string>();
            //mobsToDraw.Add("$enemy_greydwarf");
            //if (!mobsToDraw.Contains(character.m_name))
            //{
            //    return;
            //}
            Player localplayer = Player.m_localPlayer;
            string mobName = character.GetHoverName();
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
            if (w2s_footpos.z > 5f && lastPosition != pivotPos && distanceToMob < characterDrawRange)
            {

                DrawBoxESPCharacters(w2s_footpos, w2s_headpos, colorBox, distanceToMob.ToString(), character.GetHoverName(), charactersDistance, charactersName, charactersLines, charactersHealth, character.GetHealth(), character.GetMaxHealth()); //TODO passer en argument le nom du mob et définir sa width et height dans DrawBoxESP
                lastPosition = pivotPos;
            }
        }



        public void DrawBoxESPCharacters(Vector2 footpos, Vector2 headpos, Color color, string distanceToMob, string nameCharacter, bool charactersDistance,bool charactersName, bool charactersLines, bool charactersHealth, float health = 0, float maxHealth = 0)
        {
            float height = footpos.y - headpos.y;
            float widthOffset = 3f;
            float width = height / widthOffset;
            Render.DrawBox(footpos.x - (width / 2), (float)Screen.height - headpos.y - height, width, height, color, 2f);
            if (charactersDistance)
            {
                float offsetDistanceDrawing = 20f;
                Render.DrawString(new Vector2(headpos.x - (width/2) + offsetDistanceDrawing, (float)Screen.height - headpos.y + 10f), distanceToMob + "m", true); // Distance to mob
            }
            if (charactersName)
            {
                Render.DrawString(new Vector2(headpos.x, (float)Screen.height - headpos.y - 20f), nameCharacter, true); // Name of mob
            }
            if (charactersLines)
            {
                Vector2 screenCenter = new Vector2((float)Screen.width / 2, (float)Screen.height / 2);
                Vector2 characterPosition = new Vector2(footpos.x, (float)Screen.height - headpos.y);
                Render.DrawLine(screenCenter, characterPosition, color, 2f);
                //Render.DrawLine(playerPositionhead, playerPositionFoot, Color.green, 2f);
            }
            if (charactersHealth && health != 0 && maxHealth != 0)
            {
                float offsetHealthDrawing = 20f;
                Render.DrawString(new Vector2(headpos.x + (width/2) - offsetHealthDrawing, (float)Screen.height - headpos.y + 10f), health + "/" + maxHealth);
            }

        }



















        //-------------------------------------------------------Players-------------------------------------------------------//


        public void DrawESPPlayer(Player player, Color colorBox, bool playersName, bool playersDistance, bool playersLines, bool playersHealth, float playersDrawRange)
        {
            Player localplayer = Player.m_localPlayer;

            Vector3 localplayerPosition = localplayer.transform.position;

            Vector3 lastPosition = Vector3.zero;
            Vector3 pivotPos = player.transform.position;
            Vector3 playerFootPos;
            playerFootPos.x = pivotPos.x;
            playerFootPos.y = pivotPos.y - 2f;
            playerFootPos.z = pivotPos.z;
            Vector3 playerHeadPos;
            playerHeadPos.x = pivotPos.x;
            playerHeadPos.y = pivotPos.y + 4f;
            playerHeadPos.z = pivotPos.z;
            int distanceToPlayer = (int)Vector3.Distance(pivotPos, localplayerPosition);

            Vector3 w2s_footpos = Camera.main.WorldToScreenPoint(playerFootPos);
            Vector3 w2s_headpos = Camera.main.WorldToScreenPoint(playerHeadPos);
            if (w2s_footpos.z > 5f && lastPosition != pivotPos && distanceToPlayer < playersDrawRange)
            {
                Character playerAsCharacter = (Character)player;
                float health = playerAsCharacter.GetHealth();
                float maxHealth = playerAsCharacter.GetMaxHealth();
                DrawBoxESPPlayers(w2s_footpos, w2s_headpos, colorBox, distanceToPlayer.ToString(), player.GetPlayerName(),playersName,playersDistance,playersLines,playersHealth, health, maxHealth); //TODO passer en argument le nom du mob et définir sa width et height dans DrawBoxESP
                lastPosition = pivotPos;

            }
        }



        public void DrawBoxESPPlayers(Vector3 footpos, Vector3 headpos, Color color, string distanceToPlayer, string namePlayer, bool playersName, bool playersDistance, bool playersLines, bool playersHealth, float health = 0, float maxHealth = 0)
        {
            float height = footpos.y - headpos.y;
            float widthOffset = 2f;
            float width = height / widthOffset;
            Render.DrawBox(footpos.x - (width / 2), (float)Screen.height - headpos.y - height, width, height, color, 2f);
            if (playersName)
            {
                Render.DrawString(new Vector2(headpos.x, (float)Screen.height - headpos.y - 20f), namePlayer); // Name of player
            }
            if (playersDistance)
            {
                float offsetDistanceDrawing = 20f;
                Render.DrawString(new Vector2(headpos.x - (width / 2) + offsetDistanceDrawing, (float)Screen.height - headpos.y + 10f), distanceToPlayer + "m", true); // Distance to mob
            }
            
            if (playersLines)
            {
                Vector2 screenCenter = new Vector2((float)Screen.width / 2, (float)Screen.height / 2);
                Vector2 playerPosition = new Vector2(footpos.x, (float)Screen.height - headpos.y);
                Render.DrawLine(screenCenter, playerPosition, color, 2f);
                //Render.DrawLine(playerPositionhead, playerPositionFoot, Color.green, 2f);
            }

            if (playersHealth)
            {
                float offsetHealthDrawing = 20f;
                Render.DrawString(new Vector2(headpos.x + (width / 2) - offsetHealthDrawing, (float)Screen.height - headpos.y + 10f), ((int)health +1) + "/" + ((int)maxHealth +1));
            }

        }

    }
}
