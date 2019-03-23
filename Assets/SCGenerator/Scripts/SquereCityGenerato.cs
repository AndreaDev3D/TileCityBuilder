using UnityEngine;
using System.Collections;
//using UnityEditor;//<------abilitalo per salvare il prefab
using UnityEngine.UI;
using Assets.SCGenerator.Scripts.Helper;

public class SquereCityGenerato : MonoBehaviour {
    #region CityVariable
    public bool GenerateBuilding = true;
    public GameObject[] building;
    public bool GenerateRoad = true;
    public GameObject[] road;
    public bool GenerateLight = true;
    public GameObject[] light;
    public bool GenerateProp = true;
    public GameObject[] objX;
    public GameObject[] objZ;
    int[,] mapgrid;
    public int mapWidth = 20;
    public int mapHeight = 20;
    public int buildingFootprint = 10;
    public int RandWidth = 3;
    public int RandHeight = 5;
    public GameObject CamTarget;

    private GameObject SCG_Generator;
    public GameObject SCG_Prefab;
    public GameObject SCG_camTarget;
    private Camera Camera;
    #endregion
    #region UI Element
    public Slider mapSizeSlider;
    public Text mapSizeValue;
    public Slider RandWidthSlider;
    public Text mapRandWValue;
    public Slider RandHaightSlider;
    public Text mapRAndHValue;
    public Text InteractiveText;
    public Toggle GenerateBuildingToggle;
    public Toggle GenerateLightToggle;
    public Toggle GenerateRoadToggle;
    public Toggle GeneratePropToggle;
    #endregion

    void Start() {
        /*Object[]  buildingPrefab = Resources.LoadAll("Assets/SCGenerator/Resources/BuildingPrefab", typeof(GameObject));
        building = new GameObject[buildingPrefab.Length];
        for (int i = 0; i < building.Length; i++) {
            building[i] = (GameObject)buildingPrefab[i];
        }*/
            SCG_Generator = gameObject;

        SCG_Prefab = Instantiate(Resources.Load("SCG_Prefab", typeof(GameObject))) as GameObject;
        SCG_Prefab.transform.name = "SCG_CityPrefab";
        SCG_Prefab.transform.SetParent(SCG_Generator.transform);

        SCG_camTarget = Instantiate(Resources.Load("SCG_CamTarget", typeof(GameObject))) as GameObject;
        SCG_camTarget.transform.name = "SCG_camTarget";
        SCG_camTarget.transform.SetParent(SCG_Generator.transform);

        InteractiveText.text = "A base 20x20 city has been created";
        //generate base The city
        GenerateCity();
        StartCoroutine(ResestText());
        //FPS_Prefab
    }

    IEnumerator ResestText() {
        yield return new WaitForSeconds(3);
        if (InteractiveText.ToString() != "")
            InteractiveText.text = "";
    }

    void Update() {

        int myMapSize = (int)mapSizeSlider.value;
        mapHeight = myMapSize;
        mapWidth = myMapSize;
        mapSizeValue.text = myMapSize.ToString();

        int myRWText = (int)RandWidthSlider.value;
        RandWidth = myRWText;
        mapRandWValue.text = myRWText.ToString();

        int myRHText = (int)RandHaightSlider.value;
        RandHeight = myRHText;
        mapRAndHValue.text = myRHText.ToString();

        SCG_camTarget.transform.position = new Vector3(mapWidth * buildingFootprint / 2, 0, mapHeight * buildingFootprint / 2);

        GenerateBuilding = GenerateBuildingToggle.isOn;
        GenerateRoad = GenerateRoadToggle.isOn;
        GenerateLight = GenerateLightToggle.isOn;
        GenerateProp = GeneratePropToggle.isOn;
    }

    public void GenerateCity() {

        // Initialize Matrix
        mapgrid = new int[mapWidth, mapHeight];
        var rnd = new System.Random();
        for (int n = 0; n < mapHeight; n++)
        {
            var res = rnd.Next(0, mapHeight * RandHeight);
            if (res < mapWidth || mapgrid[n-1, 0] == -3)
            {
                for (int h = 0; h < mapWidth; h++)
                {
                    //road
                    int RR = Random.Range(0, mapHeight);
                    mapgrid[n, h] = -3;//place vertical road Z
                                       //mapgrid[n + 1 , h] = 1;//place building on left n right verticali road for 
                                       //mapgrid[n - 1, h] = 1;//place building on left n right verticali road for
                                       //end Road
                    if (h == mapWidth - 1)
                    {
                        mapgrid[n, h] = -5;//place StreetZ
                    }
                    if (h == 0)
                    {
                        mapgrid[n, h] = -10;
                    }
                }
            }
        }

        rnd = new System.Random();
        for (int h = 0; h < mapHeight; h++)
        {
            var res = rnd.Next(0, mapWidth * RandWidth);
            if (res < mapWidth)
            {
                for (int w = 0; w < mapWidth; w++)
                {
                    //road
                    if (mapgrid[w, h] == -3) {
                                mapgrid[w, h] = -1;//place cross road
                    //        //    mapgrid[w-1, h] = -6;//place stopX 
                    //        //    mapgrid[w + 1, h] = -6;//place stopX 
                    //        //    if (w != mapHeight - 1) {
                    //        //    mapgrid[w, h + 1] = -7;
                    //        //    mapgrid[w, h - 1] = -7;
                    //        //    mapgrid[w - 1, h] = -6;//place stopX 
                    //        //    mapgrid[w + 1, h] = -6;//place stopX 
                    //        //    }
                            }
                     else {
                        mapgrid[w, h] = -2;//place orizontal road Z
                    //            //mapgrid[w, h + 1] = 1;//place building on left n right orizontali road for roadZ
                    //            //mapgrid[w, h - 1] = 1;//place building on left n right orizontali road for roadZ
                    }
                    //end Road
                    if (w == mapHeight - 1)
                    {
                        mapgrid[w, h] = -4;
                    }
                    if (w == 0)
                    {
                        mapgrid[w, h] = -11;
                    }
                }
            }
        }

        #region generate city data

        for (int h = 0; h < mapHeight; h++) {
            for (int w = 0; w < mapWidth; w++) {
                int result = mapgrid[w, h];
                Vector3 pos = new Vector3(w * buildingFootprint, 0, h * buildingFootprint);
                if (GenerateRoad) {
                    InstancietRoad(result, pos);
                }
                if (GenerateLight) {
                    PickaLight(result, pos);
                }
                if (GenerateProp) {
                    PickaObjX(result, pos);
                    PickaObjZ(result, pos);
                }
                if (GenerateBuilding) {
                    PickaBuilding(result, pos);
                    //if(result == result)
                    //Instantiate(building[0], pos, Quaternion.identity, SCG_Prefab.transform);
                }
            }
        }
        #endregion

        InteractiveText.text = "City has been Generated";
    }

    public void PickaBuilding(int result, Vector3 pos) {
        var BuildingXlist = building.Length;
        int objXindex = Random.Range(0, BuildingXlist);
        if (result == 1)
            Instantiate(building[objXindex], pos, Quaternion.identity, SCG_Prefab.transform);
    }

    public void PickaLight(int result, Vector3 pos) {
        if (result == -1) {
            Instantiate(light[0], pos, light[0].transform.rotation, SCG_Prefab.transform);//CrossRoad light 
        }
        else if (result == -2) {
            Instantiate(light[1], pos, light[1].transform.rotation, SCG_Prefab.transform);//streetX light 
        }
        else if (result == -3) {
            Instantiate(light[2], pos, light[2].transform.rotation, SCG_Prefab.transform);//streetZ light 
        }
    }

    public void InstancietRoad(int result, Vector3 pos) {
        if (result == 0) {
            Instantiate(road[0], pos, Quaternion.identity, SCG_Prefab.transform);//empty element 
        }
        else if (result == -1) {
            Instantiate(road[1], pos, Quaternion.identity, SCG_Prefab.transform);//crossroad element 
        }
        if (result == -2) {
            Instantiate(road[2], pos, Quaternion.identity, SCG_Prefab.transform);//streetX element 
        }
        else if (result == -3) {
            Instantiate(road[3], pos, Quaternion.identity, SCG_Prefab.transform);//streetZ element 
        }
        else if (result == -4) {
            Instantiate(road[4], pos, Quaternion.identity, SCG_Prefab.transform);//endX element 
        }
        else if (result == -5) {
            Instantiate(road[5], pos, Quaternion.identity, SCG_Prefab.transform);//endZ element 
        }
        else if (result == -6) {
            Instantiate(road[6], pos, Quaternion.identity, SCG_Prefab.transform);//stopX element 
        }
        else if (result == -7) {
            Instantiate(road[7], pos, Quaternion.identity, SCG_Prefab.transform);//stopZ element 
        }
        else if (result == -8) {
            Instantiate(road[8], pos, Quaternion.Euler(0, 0, 0), SCG_Prefab.transform);//jointX 
        }
        else if (result == -9) {
            Instantiate(road[9], pos, Quaternion.Euler(0, 90, 0), SCG_Prefab.transform);//stopZ 
        }
        else if (result == -10) {
            Instantiate(road[10], pos, Quaternion.Euler(0, 180, 0), SCG_Prefab.transform);//endZ down  
        }
        else if (result == -11) {
            Instantiate(road[11], pos, Quaternion.Euler(0, 180, 0), SCG_Prefab.transform);//endX down 
        }
    }

    public void PickaObjX(int result, Vector3 pos) {
        var objXlist = objX.Length;
        int objXindex = Random.Range(0, objXlist);
        if (result == -2) {
            Instantiate(objX[objXindex], pos, Quaternion.identity, SCG_Prefab.transform);
        }
    }

    public void PickaObjZ(int result, Vector3 pos) {
        var objZlist = objZ.Length;
        int objZindex = Random.Range(0, objZlist);
        if (result == -3) {
            Instantiate(objZ[objZindex], pos, Quaternion.identity, SCG_Prefab.transform);
        }
    }

    public void ClearCity() {
        GameObject obj = SCG_Prefab;
        foreach (Transform child in obj.transform) {
            Destroy(child.gameObject);
        }
        InteractiveText.text = "City has been Deleted";
    }

    public void SaveCityPref() {
        /*
        string path = "Assets/Resources/CityPrefab/";
        GameObject obj = SCG_Prefab;
        Object prefab = PrefabUtility.CreateEmptyPrefab(path + obj.transform.name + ".prefab");
        PrefabUtility.ReplacePrefab(obj, prefab, ReplacePrefabOptions.Default);
        InteractiveText.text = "A prefab of the city has been saved : " + path + obj.transform.name + ".prefab";*/
        InteractiveText.text = "In Build the saving of the prefab is not allowed. try the editor mode download the asset is free.";
    }

    public void Enable(GameObject Obj) {
            Obj.SetActive(true);
    }

    public void Disable(GameObject Obj) {
        Obj.SetActive(false);
    }
}
