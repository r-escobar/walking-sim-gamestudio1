using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

// ---------------------------------------------------------------------------------------------------------------------------
// Procedural Tree - Simple tree mesh generation - � 2015 Wasabimole http://wasabimole.com
// ---------------------------------------------------------------------------------------------------------------------------
// BASIC USER GUIDE
//
// - Choose GameObject > Create Procedural > Procedural Tree from the Unity menu
// - Select the object to adjust the tree's properties
// - Click on Rand Seed to get a new tree of the same type
// - Click on Rand Tree to change the tree type
//
// ADVANCED USER GUIDE
//
// - Drag the object to a project folder to create a Prefab (to keep a static snapshot of the tree)
// - To add a collision mesh to the object, choose Add Component > Physics > Mesh Collider
// - To add or remove detail, change the number of sides
// - You can change the default diffuse bark materials for more complex ones (with bump-map, specular, etc.)
// - Add or replace default materials by adding them to the SampleMaterials\ folder
// - You can also change the tree generation parameters in REAL-TIME from your scripts (*)
// - Use Unity's undo to roll back any unwanted changes
//
// ADDITIONAL NOTES
// 
// The generated mesh will remain on your scene, and will only be re-computed if/when you change any tree parameters.
//
// Branch(...) is the main tree generation function (called recursively), you can inspect/change the code to add new 
// tree features. If you add any new generation parameters, remember to add them to the checksum in the Update() function 
// (so the mesh gets re-computed when they change). If you add any cool new features, please share!!! ;-)
//
// To generate a new tree at runtime, just follow the example in Editor\ProceduralTreeEditor.cs:CreateProceduralTree()

// Additional scripts under ProceduralTree\Editor are optional, used to better integrate the trees into Unity.
//
// (*) To change the tree parameters in real-time, just get/keep a reference to the ProceduralTree component of the 
// tree GameObject, and change any of the public properties of the class.
//
// >>> Please visit http://wasabimole.com/procedural-tree for more information
// ---------------------------------------------------------------------------------------------------------------------------
// VERSION HISTORY
//
// 1.02 Error fixes update
// - Fixed bug when generating the mesh on a rotated GameObject
// - Fix error when building the project
//
// 1.00 First public release
// ---------------------------------------------------------------------------------------------------------------------------
// Thank you for choosing Procedural Tree, we sincerely hope you like it!
//
// Please send your feedback and suggestions to mailto://contact@wasabimole.com
// ---------------------------------------------------------------------------------------------------------------------------

namespace Wasabimole.ProceduralTree
{
    [ExecuteInEditMode]
    public class ProceduralTree : MonoBehaviour
    {
        public const int CurrentVersion = 102;

        // ---------------------------------------------------------------------------------------------------------------------------
        // Tree parameters (can be changed real-time in editor or game)
        // ---------------------------------------------------------------------------------------------------------------------------

        public int Seed; // Random seed on which the generation is based
        [Range(1024, 65000)]
        public int MaxNumVertices = 65000; // Maximum number of vertices for the tree mesh
        [Range(3, 32)]
        public int NumberOfSides = 16; // Number of sides for tree
        [Range(0.05f, 1f)]
        public float BaseRadius = 2f; // Base radius in meters
        [Range(0.75f, 0.95f)]
        public float RadiusStep = 0.9f; // Controls how quickly radius decreases
        [Range(0.01f, 0.8f)]
        public float MinimumRadius = 0.02f; // Minimum radius for the tree's smallest branches
        [Range(0f, 1f)]
        public float BranchRoundness = 0.8f; // Controls how round branches are
        [Range(0.1f, 2f)]
        public float SegmentLength = 0.5f; // Length of branch segments
        [Range(0f, 40f)]
        public float Twisting = 20f; // How much branches twist
        [Range(0f, 0.25f)]
        public float BranchProbability = 0.1f; // Branch probability
        [Range(0, 50)]
        public int TrunkSegmentLength = 2; // Number of segments to go without branching

        public Vector3 leafScaleMin;
        public Vector3 leafScaleMax;
        public bool uniformLeafScale = true;
        public bool uniformXZLeafScale = false;
        public bool leafScaleReduction = true;
        public bool scaleAllInstances = false;
        public float leafScaleReductionFactor = 0.8f;

        public Color baseLeafColor;
        public bool randColorPerLeaf = false;
        public GameObject leafClumpPrefab;
        public GameObject[] possibleLeafClumps;
        List<GameObject> leafList;
        public bool createLeaves = true;
        public bool leavesPointingOut = false;
        public bool editMode = false;

        public float forestSpacing;
        public float spacingNoise;


        public float leafColorNoise = 0.0175f;
        public float radiusStepNoise;
        public float segmentLengthNoise;
        public float twistingNoise;
        //public float branchProbNoise;

        public bool randomizeStep = false;
        public bool randomizeSegLength = false;
        public bool randomizeTwisting = false;

        public bool twistyLeaves = false;
        public int numAxesToTwist;

        //public bool skinnyLeaves = false;

        // ---------------------------------------------------------------------------------------------------------------------------

        Transform _transform;

        float checksum; // Serialized & Non-Serialized checksums for tree rebuilds only on undo operations, or when parameters change (mesh kept on scene otherwise)
        [SerializeField, HideInInspector]
        float checksumSerialized;

        List<Vector3> vertexList; // Vertex list
        List<Vector2> uvList; // UV list
        List<int> triangleList; // Triangle list

        float[] ringShape; // Tree ring shape array

        [HideInInspector, System.NonSerialized]
        public MeshRenderer Renderer; // MeshRenderer component

        MeshFilter filter; // MeshFilter component

#if UNITY_EDITOR
        [HideInInspector]
        public string MeshInfo; // Used in ProceduralTreeEditor to show info about the tree mesh
#endif

        // ---------------------------------------------------------------------------------------------------------------------------
        // Initialise object, make sure it has MeshFilter and MeshRenderer components
        // ---------------------------------------------------------------------------------------------------------------------------

        void OnEnable()
        {
            if (filter != null && Renderer != null) return;

            //gameObject.isStatic = true;

            filter = gameObject.GetComponent<MeshFilter>();
            if (filter == null) filter = gameObject.AddComponent<MeshFilter>();
            if (filter.sharedMesh != null) checksum = checksumSerialized;
            Renderer = gameObject.GetComponent<MeshRenderer>();
            if (Renderer == null) Renderer = gameObject.AddComponent<MeshRenderer>();
        }

        // ---------------------------------------------------------------------------------------------------------------------------
        // Generate tree (only called when parameters change, or there's an undo operation)
        // ---------------------------------------------------------------------------------------------------------------------------

        public void GenerateTree()
        {
            _transform = transform;

            if(leafList != null && leafList.Count > 0)
            {
                foreach (GameObject leaf in leafList)
                {
                    DestroyImmediate(leaf);
                }
            }

            
            if(!editMode)
            {
                RadiusStep += (randomizeStep ? 1 : 0) * (RadiusStep == 0 ? Random.Range(0, radiusStepNoise * 2) : Random.Range(-radiusStepNoise, radiusStepNoise));
                SegmentLength += (randomizeSegLength ? 1 : 0) * (SegmentLength == 0 ? Random.Range(0, segmentLengthNoise * 2) : Random.Range(-segmentLengthNoise, segmentLengthNoise));
                Twisting += (randomizeTwisting ? 1 : 0) * (Twisting == 0 ? Random.Range(0, twistingNoise * 2) : Random.Range(-twistingNoise, twistingNoise));
            }
            
            //BranchProbability += BranchProbability == 0 ? Random.Range(0, branchProbNoise * 2) : Random.Range(-branchProbNoise, branchProbNoise);



            leafList = new List<GameObject>();

            gameObject.isStatic = false;

            var originalRotation = _transform.localRotation;
            var originalSeed = Random.seed;

            if (vertexList == null) // Create lists for holding generated vertices
            {
                vertexList = new List<Vector3>(MaxNumVertices);
                uvList = new List<Vector2>();
                triangleList = new List<int>(MaxNumVertices * 3);
            }
            else // Clear lists for holding generated vertices
            {
                vertexList.Clear();
                uvList.Clear();
                triangleList.Clear();
            }

            SetTreeRingShape(); // Init shape array for current number of sides

            Random.InitState(Seed);

            // Main recursive call, starts creating the ring of vertices in the trunk's base
            Branch(new Quaternion(), Vector3.zero, -1, BaseRadius, 0f, 0);

            Random.InitState(originalSeed);

            _transform.localRotation = originalRotation; // Restore original object rotation

            SetTreeMesh(); // Create/Update MeshFilter's mesh

            if(GetComponent<MeshCollider>())
            {
                GetComponent<MeshCollider>().sharedMesh = filter.sharedMesh;
            }
        }

        // ---------------------------------------------------------------------------------------------------------------------------
        // Set the tree mesh from the generated vertex lists (vertexList, uvList, triangleLists)
        // ---------------------------------------------------------------------------------------------------------------------------

        private void SetTreeMesh()
        {
            // Get mesh or create one
            var mesh = filter.sharedMesh;
            if (mesh == null) 
                mesh = filter.sharedMesh = new Mesh();
            else 
                mesh.Clear();

            // Assign vertex data
            mesh.vertices = vertexList.ToArray();
            mesh.uv = uvList.ToArray();
            mesh.triangles = triangleList.ToArray();

            // Update mesh
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            ; // Do not call this if we are going to change the mesh dynamically!

#if UNITY_EDITOR
            MeshInfo = "Mesh has " + vertexList.Count + " vertices and " + triangleList.Count / 3 + " triangles";
#endif
        }

        // ---------------------------------------------------------------------------------------------------------------------------
        // Main branch recursive function to generate tree
        // ---------------------------------------------------------------------------------------------------------------------------

        void Branch(Quaternion quaternion, Vector3 position, int lastRingVertexIndex, float radius, float texCoordV, int segmentID)
        {
            var offset = Vector3.zero;
            var texCoord = new Vector2(0f, texCoordV);
            var textureStepU = 1f / NumberOfSides;
            var angInc = 2f * Mathf.PI * textureStepU;
            var ang = 0f;

            // Add ring vertices
            for (var n = 0; n <= NumberOfSides; n++, ang += angInc) 
            {
                var r = ringShape[n] * radius;
                offset.x = r * Mathf.Cos(ang); // Get X, Z vertex offsets
                offset.z = r * Mathf.Sin(ang);
                Vector3 offsetPos = quaternion * offset;
                vertexList.Add(new Vector3(position.x + offsetPos.x, position.y + offsetPos.y, position.z + offsetPos.z)); // Add Vertex position
                uvList.Add(texCoord); // Add UV coord
                texCoord.x += textureStepU;
            }

            if (lastRingVertexIndex >= 0) // After first base ring is added ...
            {
                // Create new branch segment quads, between last two vertex rings
                for (var currentRingVertexIndex = vertexList.Count - NumberOfSides - 1; currentRingVertexIndex < vertexList.Count - 1; currentRingVertexIndex++, lastRingVertexIndex++) 
                {
                    triangleList.Add(lastRingVertexIndex + 1); // Triangle A
                    triangleList.Add(lastRingVertexIndex);
                    triangleList.Add(currentRingVertexIndex);
                    triangleList.Add(currentRingVertexIndex); // Triangle B
                    triangleList.Add(currentRingVertexIndex + 1);
                    triangleList.Add(lastRingVertexIndex + 1);
                }
            }

            // Do we end current branch?
            radius *= RadiusStep;
            if (radius < MinimumRadius || vertexList.Count + NumberOfSides >= MaxNumVertices) // End branch if reached minimum radius, or ran out of vertices
            {
                // Create a cap for ending the branch
                vertexList.Add(position); // Add central vertex
                uvList.Add(texCoord + Vector2.one); // Twist UVs to get rings effect
                for (var n = vertexList.Count - NumberOfSides - 2; n < vertexList.Count - 2; n++) // Add cap
                {
                    triangleList.Add(n);
                    triangleList.Add(vertexList.Count - 1);
                    triangleList.Add(n + 1);
                }

                if(createLeaves)
                {
                    Vector3 lastCenter = vertexList[vertexList.Count - 5] + vertexList[vertexList.Count - 6] + vertexList[vertexList.Count - 7];
                    lastCenter = lastCenter / 3;

                    Vector3 nextPoint = position;
                    Vector3 posModTemp = quaternion * new Vector3(0f, SegmentLength, 0f);
                    nextPoint.x += posModTemp.x;
                    nextPoint.y += posModTemp.y;
                    nextPoint.z += posModTemp.z;

                    SpawnLeaves(position, nextPoint);
                }

                return; 
            }

            // Continue current branch (randomizing the angle)
            texCoordV += 0.0625f * (SegmentLength + SegmentLength / radius);
            Vector3 posMod = quaternion * new Vector3(0f, SegmentLength, 0f);
            position.x += posMod.x;
            position.y += posMod.y;
            position.z += posMod.z;

            if(Twisting + BranchProbability > 0)
                _transform.rotation = quaternion;
            float x = 0f;
            float z = 0f;
            if(Twisting > 0)
            {
                x = (Random.value - 0.5f) * Twisting;
                z = (Random.value - 0.5f) * Twisting;
                _transform.Rotate(x, 0f, z);
            }
            lastRingVertexIndex = vertexList.Count - NumberOfSides - 1;

            if (Twisting + BranchProbability > 0)
                Branch(_transform.rotation, position, lastRingVertexIndex, radius, texCoordV, segmentID + 1); // Next segment
            else
                Branch(quaternion, position, lastRingVertexIndex, radius, texCoordV, segmentID + 1); // Next segment

            // Do we branch?
            if (vertexList.Count + NumberOfSides >= MaxNumVertices || Random.value > BranchProbability || segmentID < TrunkSegmentLength)
            {
                return;
            }

            // Yes, add a new branch
            _transform.rotation = quaternion;
            x = Random.value * 70f - 35f;
            x += x > 0 ? 10f : -10f;
            z = Random.value * 70f - 35f;
            z += z > 0 ? 10f : -10f;
            _transform.Rotate(x, 0f, z);
            Branch(_transform.rotation, position, lastRingVertexIndex, radius, texCoordV, segmentID + 1000);
        }


        // Spawn a clump of leaves
        void SpawnLeaves(Vector3 pos, Vector3 prevPos)
        {
            //Debug.Log("spawning leaf!");

            if (!leafClumpPrefab)
                return;
            GameObject newLeaf = Instantiate(leafClumpPrefab);

            newLeaf.transform.parent = _transform;
            newLeaf.transform.localPosition = pos;

            if (leavesPointingOut)
            {
                Vector3 pointDir = (prevPos - newLeaf.transform.position).normalized;
                Debug.Log(pointDir);

            }
            

            MeshFilter[] meshes = newLeaf.GetComponentsInChildren<MeshFilter>();

            Color32 newCol = new Color32();

            float colorNoise = Random.Range(-leafColorNoise, leafColorNoise);
            newCol = new Color(baseLeafColor.r, baseLeafColor.g + colorNoise, baseLeafColor.b, 1);


//            for(int j = 0; j < meshes.Length; j++) {
//                MeshFilter mFilter = meshes[j];
//
//                Mesh mesh = mFilter.mesh;
//
//                Vector3[] vertices = mesh.vertices;
//
//                // create new colors array where the colors will be created.
//                Color32[] colors = new Color32[vertices.Length];
//
//                if(randColorPerLeaf)
//                {
//                    newCol = new Color(baseLeafColor.r, baseLeafColor.g + j * 0.5f * colorNoise, baseLeafColor.b, 1);
//                }
//
//                for (int i = 0; i < vertices.Length; i++)
//                {
//                    colors[i] = newCol;
//                }
//
//                // assign the array of colors to the Mesh.
//                mesh.colors32 = colors;
//            }


            if(!scaleAllInstances)
            {
                newLeaf.transform.localScale = GetModifiedLeafScale();
            }

            leafList.Add(newLeaf);
        }

        public Vector3 GetModifiedLeafScale()
        {
            Vector3 baseLeafScale = leafClumpPrefab.transform.localScale;
            Vector3 leafScale;

            if (uniformLeafScale)
            {
                leafScale = baseLeafScale * Random.Range(leafScaleMin.x, leafScaleMax.x);
            }
            else if (uniformXZLeafScale)
            {
                float xScale = Random.Range(leafScaleMin.x, leafScaleMax.x);
                leafScale = Vector3.Scale(baseLeafScale, new Vector3(xScale, Random.Range(leafScaleMin.y, leafScaleMax.y), xScale));
            }
            else
            {
                leafScale = Vector3.Scale(baseLeafScale, new Vector3(Random.Range(leafScaleMin.x, leafScaleMax.x), Random.Range(leafScaleMin.y, leafScaleMax.y), Random.Range(leafScaleMin.z, leafScaleMax.z)));
            }

            if (leafScaleReduction && leafList.Count > 0)
                leafScale *= Mathf.Pow(leafScaleReductionFactor, Mathf.Min(leafList.Count, 3));

            return leafScale;
        }

        // ---------------------------------------------------------------------------------------------------------------------------
        // Try to get shared mesh for new prefab instances
        // ---------------------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        bool CanGetPrefabMesh()
        {
            // Return false if we are not instancing a new procedural tree prefab
            if (PrefabUtility.GetPrefabType(this) != PrefabType.PrefabInstance) return false;
            if (filter.sharedMesh != null) return true;

            // Try to get mesh from an existing instance
            var parentPrefab = PrefabUtility.GetCorrespondingObjectFromSource(this);
            var list = (ProceduralTree[])FindObjectsOfType(typeof(ProceduralTree));
            foreach (var go in list)
                if (go != this && PrefabUtility.GetCorrespondingObjectFromSource(go) == parentPrefab)
                {
                    filter.sharedMesh = go.filter.sharedMesh;
                    return true;
                }
            return false;
        }
#endif

        // ---------------------------------------------------------------------------------------------------------------------------
        // Set tree shape, by computing a random offset for every ring vertex
        // ---------------------------------------------------------------------------------------------------------------------------

        private void SetTreeRingShape()
        {
            ringShape = new float[NumberOfSides + 1];
            var k = (1f - BranchRoundness) * 0.5f;
            // Randomize the vertex offsets, according to BranchRoundness
            Random.InitState(Seed);
            for (var n = 0; n < NumberOfSides; n++) ringShape[n] = 1f - (Random.value - 0.5f) * k;
            ringShape[NumberOfSides] = ringShape[0];
        }

        // ---------------------------------------------------------------------------------------------------------------------------
        // Update function will return, unless the tree parameters have changed
        // ---------------------------------------------------------------------------------------------------------------------------

        public void Update()
        {
            if(editMode)
            {
                // Tree parameter checksum (add any new parameters here!)
                var newChecksum = (Seed & 0xFFFF) + NumberOfSides + SegmentLength + BaseRadius + MaxNumVertices +
                    RadiusStep + MinimumRadius + Twisting + BranchProbability + BranchRoundness;

                // Return (do nothing) unless tree parameters change
                if (newChecksum == checksum && filter.sharedMesh != null) return;

                checksumSerialized = checksum = newChecksum;

#if UNITY_EDITOR
                if (!CanGetPrefabMesh())
                    GenerateTree(); // Update tree mesh
#endif

                if(!Application.isEditor)
                {
                    GenerateTree();
                }
            }

        }

        // ---------------------------------------------------------------------------------------------------------------------------
        // Destroy procedural mesh when object is deleted
        // ---------------------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        void OnDisable()
        {
            if (filter.sharedMesh == null) return; // If tree has a mesh
            if (PrefabUtility.GetPrefabType(this) == PrefabType.PrefabInstance) // If it's a prefab instance, look for siblings
            {   
                var parentPrefab = PrefabUtility.GetCorrespondingObjectFromSource(this);
                var list = (ProceduralTree[])FindObjectsOfType(typeof(ProceduralTree));
                foreach (var go in list)
                    if (go != this && PrefabUtility.GetCorrespondingObjectFromSource(go) == parentPrefab)
                        return; // Return if there's another prefab instance still using the mesh
            }
            DestroyImmediate(filter.sharedMesh, true); // Delete procedural mesh
        }
#endif
    }
}