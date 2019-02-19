using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeliSimPack.MFD
{
  public class PitchBehavior : BaseMfdRenderer
  {
    [SerializeField]
    [Tooltip("Translation of pitch tape at zero degree of pitch")]
    float positionAtZero;

    [SerializeField]
    [Tooltip("Translation of pitch tape for every 10 degrees of pitch")]
    float translationPer10Degrees;

    [SerializeField]
    [Tooltip("Minimum pitch to be displayed")]
    float minPitch;

    [SerializeField]
    [Tooltip("Maximum pitch to be displayed")]
    float maxPitch;

    Transform body;

    override public void updateRender()
    {
      // retrieve pitch
      float pitch = Mathf.DeltaAngle(0, -body.rotation.eulerAngles.x);

      // calculate pitch tape translation
      float translation = positionAtZero + translationPer10Degrees * Mathf.Clamp(pitch, minPitch, maxPitch) / 10.0f;

      // apply translation
      rectT.anchoredPosition3D = new Vector3(rectT.anchoredPosition3D.x, translation, rectT.anchoredPosition3D.z);
    }

    override public void setHcSetup(HelicopterSetup iHcSetup)
    {
      // find required components
      body = iHcSetup.gameObject.GetComponent<Transform>();
    }
  }
}