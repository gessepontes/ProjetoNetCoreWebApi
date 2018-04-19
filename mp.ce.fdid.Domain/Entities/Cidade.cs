
namespace mp.ce.fdid.Domain.Entities
{
    public class Cidade
    {
        public int iCodCidade { get; set; }
        public int iCodEstado { get; set; }
        public string sNomeCidade { get; set; }

        public Estado Estado { get; set; }

    }
}
