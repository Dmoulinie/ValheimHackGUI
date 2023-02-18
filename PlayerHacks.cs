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

        public void setDebugFly(bool value)
        {
            Player localplayer = Player.m_localPlayer;
            if (localplayer == null)
            {
                return;
            }

            if (localplayer.IsDebugFlying() && value) // Si player vol déja et veut encore voler
            {
                return;
            }

            if (!localplayer.IsDebugFlying() && !value) // Si player vol pas et veut cancel (grave con)
            {
                return;
            }

            debugFly();

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

        public void setGodMode(bool value)
        {
            Player localplayer = Player.m_localPlayer;
            if (localplayer == null)
            {
                return;
            }
            if (localplayer.InGodMode() && value)  // Si player est en godmode et veut encore se mettre en godmode
            {
                return;
            }
            if (!localplayer.InGodMode() && !value) // Si player pas en godmode pas et veut cancel (grave con)
            {
                return;
            }
            localplayer.SetGodMode(value);

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

        public void setGhostMode(bool value)
        {
            Player localplayer = Player.m_localPlayer;
            if (localplayer == null)
            {
                return;
            }
            if (localplayer.InGhostMode() && value)
            {
                return;
            }
            if (!localplayer.InGhostMode() && !value)
            {
                return;
            }    
            localplayer.SetGhostMode(value);

        }

        public void infiniteStamina()
        {
            Player localplayer = Player.m_localPlayer;
            if (localplayer.GetStamina() < localplayer.GetMaxStamina())
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
