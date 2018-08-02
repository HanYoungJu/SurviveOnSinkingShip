using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacter m_Character;
        private Vector3 m_Move;
        private bool m_Jump;
        private bool crouch;
        private bool kick;
        public bool isLocal;
        public GameObject player_camera;
        public GameObject Respawn;
        private GameObject respawnCanvas;
        private bool respawning = false;
        TimerScript ts;
        
        

        private void Start()
        {
            m_Character = GetComponent<ThirdPersonCharacter>();
            respawnCanvas = GameObject.Find("Canvas2").transform.FindChild("Respawn").gameObject;
            ts = respawnCanvas.GetComponent<TimerScript>();
            
        }
        private void FixedUpdate()
        {
            if (!isLocal)
            {
                player_camera.SetActive(false);
                return;
            }
            // read inputs
            if (transform.position.y < -17.2f)
            {
                localRespawn();
            }
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            crouch = Input.GetKey(KeyCode.X);
            kick = Input.GetKey(KeyCode.Z);
            m_Jump = Input.GetKey(KeyCode.Space);
            m_Move = v * Vector3.forward + h * Vector3.right;

            if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;

            NetworkManager.instance.GetComponent<NetworkManager>().CommandAcction(transform.position, 
                transform.rotation, m_Move, m_Jump, crouch, kick);
            m_Character.Move(m_Move, crouch, m_Jump, kick);
        }
        void localRespawn()
        {
            if (respawning)
            {
                return;
            }
            respawning = true;
            player_camera.SetActive(false);
            respawnCanvas.SetActive(true);
            StartCoroutine(waitingRespawn());
        }

        IEnumerator waitingRespawn()
        {
            yield return new WaitForSeconds(5f);
            respawnCanvas.SetActive(false);
            //ts.Reset = true;
            NetworkManager.instance.GetComponent<NetworkManager>().respawn(this.name);
            player_camera.SetActive(true);
            respawning = false;
        }
    }
}
