using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class JetPropulsion : MonoBehaviour
{  
    #region [Header("Physics Settings")]
    public float VerticalThrust;
    public float Speed_Vertical;
    public float Speed_Horizontal;
    private float OldThrust;
    
    #endregion
    #region [Header("MyJetPlayer")]
    public Rigidbody Rigidbody;
    public ParticleSystem[] Flames;
    [HideInInspector]
    public bool InFuel;
    [HideInInspector]
    public float FuelLevel;
    [HideInInspector]
    public float AddFuel;
    [HideInInspector]
    private float maxFuel;
    public float OldFuelLevel;
    public float IncreaseSpeedVertical;
    private GameManager gameManager;
    #endregion
    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        OldThrust = VerticalThrust;
        FuelLevel = 0.5f;
        gameManager = FindObjectOfType<GameManager>();
    }
    void FixedUpdate()
    {

        #region Propulsion/Fuel
    if (gameManager != null&& FuelSlider != null)
        if (gameManager.press || Input.GetKey(KeyCode.Space))
        {
            Rigidbody.AddRelativeForce(Vector3.up * VerticalThrust);
            for (int i = 0; i < Flames.Length; i++)
                Flames[i].Play();       
            FuelLevel -= 0.1f * Time.deltaTime;
            if (InFuel)
            {
                OldFuelLevel -= 0.1f * Time.deltaTime;
            }

        }
        else
        {
            for (int i = 0; i < Flames.Length; i++)
                Flames[i].Stop();
        }
        if (FuelSlider != null && FuelSlider.value == 0)
        {
            VerticalThrust = 0;
            for (int i = 0; i < Flames.Length; i++)
                Flames[i].Stop();
        }
        else if (FuelSlider != null && FuelSlider.value < 1)
        {
            VerticalThrust = OldThrust;
        }
        #endregion
        #region Horizontal Mouvment Physics
        Vector3 direction = Vector3.left * variableJoystick.Horizontal;
        Vector3 direction_pc = Vector3.left * Input.GetAxis("Horizontal");
        Rigidbody.AddForce(direction * Speed_Horizontal * Time.deltaTime, ForceMode.VelocityChange);
        Rigidbody.AddForce(direction_pc * Speed_Horizontal * Time.deltaTime, ForceMode.VelocityChange);
        Rigidbody.gameObject.transform.rotation = Quaternion.Euler(Rigidbody.gameObject.transform.localRotation.x, -180f, variableJoystick.Horizontal * -20);//Horizontal Mouvment Trandform Balance
        #endregion
        #region Vertical Mouvment 
        Vector3 movementV = new Vector3(0.0f, 0.0f, Speed_Vertical);
        Speed_Vertical += IncreaseSpeedVertical*Time.deltaTime;
        Rigidbody.AddRelativeForce(movementV * Speed_Vertical);
        #endregion
        #region Refueling
        
        if (InFuel&& FuelSlider.value<=1.0f)
        {
            FuelLevel += 0.1f * Time.deltaTime;

        }
        if (FuelSlider.value > OldFuelLevel+AddFuel|| FuelSlider.value >= 1.0f)
        {
            InFuel = false;
        }
        FuelSlider.value = FuelLevel;
        #endregion
        
    }
    #region UI Inputs/Elements
    [HideInInspector]
    public VariableJoystick variableJoystick;
    [HideInInspector]
    public Slider FuelSlider;

    #endregion
}