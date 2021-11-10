using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace PhotonTest
{
    public class PhotonGUI : MonoBehaviour
    {
        private int m_feildHeight = 25;
        private int m_feildVerticalSpacing = 2;
        private int m_fieldBaseXPosition = 0;
        private string m_username = "Test";
        private string m_createRoomName = "Test Room";
        private string m_joinRoomName = "Test Room";
        private bool m_isMainFoldoutOpen = false;
        private bool m_isInARoom = false;

        private float GetFieldSpacing(int index) => (m_feildHeight + m_feildVerticalSpacing) * index;

        private void CreateBoxGUI(int width, int fieldRowCount)
        {
            GUI.Box(new Rect(0, GetFieldSpacing(0), width, GetFieldSpacing(fieldRowCount + 1)), "Photon");
        }

        private void CreateFoldoutGUI(int width, int fieldRowCount, bool isFoldoutOpen)
        {
            CreateBoxGUI(width, fieldRowCount);
            var photonFoldoutSign = isFoldoutOpen ? "-" : "+";
            if (GUI.Button(new Rect(m_fieldBaseXPosition, GetFieldSpacing(0), 25, m_feildHeight), $"{photonFoldoutSign}"))
            {
                m_isMainFoldoutOpen = !isFoldoutOpen;
            }
        }

        private void OnGUI()
        {
            if (m_isMainFoldoutOpen)
            {
                if (PhotonNetwork.IsConnectedAndReady)
                {
                    if (m_isInARoom)
                    {
                        CreateFoldoutGUI(100, 1, true);
                        if (GUI.Button(new Rect(m_fieldBaseXPosition, GetFieldSpacing(1), 98, m_feildHeight), $"Leave Room"))
                        {
                            m_isInARoom = !PhotonNetwork.LeaveRoom();
                        }
                    }
                    else
                    {
                        DisplayRoomSelectionGUI();
                    }
                }
                else
                {
                    CreateFoldoutGUI(150, 2, true);
                    m_username = GUI.TextField(new Rect(0, GetFieldSpacing(1), 145, m_feildHeight), m_username);
                    if (GUI.Button(new Rect(m_fieldBaseXPosition, GetFieldSpacing(2), 145, m_feildHeight), $"Connect To Server As"))
                    {
                        PhotonNetwork.LocalPlayer.NickName = m_username;
                        PhotonNetwork.ConnectUsingSettings();
                    }
                }
            }
            else
            {
                CreateFoldoutGUI(100, 0, false);
            }

        }

        private void DisplayRoomSelectionGUI()
        {
            CreateFoldoutGUI(194, 3, true);

            var createRoomFieldPosition = GetFieldSpacing(1);

            m_createRoomName = GUI.TextField(new Rect(m_fieldBaseXPosition + 92, createRoomFieldPosition, 100, m_feildHeight), m_createRoomName);
            if (GUI.Button(new Rect(m_fieldBaseXPosition, createRoomFieldPosition, 90, m_feildHeight), $"Create Room"))
            {
                m_isInARoom = PhotonNetwork.CreateRoom(m_createRoomName);
            }

            var joinRoomFieldPosition = GetFieldSpacing(2);
            m_joinRoomName = GUI.TextField(new Rect(m_fieldBaseXPosition + 92, joinRoomFieldPosition, 100, m_feildHeight), m_joinRoomName);
            if (GUI.Button(new Rect(m_fieldBaseXPosition, joinRoomFieldPosition, 90, m_feildHeight), $"Join Room"))
            {
                m_isInARoom = PhotonNetwork.JoinRoom(m_joinRoomName);
            }

            if (GUI.Button(new Rect(m_fieldBaseXPosition, GetFieldSpacing(3), 192, m_feildHeight), $"Join Random Room"))
            {
                m_isInARoom = PhotonNetwork.JoinRandomRoom();
            }
        }
    }

}