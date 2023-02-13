using System;
using System.Collections.Generic;
using UnityEngine;


namespace ValheimHack
{
    internal class PlayerHacks
    {

        public void debugFly()
        {
            Player localplayer = Player.m_localPlayer;
            if (localplayer == null)
            {
                return;
            }
            localplayer.ToggleDebugFly();
        }


        public void godMode()
        {
            Player localplayer = Player.m_localPlayer;
            if (localplayer == null)
            {
                return;
            }
            localplayer.SetGodMode(!localplayer.InGodMode());
        }

        public void infiniteStamina()
        {
            Player localplayer = Player.m_localPlayer;
            if (localplayer.GetStamina() < localplayer.GetMaxStamina() -1f)
            {
                localplayer.UseStamina(-localplayer.GetMaxStamina());
            }
        }

        public void infiniteStaminaOthers()
        {
            Player localplayer = Player.m_localPlayer;
            List<Player> allPlayers = new List<Player>();
            allPlayers = Player.GetAllPlayers();
            foreach (Player player in allPlayers)
            {
                if (player == localplayer) { // ne pas appliquer a soi-meme
                    continue;
                }
                player.UseStamina(-player.GetMaxStamina());
            }
        }


        /*-----------------------------DISABLES-----------------------------*/
        public void disableDebugFly()
        {
            Player localplayer = Player.m_localPlayer;
            if (localplayer == null)
            {
                return;
            }
            if (!localplayer.IsDebugFlying())
            {
                return;
            }
            localplayer.ToggleDebugFly();
        }

        public void disableGodMode()
        {
            Player localplayer = Player.m_localPlayer;
            if (localplayer == null)
            {
                return;
            }
            if (!localplayer.InGodMode())
            {
                return;
            }
            localplayer.SetGodMode(false);
        }

    }
}
