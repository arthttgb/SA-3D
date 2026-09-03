using UnityEngine;
using System.Collections;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("Configurações de Spawn")]
    public GameObject zombiePrefab;
    public Transform[] spawnPoints;
    
    [Header("Configurações da Horda")]
    public int inimigosIniciais = 5;
    public float tempoDescansoFinal = 3f; // Tempo após matar o último zumbi
    public TextMeshProUGUI textoHUD;

    private int hordaAtual = 1; // Começamos na horda 1
    private int inimigosVivos = 0;
    private bool esperandoJogador = false;

    void Start()
    {
        // Começa a rotina
        StartCoroutine(FluxoDoJogo());
    }

    void Update()
    {
        // Se estiver esperando o jogador e ele apertar F
        if (esperandoJogador && Input.GetKeyDown(KeyCode.F))
        {
            esperandoJogador = false;
            textoHUD.text = ""; 
        }
    }

    IEnumerator FluxoDoJogo()
    {
        while (true)
        {
            // 1. FASE DE PREPARAÇÃO: Pergunta se quer começar a horda atual
            textoHUD.text = $"Horda {hordaAtual} pronta! Pressione [F] para começar.";
            esperandoJogador = true;

            // Fica parado aqui até o Update detectar o aperto do F
            yield return new WaitUntil(() => esperandoJogador == false);

            // 2. FASE DE SPAWN: Cria os inimigos
            yield return StartCoroutine(SpawnarHorda());

            // 3. FASE DE COMBATE: Espera até que o contador de inimigos chegue a 0
            // Adicionamos um pequeno delay para não checar antes dos zumbis aparecerem
            yield return new WaitForSeconds(1f); 
            yield return new WaitUntil(() => inimigosVivos <= 0);

            // 4. FASE DE VITÓRIA: Mensagem de conclusão
            textoHUD.text = "Horda Concluída! Recuperando o fôlego...";
            yield return new WaitForSeconds(tempoDescansoFinal);
            
            hordaAtual++; // Prepara a próxima horda
        }
    }

    IEnumerator SpawnarHorda()
    {
        int totalParaSpawnar = inimigosIniciais + (hordaAtual * 2); // Aumenta 2 zumbis por horda
        
        for (int i = 0; i < totalParaSpawnar; i++)
        {
            SpawnarZumbi();
            yield return new WaitForSeconds(0.5f); // Intervalo entre o nascimento de cada zumbi
        }
    }

    void SpawnarZumbi()
    {
        if (spawnPoints.Length == 0) return;

        Transform pontoSorteado = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(zombiePrefab, pontoSorteado.position, pontoSorteado.rotation);
        
        inimigosVivos++; // Aumenta o contador quando o zumbi nasce
    }

    public void InimigoMorreu()
    {
        inimigosVivos--; // Diminui o contador quando o zumbi morre
    }
}