using UnityEngine;

namespace Player
{
    public class PlayerVisualController : MonoBehaviour
    {
        public Material playerMaterial;

        public void ActivateDamageShader()
        {
            playerMaterial.shader = Shader.Find("Ultimate 10+ Shaders/Force Field");
            playerMaterial.color = Color.red;
        }

        public void ActivateDefaultShader()
        {
            playerMaterial.color = Color.white;
            playerMaterial.shader = Shader.Find("Standard");
        }
    }
}
