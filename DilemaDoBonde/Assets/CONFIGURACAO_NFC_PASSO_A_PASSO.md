# Configuração NFC - Passo a Passo Manual

## 📋 **PASSO 1: Adicionar os Prefabs Obrigatórios**

1. Na pasta `/Assets/4. NFC Firjan/Prefabs/` você encontrará dois prefabs:
   - `[Adicionar na Cena] NFC.prefab`
   - `[Adicionar na Cena] Server.prefab`

2. **Arraste ambos os prefabs para sua cena:**
   - Drag and drop diretamente na Hierarchy
   - Ou selecione-os e pressione Ctrl+D na cena

✅ **Resultado:** Você deve ter dois GameObjects na cena com os componentes NFC configurados.

## 📋 **PASSO 2: Criar o NFCGameManager**

1. **Crie um GameObject vazio:**
   - Right-click na Hierarchy → Create Empty
   - Renomeie para "NFC Game Manager"

2. **Adicione o script NFCGameManager:**
   - Selecione o GameObject "NFC Game Manager"
   - No Inspector, clique "Add Component"
   - Digite "NFCGameManager" e adicione

## 📋 **PASSO 3: Configurar as Referências**

No componente **NFCGameManager**, configure:

### **NFC Configuration:**
- **nfcReceiver:** Arraste o GameObject que contém o componente `NFCReceiver` (do prefab NFC)
- **serverCommunication:** Arraste o GameObject que contém o componente `ServerComunication` (do prefab Server)

### **Server Configuration:**
- **serverIP:** "127.0.0.1" (ou IP do seu servidor)
- **serverPort:** 8080 (ou porta do seu servidor)

### **Game Configuration:**
- **gameId:** 10 (ID do jogo Dilema do Bonde)

### **Score Mapping - Realista:**
- **realistLogicalReasoning:** 9
- **realistSelfAwareness:** 5
- **realistDecisionMaking:** 6

### **Score Mapping - Empático:**
- **empatheticLogicalReasoning:** 8
- **empatheticSelfAwareness:** 6
- **empatheticDecisionMaking:** 6

## 📋 **PASSO 4: Criar o UI do NFC (Opcional)**

Se quiser uma interface visual para o NFC:

1. **Encontre o Canvas principal da sua cena**
2. **Crie um painel NFC:**
   - Right-click no Canvas → UI → Panel
   - Renomeie para "NFCPromptPanel"
   - Configure para ocupar a tela toda (Anchor: stretch)
   - Adicione uma cor de fundo semi-transparente

3. **Adicione textos informativos:**
   - Adicione um Text (TMP) para status
   - Adicione outro Text (TMP) para instruções
   - Adicione um Button para "Pular NFC"

4. **Configure as referências no NFCGameManager:**
   - **nfcPromptPanel:** Arraste o painel criado
   - **nfcStatusText:** Arraste o primeiro texto
   - **nfcInstructionText:** Arraste o segundo texto
   - **skipNFCButton:** Arraste o botão

5. **Desative o painel inicialmente:**
   - Desmarque o checkbox do NFCPromptPanel no Inspector

## 📋 **PASSO 5: Verificar o Sistema Existente**

Certifique-se de que sua cena já possui:
- ✅ **DilemmaGameController** (já existe no seu projeto)
- ✅ **Canvas com telas do jogo** (IdleScreen, DilemmaScreen, etc.)
- ✅ **DilemmaInputHandler** (para capturar teclas)

## 📋 **PASSO 6: Testar a Configuração**

1. **Execute o jogo**
2. **Complete um jogo de dilemas**
3. **Na tela de resultados:**
   - O sistema NFC deve ativar automaticamente após 3 segundos
   - Ou pressione a tecla **4** para ativar manualmente

## 🔧 **Configurações dos Prefabs**

### **Prefab NFC (NFCReceiver):**
- Detecta automaticamente cartões NFC
- Gera eventos quando cartão é conectado/desconectado
- Não precisa configuração adicional

### **Prefab Server (ServerComunication):**
- **IP:** Será configurado pelo NFCGameManager
- **Port:** Será configurado pelo NFCGameManager
- Não precisa configuração manual

## ⚠️ **Pontos Importantes**

1. **Os dois prefabs SÃO OBRIGATÓRIOS** - eles contêm os componentes NFC essenciais
2. **NFCGameManager deve referenciar ambos** - ele coordena a comunicação entre eles
3. **O UI é opcional** - o sistema funciona sem interface visual
4. **Use as pontuações exatas** - seguem o mapeamento da documentação da API

## 🚀 **Resumo Final**

Sua Hierarchy deve ficar assim:
```
Scene
├── [Outros objetos existentes]
├── [Adicionar na Cena] NFC (prefab)
├── [Adicionar na Cena] Server (prefab)  
├── NFC Game Manager (com NFCGameManager script)
└── Canvas
    └── NFCPromptPanel (opcional)
```

**Pronto!** O sistema NFC estará configurado e funcionando. 🎉