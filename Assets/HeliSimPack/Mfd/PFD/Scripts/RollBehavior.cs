using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeliSimPack.MFD
{
  public class RollBehavior : BaseMfdRenderer
  {
    [SerializeField]
    [Tooltip("Minimum roll angle to be displayed (in degrees)")]
    float minRoll;

    [SerializeField]
    [Tooltip("Maximum roll angle to be displayed (in degrees)")]
    float maxRoll;

    [SerializeField]
    [Tooltip("Skid/slip indicator")]
    RectTransform indicator;

    [SerializeField]
    [Tooltip("Maximum slip displayed (In degrees)")]
    float maxSlip;

    [SerializeField]
    [Tooltip("Maximum slip deviation displayed (In pixels)")]
    float maxDeviation;

    HelicopterSimulation.RigidBodyController rbController;
    Rigidbody rb;

    Transform body;


    override public void updateRender()
    {
      // Apply roll rotation
      float roll = Mathf.DeltaAngle(0, -body.rotation.eulerAngles.z);
      float rotation = Mathf.Clamp(roll, minRoll, maxRoll);
      rectT.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, rotation);

      // Skid/slip indicator
      if (null != indicator)
      {
        // calculate difference between track and heading
        float heading = body.rotation.eulerAngles.y;
        Vector3 velocityInPlane = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        float track = Mathf.Atan2(velocityInPlane.x, velocityInPlane.z) * Mathf.Rad2Deg;

        float trackHeadingDiff = track - heading;

        while (trackHeadingDiff < -180)
        {
          trackHeadingDiff += 360;
        }
        while (trackHeadingDiff > 180)
        {
          trackHeadingDiff -= 360;
        }

        // apply skid/slip translation
        float slip = Mathf.Abs(velocityInPlane.z) > 0.1f ? Mathf.Clamp(trackHeadingDiff, -maxSlip, maxSlip) : 0;
        float slipTranslation = slip / maxSlip * maxDeviation;
        indicator.localPosition = new Vector3(slipTranslation, indicator.localPosition.y, indicator.localPosition.z);
      }
    }

    override public void setHcSetup(HelicopterSetup iHcSetup)
    {
      // find required components
      body = iHcSetup.gameObject.GetComponent<Transform>();
      rbController = iHcSetup.gameObject.GetComponent<HelicopterSimulation.RigidBodyController>();
      rb = iHcSetup.gameObject.GetComponent<Rigidbody>();
    }
  }
}