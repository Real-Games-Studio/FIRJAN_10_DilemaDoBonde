# Sistema de Input Atualizado - Horizontal Axis + Mudança de Idioma

## ✅ **MUDANÇAS IMPLEMENTADAS**

### 🎮 **1. Novo Sistema de Input com Horizontal Axis**
- **Substituído**: Fire1/Fire2 → Horizontal Axis
- **Mapeamento**:
  - **Horizontal -1** (Esquerda) = Fire1 (Opção 1)
  - **Horizontal +1** (Direita) = Fire2 (Opção 2)
- **Detecção única**: Só detecta input **uma vez por movimento** (não repetir se manter pressionado)

### 🌐 **2. Mudança de Idioma na Tela Inicial**
- **Na IdleScreen**:
  - **Horizontal -1** (Esquerda) = Muda para **Português**
  - **Horizontal +1** (Direita) = Muda para **Inglês**
- **Nas outras telas**: Funciona normalmente como opções 1 e 2

### 🔧 **3. Lógica de Input Inteligente**
- **Threshold de 0.5**: Precisa mover mais da metade para registrar
- **Reset em 0.3**: Volta ao centro quando input < 0.3
- **Estado controlado**: `horizontalInputProcessed` previne repetições

## 🛠 **MODIFICAÇÕES TÉCNICAS**

### **DilemmaInputHandler.cs**
```csharp
// Novas variáveis de controle
private float lastHorizontalInput = 0f;
private bool horizontalInputProcessed = false;

// Nova função de input horizontal
void HandleHorizontalAxisInput()
{
    float horizontalInput = Input.GetAxis("Horizontal");
    
    // Detectar mudança de estado - só processa quando há movimento novo
    if (!horizontalInputProcessed)
    {
        if (horizontalInput < -0.5f) // Esquerda
        {
            OnHorizontalInput(-1);
            horizontalInputProcessed = true;
        }
        else if (horizontalInput > 0.5f) // Direita
        {
            OnHorizontalInput(1);
            horizontalInputProcessed = true;
        }
    }
    
    // Reset quando voltar ao centro
    if (Mathf.Abs(horizontalInput) < 0.3f)
    {
        horizontalInputProcessed = false;
    }
}

// Lógica contextual baseada na tela atual
void OnHorizontalInput(int direction)
{
    string currentScreenName = ScreenManager.GetCurrentScreenName();
    
    if (currentScreenName == DilemmaGameController.Instance.idleScreenName)
    {
        // Na tela inicial: mudança de idioma
        if (direction == -1) LanguageManager.Instance.SetPortuguese();
        else if (direction == 1) LanguageManager.Instance.SetEnglish();
    }
    else
    {
        // Outras telas: mapear para números 1 e 2
        int mappedNumber = (direction == -1) ? 1 : 2;
        DilemmaGameController.Instance.OnNumberInput(mappedNumber);
    }
}
```

### **ScreenManager.cs**
```csharp
// Novo método para obter tela atual
public static string GetCurrentScreenName()
{
    return currentScreenName;
}
```

## 🎯 **COMPORTAMENTO RESULTANTE**

### **📺 Na Tela Inicial (IdleScreen)**
- 🕹️ **Joystick Esquerda** → Português
- 🕹️ **Joystick Direita** → Inglês
- ⌨️ **Teclas 1, 2, 3** → Funcionam normalmente

### **🎮 Durante o Jogo (DilemmaScreen)**
- 🕹️ **Joystick Esquerda** → Escolhe Opção 1
- 🕹️ **Joystick Direita** → Escolhe Opção 2
- ⌨️ **Teclas 1, 2** → Funcionam normalmente

### **📊 Na Tela de Resultados (ResultScreen)**
- 🕹️ **Joystick Esquerda** → Funciona como tecla 1
- 🕹️ **Joystick Direita** → Funciona como tecla 2
- ⌨️ **Tecla 3** → Reiniciar jogo
- ⌨️ **Tecla 4** → Ativar NFC

## ✅ **COMPATIBILIDADE MANTIDA**
- ✅ **Todas as teclas numéricas** continuam funcionando
- ✅ **Fire3** mantido para tecla 3
- ✅ **NFC** continua funcionando com tecla 4
- ✅ **Sistema antigo** + **novo sistema** funcionam juntos

## 🎮 **TESTE RECOMENDADO**

1. **Tela Inicial**: Teste mudança de idioma com joystick
2. **Durante Jogo**: Teste escolhas com joystick
3. **Segure o Joystick**: Confirme que não repete input
4. **Teclas**: Confirme que teclado ainda funciona
5. **NFC**: Confirme que tecla 4 ainda ativa NFC

**Sistema agora suporta controle via joystick com detecção inteligente! 🎮**