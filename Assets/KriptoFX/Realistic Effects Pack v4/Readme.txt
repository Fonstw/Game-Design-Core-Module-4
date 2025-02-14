My email is "kripto289@gmail.com"
You can contact me for any questions.

My English is not very good, and if there are any translation errors, you can let me know :)


Pack includes prefabs of main effects (projectiles, aoe effect, etc) + collision effects (decals, particles, etc) + hand effects (like a particles attached to hands)

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Supported platforms:

	All platforms (PC/Consoles/VR/Mobiles)
	All effects tested on Oculus Rift CV1 with single/dual/instanced pass and works correcrly.
	Supported SRP rendering. LightWeight render pipeline (LWRP) and HighDefinition render pipeline (HDRP)

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Using on PC:
	If you want to use posteffect for PC like in the demo video:

	1) Download unity free posteffects
	https://assetstore.unity.com/packages/essentials/post-processing-stack-83912
	2) Add "PostProcessingBehaviour.cs" on main Camera.
	3) Set the "PostEffects" profile. ("\Assets\KriptoFX\Realistic Effects Pack v4\ImagePostEffects\PostEffectsProfile.asset")
	4) You should turn on "HDR" on main camera for correct posteffects. (bloom posteffect works correctly only with HDR)
	If you have forward rendering path (by default in Unity), you need disable antialiasing "edit->project settings->quality->antialiasing"
	or turn of "MSAA" on main camera, because HDR does not works with msaa. If you want to use HDR and MSAA then use "MSAA of post effect".
	It's faster then default MSAA and have the same quality.

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Using on MOBILES:
	For correct work on mobiles in your project scene you need:

	1) Add script "RFX4_MobileBloom.cs" on main camera and disable HDR on main camera. That all :)
	You need disable HDR on main camera for avoid rendering bug on unity 2018+ (maybe later it will be fixed by unity).

	This script will render scene to renderTexture with HDR format and use it for postprocessing.
	It's faster then default any posteffects, because it's avoid "OnRenderImage" and overhead on cpu readback.
	(a source https://forum.unity.com/threads/post-process-mobile-performance-alternatives-to-graphics-blit-onrenderimage.414399/#post-2759255)
	Also, I use RenderTextureFormat.RGB111110Float for mobile rendering and it have the same overhead like a default texture (RGBA32)


------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Using effects:

Simple using (without characters):

	1) Just drag and drop prefab of effect on scene and use that (for example, bufs or projectiles).

Using with characters and animations:

	You can see this video tutorial https://youtu.be/AKQCNGEeAaE

	1) You can use "animation events" for instantiating an effects in runtime using an animation. (I use this method in the demo scene)
	https://docs.unity3d.com/Manual/animeditor-AnimationEvents.html
	2) You need set the position and the rotation for an effects. I use hand bone position (or center position of arrow) and hand bone rotation.

For using effects in runtime, use follow code:

	"Instantiate(prefabEffect, position, rotation);"

Using projectile collision detection:

	Just add follow script on prefab of effect.

	void Start () {
        var physicsMotion = GetComponentInChildren<RFX4_PhysicsMotion>(true);
        if (physicsMotion != null) physicsMotion.CollisionEnter += CollisionEnter;

	    var raycastCollision = GetComponentInChildren<RFX4_RaycastCollision>(true);
        if(raycastCollision != null) raycastCollision.CollisionEnter += CollisionEnter;
    }

    private void CollisionEnter(object sender, RFX4_PhysicsMotion.RFX4_CollisionInfo e)
    {
        Debug.Log(e.HitPoint); //a collision coordinates in world space
        Debug.Log(e.HitGameObject.name); //a collided gameobject
        Debug.Log(e.HitCollider.name); //a collided collider :)
    }

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Effect modification:

All prefabs of effect have "EffectSetting" script with follow settings:

ParticlesBudget (range 0 - 1, default 1)
Allow change particles count of effect prefab. For example, particleBudget = 0.5 will reduce the number of particles in half

UseLightShadows (does not work when used mobile build target)
Some effect can use shadows and you can disable this setting for optimisation. Disabled by default for mobiles.

UseFastFlatDecalsForMobiles (works only when used mobile build target)
If you use non-flat surfaces or  have z-fight problems you can use screen space decals instead of simple quad decals.
Disabled parameter will use screen space decals but it required depth texture!

UseCustomColor
You can override color of effect by HUE. (new color will used only in play mode)
If you want use black/white colors for effect, you need manualy change materials of effects.

IsVisible
Disable this parameter in runtime will smoothly turn off an effect.

FadeoutTime
Smooth turn off time


Follow physics settings visible only if type of effect is projectile

UseCollisionDetection
You can disable collision detection and an effect will fly through the obstacles.

LimitMaxDistance
Limiting the flight of effect (at the end the effect will just disappear)

Follow settings like in the rigidbody physics
Mass
Speed
AirDrag
UseGravity
