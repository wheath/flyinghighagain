using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HeliSimPack.MFD
{
  public class FlightPlanDisplay : BaseMfdRenderer
  {
    Transform helicopter;
    HelicopterSimulation.FlightPlanController flightPlanController;
    HelicopterSimulation.MfdRangeController rangeController;
    List<Transform> waypointTransforms; // The actual waypoints in world coordinates

    [SerializeField]
    [Tooltip("The waypoints objects on the flight plan display")]
    RectTransform[] waypointImages;

    [SerializeField]
    [Tooltip("The legs objects on the flight plan display")]
    RectTransform[] legImages;

    [SerializeField]
    [Tooltip("The map object containing the waypoints and legs objects")]
    RectTransform map;

    float range = 20;

    [SerializeField]
    [Tooltip("The radius in pixels of the flight plan image")]
    float rangeInPixels = 500;

    [SerializeField]
    [Tooltip("The line width of the legs")]
    float lineWidth;

    // Use this for initialization
    void Start()
    {
      waypointTransforms = new List<Transform>();
      updateFlightPlan();
    }

    override public void updateRender()
    {
      // Retrieve displayed range
      range = rangeController.getRange(); // in Nautical Miles

      float rangeInMeters = range * 1852.0f;

      // Rotate map by H/C heading
      float mapRotation = helicopter.rotation.eulerAngles.y;
      map.localEulerAngles = new Vector3(0, 0, mapRotation);

      // Update waypoints and legs on map
      int imageIdx = 0;
      for (imageIdx = 0; imageIdx < waypointTransforms.Count; imageIdx++)
      {
        if (imageIdx < waypointImages.Length)
        {
          // WAYPOINT

          // Transform waypoint relative to H/C
          Vector3 relativeWptPos = waypointTransforms[imageIdx].position - helicopter.position;

          // Position waypoint without consideration for heading. The whole map will be rotated later for heading.
          float relativeX = relativeWptPos.x / rangeInMeters * rangeInPixels;
          float relativeY = relativeWptPos.z / rangeInMeters * rangeInPixels;

          waypointImages[imageIdx].localPosition = new Vector3(relativeX, relativeY, 0);
          // apply -heading rotation to keep waypoint images upward on the map after heading rotation is applied
          waypointImages[imageIdx].localEulerAngles = new Vector3(0, 0, -mapRotation);
          waypointImages[imageIdx].gameObject.SetActive(true);

          // Set color of waypoint
          if (imageIdx == flightPlanController.getActiveWaypoint())
          {
            waypointImages[imageIdx].GetComponent<Image>().color = Color.magenta;
          }
          else
          {
            waypointImages[imageIdx].GetComponent<Image>().color = Color.white;
          }

          // LEGS

          // https://answers.unity.com/questions/865927/draw-a-2d-line-in-the-new-ui.html
          if (imageIdx > 0)
          {
            // Find coordinates of previous waypoint
            Vector3 prev_relativeWptPos = waypointTransforms[imageIdx - 1].position - helicopter.position;

            float prev_relativeX = prev_relativeWptPos.x / rangeInMeters * rangeInPixels;
            float prev_relativeY = prev_relativeWptPos.z / rangeInMeters * rangeInPixels;

            Vector3 pointA = new Vector3(prev_relativeX, prev_relativeY, 0);
            Vector3 pointB = new Vector3(relativeX, relativeY, 0);

            // leg vector
            Vector3 differenceVector = pointB - pointA;

            // draw leg (resize rectangular image on map)
            legImages[imageIdx - 1].sizeDelta = new Vector2(differenceVector.magnitude, lineWidth);
            legImages[imageIdx - 1].pivot = new Vector2(0, 0.5f);
            legImages[imageIdx - 1].localPosition = pointA;
            float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg + mapRotation;
            legImages[imageIdx - 1].rotation = Quaternion.Euler(0, 0, angle);

            legImages[imageIdx - 1].gameObject.SetActive(true);

            // set leg color
            if (imageIdx == flightPlanController.getActiveWaypoint())
            {
              legImages[imageIdx - 1].GetComponent<Image>().color = Color.magenta;
            }
            else
            {
              legImages[imageIdx - 1].GetComponent<Image>().color = Color.white;
            }
          }
        }
      }

      // hide remaining waypoints images
      for (int i = imageIdx; i < waypointImages.Length; ++i)
      {
        waypointImages[i].gameObject.SetActive(false);
      }

      // hide remaining legs images
      for (int i = imageIdx - 1; i < legImages.Length; ++i)
      {
        if (i >= 0)
        {
          legImages[i].gameObject.SetActive(false);
        }
      }
    }

    public void updateFlightPlan()
    {
      // Retrieve flight plan in scene
      ObjectIdentifiers.FlightPlan wFlightPlan = FindObjectOfType<ObjectIdentifiers.FlightPlan>();

      if (null != wFlightPlan)
      {
        // Reset out waypoint list
        Transform[] list = wFlightPlan.GetComponentsInChildren<Transform>();
        waypointTransforms.Clear();

        for (int i = 1; i < list.Length; i++)
        {
          waypointTransforms.Add(list[i]);
        }
      }
    }

    override public void setHcSetup(HelicopterSetup iHcSetup)
    {
      // find required components
      helicopter = iHcSetup.gameObject.GetComponent<Transform>();
      flightPlanController = iHcSetup.gameObject.GetComponent<HelicopterSimulation.FlightPlanController>();
      rangeController = iHcSetup.gameObject.GetComponent<HelicopterSimulation.MfdRangeController>();
    }
  }
}
