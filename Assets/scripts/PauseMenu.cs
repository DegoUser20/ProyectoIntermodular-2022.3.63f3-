using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject opcionMenuUI;
    
    [Header("Audio")]
    public AudioMixer audioMixer;

    [Header("Opciones")]
    public Slider VolumenBarra;
    public Slider BrilloBarra;
    public Slider SensibilidadBarra;

    void Start()
    {
        float guardarVol = PlayerPrefs.GetFloat("Volumen", 1f);
        VolumenBarra.value = guardarVol;
        SetVolumen(guardarVol);

        SensibilidadBarra.value = PlayerPrefs.GetFloat("Sensibilidad", 100f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        opcionMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }

    public void AbrirOptions()
    {
        pauseMenuUI.SetActive(false);
        opcionMenuUI.SetActive(true);
    }

    public void CerrarOptions()
    {
        opcionMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void SetVolumen(float vol)
    {
        Debug.Log("SetVolumen llamado, vol = " + vol);
        vol = Mathf.Clamp(vol, 0.2f, 1f);
        
        float dB = Mathf.Log10(vol) * 20f;
        audioMixer.SetFloat("VolumenMaster", dB);
    }

    public void GuargarCambios()
    {
        PlayerPrefs.SetFloat("Volumen", VolumenBarra.value);
        PlayerPrefs.SetFloat("Sensibilidad", SensibilidadBarra.value);
        PlayerPrefs.Save();
        Debug.Log("Configuración Guardada");
    }

    public void QuitGame()
    {
        Time.timeScale = 2f;
        Application.Quit();
    }

}
