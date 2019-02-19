*****************************************************************************************************
*****************************************************************************************************
**                        __  __     ___ _____ _           ____             __                     **
**                       / / / /__  / (_) ___/(_)___ ___  / __ \____ ______/ /__                   **
**                      / /_/ / _ \/ / /\__ \/ / __ `__ \/ /_/ / __ `/ ___/ //_/                   **
**                     / __  /  __/ / /___/ / / / / / / / ____/ /_/ / /__/ ,<                      **
**                    /_/ /_/\___/_/_//____/_/_/ /_/ /_/_/    \__,_/\___/_/|_|                     **
**                                                                                                 **
*****************************************************************************************************
*****************************************************************************************************

***********************************************

Release notes
=============

v1.1
====
- Nicer PFD and ND
- TAWS terrain overlay added on PFD/ND rose

v1.0
====
- Initial release
- Helicopter physics simulation
- Basic MFDs provided

***********************************************

Thank you for downloding HeliSimPack!

This asset is meant to facilitate the simulation of a helicopter inside Unity. You'll need 4 input
axes to fly a helicopter as there are 4 flight controls axes. This is almost impossible with a
keyboard so a gamepad with 2 analog joysticks is highly recommended. Understanding helicopter
flight controls is important.

This guide explain how to use the asset. You are also encouraged to look at the demo scene that is
provided with the asset.

NOTE : There is a helicopter 3D model included in the asset for the demo scene. This model was made
       so the demo scene is more interesting but is not meant to be used in a game. You are free to
       use it if you wish to but be aware that no optimisation was done on this model. 

=================
|| Quick Setup ||
=================

You can use the default helicopter simulation very quickly by attaching the script HelicopterSetup.cs
to any game object. This script will automatically add the required scripts and voilà! You have an
object that will behave like a helicopter. However please note :

- By default, the script will look for the following axes from the input manager. Make sure you
  define them.
    - "Collective" for the collective lever
    - "Cyclic X" for the cyclic stick X-axis
    - "Cyclic Y" for the cyclic stick Y-axis
    - "Pedals" for the anti-torque pedals axis

- There will be no sound by default. You can still easily add them. The script will create a sound
  controller that looks for objects of type <EngineSound> and <RotorSound> as children of the main
  helicopter object. For example, for the rotor sound, all you need to do is add a chlid game object
  that has an audio source component and a EngineSound component and the script will find it
  automatically. The same applies to the engine sound.

- If you want your rotors to rotate, drag them in the RotorRotator component (automatically created)
  in the "Main Rotor" and "Tail Rotor" fields.

Note : it is assumed that the range of the input axes is [-1, 1] with 0 being the neutral position.

Basic MFDs are also provided. Once you have your helicopter with the Helicopter Setup component, add
the prefab MFD_UI to your scene and the MFD will automatically find the helicopter and show its
information. You can of course modify these MFDs.


============================
|| Tuning your helicopter ||
============================

You can simulate different helicopters by tuning the parameters from the different scripts. This
section describes each script and its parameters.

RigidBodyController
===================
This script is responsible of overriding Unity's Rigidbody parameters including its mass and center
of mass. It also applies a more physically accurate drag.

[Empty Weight] (float)
Weight of the helicopter body alone (not including fuel, cargo or crew). In pounds.

[Drag Info Front] (float, float)
The drag information in the Z axis of the helicopter composed of the drag coefficient and the
section area (in meters square)

[Drag Info Side] (float, float)
The drag information in the X axis of the helicopter composed of the drag coefficient and the
section area (in meters square)

[Drag Info Top] (float, float)
The drag information in the Y axis of the helicopter composed of the drag coefficient and the
section area (in meters square)

[Angular Drag] (float)
Same as Unity's Rigidbody property

[Inertia Tensor] (Vector3)
Inertia tensor of the helicopter.

[Inertia Tensor Rotation] (Quaternion)
Rotation of the inertia tensor of the helicopter.

[Interpolation] (RigidbodyInterpolation)
Same as Unity's Rigidbody Interpolation parameter

[Collision Detection] (CollisionDetectionMode)
Same as Unity's Rigidbody Collisision Detection parameter.

[Empty Center Of Mass] (Transform)
Position of the center of mass of the Helicopter only (without fuel, pilots or cargo) (used to
compute the actual center of mass)

[Pilot Position] (Transform)
Position of pilot (used to compute the actual center of mass)

[Pilot Weight] (float)
Weight of the pilot in pounds. (used to compute the actual center of mass)

[Copilot Position] (Transform)
Position of copilot (used to compute the actual center of mass)

[Copilot Weight] (float)
Weight of the copilot in pounds. (used to compute the actual center of mass)

[Cargo position] (Transform)
Position of cargo (used to compute the actual center of mass)

[Cargo Weight] (float)
Weight of the cargo in pounds. (used to compute the actual center of mass)


FuelController
==============
This script is used for fuel consumption, weight and center of mass computation. If no fuel tank is
defined, infinite fuel will be used.

[Fuel Tanks] (fuelTank)
An array of fuel tanks. Each composed of :

    [Fuel Quantity] (float)
    The actual quantity of fuel in the tank (in pounds)

    [Fuel Capacity] (float)
    The maximum capacity of the fuel tank (in pounds)

    [Fuel Tank Position] (Transform)
    Position of the fuel tank center of mass (used to compute the actual center of mass of
    the helicopter)

[Burn Rate]
The burn rate of fuel when engine are running (in pounds per hour)


EngineController
================
This script is responsible of the engine simulation. Some public functions are also availble and
described below.

[Engine Speed Damping] (float)
Factor used to filter the engine speed.

[Engine Running At Load] (bool)
Set to true if you want to avoid needing to start the engine in game.

[Time For Full Engine Speed] (float)
The time in seconds it takes for the engine to reach its nominal speed from stop.

bool isEngineRunning()
Returns true if the engine is running.

void setEngineRunning(bool iRunning)
Allows to change instantly if engine is running or not.

void startEngine()
Starts the engine (taking time to reach full speed)

void stopEngine()
Stops the engine (taking time to reach a full stop)

float getEngineSpeed()
Returns the speed of the engine in percent.


RotorRotator
============
This script will rotate the main and tail rotors according to engine speed. A filter is also
applied so transition between different speeds is smooth.

[Main Rotor] (Transform)
The main rotor transform that will be rotated.

[Main Rotor Rotation Axis] (axes)
The reference axis around which the main rotor transform will be rotated.

[Hundred Percent Main Rotor Speed] (float)
The speed of the main rotor at full speed (in degrees per second)

[Tail Rotor] (Transform)
The tail rotor transform that will be rotated.

[Tail Rotor Rotation Axis] (axes)
The reference axis around which the tail rotor transform will be rotated.

[Hundred Percent Tail Rotor Speed] (float)
The speed of the tail rotor at full speed (in degrees per second)
  
[Rotor Speed Increasing Damping] (float)
Filtering factor for the increasing rotor speed

[Rotor Speed Decreasing Damping] (float)
Filtering factor for the decreasing rotor speed


FlightControlsPositionController
================================
This script moves the flight controls in the cockpit according to the input axes values.

[Left Pedals] (Transform)
An array of Transforms for the left pedals rotation. The Transforms will be rotated around the 
local X axis. Pedals moving forward is a positive rotation.

[Right Pedals] (Transform)
An array of Transforms for the right pedals rotation. The Transforms will be rotated around the
local X axis. Pedals moving forward is a positive rotation.

[Cyclic Sticks] (Transform)
Am array of Transforms for the cyclic sticks rotation. Moving the sticks forward will be shown as
a positive rotation around the local X axis. Moving the sticks to the left will be shown as a
positive rotation around the local Z axis.

[Collective Levers] (Transform)
An array of Transforms for the collective levers rotation. The Transforms will be rotated around
the local X axis. Levers moving upward is a positive rotation.

[Maximum Cyclic X Rotation] (float)
Maximum rotation of Cyclic in X in both directions (in degrees)

[Maximum Cyclic Y Rotation] (float)
Maximum rotation of Cyclic in Y in both directions (in degrees)

[Maximum Collective Rotation] (float)
Maximum rotation of Collective lever in both directions (in degrees)

[Maximum Pedals Rotation] (float)
Maximum rotation of Pedals in both directions (in degrees)

[Collective Axis] (string)
The axis from the Input Manager that is used for the collective lever position

[Cyclic X Axis] (string)
The axis from the Input Manager that is used for the cyclic stick X position

[Cyclic Y Axis] (string)
The axis from the Input Manager that is used for the cyclic stick Y position

[Pedals Axis] (string)
The axis from the Input Manager that is used for the pedals position


FlightControlsPhysicsController
===============================
This script applies the flight controls forces to the helicopter body.

[Main Rotor Transform] (Transform)  
The Transform of the main rotor. This is used to define where the forces from the collective lever
and from the cyclic stick are applied.

[Main Rotor Diameter] (float)
Diameter of the main rotor (in meters). Thi is used to define where the forces from the cyclic
stick are applied.

[Tail Rotor Transform] (Transform)
The Transform of the tail rotor. This is used to define where the forces from the pedals are applied.

[Lift Cyclic Percentage] (float)
Percentge of lift that is also used to pitch and roll the helicopter. A value of zero would mean it
is impossible to change the helicopter attitude and a value of 100 would make the cyclic stick the
most sensitive possible.

[Minimum Lift] (float)
The lift (in Newtons) given by the main rotor when the collective lever is at its minimum position.

[Neutral Lift] (float)
The lift (in Newtons) given by the main rotor when the collective lever is at its neutral position.

[Maximum Lift] (float)
The lift (in Newtons) given by the main rotor when the collective lever is at its maximum position.

[Collective Damping] (float)
Factor for a filter applied to the collective lever so the helicopter does not react instantly to
a collective input.

[Maximum Tail Rotor Force] (float)
Maximum force (in Newtons) given by the tail rotor.

[Rotor Torque Simulated] (bool)
When TRUE, the torque from the main rotor rotation is simulated.

[Max Rotor Torque] (float)
Used only if the rotor torque is simulated. Maximum torque (in Newtons-Meters) caused by the main
rotor rotation.

[Pendulum Effect Factor] (float)
How strong the pendulum effect is. A value of 0 means the helicopter will not come by itself to
horizontal attitude if cyclic stick is at its neutral position in flight. A value of 1 means it is
difficult to change the helicopter's attitude.

[Rotor Speed Change Factor] (float)
How much the collective pitch affects the rotor speed because of air resistance and change in power
required. For a real collective lever this parameter would be 1. For a gamepad joystick 0.2 gives
realistic results.

[Rotor Speed Damping] (float)
Factor for filter applied to the rotor speed change.

[Ground Effect Parameters] (GroundEffectParameters)
Parameters for ground effect where A and B are the paramters in the equation LiftGain = A * exp(B * h) and h is the height of the main rotor above ground in units of main rotor diameters.

[Terrain Layers] (LayerMask)
Layers for terrain collision check (excluding player layer) used for ground effect.

[Vrs Parameters] (VortexRingStatePerameters)
Paramters for Vortex Ring State simulation. Where :
  [Vertical Speed] is the vertical speed (in feet/min) at which the H/C will enter Vortex Ring State if its horizontal speed is below [Horizontal Speed]
  [Horizontal Speed] is the horizontal speed (in Knots) at which the H/C will enter Vortex Ring State if its vertical speed is below [Vertical Speed]
  [E2] is the efficiency at 2 * [Vertical Speed]
  [E4] is the efficiency at  * [Vertical Speed]


SoundController
===============
This script controls the sound of the engine and the sound of the main rotor according to engine
speed and rotor speed. There is no parameter. The script looks for objects of type <EngineSound> and
<RotorSound> as children of the main helicopter object. These objects are expected to each have an
Audio Source component.


FlightPlanController
====================
This script will automatically find the flight plan in the scene (object of type "FlightPlan") and
will assume all Transforms children of the FlightPlan are waypoints. It will keep track of the
helicopter progress on flight plan.

[Distance To Reach Wayoint] (float)
In Nautical Miles. When active waypoint (magenta) gets closer than this distance, the next waypoint becomes the active waypoint.


MfdRangeController
==================
This script updates the range displayed on the MFD.

[Available Ranges] (float [])
In Nautical Miles. All available ranges from smallest to biggest

[Default Range] (float)
In Nautical Miles

[Increase Range Key] (Keycode)
Key used to increase displayed range

[Decrease Range Key] (Keycode)
Key used to decrease displayed range


TawsImageController
===================
This script generates a terrain overlay to be displayed on the PFD/ND rose.

[High Density Red] (Color)
Color representing the highest terrain on TAWS image

[High Density Yellow] (Color)
Color representing the second highest terrain on TAWS image

[Low Density Yellow] (Color)
Color representing the third highest terrain on TAWS image

[High Density Green] (Color)
Color representing the fourth highest terrain on TAWS image

[Low Density Green] (Color)
Color representing the fifth highest terrain on TAWS image

[Water Blue] (Color)
Color used to show water or terrain with altitude of 0 if property "Is Water Shown Blue" is true

[High Density Red Lower Boundary] (float)
Relative altitude in feet from which terrain will be shown in "High Density Red" color

[High Density Yellow Lower Boundary] (float)
Relative altitude in feet from which terrain will be shown in "High Density Yellow" color

[Low Density Yellow Lower Boundary] (float)
Relative altitude in feet from which terrain will be shown in "Low Density Yellow" color

[High Density Green Lower Boundary} (float)
Relative altitude in feet from which terrain will be shown in "High Density Green" color

[Low Density Green Lower Boundary] (float)
Relative altitude in feet from which terrain will be shown in "Low Density Green" color

[Is Water Shown Blue] (bool)
If true, water and terrain at altitude 0 will be shown in "Water Blue" color

[Refresh Period] (float)
Time it takes in seconds to refresh the whole image

[Radius In Pixels] (int)
Height of the image in pixels. Lower number will show bigger pixels

[Filter Mode] (FilterMode)
How blurry will the image be

[Maximum Terrain Height] (float)
Set this property to highest the terrain could be (in feet). For performance keep this number as low as possible

[Terrain Layers] (LayerMask)
Layers composing the terrain. Only these layers will be used to generate the TAWS image.


WeightOnWheelsController
========================
This script checks if helicopter is on ground

[Points Of Contact] (pointOfContact)
All points of contact with the ground to be checked. Position is relative to main object's transfrom.

[Layers To Checks] (LayerMask)
Layers to check for collisions with.


============================
|| Questions and comments ||
============================

For any question or comment : alx.viau@gmail.com



*****************************************************************************************************
*****************************************************************************************************
**                                                                                                 **  
**                _/_/    _/                          _/      _/  _/                               **  
**             _/    _/  _/    _/_/    _/    _/      _/      _/        _/_/_/  _/    _/            **  
**            _/_/_/_/  _/  _/_/_/_/    _/_/        _/      _/  _/  _/    _/  _/    _/             **  
**           _/    _/  _/  _/        _/    _/        _/  _/    _/  _/    _/  _/    _/              **  
**          _/    _/  _/    _/_/_/  _/    _/          _/      _/    _/_/_/    _/_/_/               **  
**                                                                                                 **
*****************************************************************************************************
*****************************************************************************************************
                                                      





















