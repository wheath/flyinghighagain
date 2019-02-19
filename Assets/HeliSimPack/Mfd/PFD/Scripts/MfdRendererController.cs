using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeliSimPack.MFD
{
  public class MfdRendererController : MonoBehaviour
  {

    [SerializeField]
    [Tooltip("Target refresh rate (In Hz). Not guaranteed")]
    float refreshRateTarget;

    float targetPeriod;
    float lastUpdated = 0;
    float deltaBetweenUpdates = 0;

    BaseMfdRenderer[] renderers;
    int toBeUpdatedRenderer = 0;
    int numberOfRenderersToUpdate = 1;

    HelicopterSetup hcSetup;

    // Use this for initialization
    void Start()
    {
      // Find the helicopter object
      if (null == hcSetup)
      {
        hcSetup = FindObjectOfType<HelicopterSetup>();
      }

      // Find all children renderers
      renderers = GetComponentsInChildren<BaseMfdRenderer>();

      // Calculate target period in seconds
      if (!Mathf.Approximately(0.0f, refreshRateTarget))
      {
        targetPeriod = 1 / refreshRateTarget;
      }
      else
      {
        // Use a target reresh rate of 20 Hz by default
        targetPeriod = 1.0f / 20.0f;
      }

      // initialize children
      foreach (BaseMfdRenderer r in renderers)
      {
        r.setHcSetup(hcSetup);
      }
    }

    void Update()
    {
      // update only "numberOfRenderersToUpdate" rendrers
      for (int i = 0; i < numberOfRenderersToUpdate; ++i)
      {
        renderers[toBeUpdatedRenderer].render();

        toBeUpdatedRenderer++;

        if (toBeUpdatedRenderer >= renderers.Length)
        {
          toBeUpdatedRenderer = 0;
          deltaBetweenUpdates = Time.time - lastUpdated;
          lastUpdated = Time.time;
        }
      }

      // Increase number of renderers to update per iteration if we are too slow
      if (deltaBetweenUpdates > targetPeriod * 1.1f)
      {
        numberOfRenderersToUpdate++;
      }
      // Decrease number of renderers to update per iteration if we are too fast
      else if (deltaBetweenUpdates < targetPeriod * 0.9f)
      {
        numberOfRenderersToUpdate--;

        if (0 >= numberOfRenderersToUpdate)
        {
          numberOfRenderersToUpdate = 1;
        }
      }
    }
  }
}