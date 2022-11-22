using GorillaLocomotion;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace BalloonFloater.Scripts
{
    public class BFManager : MonoBehaviour
    {
        public Player player;
        public bool isFloating = false;
        public List<Rigidbody> rigidbodies = new List<Rigidbody>();

        public void DestroyBalloon(Rigidbody rb) => StartCoroutine(IDestroyBalloon(rb));

        internal IEnumerator IDestroyBalloon(Rigidbody rb)
        {
            yield return new WaitForSeconds(Plugin.Instance.settings.destroyTime);
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

        internal void FixedUpdate()
        {
            isFloating = Plugin.Instance.EquippedBalloons != 0;
            if (isFloating && Plugin.Instance.inRoom)
            {
                if (player == null)
                    player = FindObjectOfType<Player>();

                if (player != null)
                {
                    if (player.GetComponent<Rigidbody>().velocity.y < Plugin.Instance.settings.playerMaxGain * Plugin.Instance.EquippedBalloons)
                        player.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0.5f * Plugin.Instance.EquippedBalloons, 0), ForceMode.VelocityChange);

                    if (rigidbodies.Count != 0)
                    {
                        foreach (var rb in rigidbodies)
                            rb.AddForce(new Vector3(0, Plugin.Instance.settings.balloonGain * Plugin.Instance.EquippedBalloons, 0), ForceMode.VelocityChange);
                    }
                }
            }
        }
    }
}
