
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ValheimHackGUI
{
    internal class Teleport
    {
        public List<Tuple<Vector3, Quaternion>> teleportLocations;


        public void TeleportToPlayer(Player targetPlayer)
        {
            Player localplayer = Player.m_localPlayer;
            if (localplayer == null) {
                return;
            }
            Vector3 targetPosition = targetPlayer.transform.position;
            Quaternion targetRotation = targetPlayer.transform.rotation;

            localplayer.TeleportTo(targetPosition, targetRotation, false);
        }

        public void TeleportToMe(Player playetToTeleport)
        {
            Player localplayer = Player.m_localPlayer;
            if (localplayer == null)
            {
                return;
            }
            Vector3 localPosition = localplayer.transform.position;
            Quaternion localRotation = localplayer.transform.rotation;

            playetToTeleport.TeleportTo(localPosition, localRotation, false);
        }


        public void TeleportPlayerToPlayer(Player playerToTeleport, Player targetPlayer)
        {
            if (playerToTeleport == null || targetPlayer == null)
            {
                return;
            }

            Vector3 targetPlayerPosition = targetPlayer.transform.position;
            Quaternion targetPlayerRotation = targetPlayer.transform.rotation;

            playerToTeleport.TeleportTo(targetPlayerPosition, targetPlayerRotation, false);
        }

        public void SaveLocation()
        {
            Player localplayer = Player.m_localPlayer;
            teleportLocations.Add(Tuple.Create(localplayer.transform.position, localplayer.transform.rotation));
        }

        public List<Tuple<Vector3,Quaternion>> GetAllLocations()
        {
            return teleportLocations;
        }

        public void TeleportToLocation(Tuple<Vector3,Quaternion> tupleTarget)
        {
            Player localplayer = Player.m_localPlayer;
            if (localplayer == null) 
            {
                return;
            }
            
            Vector3 targetPosition = tupleTarget.Item1;
            Quaternion targetRotation = tupleTarget.Item2;

            localplayer.TeleportTo(targetPosition, targetRotation, true);

        }
    }
}
