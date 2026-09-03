using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    [Header("Configurações do Recuo")]
    public float recoilX = -2f;      // O quanto a arma sobe (mantenha negativo)
    public float recoilY = 0.5f;     // O quanto ela puxa para os lados aleatoriamente

    [Header("Velocidades do Efeito")]
    public float snapSpeed = 6f;     // Rapidez com que a câmera toma o tranco
    public float returnSpeed = 8f;   // Rapidez com que a mira volta ao centro original

    private Vector3 currentRotation;
    private Vector3 targetRotation;

    void Update()
    {
        // Interpola suavemente em direção à rotação de recuo gerada pelo tiro
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Lerp(currentRotation, targetRotation, snapSpeed * Time.fixedDeltaTime);
        
        // Aplica a rotação de recuo diretamente no objeto da câmera
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    // Essa função será chamada pelo script da sua arma sempre que ela atirar!
    public void TriggerRecoil()
    {
        // Sobe a mira e gera um desvio aleatório para a esquerda ou direita
        float randomY = Random.Range(-recoilY, recoilY);
        targetRotation += new Vector3(recoilX, randomY, 0);
    }
}