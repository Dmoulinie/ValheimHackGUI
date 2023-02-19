
using UnityEngine;

namespace ValheimHackGUI
{
    internal class Teleport
    {
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

        public void TeleportToMe(Player initialPlayer)
        {
            Player localplayer = Player.m_localPlayer;
            if (localplayer == null)
            {
                return;
            }
            Vector3 targetPosition = localplayer.transform.position;
            Quaternion targetRotation = localplayer.transform.rotation;

            initialPlayer.TeleportTo(targetPosition, targetRotation, false);
        }
    }
}
