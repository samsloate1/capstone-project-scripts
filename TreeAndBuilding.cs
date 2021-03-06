﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreeAndBuilding : MonoBehaviour {

	private Vector2 position = new Vector2 (0f,0f);
	private Dictionary<Vector2 , float> noises = new Dictionary<Vector2,float>();
	private int treeDensity = 0;
	private int buildingDensity = 0;
	private float bigNoise;
	private GameObject cube;
	private GameObject tree;
	private GameObject building;
	private float cubeSize;
	private bool a = false;
	private static GameObject TreeContainer;
	private static GameObject BuildingContainer;
	// Use this for initialization
	public void Begin(Vector2 posit, Dictionary<Vector2,float> noise, int trees, int buildings,GameObject cubeObj, 
	                  GameObject treeObj, GameObject buildingObj,float cubeSz){
		if (TreeContainer == null)
			TreeContainer = new GameObject ();
		if (BuildingContainer == null)
			BuildingContainer = new GameObject ();
		position = posit;
		noises = noise;
		//float bigNoise = Mathf.PerlinNoise (posit.x, posit.y);
		treeDensity = trees;
		buildingDensity = buildings;
		cube = cubeObj;
		tree = treeObj;
		building = buildingObj;
		cubeSize = cubeSz;
		StartCoroutine ("placeTrees");
		StartCoroutine ("placeBuildings");

	}
	void makeTrees(Vector2 v){
		Vector3 down = transform.TransformDirection(Vector3.down);
		RaycastHit hit;
		//Debug.Log ("Here");
		if(Physics.Raycast(new Vector3((v.x)*cubeSize + (position.x-1/2)*20*cubeSize,300f,
		                               ((v.y)*cubeSize + (position.y-1/2)*20*cubeSize)),down,out hit)
		  								 && (hit.collider == cube.GetComponent<Collider>() as Collider)){
			GameObject treeLoc = Instantiate (tree);
			TreeGenerator t = treeLoc.GetComponent<TreeGenerator> () as TreeGenerator;
			MeshCollider c = treeLoc.GetComponent<MeshCollider> () as MeshCollider;
			//CapsuleCollider c = treeLoc.GetComponent<CapsuleCollider>() as CapsuleCollider;
			//Set each component of a tree and Instantiate here
			/*
			t.segments = new int[]{(int)Random.Range(1,5), 1};
			t.segmentLength = new float[]{(float)Random.Range(1,5), 0f};
			t.radius = new float[]{(float)Random.Range(3,5), 10f};
			t.upCurve = new float[]{(float)Random.Range(3,5), 10f};
			t.maxTurn = new float[]{(float)Random.Range(3,5), 10f};
			t.branchChance = new float[]{(float)Random.Range(3,5), 10f};
			t.branchDeviation = new float[]{(float)Random.Range(3,5), 10f};
			*/
			treeLoc.transform.position = hit.point;
			//treeLoc.transform.parent = cube.transform;
			treeLoc.GetComponent<TreeGenerator> ().Init();
			StartCoroutine(treeLoc.GetComponent<TreeGenerator> ().Grow());
			treeLoc.transform.parent = TreeContainer.transform;
			treeLoc.SetActive(true);
			c.sharedMesh = treeLoc.GetComponent<MeshFilter>().mesh;
			//treeLoc.GetComponent<CapsuleCollider>().radius = t.
			//t.Init ();
			//Debug.Log ("Enter");
		}

	}
		
	void makeBuildings(Vector2 v){
		Vector3 down = transform.TransformDirection(Vector3.down);
		RaycastHit hit;
		if(Physics.Raycast(new Vector3((v.x)*cubeSize + (position.x-1/2)*20*cubeSize,300f,
		                               ((v.y)*cubeSize + (position.y-1/2)*20*cubeSize))
		                   ,down,out hit) && (hit.collider == cube.GetComponent<Collider>() as Collider)){
			GameObject buildingA = Instantiate (building);
			CylBuildingMaker cbm =  buildingA.GetComponent <CylBuildingMaker> () as CylBuildingMaker;
			MeshCollider c = buildingA.GetComponent<MeshCollider> () as MeshCollider;
			//Set each component of a tree and Instantiate here
			/*cbm.faces = new int[1];
			cbm.radius = 1;
			cbm.segments = 1;
			cbm.segmentHeight = 1;
			cbm.topChances = new float[1];
			cbm.expandChance = 1; 
			cbm.windowChance = 1;
			cbm.maxRad = new float[1];
			cbm.windowHeight = 1;
			cbm.windowInset = 1;*/
			//building.transform.parent = buildingParent.transform
		
			cbm.BuildMe();
			buildingA.transform.parent = BuildingContainer.transform;
			buildingA.transform.position = hit.point;
			buildingA.SetActive(true);
			c.sharedMesh = buildingA.GetComponent<MeshFilter> ().mesh;
		}
	}

	IEnumerator placeTrees(){

		//They all start at zero
		for (int i = 0; i < 50; i ++) {
			Vector2 v = new Vector2(Random.Range(-10,10),
			                        Random.Range(-10,10));
			float x = Mathf.RoundToInt(v.x);
			float z = Mathf.RoundToInt(v.y);

			if(noises[new Vector2(x, z)] < .22f){
				makeTrees(v);
			}
		}
		yield return null;
	}

	IEnumerator placeBuildings(){
		//Keep track of other ones close by
		for (int i = 0; i < 50; i ++) {
			Vector2 v = new Vector2(Random.Range(-10,10),
			                        Random.Range(-10,10));
			float x = Mathf.RoundToInt(v.x);
			float z = Mathf.RoundToInt(v.y);
			
			if(noises[new Vector2(x, z)] <.25f &&  noises[new Vector2(x, z)] >.22f){
				makeBuildings(v);
				yield return new WaitForSeconds(.05f);
			}
		}
		yield return null;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
