using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HeliSimPack.MFD
{
  public class RotorSpeedBeavior : BaseMfdRenderer
  {
    [SerializeField]
    [Tooltip("Needle indicating rotor speed")]
    RectTransform needle;

    [SerializeField]
    [Tooltip("The needle's images")]
    Image[] needleImages;

    [SerializeField]
    [Tooltip("Arcs following the needle")]
    RectTransform[] arcs;

    [SerializeField]
    [Tooltip("The arcs' images")]
    Image[] arcImages;

    [SerializeField]
    [Tooltip("The angular width of a single arc")]
    float arcAngularWidth;

    [SerializeField]
    [Tooltip("Lower boundary of red zone (in percent)")]
    float lowerRedAngleBoundary;

    [SerializeField]
    [Tooltip("Upper boundary of red zone (in percent)")]
    float upperRedAngleBoundary;

    [SerializeField]
    [Tooltip("Lower boundary of yellow zone (in percent)")]
    float lowerYellowAngleBoundary;

    [SerializeField]
    [Tooltip("Upper boundary of yellow zone (in percent)")]
    float upperYellowAngleBoundary;

    HelicopterSimulation.RotorRotator rotorRotator;

    [SerializeField]
    [Tooltip("Needle rotation at 0 percent RPM")]
    float rotationAt0;

    [SerializeField]
    [Tooltip("Needle rotation at 50 percent RPM")]
    float rotationAt50;

    [SerializeField]
    [Tooltip("Needle rotation at 100 percent RPM")]
    float rotationAt100;

    [SerializeField]
    [Tooltip("Needle rotation at 110 percent RPM")]
    float rotationAt110;

    [SerializeField]
    [Tooltip("Rotor speed readout (in percent)")]
    Text readout;

    override public void updateRender()
    {
      if (null != rotorRotator)
      {
        // Get rotor speed in percent
        float rotorSpeed = rotorRotator.getRotorSpeed();

        // calculate needle rotation
        float rotation = 0;
        if (Mathf.Abs(rotorSpeed) >= 0.0f && Mathf.Abs(rotorSpeed) <= 50.0f)
        {
          rotation = rotationAt0 + (Mathf.Abs(rotorSpeed) - 0.0f) / (50.0f - 0.0f) * (rotationAt50 - rotationAt0);
        }
        else if (Mathf.Abs(rotorSpeed) > 50.0f && Mathf.Abs(rotorSpeed) <= 100.0f)
        {
          rotation = rotationAt50 + (Mathf.Abs(rotorSpeed) - 50.0f) / (100.0f - 50.0f) * (rotationAt100 - rotationAt50);
        }
        else if (Mathf.Abs(rotorSpeed) > 100.0f && Mathf.Abs(rotorSpeed) <= 110.0f)
        {
          rotation = rotationAt100 + (Mathf.Abs(rotorSpeed) - 100.0f) / (110.0f - 100.0f) * (rotationAt110 - rotationAt100);
        }
        else // > 110
        {
          rotation = rotationAt110;
        }

        // compute needle, tape, and arcs color
        Color col = Color.green;
        if (isBetween(rotation, 0, lowerRedAngleBoundary))
        {
          col = Color.red;
        }
        else if (isBetween(rotation, lowerRedAngleBoundary, lowerYellowAngleBoundary))
        {
          col = Color.yellow;
        }
        else if (isBetween(rotation, lowerYellowAngleBoundary, upperYellowAngleBoundary))
        {
          col = Color.green;
        }
        else if (isBetween(rotation, upperYellowAngleBoundary, upperRedAngleBoundary))
        {
          col = Color.yellow;
        }
        else
        {
          col = Color.red;
        }

        // apply color
        readout.color = col;

        foreach (Image needle in needleImages)
        {
          needle.color = col;
        }

        // update arcs to follow the needle
        float lastArcRotation = 0;
        foreach (RectTransform arc in arcs)
        {
          float arcRotation = 0;
          if (Mathf.Approximately(0, lastArcRotation))
          {
            arcRotation = Mathf.Clamp(rotation, -360, 0);
          }
          else
          {
            arcRotation = Mathf.Clamp(lastArcRotation - arcAngularWidth, -360, 0);
          }

          lastArcRotation = arcRotation;

          arc.localEulerAngles = new Vector3(0, 0, arcRotation);
        }

        foreach (Image arc in arcImages)
        {
          arc.color = col;
        }

        // apply needle rotation
        needle.localEulerAngles = new Vector3(needle.transform.localEulerAngles.x, needle.transform.localEulerAngles.y, rotation);

        // Update readout (100 percent shown as 105 percent)
        readout.text = ((int)(rotorSpeed * 1.05f + 0.5f)).ToString();
      }
    }

    override public void setHcSetup(HelicopterSetup iHcSetup)
    {
      // find required components
      rotorRotator = iHcSetup.gameObject.GetComponent<HelicopterSimulation.RotorRotator>();
    }

    bool isBetween(float iValue, float iLimit1, float iLimit2)
    {
      return Mathf.Approximately(iValue, Mathf.Clamp(iValue, iLimit1, iLimit2))
          || Mathf.Approximately(iValue, Mathf.Clamp(iValue, iLimit2, iLimit1));
    }
  }
}