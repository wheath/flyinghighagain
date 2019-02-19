using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeliSimPack.MFD
{
  public class SpeedDialBehavior : BaseMfdRenderer
  {
    [SerializeField]
    [Tooltip("Neelde indicating the airspeed")]
    RectTransform needle;

    [SerializeField]
    [Tooltip("Needle rotation (in degrees) at 0 knots")]
    float rotationAt0;

    [SerializeField]
    [Tooltip("Needle rotation (in degrees) at 20 knots")]
    float rotationAt20;

    [SerializeField]
    [Tooltip("Needle rotation (in degrees) at 40 knots")]
    float rotationAt40;

    [SerializeField]
    [Tooltip("Needle rotation (in degrees) at 60 knots")]
    float rotationAt60;

    [SerializeField]
    [Tooltip("Needle rotation (in degrees) at 80 knots")]
    float rotationAt80;

    [SerializeField]
    [Tooltip("Needle rotation (in degrees) at 100 knots")]
    float rotationAt100;

    [SerializeField]
    [Tooltip("Needle rotation (in degrees) at 120 knots")]
    float rotationAt120;

    [SerializeField]
    [Tooltip("Needle rotation (in degrees) at 140 knots")]
    float rotationAt140;

    [SerializeField]
    [Tooltip("Needle rotation (in degrees) at 160 knots")]
    float rotationAt160;

    [SerializeField]
    [Tooltip("Needle rotation (in degrees) at 180 knots")]
    float rotationAt180;

    [SerializeField]
    [Tooltip("Needle rotation (in degrees) at 200 knots")]
    float rotationAt200;

    [SerializeField]
    [Tooltip("The units of the speed's rolling readout")]
    RectTransform rollingOnes;

    [SerializeField]
    [Tooltip("Translation of rolling readout units for every 1 KTS")]
    float translationPer1Kts;

    [SerializeField]
    [Tooltip("The tens of the speed's rolling readout")]
    RectTransform rollingTens;

    [SerializeField]
    [Tooltip("Translation of rolling readout tens for every 10 KTS")]
    float translationPer10Kts;

    [SerializeField]
    [Tooltip("The hundreds of the speed's rolling readout")]
    RectTransform rollingHundreds;

    [SerializeField]
    [Tooltip("Translation of rolling readout hundreds for every 100 KTS")]
    float translationPer100Kts;

    Rigidbody rb;

    override public void updateRender()
    {
      // get horizontal speed as a first approximation
      if (null != rb)
      {
        // get horizontal speed
        float heading = rb.rotation.eulerAngles.y;
        Vector3 velocityInPlane = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        // get forward speed from horiontal speed and heading
        velocityInPlane = Quaternion.AngleAxis(-heading, Vector3.up) * velocityInPlane;
        float SpeedMeterperSec = velocityInPlane.z > 0 ? velocityInPlane.z : 0;

        // convert to Knots
        float Ias = SpeedMeterperSec * 1.94384f;

        // calculate needle rotation
        float rotation = 0;
        if (Ias < 20)
        {
          rotation = rotationAt0 + (Ias - 0) / (20 - 0) * (rotationAt20 - rotationAt0);
        }
        else if (Ias < 40)
        {
          rotation = rotationAt20 + (Ias - 20) / (40 - 20) * (rotationAt40 - rotationAt20);
        }
        else if (Ias < 60)
        {
          rotation = rotationAt40 + (Ias - 40) / (60 - 40) * (rotationAt60 - rotationAt40);
        }
        else if (Ias < 80)
        {
          rotation = rotationAt60 + (Ias - 60) / (80 - 60) * (rotationAt80 - rotationAt60);
        }
        else if (Ias < 100)
        {
          rotation = rotationAt80 + (Ias - 80) / (100 - 80) * (rotationAt100 - rotationAt80);
        }
        else if (Ias < 120)
        {
          rotation = rotationAt100 + (Ias - 100) / (120 - 100) * (rotationAt120 - rotationAt100);
        }
        else if (Ias < 140)
        {
          rotation = rotationAt120 + (Ias - 120) / (140 - 120) * (rotationAt140 - rotationAt120);
        }
        else if (Ias < 160)
        {
          rotation = rotationAt140 + (Ias - 140) / (160 - 140) * (rotationAt160 - rotationAt140);
        }
        else if (Ias < 180)
        {
          rotation = rotationAt160 + (Ias - 160) / (180 - 160) * (rotationAt180 - rotationAt160);
        }
        else if (Ias <= 200)
        {
          rotation = rotationAt180 + (Ias - 180) / (200 - 180) * (rotationAt200 - rotationAt180);
        }

        // apply rotation
        needle.localEulerAngles = new Vector3(0, 0, rotation);

        // calculate and apply rolling readout units translation
        float onesTranslation = (Ias % 10.0f) * translationPer1Kts;
        rollingOnes.localPosition = new Vector3(0, onesTranslation, 0);

        // calculate and apply rolling readout tens translation
        float tensTranslation = Mathf.Floor((Ias % 100.0f) / 10.0f) * translationPer10Kts;
        if ((Ias % 10.0f) > 9.0f)
        {
          tensTranslation += (Ias % 10.0f - 9.0f) * translationPer10Kts;
        }
        rollingTens.localPosition = new Vector3(0, tensTranslation, 0);

        // calculate and apply rolling readout hundreds translation
        float hundredsTranslation = Mathf.Floor((Ias % 1000.0f) / 100.0f) * translationPer100Kts;
        if ((Ias % 100.0f) > 99.0f)
        {
          hundredsTranslation += (Ias % 100.0f - 99.0f) * translationPer100Kts;
        }
        rollingHundreds.localPosition = new Vector3(0, hundredsTranslation, 0);
      }
    }

    override public void setHcSetup(HelicopterSetup iHcSetup)
    {
      rb = iHcSetup.gameObject.GetComponent<Rigidbody>();
    }
  }
}