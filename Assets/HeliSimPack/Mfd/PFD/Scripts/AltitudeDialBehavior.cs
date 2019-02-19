using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeliSimPack.MFD
{

  public class AltitudeDialBehavior : BaseMfdRenderer
  {
    [SerializeField]
    [Tooltip("The neelde showing the altitude's hundreds")]
    RectTransform needle100;

    [SerializeField]
    [Tooltip("The neelde showing the altitude's tousands")]
    RectTransform needle1000;

    [SerializeField]
    [Tooltip("Default value for rotation of needles (in degrees)")]
    float rotationAtZero;

    [SerializeField]
    [Tooltip("Rotation (in degrees) for every 100 feet")]
    float rotationPer100Feet;

    [SerializeField]
    [Tooltip("The rolling readout's tens")]
    RectTransform rollingTens;

    [SerializeField]
    [Tooltip("The rolling readout's tens translation for every 20 feet")]
    float translationPer20Feet;

    [SerializeField]
    [Tooltip("The rolling readout's hundreds")]
    RectTransform rollingHundreds;

    [SerializeField]
    [Tooltip("The rolling readout's hundreds translation for every 100 feet")]
    float translationPer100Feet;

    [SerializeField]
    [Tooltip("The rolling readout's tousands")]
    RectTransform rollingTousands;

    [SerializeField]
    [Tooltip("The rolling readout's tousands translation for every 1000 feet")]
    float translationPer1000Feet;

    [SerializeField]
    [Tooltip("Minimum altitude to display (in feet)")]
    float minAlt;

    [SerializeField]
    [Tooltip("Macimum altitude to display (in feet)")]
    float maxAlt;

    Rigidbody rb;

    override public void updateRender()
    {
      if (null != rb)
      {
        float Alt = rb.position.y * 3.28084f; // alttude in feet

        // rotation of needles
        float rotation100 = rotationAtZero + rotationPer100Feet * Mathf.Clamp(Alt, minAlt, maxAlt) / 100.0f;
        float rotation1000 = rotationAtZero + rotationPer100Feet * Mathf.Clamp(Alt, minAlt, maxAlt) / 1000.0f;

        // apply rotations
        needle100.localEulerAngles = new Vector3(0, 0, rotation100);
        needle1000.localEulerAngles = new Vector3(0, 0, rotation1000);

        // translation of rolling readout
        float tensTranslation = (Alt % 100.0f) / 20.0f * translationPer20Feet;

        float hundredsTranslation = Mathf.Floor((Alt % 1000.0f) / 100.0f) * translationPer100Feet;
        if ((Alt % 100.0f) > 90.0f)
        {
          hundredsTranslation += (Alt % 100.0f - 90.0f) / 10.0f * translationPer100Feet;
        }

        float tousandsTranslation = Mathf.Floor((Alt % 10000.0f) / 1000.0f) * translationPer1000Feet;
        if ((Alt % 1000.0f) > 990.0f)
        {
          tousandsTranslation += (Alt % 1000.0f - 990.0f) / 10.0f * translationPer1000Feet;
        }

        // apply translation
        rollingTens.localPosition = new Vector3(0, tensTranslation, 0);
        rollingHundreds.localPosition = new Vector3(0, hundredsTranslation, 0);
        rollingTousands.localPosition = new Vector3(0, tousandsTranslation, 0);
      }
    }

    override public void setHcSetup(HelicopterSetup iHcSetup)
    {
      // find required components
      rb = iHcSetup.gameObject.GetComponent<Rigidbody>();
    }
  }
}
