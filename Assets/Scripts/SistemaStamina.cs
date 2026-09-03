using UnityEngine;
using UnityEngine.UI; // Necessário para controlar o Slider da tela

public class SistemaStamina : MonoBehaviour
{
    [Header("Configurações de Velocidade")]
    public float velocidadeAndar = 6f;   // Substitui os 6f originais do seu script
    public float velocidadeCorrer = 12f; // Velocidade ao correr

    [Header("Configurações de Fôlego")]
    public float staminaMaxima = 100f;
    public float staminaAtual;
    public float custoDrenagem = 20f;    // Quanto gasta por segundo
    public float ganhoRegen = 15f;       // Quanto recupera por segundo

    [Header("Referências da UI")]
    public Slider barraStamina;          // Arraste o Slider do Canvas aqui

    private PlayerMovement mov;          // Guarda o link para o seu script original

    void Start()
    {
        // Encontra o seu script PlayerMovement que está no mesmo objeto
        mov = GetComponent<PlayerMovement>();
        
        staminaAtual = staminaMaxima;

        if (barraStamina != null)
        {
            barraStamina.maxValue = staminaMaxima;
            barraStamina.value = staminaMaxima;
        }
    }

    void Update()
    {
        if (mov == null) return;

        // Verifica se o jogador está apertando Shift e se movendo
        bool apertandoCorrer = Input.GetKey(KeyCode.LeftShift);
        
        // Usa o 'controller' do seu script original para ver se há movimento físico
        bool estaSeMovendo = mov.controller.velocity.magnitude > 0.1f;

        if (apertandoCorrer && estaSeMovendo && staminaAtual > 0)
        {
            // 🏃 CORRENDO: Altera o 'speed' público do seu script de fora para dentro
            mov.speed = velocidadeCorrer;
            staminaAtual -= custoDrenagem * Time.deltaTime;
        }
        else
        {
            // 🚶 ANDANDO: Devolve o valor de caminhada para o seu script original
            mov.speed = velocidadeAndar;
            
            if (staminaAtual < staminaMaxima)
            {
                staminaAtual += ganhoRegen * Time.deltaTime;
            }
        }

        // Garante que a stamina não passe dos limites (0 a 100)
        staminaAtual = Mathf.Clamp(staminaAtual, 0, staminaMaxima);
        
        // Atualiza a barra amarela na tela
        if (barraStamina != null) 
            barraStamina.value = staminaAtual;
    }
}