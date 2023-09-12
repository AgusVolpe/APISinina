using System.Net;

namespace TPFinalBitwise.Models
{
    public class RespuestaAPI
    {
        public RespuestaAPI()
        {
            MensajesError = new List<string>();
        }

        public HttpStatusCode StatusCode { get; set; }
        public bool EsExitoso { get; set; }
        public List<string> MensajesError { get; set; }
        public object Resultado { get; set; }
    }
}
