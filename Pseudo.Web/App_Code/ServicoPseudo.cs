using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// NOTE: If you change the class name "ServicoPseudo" here, you must also update the reference to "ServicoPseudo" in Web.config.
public class ServicoPseudo : IServicoPseudo {
    #region IServicoPseudo Members

    List<Item> IServicoPseudo.CarregarItens() {
        List<Item> itens = new List<Item>();
        itens.Add(new Item("Content/Imagens/Casa.png", "Casa", 200));
        itens.Add(new Item(String.Empty, "Outro", 100));
        return itens;
    }

    #endregion
}
