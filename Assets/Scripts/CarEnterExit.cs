using UnityEngine;

public class CarEnterExit : MonoBehaviour
{
    [Header("Referências")]
    public Transform player;
    public GameObject playerObject;
    public MonoBehaviour playerMovement;

    [Header("Configurações")]
    public Transform exitPoint;
    public float enterDistance = 3f;

    [Header("Scripts do Carro")]
    public MonoBehaviour carController;

    private bool playerInside = false;

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        // Entrar no carro
        if (!playerInside && distance <= enterDistance && Input.GetKeyDown(KeyCode.E))
        {
            EnterCar();
        }

        // Sair do carro
        else if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            ExitCar();
        }
    }

    void EnterCar()
    {
        playerInside = true;

        // Desativa player
        playerObject.SetActive(false);

        // Ativa controle do carro
        carController.enabled = true;
    }

    void ExitCar()
    {
        playerInside = false;

        // Coloca player fora do carro
        playerObject.transform.position = exitPoint.position;

        // Ativa player
        playerObject.SetActive(true);

        // Desativa controle do carro
        carController.enabled = false;
    }
}
