# Configuração do Servidor via JSON

## 🔧 **IMPLEMENTAÇÃO REALIZADA**

### **1. Estrutura do config.json**
Adicionada seção `server` no arquivo `/Assets/StreamingAssets/config.json`:

```json
{
  "timeoutSeconds": 60,
  "choiceDisplayTime": 5,
  "resultDisplayTime": 10,
  "server": {
    "ip": "192.168.0.185",
    "port": 8080
  },
  "profiles": {
    // ... resto da configuração
  }
}
```

### **2. Classe ServerConfig**
Criada classe `/Assets/Scripts/ServerConfig.cs` para:
- ✅ **Carregamento automático**: Lê config.json na inicialização
- ✅ **Deserialização JSON**: Usa Newtonsoft.Json
- ✅ **Fallback seguro**: Valores padrão se arquivo não existir
- ✅ **Reload em runtime**: Permite recarregar configuração
- ✅ **Logs informativos**: Debug de carregamento

### **3. NFCGameManager Atualizado**
- ✅ **Auto-carregamento**: IP/porta vem automaticamente do JSON
- ✅ **Logs detalhados**: Mostra configuração carregada
- ✅ **Método reload**: `ReloadServerConfiguration()` para atualizar em runtime
- ✅ **Compatibilidade**: Mantém campos no Inspector como informativo

### **4. Input para Desenvolvimento**
- ✅ **Tecla 5**: Recarrega configuração do servidor durante desenvolvimento
- ✅ **Feedback visual**: Logs mostram nova configuração aplicada

## 🔄 **FLUXO DE CARREGAMENTO**

### **Inicialização**
```
Start() → InitializeServer() → ServerConfig.Server → 
Carrega config.json → Aplica IP/porta → Debug log
```

### **Arquivo config.json**
```
/Assets/StreamingAssets/config.json
    ↓
ServerConfig.LoadConfiguration()
    ↓ 
JsonConvert.DeserializeObject<GameConfiguration>()
    ↓
NFCGameManager.serverIP/serverPort atualizados
    ↓
ServerComunication.Ip/Port configurados
```

## 🧪 **COMO TESTAR**

### **1. Verificar Carregamento**
```
1. Iniciar o jogo
2. Verificar log: "Configuração carregada: Servidor 192.168.0.185:8080"
3. Verificar log: "ServerComunication inicializado com configuração do JSON"
```

### **2. Alterar Configuração**
```
1. Editar /Assets/StreamingAssets/config.json
2. Alterar server.ip ou server.port
3. Pressionar tecla 5 no jogo (reload)
4. Verificar logs: "Nova configuração aplicada: [novo_ip:porta]"
```

### **3. Teste de Fallback**
```
1. Renomear config.json temporariamente
2. Iniciar jogo
3. Verificar log: "Arquivo config.json não encontrado"
4. Verificar que usa valores padrão (127.0.0.1:8080)
```

## ⚙️ **CONFIGURAÇÕES DISPONÍVEIS**

### **server (novo)**
```json
"server": {
  "ip": "192.168.0.185",    // IP do servidor
  "port": 8080              // Porta do servidor
}
```

### **timeouts (existentes)**
```json
"timeoutSeconds": 60,        // Timeout geral
"choiceDisplayTime": 5,      // Tempo de exibição da escolha
"resultDisplayTime": 10      // Tempo de exibição do resultado
```

## 🔧 **VANTAGENS DA IMPLEMENTAÇÃO**

1. **Flexibilidade**: Mudança de servidor sem recompilar
2. **Ambientes**: Diferentes configs para dev/prod
3. **Manutenção**: Centralizadas em um arquivo
4. **Segurança**: Valores sensíveis fora do código
5. **Debugging**: Reload em runtime para testes

## 📁 **ARQUIVOS MODIFICADOS**

- ✅ **`/Assets/StreamingAssets/config.json`** - Adicionada seção server
- ✅ **`/Assets/Scripts/ServerConfig.cs`** - Nova classe para carregamento
- ✅ **`/Assets/Scripts/NFCGameManager.cs`** - Usa configuração do JSON
- ✅ **`/Assets/1. Project/Scripts/DilemmaInputHandler.cs`** - Tecla 5 para reload

## 🎯 **EXEMPLO DE USO**

### **Desenvolvimento Local**
```json
"server": { "ip": "127.0.0.1", "port": 8080 }
```

### **Servidor de Testes**
```json
"server": { "ip": "192.168.0.185", "port": 8080 }
```

### **Produção**
```json
"server": { "ip": "10.0.0.100", "port": 443 }
```

**🎯 Agora o IP e porta do servidor são carregados dinamicamente do config.json!**