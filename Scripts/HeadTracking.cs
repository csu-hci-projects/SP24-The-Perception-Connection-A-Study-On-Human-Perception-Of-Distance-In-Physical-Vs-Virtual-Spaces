using UnityEngine;
using Windows.Kinect;

public class BodyTracking : MonoBehaviour
{
    private KinectSensor sensor;
    private BodyFrameReader bodyFrameReader;
    private Body[] bodies;
    private Body trackedBody; // The body currently being tracked

    public GameObject headSphere;
    public GameObject kinectSensorBar;
    public float scaleFactor = 1f;

    private const float metersToInches = 39.37f;

    [Header("Eye Offset")]
    public float eyeOffsetInches = 4f;

    void Start()
    {
        sensor = KinectSensor.GetDefault();

        if (sensor != null)
        {
            bodyFrameReader = sensor.BodyFrameSource.OpenReader();

            if (!sensor.IsOpen)
            {
                sensor.Open();
            }
        }

        if (kinectSensorBar == null)
        {
            Debug.LogError("KinectSensorBar GameObject is not assigned!");
        }

        headSphere.transform.parent = kinectSensorBar.transform;
    }

    void Update()
    {
        if (bodyFrameReader == null) return; // Early exit for optimization

        using (var frame = bodyFrameReader.AcquireLatestFrame()) // Using 'using' for auto-dispose
        {
            if (frame != null)
            {
                if (bodies == null)
                {
                    bodies = new Body[sensor.BodyFrameSource.BodyCount];
                }

                frame.GetAndRefreshBodyData(bodies);

                if (trackedBody == null || !trackedBody.IsTracked)
                {
                    FindBodyToTrack();
                }

                if (trackedBody != null && trackedBody.IsTracked)
                {
                    UpdateHeadPosition();
                }
            }
        }
    }

    void FindBodyToTrack()
    {
        foreach (var body in bodies)
        {
            if (body != null && body.IsTracked)
            {
                trackedBody = body;
                return; // Early exit for optimization
            }
        }
    }

    void UpdateHeadPosition()
    {
        var localTransform = kinectSensorBar.transform; // Store repeated properties in local variables for optimization
        var parentScale = localTransform.parent != null ? localTransform.parent.localScale : Vector3.one;

        Vector3 headPosition = GetVector3FromJoint(trackedBody.Joints[JointType.Head]) * scaleFactor * metersToInches;
        headPosition = localTransform.rotation * headPosition; // Rotate according to the sensor's orientation
        headPosition = Vector3.Scale(headPosition, parentScale); // Consider the scaling of the room

        headSphere.transform.localPosition = localTransform.InverseTransformPoint(localTransform.position + headPosition);
    }

    Vector3 GetVector3FromJoint(Windows.Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X, joint.Position.Y + (eyeOffsetInches / metersToInches), -joint.Position.Z);
    }

    void OnApplicationQuit()
    {
        if (bodyFrameReader != null)
        {
            bodyFrameReader.Dispose();
            bodyFrameReader = null;
        }

        if (sensor != null)
        {
            if (sensor.IsOpen)
            {
                sensor.Close();
            }
            sensor = null;
        }
    }
}
