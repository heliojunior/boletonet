using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public class ArquivoRetornoCNAB400 : AbstractArquivoRetorno, IArquivoRetorno
    {

        private List<DetalheRetorno> _listaDetalhe = new List<DetalheRetorno>();

        public List<DetalheRetorno> ListaDetalhe
        {
            get { return _listaDetalhe; }
            set { _listaDetalhe = value; }
        }

        #region Construtores

        public ArquivoRetornoCNAB400()
		{
            this.TipoArquivo = TipoArquivo.CNAB400;
        }

        #endregion

        #region Métodos de instância

        public override void LerArquivoRetorno(IBanco banco, Stream arquivo, bool closeStream = true)
        {
            try
            {
                StreamReader stream = new StreamReader(arquivo, System.Text.Encoding.UTF8);
                string linha = "";


                // Lendo o arquivo
                //linha = stream.ReadLine();

                //// Próxima linha (DETALHE)
                //linha = stream.ReadLine();

                //while (DetalheRetorno.PrimeiroCaracter(linha) == "1")
                //{
                //    DetalheRetorno detalhe = banco.LerDetalheRetornoCNAB400(linha);
                //    ListaDetalhe.Add(detalhe);
                //    OnLinhaLida(detalhe, linha);
                //    linha = stream.ReadLine();
                //}

                while ((linha = stream.ReadLine()) != null)
                {
                    if (!String.IsNullOrEmpty(linha))
                    {
                        //DetalheRetorno detalheRetorno = new DetalheRetorno();

                        switch (DetalheRetorno.PrimeiroCaracter(linha))
                        {
                            case "0": //Header de arquivo
                                OnLinhaLida(null, linha);
                                HeaderArquivo400 = banco.LerHeaderArquivoRetornoCNAB400(linha);
                                break;
                            case "1": //Detalhe
                                DetalheRetorno detalhe = banco.LerDetalheRetornoCNAB400(linha);
                                ListaDetalhe.Add(detalhe);
                                OnLinhaLida(detalhe, linha);
                                break;
                            //case "9": //Trailler de arquivo
                            //    OnLinhaLida(null, linha, EnumTipodeLinhaLida.TraillerDeArquivo);
                            //    break;
                        }
                    }
                }

                if (closeStream)
                    stream.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler arquivo.", ex);
            }
        }

        #endregion
    }
}
