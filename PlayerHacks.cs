using System;
using System.Collections.Generic;
using UnityEngine;
using ValheimHackGUI;

namespace ValheimHack
{
    internal class PlayerHacks
    {
        public bool debugFly()
        {
            Player localplayer = Player.m_localPlayer;
            if (localplayer == null)
            {
                return false;
            }
            return localplayer.ToggleDebugFly();
            
        }


        public bool godMode()
        {
            Player localplayer = Player.m_localPlayer;
            if (localplayer == null)
            {
                return false;
            }
            localplayer.SetGodMode(!localplayer.InGodMode());
            return localplayer.InGodMode();
        }

        public bool ghostMode()
        {
            Player localplayer = Player.m_localPlayer;
            if (localplayer == null)
            {
                return false;
            }
            localplayer.SetGhostMode(!localplayer.InGhostMode());
            return localplayer.InGhostMode();
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

        public void disableGhostMode()
        {
            Player localplayer = Player.m_localPlayer;
            if (localplayer == null)
            {
                return;
            }
            if (!localplayer.InGhostMode())
            {
                return;
            }
            localplayer.SetGhostMode(false);
        }

    }
}
