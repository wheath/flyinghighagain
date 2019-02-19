using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HeliSimPack.MFD
{
  public class RoseBehavior : BaseMfdRenderer
  {
    [SerializeField]
    [Tooltip("The compass rose")]
    RectTransform rose;

    [SerializeField]
    [Tooltip("The track ball on top of compass rose")]
    RectTransform trackBall;

    Transform body;
    Rigidbody rb;

    [SerializeField]
    [Tooltip("Heading readout")]
    Text readout;

    [SerializeField]
    [Tooltip("Rotation of compass rose for heading of zero degree (in degrees)")]
    float rotationAtZero;

    [SerializeField]
    [Tooltip("The labels indication the range ring ranges")]
    Text[] ranges;

    HelicopterSimulation.MfdRangeController rangeController;

    override public void updateRender()
    {
      // Get heading
      float heading = body.rotation.eulerAngles.y;

      // Get track
      Vector3 velocityInPlane = new Vector3(rb.velocity.x, 0, rb.velocity.z);
      velocityInPlane = Quaternion.AngleAxis(-heading, Vector3.up) * velocityInPlane;
      float track = -Mathf.Atan2(velocityInPlane.x, velocityInPlane.z) * Mathf.Rad2Deg;

      // Apply track ball rotation
      trackBall.gameObject.SetActive(Mathf.Abs(velocityInPlane.z) > 0.1);

      // keep heading in range [1,360]
      while (heading >= 360.5)
      {
        heading -= 360;
      }
      while (heading < 0.5)
      {
        heading += 360;
      }

      // keep track in range [1,360]
      while (track >= 360)
      {
        track -= 360;
      }
      while (track < 0)
      {
        track += 360;
      }

      // calculate and apply rotation of compass rose
      float rotation = rotationAtZero + heading;
      rose.localEulerAngles = new Vector3(rose.transform.localEulerAngles.x, rose.transform.localEulerAngles.y, rotation);

      // apply track ball rotation
      trackBall.localEulerAngles = new Vector3(trackBall.localEulerAngles.x, trackBall.localEulerAngles.y, track);

      // update heading readout
      readout.text = (heading + 0.5f) < 10 ? "00" + ((int)(heading + 0.5f)).ToString() :
                     (heading + 0.5f) < 100 ? "0" + ((int)(heading + 0.5f)).ToString() : ((int)(heading + 0.5f)).ToString();

      // retrive map range
      float range = rangeController.getRange();

      // auto-fill the range rings labels
      for (int i = 0; i < ranges.Length; ++i)
      {
        ranges[i].text = (range / ((float)(ranges.Length)) * (float)(i + 1)).ToString();
      }
    }

    override public void setHcSetup(HelicopterSetup iHcSetup)
    {
      // find required components
      body = iHcSetup.gameObject.GetComponent<Transform>();
      rb = iHcSetup.gameObject.GetComponent<Rigidbody>();
      rangeController = iHcSetup.gameObject.GetComponent<HelicopterSimulation.MfdRangeController>();
    }
  }
}