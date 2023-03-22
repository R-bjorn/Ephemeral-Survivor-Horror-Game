using System.IO;
using System.Linq;
using System.Text;
using Packages.StarterAssets.InputSystem;
using StarterAssets;
using UnityEditor;
using UnityEngine;

namespace Packages.StarterAssets.Editor
{
    public partial class StarterAssetsDeployMenu : ScriptableObject
    {
        // prefab paths
        private const string PlayerArmaturePrefabName = "PlayerArmature";

#if STARTER_ASSETS_PACKAGES_CHECKED
        /// <summary>
        /// Check the Armature, main camera, cinemachine virtual camera, camera target and references
        /// </summary>
        [MenuItem(global::StarterAssets.StarterAssetsDeployMenu.MenuRoot + "/Reset Third Person Controller Armature", false)]
        static void ResetThirdPersonControllerArmature()
        {
            var thirdPersonControllers = FindObjectsOfType<ThirdPersonController.Scripts.ThirdPersonController>();
            var player = thirdPersonControllers.FirstOrDefault(controller =>
                controller.GetComponent<Animator>() && controller.CompareTag(global::StarterAssets.StarterAssetsDeployMenu.PlayerTag));

            GameObject playerGameObject = null;

            // player
            if (player == null)
            {
                if (global::StarterAssets.StarterAssetsDeployMenu.TryLocatePrefab(PlayerArmaturePrefabName, null, new[] { typeof(ThirdPersonController.Scripts.ThirdPersonController), typeof(StarterAssetsInputs) }, out GameObject prefab, out string _))
                {
                    global::StarterAssets.StarterAssetsDeployMenu.HandleInstantiatingPrefab(prefab, out playerGameObject);
                }
                else
                {
                    Debug.LogError("Couldn't find player armature prefab");
                }
            }
            else
            {
                playerGameObject = player.gameObject;
            }

            if (playerGameObject != null)
            {
                // cameras
                global::StarterAssets.StarterAssetsDeployMenu.CheckCameras(playerGameObject.transform, GetThirdPersonPrefabPath());
            }
        }

        [MenuItem(global::StarterAssets.StarterAssetsDeployMenu.MenuRoot + "/Reset Third Person Controller Capsule", false)]
        static void ResetThirdPersonControllerCapsule()
        {
            var thirdPersonControllers = FindObjectsOfType<ThirdPersonController.Scripts.ThirdPersonController>();
            var player = thirdPersonControllers.FirstOrDefault(controller =>
                !controller.GetComponent<Animator>() && controller.CompareTag(global::StarterAssets.StarterAssetsDeployMenu.PlayerTag));

            GameObject playerGameObject = null;

            // player
            if (player == null)
            {
                if (global::StarterAssets.StarterAssetsDeployMenu.TryLocatePrefab(global::StarterAssets.StarterAssetsDeployMenu.PlayerCapsulePrefabName, null, new[] { typeof(ThirdPersonController.Scripts.ThirdPersonController), typeof(StarterAssetsInputs) }, out GameObject prefab, out string _))
                {
                    global::StarterAssets.StarterAssetsDeployMenu.HandleInstantiatingPrefab(prefab, out playerGameObject);
                }
                else
                {
                    Debug.LogError("Couldn't find player capsule prefab");
                }
            }
            else
            {
                playerGameObject = player.gameObject;
            }

            if (playerGameObject != null)
            {
                // cameras
                global::StarterAssets.StarterAssetsDeployMenu.CheckCameras(playerGameObject.transform, GetThirdPersonPrefabPath());
            }
        }

        static string GetThirdPersonPrefabPath()
        {
            if (global::StarterAssets.StarterAssetsDeployMenu.TryLocatePrefab(PlayerArmaturePrefabName, null, new[] { typeof(ThirdPersonController.Scripts.ThirdPersonController), typeof(StarterAssetsInputs) }, out GameObject _, out string prefabPath))
            {
                var pathString = new StringBuilder();
                var currentDirectory = new FileInfo(prefabPath).Directory;
                while (currentDirectory.Name != "Assets")
                {
                    pathString.Insert(0, $"/{currentDirectory.Name}");
                    currentDirectory = currentDirectory.Parent;
                }

                pathString.Insert(0, currentDirectory.Name);
                return pathString.ToString();
            }

            return null;
        }
#endif
    }
}