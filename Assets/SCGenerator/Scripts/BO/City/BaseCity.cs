using Assets.SCGenerator.Scripts.BO.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.SCGenerator.Scripts.BO.City
{
    public class BaseCity : MonoBehaviour
    {
        public static bool GenerateStreet = true;
        public static bool GenerateBuilding = false;
        public static bool GenerateLight = false;
        public static bool GenerateProp = false;

        public static string RoadListDirectory = @"Assets/SCGenerator/Resources/RoadPrefab";
        public static string BuildingListDirectory = "Assets/SCGenerator/Resources/BuildingPrefab";
        public static string LightListDirectory = "Assets/SCGenerator/Resources/LightPrefab";
        public static string PropListDirectory = "Assets/SCGenerator/Resources/PropsXPrefab";


        public static List<UnityEngine.GameObject> RoadList;
        public static List<UnityEngine.GameObject> BuildingList;
        public static List<UnityEngine.GameObject> LightList;
        public static List<UnityEngine.GameObject> PropList;

        private static GameObject SCG_Generator
        {
            get
            {
                var scg = GameObject.Find(nameof(SCG_Generator));
                return scg == null ? Instantiate(new GameObject(nameof(SCG_Generator))) : scg;
            }
            set { value = SCG_Generator; }
        }

        //private static GameObject SCG_Prefab;
        public static GameObject SCG_Prefab
        {
            get
            {
                var scg = GameObject.Find(nameof(SCG_Prefab));
                if (scg == null)
                {
                    var go = new GameObject(nameof(SCG_Prefab));
                    go.transform.name = nameof(SCG_Prefab);
                    go.transform.SetParent(SCG_Generator.transform);
                    scg = Instantiate(go);
                }
                return scg;
            }
            set { value = SCG_Prefab; }
        }

        private static GameObject SCG_Road
        {
            get
            {
                var scg = GameObject.Find(nameof(SCG_Road));
                if (scg == null)
                {
                    var go = new GameObject(nameof(SCG_Road));
                    go.transform.name = nameof(SCG_Road);
                    go.transform.SetParent(SCG_Prefab.transform);
                    scg = Instantiate(go);
                }
                return scg == null ? Instantiate(new GameObject(nameof(SCG_Road))) : scg;
            }
            set { value = SCG_Road; }
        }

        private static int[,] Mapgrid;
        public static int MapWidth = 50;
        public static int MapHeight = 50;
        public static int RandMin = 3;
        public static int RandMax = 50;
        public static int Step = 500;
        public static int BuildingFootprint = 10;

        private static int mapX = 0;
        private static int mapY = 0;
        public BaseCity()
        {
            RoadList = new List<GameObject>();
            BuildingList = new List<GameObject>();
            LightList = new List<GameObject>();
            PropList = new List<GameObject>();
            RefreshAssets();
        }


        public static void GenerateCity()
        {
            int randomDirection;
            var next = new System.Random();
            mapX = MapWidth/2;
            mapY = MapHeight/2;

            Mapgrid = new int[MapWidth, MapHeight];

            for (int i = 0; i < Step; i++)
            {
                randomDirection = next.Next(1, 4);
                var random = Random.Range(RandMin, RandMax);

                var res = WalkFor(2, randomDirection);
                if (!res)
                {
                    i--;
                    //Debug.Log("Counter Error");
                }
            }



            for (int h = 0; h < MapHeight; h++)
            {
                for (int w = 0; w < MapWidth; w++)
                {
                    if(Mapgrid[w, h] == 0) 
                    InstanceTest(w, h, Helper.Enumerator.RoadType.Empty);
                    //int result = Mapgrid[w, h];
                    //var roadResult = (Helper.Enumerator.RoadType)Enum.ToObject(typeof(Helper.Enumerator.RoadType), result);
                    //Vector3 pos = new Vector3(w * BuildingFootprint + (BuildingFootprint / 2), 0, h * BuildingFootprint + (BuildingFootprint / 2));
                    //if (GenerateStreet)
                    //{
                    //    InstancietRoad(roadResult, pos);
                    //}
                }
            }
        }

        private static void InstanceTest(int x, int y, Helper.Enumerator.RoadType type = Helper.Enumerator.RoadType.Cross)
        {
            Vector3 pos = new Vector3(x * BuildingFootprint + (BuildingFootprint / 2), 0, y * BuildingFootprint + (BuildingFootprint / 2));
            InstancietRoad(type, pos);
        }

        private static bool WalkFor(int step, int direction)
        {
            try
            {
                switch (direction)
                {
                    case 1:
                        //walk right
                        if (mapX + step <= MapWidth)
                        {
                            for (int i = 0; i < step; i++)
                            {
                                if (Mapgrid[mapX + i, mapY] == 1)
                                    throw new Exception("Place filled");
                                Mapgrid[mapX + i, mapY] = 1;
                                InstanceTest(mapX + i, mapY);
                            }
                            mapX += step;
                            return true;
                        }
                        break;
                    case 2:
                        //walk left
                        if (mapX - step >= 0)
                        {
                            for (int i = 0; i < step; i++)
                            {
                                if (Mapgrid[mapX - i, mapY] == 1)
                                    throw new Exception("Place filled");
                                Mapgrid[mapX - i, mapY] = 1;
                                InstanceTest(mapX - i, mapY);
                            }
                            mapX -= step;
                            return true;
                        }
                        break;
                    case 3:
                        //walk up
                        if (mapY + step <= MapHeight)
                        {
                            for (int i = 0; i < step; i++)
                            {
                                if (Mapgrid[mapX, mapY + i] == 1)
                                    throw new Exception("Place filled");
                                Mapgrid[mapX, mapY + i] = 1;
                                InstanceTest(mapX, mapY + i);
                            }
                            mapY += step;
                            return true;
                        }
                        break;
                    case 4:
                        //walk down
                        if (mapY - step >= 0)
                        {
                            for (int i = 0; i < step; i++)
                            {
                                if (Mapgrid[mapX, mapY - i] == 1)
                                    throw new Exception("Place filled");
                                Mapgrid[mapX, mapY - i] = 1;
                                InstanceTest(mapX, mapY - i);
                            }
                            mapY -= step;
                            return true;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                //mapX = Random.Range(0, MapWidth);
                //mapY = Random.Range(0, MapHeight);

                mapX = 0;
                mapY = 0;
                return true;
            }
            return false;
        }

        public static bool GenerateCity_Old()
        {
            RefreshAssets();
            // Initialize Matrix
            Mapgrid = new int[MapWidth, MapHeight];
            //
            System.Random _rand = new System.Random();
            var resultsWidth = Enumerable.Range(0, _rand.Next(MapWidth))
                                    .Select(r => _rand.Next(MapWidth*3))
                                    .OrderByDescending(w=>w)
                                    .ToList();

            //Debug.Log("Z :"+string.Join(",", resultsWidth));

            var resultsHeight = Enumerable.Range(0, _rand.Next(MapHeight))
                                    .Select(r => _rand.Next(MapHeight*5))
                                    .OrderByDescending(w => w)
                                    .ToList();
            //Debug.Log("X :" + string.Join(",", resultsHeight));
            // Horizontal
            for (int w = 0; w < MapHeight; w++)
            {
                for (int h = 0; h < MapWidth; h++)
                {
                    if (resultsWidth.Any(a => a == h))
                    {
                        //road
                        Mapgrid[w, h] = Convert.ToInt16(Helper.Enumerator.RoadType.MainHorizontal);
                    }
                }
            }
            // Vertical
            for (int h = 0; h < MapHeight; h++)
            {
                for (int w = 0; w < MapWidth; w++)
                {
                    if (resultsHeight.Any(a => a == w))
                    {
                        //road
                        if (Mapgrid[w, h] == Convert.ToInt16(Helper.Enumerator.RoadType.MainHorizontal))
                        {
                            Mapgrid[w, h] = Convert.ToInt16(Helper.Enumerator.RoadType.Cross);
                        }
                        else
                        {
                            Mapgrid[w, h] = Convert.ToInt16(Helper.Enumerator.RoadType.MainVertical);
                        }
                    }
                        
                }
            }

            for (int h = 0; h < MapHeight; h++)
            {
                for (int w = 0; w < MapWidth; w++)
                {
                    int result = Mapgrid[w, h];
                    var roadResult = (Helper.Enumerator.RoadType)Enum.ToObject(typeof(Helper.Enumerator.RoadType), result);
                    Vector3 pos = new Vector3(w * BuildingFootprint + (BuildingFootprint/2), 0, h * BuildingFootprint + (BuildingFootprint / 2));
                    if (GenerateStreet)
                    {
                        InstancietRoad(roadResult, pos);
                    }
                    //if (GenerateLight) {
                    //    PickaLight(result, pos);
                    //}
                    //if (GenerateProp) {
                    //    PickaObjX(result, pos);
                    //    PickaObjZ(result, pos);
                    //}
                    //if (GenerateBuilding) {
                    //    PickaBuilding(result, pos);
                    //    //if(result == result)
                    //    //Instantiate(building[0], pos, Quaternion.identity, SCG_Prefab.transform);
                    //}
                }
            }

            Console.Write("City Generated");
            return true;
        }

        public static void RefreshAssets()
        {
            // Road
            RoadList = new List<GameObject>();
            foreach (var item in Directory.EnumerateFiles(BaseCity.RoadListDirectory, "*.prefab").ToList())
            {
                var a = (GameObject)AssetDatabase.LoadAssetAtPath(item, typeof(GameObject));
                if (a.GetComponent<BaseRoad>())
                {
                    RoadList.Add(a);
                }
            }
            // Building
            BuildingList = new List<GameObject>();
            foreach (var item in Directory.EnumerateFiles(BuildingListDirectory, "*.prefab").ToList())
            {
                var a = (GameObject)AssetDatabase.LoadAssetAtPath(item, typeof(GameObject));
                if (a.GetComponent<BaseBuilding>())
                {
                    BuildingList.Add(a);
                }
            }
            // Light
            LightList = new List<GameObject>();
            foreach (var item in Directory.EnumerateFiles(LightListDirectory, "*.prefab").ToList())
            {
                var a = (GameObject)AssetDatabase.LoadAssetAtPath(item, typeof(GameObject));
                if (a.GetComponent<BaseLight>())
                {
                    LightList.Add(a);
                }
            }
            // Props
            PropList = new List<GameObject>();
            foreach (var item in Directory.EnumerateFiles(PropListDirectory, "*.prefab").ToList())
            {
                var a = (GameObject)AssetDatabase.LoadAssetAtPath(item, typeof(GameObject));
                if (a.GetComponent<BaseProps>())
                {
                    BaseCity.PropList.Add(a);
                }
            }
        }
        
        public static void InstancietRoad(Helper.Enumerator.RoadType result, Vector3 pos)
        {
            //SCG_Prefab = GameObject.Find("SCG_CityPrefab");
            var list = new List<GameObject>();
            foreach (var item in RoadList)
            {
                if (item.GetComponent<BaseRoad>().RoadType == result)
                {
                    list.Add(item);
                }
            }
            //var list = RoadList.Where(w => w.GetComponent<BaseRoad>().RoadType == result);
            if (list.Count() <= 0)
                return;
            var randomItem = list[new System.Random().Next(list.Count())];
            Instantiate(randomItem, pos, Quaternion.identity, SCG_Road.transform);
        }

        public static void Clear()
        {
            DestroyImmediate(SCG_Road);
            RefreshAssets();
            //foreach (Transform child in SCG_Prefab.transform)
            //{
            //}
        }

    }
}
