# Sistema NFC - Dilema do Bonde

## Visão Geral

Este sistema permite que os jogadores salvem suas pontuações do jogo "Dilema do Bonde" em um servidor usando cartões NFC. O sistema mapeia os resultados morais (Realista vs Empático) para as habilidades específicas conforme documentado na API Firjan.

## Mapeamento de Pontuações

### Perfil Realista
- **Raciocínio Lógico**: 9 pontos
- **Autoconsciência**: 5 pontos  
- **Tomada de decisão**: 6 pontos

### Perfil Empático
- **Raciocínio Lógico**: 8 pontos
- **Autoconsciência**: 6 pontos
- **Tomada de decisão**: 6 pontos

## Configuração Inicial

### 1. Scripts Principais
Certifique-se de que os seguintes scripts estão no projeto:
- `NFCGameManager.cs` - Gerenciador principal do sistema NFC
- `NFCServerConfig.cs` - Configurações do servidor e pontuações
- `NFCSetupHelper.cs` - Helper para configuração automática
- `NFCDebugTester.cs` - Ferramenta de debug e teste

### 2. Setup Automático
1. Adicione o script `NFCSetupHelper` a qualquer GameObject na cena
2. Configure o `autoSetupOnStart = true`
3. Execute o jogo - o sistema será configurado automaticamente

### 3. Setup Manual

#### 3.1 NFCReceiver
- Adicione um GameObject com o componente `NFCReceiver`
- Este componente já está incluído no pacote NFC Firjan

#### 3.2 ServerComunication  
- Adicione um GameObject com o componente `ServerComunication`
- Configure:
  - **IP**: Endereço do servidor (padrão: 127.0.0.1)
  - **Port**: Porta do servidor (padrão: 8080)

#### 3.3 NFCGameManager
- Adicione um GameObject com o componente `NFCGameManager`
- Configure as referências:
  - **nfcReceiver**: Referência ao NFCReceiver
  - **serverCommunication**: Referência ao ServerComunication
  - **nfcPromptPanel**: Painel UI para interação NFC
  - **nfcStatusText**: Texto de status
  - **nfcInstructionText**: Texto de instruções
  - **skipNFCButton**: Botão para pular NFC

### 4. Configuração do Servidor
Edite os valores no `NFCGameManager` ou crie um `NFCServerConfigAsset`:

```csharp
[Header("Server Configuration")]
public string serverIP = "127.0.0.1";    // IP do servidor
public int serverPort = 8080;            // Porta do servidor

[Header("Game Configuration")]  
public int gameId = 10;                   // ID do jogo (Dilema do Bonde)
```

## Como Funciona

### 1. Fluxo do Jogo
1. Jogador completa o jogo de dilemas morais
2. Sistema calcula se é "Realista" ou "Empático"
3. Tela de resultados é exibida
4. Após 3 segundos, o sistema NFC é ativado automaticamente
5. Jogador pode usar cartão NFC ou pressionar tecla 4

### 2. Integração com NFC
- **Detecção automática**: O sistema detecta quando um cartão NFC é aproximado
- **Envio de dados**: Pontuações são enviadas para o servidor automaticamente
- **Feedback visual**: Interface mostra status da operação
- **Timeout**: Retorna ao menu inicial após 30 segundos

### 3. API do Servidor
O sistema envia dados no formato:

```json
{
  "nfcId": "CARD123",
  "gameId": 10,
  "skill1": 9,    // Raciocínio Lógico
  "skill2": 5,    // Autoconsciência  
  "skill3": 6     // Tomada de decisão
}
```

## Controles

### Durante o Jogo
- **Teclas 1-2**: Escolher opções dos dilemas
- **Tecla 3**: Reiniciar jogo (na tela de resultados)

### Sistema NFC
- **Tecla 4**: Ativar sessão NFC manualmente
- **Cartão NFC**: Aproximar do leitor para salvar pontuação
- **Skip Button**: Pular NFC e retornar ao menu

## Debug e Teste

### NFCDebugTester
Use o script `NFCDebugTester` para testar sem hardware:

- **F1**: Simular conexão NFC
- **F2**: Simular desconexão NFC  
- **F3**: Testar conexão com servidor
- **F4**: Simular resultados do jogo

### Console Debug
O sistema registra informações detalhadas no Console:
- Status de conexão NFC
- Dados enviados ao servidor
- Códigos de resposta HTTP
- Erros de comunicação

## Configuração do UI

### Painel NFC
O sistema espera um painel UI com:
- **Background semi-transparente**
- **Texto de status** (TMP_Text)
- **Texto de instruções** (TMP_Text)  
- **Botão Skip** (Button)

### Auto-criação
O `NFCSetupHelper` pode criar o painel automaticamente se não existir.

## Resolução de Problemas

### NFC não detecta cartões
1. Verificar se o leitor NFC está conectado
2. Confirmar se o `NFCReceiver` está na cena
3. Checar logs do Console para erros

### Servidor não responde
1. Verificar IP e porta do servidor
2. Confirmar se servidor está executando
3. Testar com `NFCDebugTester` (F3)

### UI não aparece
1. Verificar se `nfcPromptPanel` está configurado
2. Usar `NFCSetupHelper` para criar painel automaticamente
3. Confirmar que há um Canvas na cena

## Arquivos Modificados

### Scripts Existentes Modificados:
- `ResultScreen.cs` - Integração com NFC após resultados
- `DilemmaInputHandler.cs` - Suporte para tecla 4
- `DilemmaGameController.cs` - Ativação manual do NFC

### Scripts Novos Criados:
- `NFCGameManager.cs` - Gerenciador principal
- `NFCServerConfig.cs` - Configurações
- `NFCSetupHelper.cs` - Setup automático
- `NFCDebugTester.cs` - Ferramenta de debug

## Requisitos

- Unity 6000.0+
- Pacote NFC Firjan instalado
- Newtonsoft JSON (já incluído)
- TextMeshPro
- New Input System

## Suporte

Para problemas relacionados ao hardware NFC ou API do servidor, consulte a documentação oficial da API Firjan.