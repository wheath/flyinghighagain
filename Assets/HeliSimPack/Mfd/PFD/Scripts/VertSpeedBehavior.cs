using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HeliSimPack.MFD
{
  public class VertSpeedBehavior : BaseMfdRenderer
  {
    [SerializeField]
    [Tooltip("Needle indicating vertical speed")]
    RectTransform needle;

    Rigidbody rb;

    [SerializeField]
    [Tooltip("Rotation of needle (in degrees) at 0 feet per minute")]
    float rotationAt0;

    [SerializeField]
    [Tooltip("Rotation of needle (in degrees) at 500 feet per minute")]
    float rotationAt500;

    [SerializeField]
    [Tooltip("Rotation of needle (in degrees) at 1000 feet per minute")]
    float rotationAt1000;

    [SerializeField]
    [Tooltip("Rotation of needle (in degrees) at 2000 feet per minute")]
    float rotationAt2000;

    [SerializeField]
    [Tooltip("Rotation of needle (in degrees) at 4000 feet per minute")]
    float rotationAt4000;

    [SerializeField]
    [Tooltip("Rotation of needle (in degrees) at 6000 feet per minute")]
    float rotationAt6000;

    [SerializeField]
    [Tooltip("Vertical speed readout")]
    Text readout;

    override public void updateRender()
    {
      if (null != rb)
      {
        // retrieve vertical speed in feet per minute
        float vertSpeed = rb.velocity.y * 196.85f;

        // Avoid near zero values
        if (vertSpeed > -20 && vertSpeed < 20)
        {
          vertSpeed = 0;
        }

        // calculate needle rotation
        float rotation = rotationAt0;
        if (Mathf.Abs(vertSpeed) <= 500.0f)
        {
          rotation = rotationAt0 + (Mathf.Abs(vertSpeed) - 0.0f) / (500.0f - 0.0f) * (rotationAt500 - rotationAt0);
        }
        else if (Mathf.Abs(vertSpeed) > 500.0f && Mathf.Abs(vertSpeed) <= 1000.0f)
        {
          rotation = rotationAt500 + (Mathf.Abs(vertSpeed) - 500.0f) / (1000.0f - 500.0f) * (rotationAt1000 - rotationAt500);
        }
        else if (Mathf.Abs(vertSpeed) > 1000.0f && Mathf.Abs(vertSpeed) <= 2000.0f)
        {
          rotation = rotationAt1000 + (Mathf.Abs(vertSpeed) - 1000.0f) / (2000.0f - 1000.0f) * (rotationAt2000 - rotationAt1000);
        }
        else if (Mathf.Abs(vertSpeed) > 2000.0f && Mathf.Abs(vertSpeed) <= 4000.0f)
        {
          rotation = rotationAt2000 + (Mathf.Abs(vertSpeed) - 2000.0f) / (4000.0f - 2000.0f) * (rotationAt4000 - rotationAt2000);
        }
        else if (Mathf.Abs(vertSpeed) > 4000.0f && Mathf.Abs(vertSpeed) <= 6000.0f)
        {
          rotation = rotationAt4000 + (Mathf.Abs(vertSpeed) - 4000.0f) / (6000.0f - 4000.0f) * (rotationAt6000 - rotationAt4000);
        }
        else // > 8000
        {
          rotation = rotationAt6000;
        }

        // apply sign (+ / -)
        if (!Mathf.Approximately(0, vertSpeed))
        {
          rotation *= -vertSpeed / Mathf.Abs(vertSpeed);
        }

        // apply rotation
        needle.localEulerAngles = new Vector3(needle.transform.localEulerAngles.x, needle.transform.localEulerAngles.y, rotation);

        // update readout
        float readoutSpeed = Mathf.Round((vertSpeed) / 10) * 10;
        readout.text = ((int)(readoutSpeed)).ToString();
      }
    }

    override public void setHcSetup(HelicopterSetup iHcSetup)
    {
      // find required components
      rb = iHcSetup.gameObject.GetComponent<Rigidbody>();
    }
  }
}