using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [Header("Configurações do Balanço")]
    public float frequencia = 10f;    
    public float amplitude = 0.05f;   
    public float suavidade = 10f;    

    [Header("Referências")]
    public CharacterController controller; 

    private float timer = 0f;
    private float posicaoPadraoY;

    void Start()
    {
        // Salva a altura original da câmera dentro do Player
        posicaoPadraoY = transform.localPosition.y;
    }

    void Update()
    {
        if (controller == null) return;

        // 1. Verificamos se há movimento (usando as teclas como backup caso a velocidade seja 0)
        bool estaMovendo = controller.velocity.magnitude > 0.1f || Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;

        if (estaMovendo && controller.isGrounded)
        {
            timer += Time.deltaTime * frequencia;

            // 2. Calculamos as novas posições X e Y simultaneamente
            float novoY = posicaoPadraoY + Mathf.Sin(timer) * amplitude;
            float novoX = Mathf.Cos(timer / 2) * (amplitude * 1.5f); // Balanço lateral sutil

            // 3. Criamos o destino final
            Vector3 destino = new Vector3(novoX, novoY, transform.localPosition.z);

            // 4. Aplicamos suavemente (Um único Lerp para tudo!)
            transform.localPosition = Vector3.Lerp(transform.localPosition, destino, Time.deltaTime * suavidade);
        }
        else
        {
            // 5. Se parar, volta para o centro (X=0) e altura padrão
            timer = 0;
            Vector3 posicaoOriginal = new Vector3(0, posicaoPadraoY, transform.localPosition.z);
            transform.localPosition = Vector3.Lerp(transform.localPosition, posicaoOriginal, Time.deltaTime * suavidade);
        }
    }
}