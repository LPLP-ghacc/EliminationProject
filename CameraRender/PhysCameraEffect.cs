using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class PhysCameraEffect : MonoBehaviour
{
    public PlayerInput input;
    public float intensity = 0.1f;
    public float returnSpeed = 1f;
    public float smoothingSpeed;
    private Volume volume;
    private LensDistortion distortion;
    private Vignette vignette;

    private float lastLookX;
    private float lastLookY;

    private void Start()
    {
        volume = GetComponent<Volume>();

        if (volume.profile.TryGet(out Vignette vignetteEffect))
        {
            vignette = vignetteEffect;
            vignette.active = true;
        }
        else
        {
            Debug.LogWarning("Vignette effect not found in the Volume profile.");
        }

        if (volume.profile.TryGet(out LensDistortion _distortion))
        {
            distortion = _distortion;
            distortion.active = true; 
        }
        else
        {
            Debug.LogWarning("Distortion effect not found in the Volume profile.");
        }
    }

    private void LateUpdate()
    {
        MouseLook();
        ReturnToCenter();

    }

    private void MouseLook()
    {
        var lookX = input.look.x;
        var lookY = input.look.y;

        float clampedX = Mathf.Clamp(lastLookX, -intensity, intensity);
        float clampedY = Mathf.Clamp(lastLookY, -intensity, intensity);

        float smoothedLookX = Mathf.Lerp(clampedX, lookX, Time.deltaTime * smoothingSpeed);
        float smoothedLookY = Mathf.Lerp(clampedY, lookY, Time.deltaTime * smoothingSpeed);

        Vector2 targetValue = new Vector2(0.5f + clampedX, 0.5f + clampedY);

        if (vignette != null)
        {
            vignette.center.value = targetValue;
        }

        if (distortion != null)
        {
            distortion.center.value = targetValue;
        }

        lastLookX = smoothedLookX;
        lastLookY = smoothedLookY;
    }

    private void ReturnToCenter()
    {
        if (vignette != null && distortion != null)
        {
            var actualValue = vignette.center.value;

            var targetValue = Vector2.Lerp(actualValue, new Vector2(0.5f, 0.5f), Time.deltaTime * returnSpeed);

            vignette.center.value = targetValue;
            distortion.center.value = targetValue;
        }
    }
}
