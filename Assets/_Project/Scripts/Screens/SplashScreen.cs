using System.Collections;
using UnityEngine;

/// <summary>
/// Classe para a primeira tela do App(Splash Screen)
/// </summary>
public class SplashScreen : MonoBehaviour
{
    [SerializeField] private float timeDelay = 2f;

	private IEnumerator Start ()
    {
        yield return new WaitForSeconds(timeDelay);
        Destroy(gameObject);
	}
}