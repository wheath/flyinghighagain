using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HeliSimPack.MFD
{
  public class RadioAltimeterBehavior : BaseMfdRenderer
  {
    Transform helicopterTranform;
    float actualRadAlt;
    float filteredRadAlt;

    [SerializeField]
    [Tooltip("The gameobject containing the radalt displayed. Will be hidden when radalt is above 2500 feet")]
    GameObject RadAltObject;

    [SerializeField]
    [Tooltip("Intrument height relative to helicopte's rigidbody position (in feet)")]
    float calibration;

    [SerializeField]
    [Tooltip("Readout for radio altitude")]
    Text readout;

    [SerializeField]
    [Tooltip("Filter damping")]
    float damping = 0.1f;

    private void FixedUpdate()
    {
      // Get actual radalt
      RaycastHit hit;
      if (Physics.Raycast(helicopterTranform.position, new Vector3(0, -1, 0), out hit, 800)) // 2500 ft is 762 meters, let's have some margin
      {
        actualRadAlt = hit.distance * 3.28084f + calibration;
      }

      // Filter radalt
      filteredRadAlt = Mathf.Lerp(filteredRadAlt, actualRadAlt, damping);
    }

    override public void updateRender()
    {
      // Show radalt only when below 2500 feet. Above that the readings of radio altitude is considered invalid
      RadAltObject.SetActive(2500 > filteredRadAlt);

      // Update readout
      readout.text = ((int)filteredRadAlt).ToString();
    }

    override public void setHcSetup(HelicopterSetup iHcSetup)
    {
      // find required components
      helicopterTranform = iHcSetup.gameObject.GetComponent<Transform>();
    }
  }
}