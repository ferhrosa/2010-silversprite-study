using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// NOTE: If you change the interface name "IServicoPseudo" here, you must also update the reference to "IServicoPseudo" in Web.config.
[ServiceContract]
public interface IServicoPseudo {
    [OperationContract]
    List<Item> CarregarItens();
}
[DataContract]
public class Item {
    [DataMember]
    public string Imagem;
    [DataMember]
    public string Descricao;
    [DataMember]
    public int Preco;

    public Item(string imagem, string descricao, int preco) {
        this.Imagem = imagem;
        this.Preco = preco;
        this.Descricao = descricao;
    }
}
