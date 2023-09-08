using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class StreamInitializer : MonoBehaviour
{
    public Transform attachedParent { get; private set; }
    public IEnumerator StreamBulletAttack(BulletProperties bullet, Team team, UnityEngine.Object obj, GameObject bulletGO, Vector3 offset, float firingAngle, float streamInterval, float duration, AudioClip audioClip)
    {
        if (attachedParent == null)
        {
            UnityEngine.Debug.LogError("StreamInitializer's SetOriginTransform(transform) method not yet declared in its parent class");
            yield break;
        }
        float attackEndTime = Time.time + duration;
        while (Time.time <= attackEndTime)
        {
            Vector3 worldOffset = attachedParent.rotation * offset;
            Vector3 firingDirection = -attachedParent.transform.right; // The original firing direction

            // Create a quaternion to represent the desired rotation
            Quaternion rotationQuaternion = Quaternion.Euler(0f, 0f, firingAngle);

            // Rotate the firing direction vector
            Vector3 rotatedDirection = rotationQuaternion * firingDirection;

            GenericActions.BulletAttack(bullet, team, obj, Instantiate(bulletGO, attachedParent.position + worldOffset, attachedParent.rotation), rotatedDirection);
            if (audioClip != null) GlobalSoundManager.GlobalSoundPlayer.PlayOneShot(audioClip);
            UnityEngine.Debug.Log("On the coroutine's firing loop");
            yield return new WaitForSeconds(streamInterval);
        }
        yield break;
    }

    public IEnumerator StreamMissileAttack(MissileProperties missileProperties, Team team, UnityEngine.Object obj, GameObject missileGO, Vector3 missileDirection, GameObject parentTransform, float streamInterval, float duration, AudioClip audioClip)
    {
        if (attachedParent == null)
        {
            UnityEngine.Debug.LogError("StreamInitializer's SetOriginTransform(transform) method not yet declared in its parent class");
            yield break;
        }
        float attackEndTime = Time.time + duration;
        while (Time.time <= attackEndTime)
        {
            GenericActions.MissileAttack(missileProperties, team, obj, Instantiate(missileGO, attachedParent.position, Quaternion.identity), missileDirection, parentTransform);
            if (audioClip != null) GlobalSoundManager.GlobalSoundPlayer.PlayOneShot(audioClip);
            yield return new WaitForSeconds(streamInterval);
        }
        yield break;
    }

    public void SetOriginTransform(Transform originTransform)
    {
        attachedParent = originTransform;
    }
}
