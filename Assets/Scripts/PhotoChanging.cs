using UnityEngine;
using UnityEngine.UI;

public class PhotoChanging : MonoBehaviour
{
    [HideInInspector] public Texture2D texture2D;

    public void AssignPhoto()
    {
        texture2D = PickImage(100);
    }
    private Texture2D PickImage(int maxSize)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                /*/  Material material = GetComponent<Renderer>().material;
                  if (!material.shader.isSupported) // happens when Standard shader is not included in the build
                      material.shader = Shader.Find("Legacy Shaders/Diffuse");/*/

                // material.mainTexture = texture;

                GetComponent<RawImage>().texture = texture;
                texture2D = texture;
            }
        });

        Debug.Log("Permission result: " + permission);
        return texture2D;
    }

}
