# Sistema "Dilema do Bonde"

Este sistema implementa um jogo interativo de dilemas éticos que será usado em eventos.

## Estrutura do Sistema

### Scripts Principais
- `DilemmaGameController.cs` - Controlador principal do jogo
- `DilemmaInputHandler.cs` - Gerencia inputs do teclado
- `DilemmaConfig.cs` - Classes de dados para deserialização JSON

### Scripts de Telas
- `IdleScreen.cs` - Tela inicial de call-to-action
- `DilemmaScreen.cs` - Tela que mostra os dilemas
- `ChoiceScreen.cs` - Tela de confirmação da escolha (5 segundos)
- `ResultScreen.cs` - Tela final com perfil do jogador

### Configuração JSON
O arquivo `/StreamingAssets/config.json` contém:
- **timeoutSeconds**: Timeout em segundos (60s por padrão)
- **choiceDisplayTime**: Tempo de exibição da tela de confirmação da escolha (5s por padrão)
- **resultDisplayTime**: Tempo de exibição da tela de resultado final (10s por padrão)
- **profiles**: Perfis "O realista" e "O empático" com nomes e descrições
- **dilemmas**: Array com os 5 dilemas e suas opções A e B

## Fluxo do Jogo

1. **Tela Idle**: Pressione 1 ou 2 para começar
2. **Dilema**: Mostra dilema atual, pressione 1 (opção A) ou 2 (opção B)
3. **Confirmação**: Mostra escolha por 5 segundos
4. **Próximo Dilema**: Repete para os 5 dilemas
5. **Resultado**: Mostra perfil final baseado nas respostas

## Setup na Scene

1. Adicione o prefab `[MUST BE ON SCENE] ScreenCanvasController` à scene
2. Crie um GameObject vazio e adicione os componentes:
   - `DilemmaGameController`
   - `DilemmaInputHandler`
3. Configure as telas com os seguintes nomes:
   - "IdleScreen"
   - "DilemmaScreen" 
   - "ChoiceScreen"
   - "ResultScreen"

## Sistema de Pontuação

- Mais respostas tipo "realist" = Perfil "O realista"
- Mais respostas tipo "empathetic" = Perfil "O empático"
- Timeout de 60 segundos volta para tela inicial
- Tela de confirmação da escolha exibe por 5 segundos (configurável)
- Tela de resultado retorna automaticamente após 10 segundos (configurável)

## Inputs Suportados

- Teclas numéricas: 1, 2
- Teclado numérico: Num 1, Num 2

## Configuração dos Dilemas

Os 5 dilemas estão salvos no JSON com as seguintes informações para cada um:
- ID, título, descrição, pergunta
- Opção A e B com texto e tipo (realist/empathetic)

O sistema é totalmente configurável através do arquivo JSON na pasta StreamingAssets.