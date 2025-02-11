using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.InputSystem;
using TMPro; // Import TextMeshPro namespace
using UnityEngine.UI; // Import UnityEngine.UI namespace

public class IMUSaver4 : MonoBehaviour
{
    private List<string> imuDataListSave = new List<string>();
    public Button saveButton; // Start recording button
    public Button stopButton; // Stop recording button
    public TMP_Text filePathText; // Use TMP_Text for TextMeshPro
    public TMP_Text fileSaveState; // Use TMP_Text for TextMeshPro
    private string filePath;
    private bool isRecording = false; // Flag to control recording

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "IMUData_ver1.csv");

        // Display the file path in the UI
        if (filePathText != null)
        {
            filePathText.text = "File Path: " + filePath;
        }

        // Initialize file save state
        if (fileSaveState != null)
        {
            fileSaveState.text = "File recording not started";
        }

        // Set button listeners
        if (saveButton != null)
        {
            saveButton.onClick.AddListener(StartRecording);
        }
        if (stopButton != null)
        {
            stopButton.onClick.AddListener(StopRecording);
        }
    }

    void Update()
    {
        if (!isRecording) return; // Skip data collection if not recording

        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string dataEntry = timestamp;

        if (UnityEngine.InputSystem.Accelerometer.current?.enabled == true)
        {
            Vector3 acceleration = UnityEngine.InputSystem.Accelerometer.current.acceleration.ReadValue();
            dataEntry += $",{acceleration.x},{acceleration.y},{acceleration.z}";
        }
        else dataEntry += ",,,";

        if (UnityEngine.InputSystem.Gyroscope.current?.enabled == true)
        {
            Vector3 angularVelocity = UnityEngine.InputSystem.Gyroscope.current.angularVelocity.ReadValue();
            dataEntry += $",{angularVelocity.x},{angularVelocity.y},{angularVelocity.z}";
        }
        else dataEntry += ",,,";

        if (UnityEngine.InputSystem.GravitySensor.current?.enabled == true)
        {
            Vector3 gravity = UnityEngine.InputSystem.GravitySensor.current.gravity.ReadValue();
            dataEntry += $",{gravity.x},{gravity.y},{gravity.z}";
        }
        else dataEntry += ",,,";

        if (UnityEngine.InputSystem.AttitudeSensor.current?.enabled == true)
        {
            Quaternion attitude = UnityEngine.InputSystem.AttitudeSensor.current.attitude.ReadValue();
            Vector3 eulerAngles = attitude.eulerAngles;
            dataEntry += $",{eulerAngles.x},{eulerAngles.y},{eulerAngles.z}";
        }
        else dataEntry += ",,,";

        imuDataListSave.Add(dataEntry);
    }

    void StartRecording()
    {
        // Check if the file can be opened for writing
        try
        {
            using (FileStream fs = File.Create(filePath))
            {
                fs.Close();
            }
            if (fileSaveState != null)
            {
                fileSaveState.text = "The file is being recorded";
            }

            // Add CSV header
            imuDataListSave.Add("Timestamp,Acceleration_X,Acceleration_Y,Acceleration_Z,Gyroscope_X,Gyroscope_Y,Gyroscope_Z,Gravity_X,Gravity_Y,Gravity_Z,Attitude_X,Attitude_Y,Attitude_Z");

            // Start recording
            isRecording = true;
        }
        catch (Exception e)
        {
            if (fileSaveState != null)
            {
                fileSaveState.text = "The file recording is failed";
            }
            Debug.LogError("Failed to open the file: " + e.Message);

            // Disable file-related activities
            isRecording = false;
        }
    }

    void StopRecording()
    {
        if (isRecording)
        {
            SaveDataToFile4();

            if (fileSaveState != null)
            {
                fileSaveState.text = "Recording stopped and file saved";
            }

            // Stop recording
            isRecording = false;
        }
    }

    public void SaveDataToFile4()
    {
        if (!isRecording) return; // Skip file-related activities if not recording

        File.WriteAllLines(filePath, imuDataListSave);
    }
}
