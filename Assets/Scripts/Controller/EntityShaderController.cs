using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public class EntityShaderController : MonoBehaviour
    {
        [SerializeField] private Material defaultMaterial;
        [SerializeField] float disolveSpeed = 1;
        [SerializeField] Color disolveColor = Color.red;   
        
        private Shader _defaultShader;
        private Shader _disolveShader;
        private string mainBodyMaterialName;
        private List<Material> mainBodyMaterials = new List<Material>();

        private void Awake()
        {
            mainBodyMaterialName = defaultMaterial.name;
            // Find all child renderers
            Renderer[] renderers = GetComponentsInChildren<Renderer>();

            foreach (Renderer rend in renderers)
            {
                Material[] mats = rend.materials; // Get all materials (instantiates automatically)
                bool changed = false;

                for (int i = 0; i < mats.Length; i++)
                {
                    if (mats[i].name.Contains(mainBodyMaterialName))
                    {
                        // Only store the instantiated material for later shader change
                        mainBodyMaterials.Add(mats[i]);
                    }
                }
            }
        }

        private void Start()
        {
            _defaultShader = Shader.Find("Standard");
            _disolveShader = Shader.Find("Ultimate 10+ Shaders/Dissolve");
            defaultMaterial.shader = _defaultShader;
        }
        
        public void ChangeMainBodyShader()
        {
            if (_disolveShader == null)
            {
                Debug.LogWarning("Hit shader not assigned!");
                return;
            }

            foreach (Material mat in mainBodyMaterials)
            {
                mat.shader = _disolveShader;
                mat.SetColor("_EdgeColor", disolveColor);
            }
        }
        
        public IEnumerator ExecuteDisolveEffect()
        {
            ChangeMainBodyShader();
            float disolveEffect = 0;
            while (disolveEffect < 1)
            {
                disolveEffect += disolveSpeed * Time.deltaTime;
                foreach (var mat in mainBodyMaterials)
                {
                    mat.SetFloat("_Cutoff", disolveEffect);
                }
                yield return null;
            }
            yield return null;
        }

    }
}