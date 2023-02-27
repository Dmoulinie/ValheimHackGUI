
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ValheimHackGUI
{
    internal class Teleport
    {
        public List<Tuple<Vector3, Quaternion>> teleportLocations = new List<Tuple<Vector3,Quaternion>>();
        public Tuple<Vector3, Quaternion> location;
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

        public void SaveLocation(string name)
        {
            Player localplayer = Player.m_localPlayer;
            string myDocumentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string valheimMenuFolder = Path.Combine(myDocumentsFolder, "Valheim_Menu");
            if (!Directory.Exists(valheimMenuFolder))
            {
                Directory.CreateDirectory(valheimMenuFolder);
            }
            string filePath = Path.Combine(valheimMenuFolder, "locations.txt");
            string location = localplayer.transform.position.ToString() + " " + localplayer.transform.rotation.ToString() + " " + name + ";" + Environment.NewLine;
            File.AppendAllText(filePath, location);
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

            localplayer.TeleportTo(targetPosition, targetRotation, false);

        }

        public Tuple<Vector3,Quaternion> getCurrentLocation()
        {
            return location;
        }
    }
}
