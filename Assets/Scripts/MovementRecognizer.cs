using System.Collections.Generic;
using UnityEngine;
using PDollarGestureRecognizer;

public class MovementRecognizer : MonoBehaviour
{
    public Transform movementSource;
    public float newPositionThresholdDistance = 0.05f;
    public GameObject debugCubePrefab;
    public bool creationMode = true;
    public string newGestureName;
    public float recognitionThreshold = 0.9f;
    public List<Gesture> trainingSet = new List<Gesture>();

    private bool isMoving = false;
    private List<Vector3> positionsList = new List<Vector3>();

    void Update()
    {
        if (isMoving)
            UpdateMovement();
    }

    public void StartMovement()
    {
        isMoving = true;
        positionsList.Clear();

        AddPointFromMovementSource();
    }

    void AddPointFromMovementSource()
    {
        positionsList.Add(movementSource.position);
        if (debugCubePrefab)
        {
            GameObject spawnedDebugCube = Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity);
            Destroy(spawnedDebugCube, 5);
        }
    }

    void UpdateMovement()
    {
        Vector3 lastPosition = positionsList[positionsList.Count - 1];

        if (Vector3.Distance(movementSource.position, lastPosition) > newPositionThresholdDistance)
        {
            AddPointFromMovementSource();
        }
    }

    public string EndMovement()
    {
        isMoving = false;
        AddPointFromMovementSource();

        Point[] pointArray = new Point[positionsList.Count];

        for (int i = 0; i < positionsList.Count; i++)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionsList[i]);
            pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0);
        }

        Gesture newGesture = new Gesture(pointArray);

        if (creationMode)
        {
            newGesture.Name = newGestureName;
            trainingSet.Add(newGesture);

            return null;
        }
        else
        {
            Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray());

            if (result.Score > recognitionThreshold)
                return result.GestureClass;
            else
                return null;
        }
    }
}

//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.XR;
//using PDollarGestureRecognizer;
//using UnityEngine.XR.Interaction.Toolkit;
//using System.IO;
//using UnityEngine.Events;

//public class MovementRecognizer : MonoBehaviour
//{
//    public XRNode inputSource;
//    public InputHelpers.Button inputButton;
//    public float inputThreshold = 0.1f;
//    public Transform movementSource;

//    public float newPositionThresholdDistance = 0.05f;
//    public GameObject debugGamePrefab;
//    public bool creationMode = true;
//    public string newGestureName;

//    public float recognitionThreshold = 0.9f;
//    public List<Gesture> trainingSet = new List<Gesture>();

//    [System.Serializable]
//    public class UnityStringEvent : UnityEvent<string> { }
//    public UnityStringEvent OnRecognized;

//    private bool isMoving = false;
//    private List<Vector3> positionList = new List<Vector3>();

//    private void Start()
//    {
//        //string[] gestureFiles = Directory.GetFiles(Application.persistentDataPath, "*.xml");
//        //foreach (var item in gestureFiles)
//        //{
//        //    Debug.Log("Girdi");
//        //    trainingSet.Add(GestureIO.ReadGestureFromFile(item));
//        //}
//        //Debug.Log(trainingSet.Count);
//    }

//    private void Update()
//    {
//        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), inputButton, out bool isPressed, inputThreshold);

//        // Start The Movement
//        if(!isMoving && isPressed )
//        {
//            StartMovement();
//        }
//        //Ending The Movement
//        else if (isMoving && !isPressed)
//        {
//            EndMovement();
//        }
//        //Updating The Movement
//        else if (isMoving && isPressed)
//        {
//            UpdateMovement();
//        }
//    }

//    public void StartMovement()
//    {
//        Debug.Log("Start");
//        isMoving = true;
//        positionList.Clear();
//        positionList.Add(movementSource.position);

//        if (debugGamePrefab)
//            Destroy(Instantiate(debugGamePrefab, movementSource.position, Quaternion.identity), 3);
//    }

//    public void EndMovement()
//    {
//        Debug.Log("End");
//        isMoving = false;

//        //Create The Gesture from The Position List
//        Point[] pointArray = new Point[positionList.Count];

//        for (int i = 0; i < positionList.Count; i++)
//        {

//            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionList[i]);

//            pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0);
//        }

//        Gesture newGesture = new Gesture(pointArray);

//        //Add A new gesture to training set
//        if(creationMode)
//        {
//            newGesture.Name = newGestureName;
//            trainingSet.Add(newGesture);

//            string fileName = Application.persistentDataPath + "/" + newGestureName + ".xml";
//            GestureIO.WriteGesture(pointArray, newGestureName, fileName);
//        }
//        //recognize
//        else
//        {
//            Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray());
//            Debug.Log(result.GestureClass + " " + result.Score);
//            if(result.Score > recognitionThreshold)
//            {
//                OnRecognized.Invoke(result.GestureClass);
//            }
//        }
//    }

//    public void UpdateMovement()
//    {
//        Debug.Log("Update");
//        Vector3 lastPosition = positionList[positionList.Count - 1];

//        if(Vector3.Distance(movementSource.position, lastPosition) > newPositionThresholdDistance)
//        {
//            positionList.Add(movementSource.position);
//            if (debugGamePrefab)
//                Destroy(Instantiate(debugGamePrefab, movementSource.position, Quaternion.identity), 3);
//        }
//    }
//}
