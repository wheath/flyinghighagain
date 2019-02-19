using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HeliSimPack.MFD
{
  public class TawsScannerBehavior : BaseMfdRenderer
  {
    [SerializeField]
    [Tooltip("The left mark of the TAWS scanner")]
    RectTransform markLeft;

    [SerializeField]
    [Tooltip("The right mark of the TAWS scanner")]
    RectTransform markRight;

    HelicopterSimulation.TawsImageController taws;

    override public void updateRender()
    {
      // get azimuth
      float azimuth = taws.getAzimuth();
      bool isActive = taws.isTawsOn();

      // show marks only when TAWS is active
      markLeft.gameObject.SetActive(isActive);
      markRight.gameObject.SetActive(isActive);

      // Rotate marks
      markLeft.localEulerAngles = new Vector3(0, 0, -azimuth);
      markRight.localEulerAngles = new Vector3(0, 0, azimuth);
    }

    override public void setHcSetup(HelicopterSetup iHcSetup)
    {
      // find required components
      taws = iHcSetup.gameObject.GetComponent<HelicopterSimulation.TawsImageController>();
    }
  }
}