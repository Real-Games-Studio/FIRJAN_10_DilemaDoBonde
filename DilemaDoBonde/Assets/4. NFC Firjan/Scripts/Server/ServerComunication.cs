using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace _4._NFC_Firjan.Scripts.Server
{
	public class ServerComunication : MonoBehaviour
	{
		public string Ip;
		public int Port;

		[Header("Timeout Configuration")]
		[Tooltip("Timeout em segundos para requisições HTTP (padrão: 5 segundos)")]
		public int httpTimeoutSeconds = 5;

		private HttpClient _client;

		private void Awake()
		{
			_client = new HttpClient
			{
				Timeout = TimeSpan.FromSeconds(httpTimeoutSeconds)
			};
			Debug.Log($"[ServerComunication] HttpClient inicializado com timeout de {httpTimeoutSeconds} segundos");
		}

		private string GetFullEndGameUrl(string nfcId)
		{
			return $"http://{Ip}:{Port}/users/{nfcId}/endgame";
		}

		private string GetFullNfcUrl(string nfcId)
		{
			return $"http://{Ip}:{Port}/users/{nfcId}";
		}

		public async Task<HttpStatusCode> UpdateNfcInfoFromGame(GameModel gameInfo)
		{
			try
			{
				var url = GetFullNfcUrl(gameInfo.nfcId);
				var request = new HttpRequestMessage(HttpMethod.Post, url);
				var content = new StringContent(gameInfo.ToString());
				request.Content = content;
				
				Debug.Log($"[ServerComunication] Enviando para {url}: {gameInfo.ToString()}");
				
				var response = await _client.SendAsync(request);
				
				Debug.Log($"[ServerComunication] Resposta recebida: {response.StatusCode}");
				return response.StatusCode;
			}
			catch (TaskCanceledException)
			{
				Debug.LogWarning($"[ServerComunication] Timeout ao conectar com servidor em {Ip}:{Port} (timeout: {httpTimeoutSeconds}s)");
				return HttpStatusCode.RequestTimeout;
			}
			catch (HttpRequestException ex)
			{
				Debug.LogError($"[ServerComunication] Erro de conexão HTTP: {ex.Message}");
				return HttpStatusCode.ServiceUnavailable;
			}
			catch (Exception ex)
			{
				Debug.LogError($"[ServerComunication] Erro inesperado: {ex.Message}");
				return HttpStatusCode.InternalServerError;
			}
		}

		public async Task<EndGameResponseModel> GetNfcInfo(string nfcId)
		{
			try
			{
				var url = GetFullNfcUrl(nfcId);
				var request = new HttpRequestMessage(HttpMethod.Get, url);
				Debug.Log($"[ServerComunication] GET request para {url}");
				
				var response = await _client.SendAsync(request);
				
				Debug.Log($"[ServerComunication] Response code: {response.StatusCode}");
				if (response.StatusCode == HttpStatusCode.OK)
				{
					return JsonConvert.DeserializeObject<EndGameResponseModel>(await response.Content.ReadAsStringAsync());
				}
				else
				{
					return null;
				}
			}
			catch (TaskCanceledException)
			{
				Debug.LogWarning($"[ServerComunication] Timeout ao buscar dados do NFC (timeout: {httpTimeoutSeconds}s)");
				return null;
			}
			catch (Exception ex)
			{
				Debug.LogError($"[ServerComunication] Erro ao buscar dados: {ex.Message}");
				return null;
			}
		}
		
		public async Task<EndGameResponseModel> PostNameForEndGameInfo(EndGameRequestModel endGameRequestModel, string nfcId)
		{
			try
			{
				var url = GetFullEndGameUrl(nfcId);
				var request = new HttpRequestMessage(HttpMethod.Post, url);
				var content = new StringContent(endGameRequestModel.ToString());
				request.Content = content;
				
				Debug.Log($"[ServerComunication] POST EndGame para {url}: {endGameRequestModel.ToString()}");
				
				var response = await _client.SendAsync(request);
				
				Debug.Log($"[ServerComunication] Response code: {response.StatusCode}");
				if (response.StatusCode == HttpStatusCode.OK)
				{
					return Newtonsoft.Json.JsonConvert.DeserializeObject<EndGameResponseModel>(await response.Content.ReadAsStringAsync());
				}
				else
				{
					Debug.LogWarning($"[ServerComunication] Response code não OK: {response.StatusCode}");
					return null;
				}
			}
			catch (TaskCanceledException)
			{
				Debug.LogWarning($"[ServerComunication] Timeout ao enviar endgame (timeout: {httpTimeoutSeconds}s)");
				return null;
			}
			catch (Exception ex)
			{
				Debug.LogError($"[ServerComunication] Erro ao enviar endgame: {ex.Message}");
				return null;
			}
		}
		
	}
}
