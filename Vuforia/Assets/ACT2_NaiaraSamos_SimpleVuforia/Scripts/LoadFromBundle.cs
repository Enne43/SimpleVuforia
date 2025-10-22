using System.Collections;
using System.IO;
using UnityEngine;

public class LoadFromBundle : MonoBehaviour
{
    [SerializeField] Transform modelParent;

    private void Start()
    {
        StartCoroutine(LoadAllBundles());
    }

    IEnumerator LoadAllBundles()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "Bundles");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            Debug.Log($"[BundleLoader] Carpeta creada: {folderPath}");
            yield break;
        }

        string[] bundleFiles = Directory.GetFiles(folderPath, "*.bundle");
        Debug.Log($"[BundleLoader] Bundles encontrados: {bundleFiles.Length}");

        foreach (string file in bundleFiles)
        {
            var bundleLoadRequest = AssetBundle.LoadFromFileAsync(file);
            yield return bundleLoadRequest;

            AssetBundle bundle = bundleLoadRequest.assetBundle;
            if (bundle == null)
            {
                Debug.LogError($"[BundleLoader] No se pudo cargar: {file}");
                continue;
            }

            var loadAssets = bundle.LoadAllAssetsAsync<GameObject>();
            yield return loadAssets;

            foreach (var asset in loadAssets.allAssets)
            {
                GameObject model = Instantiate((GameObject)asset, modelParent);
                model.SetActive(false);

                AddModels.instance.models.Add(model);
                Debug.Log($"[BundleLoader] Modelo agregado: {asset.name}");
            }

            bundle.Unload(false);
        }
    }
}