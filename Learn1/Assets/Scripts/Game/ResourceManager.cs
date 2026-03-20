using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

public class ResourceManager : MonoBehaviour
{
    #region 单例实现
    public static ResourceManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    private Dictionary<string, Dictionary<string, Object>> resources = new();

    public async void Init()
    {
        await LoadResourcesByLabelAsync<GameObject>("UIPanel");
        await LoadResourcesByLabelAsync<CharacterConfiguration>("CharacterConfiguration");
        await LoadResourcesByLabelAsync<RelicConfiguration>("RelicConfiguration");
    }

    public async Task<T> LoadResourceAsync<T>(string address, string label = "default") where T: Object
    {
        if (!resources.ContainsKey(label))
        {
            resources[label] = new Dictionary<string, Object>();
        }
        if (resources[label].ContainsKey(address))
        {
            return resources[label][address] as T;
        }
        
        var handle = Addressables.LoadAssetAsync<T>(address);
        await handle.Task;
        if(handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            resources[label][address] = handle.Result;
            return handle.Result;
        }
        else
        {
            Debug.LogWarning($"加载资源失败: {label}/{address}");
            return null;
        }
    }

    public async Task<List<T>> LoadResourcesByLabelAsync<T>(string label) where T : Object
    {
        if(!resources.ContainsKey(label))
        {
            resources[label] = new Dictionary<string, Object>();
        }
        IList<IResourceLocation> locations = await Addressables.LoadResourceLocationsAsync(label).Task;

        if (locations == null || locations.Count == 0)
        {
            Debug.LogError($"未找到标签对应的资源: {label}");
            return null;
        }

        List<T> loadedResources = new List<T>();

        foreach (var location in locations)
        {
            string address = location.PrimaryKey;
            if (!resources[label].ContainsKey(address))
            {
                var handle = Addressables.LoadAssetAsync<T>(address);
                await handle.Task;
                if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                {
                    resources[label][address] = handle.Result;
                    loadedResources.Add(handle.Result);
                }
                else
                {
                    Debug.LogError($"加载资源失败: {address}");
                }
            }
            else
            {
                loadedResources.Add(resources[label][address] as T);
            }
        }
        return loadedResources;
    }

    /// <summary>
    /// 释放单个资源
    /// </summary>
    /// <param name="label"></param>
    /// <param name="address"></param>
    public void ReleaseResource(string label, string address)
    {
        if (resources.TryGetValue(label, out var innerDict)
            && innerDict != null
            && innerDict.ContainsKey(address))
        {
            Addressables.Release(innerDict[address]);
            innerDict.Remove(address);
        }
        else
        {
            Debug.LogWarning($"尝试释放未缓存的资源: {label} - {address}");
        }
    }

    /// <summary>
    /// 按标签释放资源分类
    /// </summary>
    /// <param name="label"></param>
    public void ReleaseResourcesByLabel(string label)
    {
        if (resources.TryGetValue(label, out var innerDict)
            && innerDict != null)
        {
            foreach (var resource in innerDict.Values)
            {
                Addressables.Release(resource);
            }
            innerDict.Clear();
        }
        else
        {
            Debug.LogWarning($"尝试释放未缓存的资源分类: {label}");
        }
    }

    /// <summary>
    /// 释放所有资源
    /// </summary>
    public void ReleaseAllResources()
    {
        foreach (var label in resources.Keys)
        {
            ReleaseResourcesByLabel(label);
        }

        resources.Clear();
    }

    /// <summary>
    /// 检查资源是否已加载
    /// </summary>
    /// <param name="label">资源标签</param>
    /// <param name="resourcePath">资源地址</param>
    /// <returns>是否已加载</returns>
    public bool IsResourceLoaded(string label, string resourcePath)
    {
        return resources.ContainsKey(label) && resources[label].ContainsKey(resourcePath);
    }


    /// <summary>
    /// 打印当前所有已加载资源的状态
    /// </summary>
    public void PrintLoadedResourcesStatus()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("--- ResourceManager Status ---");

        if (resources.Count == 0)
        {
            sb.AppendLine("No resources loaded yet.");
        }
        else
        {
            foreach (var labelEntry in resources)
            {
                string label = labelEntry.Key;
                var innerDict = labelEntry.Value;
                sb.AppendLine($"[Label: '{label}'] ({innerDict.Count} assets loaded)");

                foreach (var resourceEntry in innerDict)
                {
                    string address = resourceEntry.Key;
                    Object resource = resourceEntry.Value;
                    sb.AppendLine($"  - Address: {address} | Type: {resource.GetType().Name}");
                }
            }
        }

        sb.AppendLine("------------------------------");
        Debug.Log(sb.ToString());
    }
}
