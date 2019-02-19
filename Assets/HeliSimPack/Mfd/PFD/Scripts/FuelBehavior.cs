using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HeliSimPack.MFD
{
  public class FuelBehavior : BaseMfdRenderer
  {
    [SerializeField]
    [Tooltip("Tape showing remaining fuel quantity")]
    Image tapeImage;

    [SerializeField]
    [Tooltip("Default translation of tape when remaining fuel is zero")]
    float positionAtZero;

    [SerializeField]
    [Tooltip("Translation of tape when remaining fuel is 100 percent of tank capcity")]
    float positionAtHundred;

    HelicopterSimulation.FuelController fuelController;

    [SerializeField]
    [Tooltip("Readout showing remaining fuel quantity in pouds")]
    Text readout;

    [SerializeField]
    [Tooltip("Fuel quantity (in Hours) for which the tape turns yellow")]
    float remainingTimeYellow = 1;

    [SerializeField]
    [Tooltip("Fuel quantity (in Hours) for which the tape turns red")]
    float remainingTimeRed = 0.5f;

    override public void updateRender()
    {
      if (null != fuelController)
      {
        // retrieve fuel quantity and capacity
        float fuelRemaining = fuelController.totalFuelRemaining();
        float fuelCapacity = fuelController.totalFuelCapacity();

        // compute remaining time of fuel for tape color
        float remainingTime = fuelRemaining / fuelController.burnRate;

        // compute tape's translation
        float percent = fuelRemaining / fuelCapacity * 100.0f;
        float translation = positionAtZero + (positionAtHundred - positionAtZero) * percent / 100.0f;

        // apply tape translation
        rectT.anchoredPosition3D = new Vector3(rectT.anchoredPosition3D.x, translation, rectT.anchoredPosition3D.z);

        // update readout
        readout.text = ((int)(fuelRemaining)).ToString();

        // set tape color
        if (remainingTime > remainingTimeYellow)
        {
          readout.color = Color.green;
          tapeImage.color = Color.green;
        }
        else if (remainingTime > remainingTimeRed)
        {
          readout.color = Color.yellow;
          tapeImage.color = Color.yellow;
        }
        else
        {
          readout.color = Color.red;
          tapeImage.color = Color.red;
        }
      }
    }

    override public void setHcSetup(HelicopterSetup iHcSetup)
    {
      // find required components
      fuelController = iHcSetup.gameObject.GetComponent<HelicopterSimulation.FuelController>();
    }
  }
}