using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Glitcher : MonoBehaviour {

    public GameObject Glitch;
    public Material GlitchMaterial;

    private bool _glitchActive = true;
    private GameObject glitchCube;

    public void Init() {
        var old = Glitch.transform.rotation;
        Glitch.transform.rotation = Quaternion.identity;
        Bounds bounds = GetBounds(Glitch);
        glitchCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        var cubeRend = glitchCube.GetComponent<Renderer>();
        if (cubeRend != null) {
            cubeRend.material = GlitchMaterial;
        }
        glitchCube.transform.position = bounds.center;
        glitchCube.transform.localScale = bounds.size;
        glitchCube.transform.localScale += new Vector3(0.006f, 0.006f, 0.006f);
        glitchCube.transform.SetParent(Glitch.transform, true);
        glitchCube.layer = 11;
        Glitch.transform.rotation = old;
    }

    private Bounds GetBounds(GameObject go) {
        Bounds bounds = new Bounds(go.transform.position, Vector3.zero);
        // Renderer Bounds
        var thisCompRend = go.GetComponent<Renderer>();
        if (thisCompRend != null) {
            bounds.Encapsulate(thisCompRend.bounds);
        }
        var allDescendantsRend = go.GetComponentsInChildren<Renderer>();
        foreach (var desc in allDescendantsRend) {
            //if (desc.gameObject.GetComponent<KMHighlightable>() != null) continue;
            //if (desc.gameObject.GetComponentInParent<KMHighlightable>() != null) continue;
            if (desc != null) {
                bounds.Encapsulate(desc.bounds);
            }
        }

        // Collider Bounds
        var thisCompColl = go.GetComponent<Collider>();
        if (thisCompColl != null) {
            bounds.Encapsulate(thisCompColl.bounds);
        }
        var allDescendantsColl = go.GetComponentsInChildren<Collider>();
        foreach (var desc in allDescendantsColl) {
            //if (desc.gameObject.GetComponent<KMHighlightable>() != null) continue;
            //if (desc.gameObject.GetComponentInParent<KMHighlightable>() != null) continue;
            if (desc != null) {
                bounds.Encapsulate(desc.bounds);
            }
        }
        return bounds;
    }

    public void ToggleGlitch() {
        if (!_glitchActive) {
            glitchCube.SetActive(true);
            _glitchActive = true;
        } else {
            glitchCube.SetActive(false);
            _glitchActive = false;
        }
    }

    private void Update() {
        if (Input.GetKeyUp(KeyCode.E)) {
            ToggleGlitch();
        }
    }
}
