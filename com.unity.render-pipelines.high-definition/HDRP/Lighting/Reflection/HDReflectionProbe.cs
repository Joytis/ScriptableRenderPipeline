using UnityEngine.Serialization;
using UnityEngine.Experimental.Rendering.HDPipeline;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering
{
    [RequireComponent(typeof(ReflectionProbe), typeof(MeshFilter), typeof(MeshRenderer))]
    public class HDReflectionProbe : HDProbe
    {
        [HideInInspector]
        public float version = 1.2f;

        ReflectionProbe m_LegacyProbe;
        ReflectionProbe legacyProbe { get { return m_LegacyProbe ?? (m_LegacyProbe = GetComponent<ReflectionProbe>()); } }

        //data only keeped for migration
        [SerializeField]
        Shape influenceShape;
        [SerializeField]
        float influenceSphereRadius = 3.0f;
        [SerializeField]
        float sphereReprojectionVolumeRadius = 1.0f;
        [SerializeField]
        bool useSeparateProjectionVolume = false;
        [SerializeField]
        Vector3 boxReprojectionVolumeSize = Vector3.one;
        [SerializeField]
        Vector3 boxReprojectionVolumeCenter = Vector3.zero;
        [SerializeField]
        float maxSearchDistance = 8.0f;
        [SerializeField]
        Texture previewCubemap;
        [SerializeField]
        Vector3 blendDistancePositive = Vector3.zero;
        [SerializeField]
        Vector3 blendDistanceNegative = Vector3.zero;
        [SerializeField]
        Vector3 blendNormalDistancePositive = Vector3.zero;
        [SerializeField]
        Vector3 blendNormalDistanceNegative = Vector3.zero;
        [SerializeField]
        Vector3 boxSideFadePositive = Vector3.one;
        [SerializeField]
        Vector3 boxSideFadeNegative = Vector3.one;

        //editor value that need to be saved for easy passing from simplified to advanced and vice et versa
        // /!\ must not be used outside editor code
        [SerializeField]
        Vector3 editorAdvancedModeBlendDistancePositive;
        [SerializeField]
        Vector3 editorAdvancedModeBlendDistanceNegative;
        [SerializeField]
        float editorSimplifiedModeBlendDistance;
        [SerializeField]
        Vector3 editorAdvancedModeBlendNormalDistancePositive;
        [SerializeField]
        Vector3 editorAdvancedModeBlendNormalDistanceNegative;
        [SerializeField]
        float editorSimplifiedModeBlendNormalDistance;
        [SerializeField]
        bool editorAdvancedModeEnabled;

        public Vector3 boxBlendCenterOffset { get { return (blendDistanceNegative - blendDistancePositive) * 0.5f; } }
        public Vector3 boxBlendSizeOffset { get { return -(blendDistancePositive + blendDistanceNegative); } }
        public Vector3 boxBlendNormalCenterOffset { get { return (blendNormalDistanceNegative - blendNormalDistancePositive) * 0.5f; } }
        public Vector3 boxBlendNormalSizeOffset { get { return -(blendNormalDistancePositive + blendNormalDistanceNegative); } }


        public float sphereBlendRadiusOffset { get { return -blendDistancePositive.x; } }
        public float sphereBlendNormalRadiusOffset { get { return -blendNormalDistancePositive.x; } }


        private void Awake()
        {
            if (version < 1.1f)
                MigrateTo1Dot1();
            if (version < 1.2f)
                MigrateTo1Dot2();
        }

        void MigrateTo1Dot1()
        {
            mode = legacyProbe.mode;
            refreshMode = legacyProbe.refreshMode;
            version = 1.1f;
        }

        void MigrateTo1Dot2()
        {
            influenceVolume.shape = influenceShape;
            version = 1.2f;
        }

        public override ReflectionProbeMode mode
        {
            set
            {
                base.mode = value;
                legacyProbe.mode = value; //ensure compatibility till we capture without the legacy component
            }
        }
        public override ReflectionProbeRefreshMode refreshMode
        {
            set
            {
                base.refreshMode = value;
                legacyProbe.refreshMode = value; //ensure compatibility till we capture without the legacy component
            }
        }
    }
}
