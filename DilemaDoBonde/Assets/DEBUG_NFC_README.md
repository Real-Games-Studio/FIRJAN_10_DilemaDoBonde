# Debug NFC - Solucionando Problemas de Leitura

## 🔧 **MELHORIAS IMPLEMENTADAS**

### **1. Auto-Detecção de Componentes**
- **NFCReceiver**: Busca automaticamente na cena se não configurado
- **ServerComunication**: Busca automaticamente na cena se não configurado
- **Logs informativos**: Confirma inicialização bem-sucedida

### **2. Logs de Debug Detalhados**
- **StartNFCSession**: Mostra quando é ativado
- **OnNFCConnected**: Mostra estado atual (isWaitingForNFC, gameResultsSent)
- **Inicialização**: Confirma se componentes foram encontrados

### **3. Logs Limpos**
- **Removido spam**: Log "Aguardando leitor NFC..." só aparece durante sessão ativa
- **Debug focado**: Apenas logs relevantes para troubleshooting

## 🔍 **COMO DEBUGAR PROBLEMAS DE NFC**

### **Passo 1: Verificar Inicialização**
Ao iniciar o jogo, deve aparecer no Console:
```
NFCReceiver inicializado com sucesso!
ServerComunication inicializado: 127.0.0.1:8080
```

### **Passo 2: Verificar Ativação da Sessão NFC**
Na tela de resultados (3 segundos após aparecer), deve aparecer:
```
[DEBUG] StartNFCSession chamado
[DEBUG] Ativando sessão NFC...
[DEBUG] Painel NFC ativado, isWaitingForNFC = true
[DEBUG] Feedback mostrado na ResultScreen
```

### **Passo 3: Verificar Leitura do Cartão**
Quando aproximar o cartão NFC:
```
[DEBUG] NFC Conectado: [ID_DO_CARTAO] no leitor [NOME_DO_LEITOR]
[DEBUG] Estado atual - isWaitingForNFC: True, gameResultsSent: False
[DEBUG] Processando NFC - iniciando envio de dados...
```

## ❌ **PROBLEMAS POSSÍVEIS E SOLUÇÕES**

### **Problema 1: Componentes não encontrados**
**Sintoma**: Erro "NFCReceiver não encontrado" ou "ServerCommunication não encontrado"
**Solução**: 
- Verificar se os prefabs `[Adicionar na Cena] NFC` e `[Adicionar na Cena] Server` estão na cena
- Verificar se os GameObjects estão ativos

### **Problema 2: NFC conecta mas nada acontece**
**Sintoma**: Aparece "NFC Conectado" mas não processa
**Possível Causa**: `isWaitingForNFC = false`
**Verificar**:
- Se StartNFCSession foi chamado na ResultScreen
- Se não passou do timeout de 30 segundos
- Se gameResultsSent não está true de uma sessão anterior

### **Problema 3: Leitor NFC não detectado**
**Sintoma**: Erro "SmartCardException" no Console
**Solução**: 
- Verificar se o serviço Windows Smart Card está rodando
- Conectar o leitor NFC físico
- Reiniciar o Unity

### **Problema 4: StartNFCSession não é chamado**
**Sintoma**: Não aparece "[DEBUG] StartNFCSession chamado"
**Verificar**:
- Se está na ResultScreen
- Se passou 3 segundos após entrar na tela
- Se NFCGameManager.Instance existe

## 🧪 **TESTE COMPLETO**

### **1. Teste de Inicialização**
```
1. Iniciar jogo
2. Verificar logs de inicialização no Console
3. Confirmar que não há erros vermelhos
```

### **2. Teste de Sessão NFC**
```
1. Jogar até o final
2. Esperar 3 segundos na tela de resultados
3. Verificar logs de ativação da sessão
4. Verificar feedback "Aguardando cartão NFC..."
```

### **3. Teste de Leitura**
```
1. Com sessão ativa, aproximar cartão NFC
2. Verificar logs de detecção
3. Verificar processamento e envio
4. Verificar feedback "Dados gravados com sucesso!"
```

## 🔧 **CHECKLIST DE CONFIGURAÇÃO**

- ✅ **Prefab NFC** na cena: `/[Adicionar na Cena] NFC`
- ✅ **Prefab Server** na cena: `/[Adicionar na Cena] Server`
- ✅ **NFCGameManager** configurado no GameObject NFC
- ✅ **Referências** preenchidas (auto-detecta se vazias)
- ✅ **Leitor NFC** conectado ao PC
- ✅ **Smart Card Service** rodando no Windows

**Agora os logs detalhados vão ajudar a identificar exatamente onde está o problema! 🔍**