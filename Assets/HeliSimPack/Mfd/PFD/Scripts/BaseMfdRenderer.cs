using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeliSimPack.MFD
{
  // Base class for rendering MFD
  public class BaseMfdRenderer : MonoBehaviour
  {

    protected RectTransform rectT;

    protected HelicopterSetup hcSetup;

    [SerializeField]
    protected Camera cam;

    // Use this for initialization
    void Start()
    {
      rectT = GetComponent<RectTransform>();
      cam.enabled = false;
      initialize();
    }

    virtual public void render()
    {
      updateRender();
      cam.enabled = true;
      cam.Render();
      cam.enabled = false;
    }

    virtual public void updateRender()
    {
      // for derrived classes
    }

    virtual public void initialize()
    {
      // for derrived classes
    }

    virtual public void setHcSetup(HelicopterSetup iHcSetup)
    {
      hcSetup = iHcSetup;
    }
  }
}
