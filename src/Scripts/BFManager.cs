using GorillaLocomotion;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;

namespace BalloonFloater.Scripts
{
    public class BFManager : MonoBehaviourPunCallbacks
    {
        public Player player;
        public bool isFloating = false;
        public List<Rigidbody> rigidbodies = new List<Rigidbody>();

        public void DestroyBalloon(Rigidbody rb) => StartCoroutine(IDestroyBalloon(rb));

        internal IEnumerator IDestroyBalloon(Rigidbody rb)
        {
            yield return new WaitForSeconds(Plugin.Instance.data.destroyTime);
            // Creates a big cube that will act as a GorillaThrowable or a SlingshotProjectile that will result in the balloon exploding just after release
            GameObject tempObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            tempObject.layer = LayerMask.NameToLayer("GorillaThrowable");
            tempObject.GetComponent<Renderer>().forceRenderingOff = true;
            tempObject.transform.position = rb.gameObject.transform.position;
            tempObject.transform.localScale = Vector3.one * 0.28f;

            // Delete the cube shortly after creation
            Destroy(tempObject, 0.1f);

            yield break;
        }

        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            Plugin.Instance.EquippedBalloons = 0;
            isFloating = false;
            rigidbodies.Clear();
        }

        internal void FixedUpdate()
        {
            isFloating = Plugin.Instance.EquippedBalloons != 0;
            if (isFloating && Plugin.Instance.inRoom)
            {
                if (player == null) player = FindObjectOfType<Player>();

                if (player != null)
                {
                    if (player.GetComponent<Rigidbody>().velocity.y < Plugin.Instance.data.playerMaxGain * Plugin.Instance.EquippedBalloons * 1.5f)
                        player.GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * Plugin.Instance.data.playerGain * Plugin.Instance.EquippedBalloons, ForceMode.VelocityChange);

                    if (Plugin.Instance.data.movementMode == 1)
                    {
                        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 axis);
                        if (axis.y > 0.5f) player.GetComponent<Rigidbody>().AddForce(player.headCollider.transform.forward * (Plugin.Instance.data.playerGain + 0.5f * 2), ForceMode.Impulse);
                        else if (axis.y < -0.5f) player.GetComponent<Rigidbody>().AddForce(player.headCollider.transform.forward * (Plugin.Instance.data.playerGain + 0.5f * 2) * -1, ForceMode.Impulse);
                    }

                    if (rigidbodies.Count != 0)
                    {
                        foreach (var rb in rigidbodies)
                            rb.AddForce(Vector3.up * Plugin.Instance.data.balloonGain * Plugin.Instance.EquippedBalloons * 1.15f, ForceMode.VelocityChange);
                    }
                }
            }
        }
    }
}
