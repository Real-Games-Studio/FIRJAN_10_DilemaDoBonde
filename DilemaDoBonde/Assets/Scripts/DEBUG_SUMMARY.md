# DEBUG SUMMARY - Sistema de Telas

## 🔴 PROBLEMA PRINCIPAL IDENTIFICADO

**A tela inicial está configurada como "CTA" mas essa tela NÃO EXISTE na cena!**

Quando `ScreenManager.SetCallScreen("CTA")` é chamado:
1. O evento `CallScreen` é disparado para todas as telas
2. Cada `CanvasScreen` verifica se `screenName == "CTA"`
3. Como NENHUMA tela tem o nome "CTA", todas chamam `TurnOff()`
4. Resultado: TODAS as telas ficam desativadas e o jogo parece travado

## 🔍 POR QUE FUNCIONA NO EDITOR MAS NÃO NA BUILD?

No Editor, você pode estar:
- Iniciando com uma tela já ativa manualmente
- Tendo comportamentos diferentes de serialização
- Cache do Editor mascarando o problema

Na Build:
- Tudo começa do zero, sem cache
- A serialização é mais estrita
- O problema se manifesta claramente

## ✅ SOLUÇÃO IMEDIATA

### Opção 1: Usar o Menu do Editor (RECOMENDADO)
1. No Unity Editor, vá em `Tools > Fix Screen System - Set Initial to IdleScreen`
2. Salve a cena
3. Faça uma nova build

### Opção 2: Manual
1. Selecione o GameObject `[MUST BE ON SCENE] ScreenCanvasController`
2. No Inspector, mude o campo `Inicial Screen` de "CTA" para **"IdleScreen"**
3. Salve a cena
4. Faça uma nova build

## 📋 TELAS DISPONÍVEIS NO PROJETO

- ✅ `IdleScreen` - Tela inicial recomendada
- ✅ `DilemmaScreen` - Tela de dilema
- ✅ `ChoiceScreen` - Tela de escolha
- ✅ `ResultScreen` - Tela de resultado
- ❌ `CTA` - **NÃO EXISTE!**

## 🛠️ MELHORIAS ADICIONADAS

### 1. Sistema de Debug Completo
Logs adicionados em:
- `CanvasScreen.Awake()` - Rastreia inicialização de telas
- `CanvasScreen.CallScreenListner()` - Rastreia chamadas de tela
- `CanvasScreen.TurnOn()/TurnOff()` - Rastreia ativação/desativação
- `ScreenManager.SetCallScreen()` - Rastreia requisições de tela
- `ScreenCanvasController.Start()` - Rastreia inicialização do sistema

### 2. ScreenSystemDebugger.cs
Script que loga o estado de todas as telas a cada 2 segundos:
- Quais telas estão ativas
- Valores de alpha, interactable, blocksRaycasts
- Estado do ScreenManager

**Como usar:**
1. Adicione o componente `ScreenSystemDebugger` no GameObject `[MUST BE ON SCENE] ScreenCanvasController`
2. Faça uma build
3. Analise os logs

### 3. ScreenSystemValidator.cs (Editor Only)
Menu no Editor para validar a configuração:
- `Tools > Validate Screen System` - Valida todo o sistema
- `Tools > Fix Screen System - Set Initial to IdleScreen` - Corrige automaticamente

### 4. Proteções Contra Erros
- Validação de `screenName` null/vazio
- Verificação se tela inicial existe
- Sincronização dos campos `data` e `screenData`
- Stack trace quando telas são desativadas

## 📊 LOGS ESPERADOS NA BUILD

### Startup Normal (após correção):
```
[ScreenCanvasController] Start - Beginning initialization
[ScreenCanvasController] Configured initial screen: 'IdleScreen'
[ScreenCanvasController] Found 4 CanvasScreen components in scene
[ScreenCanvasController] Available screen: 'IdleScreen' on GameObject 'IdleScreen'
[ScreenCanvasController] Available screen: 'DilemmaScreen' on GameObject 'DilemmaScreen'
[ScreenCanvasController] Available screen: 'ChoiceScreen' on GameObject 'ChoiceScreen'
[ScreenCanvasController] Available screen: 'ResultScreen' on GameObject 'ResultScreen'
[ScreenCanvasController] Calling initial screen: 'IdleScreen'
[ScreenManager] SetCallScreen called - Screen: 'IdleScreen', Listeners: 4
[CanvasScreen] 'IdleScreen' CallScreenListner - Requested: 'IdleScreen', Match: True
[CanvasScreen] 'IdleScreen' TurnOn - GameObject: IdleScreen
[CanvasScreen] 'DilemmaScreen' CallScreenListner - Requested: 'IdleScreen', Match: False
[CanvasScreen] 'DilemmaScreen' TurnOff - GameObject: DilemmaScreen
...
```

### Startup com Erro (problema atual):
```
[ScreenCanvasController] Configured initial screen: 'CTA'
[ScreenCanvasController] CRITICAL ERROR - Initial screen 'CTA' does NOT EXIST in scene!
[ScreenManager] SetCallScreen called - Screen: 'CTA', Listeners: 4
[CanvasScreen] 'IdleScreen' CallScreenListner - Requested: 'CTA', Match: False
[CanvasScreen] 'IdleScreen' TurnOff - GameObject: IdleScreen
[CanvasScreen] 'DilemmaScreen' CallScreenListner - Requested: 'CTA', Match: False
[CanvasScreen] 'DilemmaScreen' TurnOff - GameObject: DilemmaScreen
...
```

## 📁 ARQUIVOS MODIFICADOS

1. `/Assets/1. Project/Scripts/CanvasScreen/CanvasScreen.cs` - Logs e validações
2. `/Assets/1. Project/Scripts/CanvasScreen/ScreenManager.cs` - Logs
3. `/Assets/1. Project/Scripts/CanvasScreen/ScreenCanvasController.cs` - Logs e validação

## 📁 ARQUIVOS CRIADOS

1. `/Assets/Scripts/ScreenSystemDebugger.cs` - Debug runtime
2. `/Assets/Scripts/ScreenSystemValidator.cs` - Validação no Editor
3. `/Assets/Scripts/DEBUG_BUILD_INSTRUCTIONS.txt` - Instruções de debug
4. `/Assets/Scripts/DEBUG_SUMMARY.md` - Este documento

## 🔄 PRÓXIMOS PASSOS

1. ✅ Execute `Tools > Validate Screen System` no Editor
2. ✅ Se houver erro, execute `Tools > Fix Screen System - Set Initial to IdleScreen`
3. ✅ Salve a cena
4. ✅ Faça uma nova build
5. ✅ Teste a build - agora deve funcionar!
6. 📋 (Opcional) Adicione `ScreenSystemDebugger` para monitoramento contínuo
7. 🧹 (Opcional) Remova os logs de debug depois que tudo funcionar

## ⚠️ IMPORTANTE

Sempre execute `Tools > Validate Screen System` antes de fazer uma build!
Isso evitará problemas como este no futuro.

---

**Data de criação:** $(Get-Date)
**Unity Version:** 6000.0
**Status:** Debugging implementado - Aguardando correção e teste
