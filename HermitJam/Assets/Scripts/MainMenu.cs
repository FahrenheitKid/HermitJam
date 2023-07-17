using System.Collections;
using System.Collections.Generic;
using HermitJam;
using UnityEngine;
using UnityEngine.SceneManagement;
using  Zenject;

public class MainMenu : MonoBehaviour
{
    ZenjectSceneLoader _sceneLoader;
    [SerializeField] private Difficulty m_Difficulty;
    
    [Inject]
    public void Construct(ZenjectSceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        _sceneLoader.LoadScene("Game", LoadSceneMode.Single, (container) =>
        {
            container.BindInstance(m_Difficulty).WhenInjectedInto<GameInstaller>();
        });
    }
}
