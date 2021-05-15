
using UnityEngine;
using VRC.SDKBase;
using UnityEngine.UI;

#if UDON
using UdonSharp;
using VRC.Udon;

public class GlobalToggle : UdonSharpBehaviour
{
    [UdonSynced]
    private bool syncedValue;
    private bool localValue;
    private bool deserializing;
    private Toggle toggle;
    private VRCPlayerApi localPlayer;

    private void Start()
    {
        toggle = transform.GetComponent<Toggle>();
        localPlayer = Networking.LocalPlayer;
        syncedValue = localValue = toggle.isOn;
        deserializing = false;
    }

    public override void OnDeserialization()
    {
        deserializing = true;
        if(!Networking.IsOwner(gameObject))
        {
            toggle.isOn = syncedValue;
        }
        deserializing = false;
    }

    public override void OnPreSerialization()
    {
        syncedValue = localValue;
    }

    public void ToggleUpdate()
    {
        if (!Networking.IsOwner(gameObject) && !deserializing) 
            Networking.SetOwner(localPlayer, gameObject);
        localValue = syncedValue = toggle.isOn;
    }
}

#else
public class GlobalToggle : MonoBehaviour { }
#endif