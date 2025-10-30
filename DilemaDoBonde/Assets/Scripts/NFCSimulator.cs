using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace _4._NFC_Firjan.Scripts.NFC
{
    public class NFCSimulator : MonoBehaviour
    {
        [Header("Test Data")]
        [Tooltip("ID do cartão NFC simulado")]
        public string simulatedCardId = "TEST_CARD_001";
        
        [Tooltip("Nome do leitor NFC simulado")]
        public string simulatedReaderName = "Virtual NFC Reader";

        [Header("Events")]
        [Tooltip("Evento chamado quando NFC é lido (pressionar F1 no editor)")]
        public UnityEvent<string, string> OnNFCRead;

        private void Update()
        {
#if UNITY_EDITOR
            if (Keyboard.current != null && Keyboard.current[Key.F1].wasPressedThisFrame)
            {
                SimulateNFCRead();
            }
#endif
        }

        public void SimulateNFCRead()
        {
            Debug.Log($"<color=cyan>[NFC Simulator]</color> NFC lido - Card ID: {simulatedCardId}, Reader: {simulatedReaderName}");
            OnNFCRead?.Invoke(simulatedCardId, simulatedReaderName);
        }

        private void OnGUI()
        {
#if UNITY_EDITOR
            GUILayout.BeginArea(new Rect(10, 10, 250, 80));
            GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
            GUILayout.Box("NFC Simulator (Editor Only)");
            GUI.backgroundColor = Color.white;
            GUILayout.Label($"Card ID: {simulatedCardId}");
            GUILayout.Label("Pressione F1 para simular NFC");
            GUILayout.EndArea();
#endif
        }
    }
}
