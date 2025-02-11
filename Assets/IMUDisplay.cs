using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
// Required for TextMeshPro

// Store the data as a ArrayList (App will have a reference to data) 
// Add a save button
// Menu to view data
// Export feature??? (On phone->Database)
public class SensorDataReader : MonoBehaviour
{
    public TextMeshProUGUI sensorDataText; // Drag the Text UI element here in the Inspector

    void Start()
    {
        // Enable sensors using fully qualified names
        EnableSensor(UnityEngine.InputSystem.Accelerometer.current);
        EnableSensor(UnityEngine.InputSystem.Gyroscope.current);
        EnableSensor(UnityEngine.InputSystem.GravitySensor.current);
        EnableSensor(UnityEngine.InputSystem.AttitudeSensor.current);

        // Log enabled status
        Debug.Log($"Accelerometer enabled: {UnityEngine.InputSystem.Accelerometer.current?.enabled}");
        Debug.Log($"Gyroscope enabled: {UnityEngine.InputSystem.Gyroscope.current?.enabled}");
        Debug.Log($"Gravity Sensor enabled: {UnityEngine.InputSystem.GravitySensor.current?.enabled}");
        Debug.Log($"Attitude Sensor enabled: {UnityEngine.InputSystem.AttitudeSensor.current?.enabled}");
    }

    void Update()
    {
        string dataOutput = "Sensor Data:\n";

        // Collect sensor data if available
        if (UnityEngine.InputSystem.Accelerometer.current?.enabled == true)
        {
            Vector3 acceleration = UnityEngine.InputSystem.Accelerometer.current.acceleration.ReadValue();
            dataOutput += $"Acceleration: {acceleration}\n";
        }

        if (UnityEngine.InputSystem.Gyroscope.current?.enabled == true)
        {
            Vector3 angularVelocity = UnityEngine.InputSystem.Gyroscope.current.angularVelocity.ReadValue();
            dataOutput += $"Gyroscope Angular Velocity: {angularVelocity}\n";
        }

        if (UnityEngine.InputSystem.GravitySensor.current?.enabled == true)
        {
            Vector3 gravity = UnityEngine.InputSystem.GravitySensor.current.gravity.ReadValue();
            dataOutput += $"Gravity: {gravity}\n";
        }

        if (UnityEngine.InputSystem.AttitudeSensor.current?.enabled == true)
        {
            Quaternion attitude = UnityEngine.InputSystem.AttitudeSensor.current.attitude.ReadValue();
            dataOutput += $"Attitude (Rotation): {attitude.eulerAngles}\n";
        }

        Debug.Log("Data: " + dataOutput);

        // Update UI text
        if (sensorDataText != null)
        {
            sensorDataText.text = dataOutput;
        }
        else
        {
            Debug.LogWarning("No TextMeshProUGUI assigned for displaying sensor data.");
        }
    }

    private void EnableSensor(InputDevice sensor)
    {
        if (sensor != null && !sensor.enabled)
        {
            InputSystem.EnableDevice(sensor);
            Debug.Log($"{sensor.GetType().Name} enabled.");
        }
    }

    private void DisableSensor(InputDevice sensor)
    {
        if (sensor != null && sensor.enabled)
        {
            InputSystem.DisableDevice(sensor);
            Debug.Log($"{sensor.GetType().Name} disabled.");
        }
    }

    void OnDestroy()
    {
        // Clean up by disabling sensors
        DisableSensor(UnityEngine.InputSystem.Accelerometer.current);
        DisableSensor(UnityEngine.InputSystem.Gyroscope.current);
        DisableSensor(UnityEngine.InputSystem.GravitySensor.current);
        DisableSensor(UnityEngine.InputSystem.AttitudeSensor.current);
    }
}


