using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JassBot.Messages;

namespace JassBot.Bot
{
    /// <summary>
    /// Some code from http://codereview.stackexchange.com/questions/41591/websockets-client-code-and-making-it-production-ready
    /// Documentation: https://msdn.microsoft.com/en-us/library/system.net.websockets.clientwebsocket(v=vs.110).aspx?cs-save-lang=1
    /// 
    /// Maybe another good source: https://gist.github.com/xamlmonkey/4737291
    /// </summary>
    public class WebSocketClient
    {
        private ClientWebSocket _webSocket;
        private readonly UTF8Encoding _encoder = new UTF8Encoding();

        private const int ReceiveChunkSize = 1024;

        private WebSocketListener _webSocketListener;

        public async void ConnectToServer(WebSocketListener webSocketListener, Uri serverUri)
        {
            _webSocketListener = webSocketListener;
            await ConnectToSocket(serverUri);
            Console.WriteLine("Web socket closed");
            Console.ReadKey();
        }

        public async void Send(string dataAsJson, string playerName)
        {
            var buffer = _encoder.GetBytes(dataAsJson);
            await _webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            Console.WriteLine("--> " + playerName + " sends: " + dataAsJson);
        }

        private async Task ConnectToSocket(Uri serverUri)
        {
            try
            {
                _webSocket = new ClientWebSocket();
                await _webSocket.ConnectAsync(serverUri, CancellationToken.None);
                _webSocketListener.Open(this);
                await StartListen();
                _webSocketListener.OnClose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex);
            }
            finally
            {
                _webSocket?.Dispose();
                Console.WriteLine();
            }
        }

        private async Task StartListen()
        {
            var buffer = new byte[ReceiveChunkSize];
            try
            {
                while (_webSocket.State == WebSocketState.Open)
                {
                    var stringResult = new StringBuilder();

                    WebSocketReceiveResult result;
                    do
                    {
                        result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                        }
                        else
                        {
                            var str = Encoding.UTF8.GetString(buffer, 0, result.Count);
                            stringResult.Append(str);
                        }

                    } while (!result.EndOfMessage);

                    var message = stringResult.ToString();

                    if (message.Trim() == "")
                    {
                        Console.WriteLine("Empty message");
                    } else {
                        _webSocketListener.Message(message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                _webSocket.Dispose();
            }
        }
    }
}
