using UnityEngine;
using System.Linq;

public class GlitchModule : MonoBehaviour {
    public KMBombModule BombModule;
    public Glitcher Glitcher;
    public KMSelectable Button;

    public static readonly string[] _ignoredModules = {
        "SouvenirModule",
        "MemoryV2",
        "HexiEvilFMN",
        "TurnTheKey",
        "TurnTheKeyAdvanced",
        "theSwan",
        "GlitchModule"
    };

    private GameObject _glitchyModule;
    private bool _isSolved = false;

    private static int _moduleIDCounter = 1;
    private int _moduleID;

    protected void Start() {
        _moduleID = _moduleIDCounter++;
        BombModule.OnActivate += OnActivate;
        Button.OnInteract += ButtonPress;
        var modules = transform.parent.GetComponentsInChildren<KMBombModule>();
        var allowedModules = modules.Where(x => !_ignoredModules.Contains(x.ModuleType));
        if (allowedModules.Count() == 0) {
            BombModule.HandlePass();
        } else {
            var gm = allowedModules.ToArray()[Random.Range(0, allowedModules.Count())];
            Debug.LogFormat("[Glitch #{0}] Selected Module: {1}", _moduleID, gm.ModuleDisplayName);
            _glitchyModule = gm.gameObject;
            Glitcher.Glitch = _glitchyModule;
        }
    }

    bool ButtonPress() {
        if (!_isSolved) {
            BombModule.HandlePass();
            _isSolved = true;
        }
        Glitcher.ToggleGlitch();
        return false;
    }

    void OnActivate() {
        Glitcher.Init();
    }
}
