using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class StreamInitializer : MonoBehaviour
{
    public Transform attachedParent { get; private set; }
    public IEnumerator StreamBulletAttack(BulletProperties bullet, Team team, UnityEngine.Object obj, GameObject bulletGO, Vector3 bulletDirection, float streamInterval, float duration, AudioClip audioClip)
    {
        if (attachedParent == null)
        {
            UnityEngine.Debug.LogError("StreamInitializer's SetOriginTransform(transform) method not yet declared in its parent class");
            yield break;
        }
        float attackEndTime = Time.time + duration;
        while (Time.time <= attackEndTime)
        {
            GenericActions.BulletAttack(bullet, team, obj, Instantiate(bulletGO, attachedParent.position, Quaternion.identity), bulletDirection);
            if (audioClip != null) GlobalSoundManager.GlobalSoundPlayer.PlayOneShot(audioClip);
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