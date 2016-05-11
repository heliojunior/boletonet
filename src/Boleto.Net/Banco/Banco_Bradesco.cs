using System;
using System.Collections.Generic;
using System.Web.UI;
using BoletoNet;
using Microsoft.VisualBasic;
using System.Text;

[assembly: WebResource("BoletoNet.Imagens.237.jpg", "image/jpg")]

namespace BoletoNet
{
    /// <author>  
    /// Eduardo Frare
    /// Stiven 
    /// </author>    
    internal class Banco_Bradesco : AbstractBanco, IBanco
    {
        private string _dacNossoNumero = string.Empty;
        private int _dacBoleto = 0;

        /// <summary>
        /// Classe responsavel em criar os campos do Banco Banco_Bradesco.
        /// </summary>
        internal Banco_Bradesco()
        {
            this.Codigo = 237;
            this.Digito = "2";
            this.Nome = "Bradesco";
        }

        /// <summary>
        /// Calcula o digito do Nosso Numero
        /// </summary>
        public string CalcularDigitoNossoNumero(Boleto boleto)
        {
            return Mod11Bradesco(boleto.Carteira + boleto.NossoNumero, 7);
        }

        #region IBanco Members

        /// <summary>
        /// A linha digitável será composta por cinco campos:
        ///      1º campo
        ///          composto pelo código de Banco, código da moeda, as cinco primeiras posições do campo 
        ///          livre e o dígito verificador deste campo;
        ///      2º campo
        ///          composto pelas posições 6ª a 15ª do campo livre e o dígito verificador deste campo;
        ///      3º campo
        ///          composto pelas posições 16ª a 25ª do campo livre e o dígito verificador deste campo;
        ///      4º campo
        ///          composto pelo dígito verificador do código de barras, ou seja, a 5ª posição do código de 
        ///          barras;
        ///      5º campo
        ///          Composto pelo fator de vencimento com 4(quatro) caracteres e o valor do documento com 10(dez) caracteres, sem separadores e sem edição.
        /// 
        /// </summary>
        public override void FormataLinhaDigitavel(Boleto boleto)
        {

            //BBBMC.CCCCD1 CCCCC.CCCCCD2 CCCCC.CCCCCD3 D4 FFFFVVVVVVVVVV


            #region Campo 1

            string Grupo1 = string.Empty;

            string BBB = boleto.CodigoBarra.Codigo.Substring(0, 3);
            string M = boleto.CodigoBarra.Codigo.Substring(3, 1);
            string CCCCC = boleto.CodigoBarra.Codigo.Substring(19, 5);
            string D1 = Mod10(BBB + M + CCCCC).ToString();

            Grupo1 = string.Format("{0}{1}{2}.{3}{4} ", BBB, M, CCCCC.Substring(0, 1), CCCCC.Substring(1, 4), D1);


            #endregion Campo 1

            #region Campo 2

            string Grupo2 = string.Empty;

            string CCCCCCCCCC2 = boleto.CodigoBarra.Codigo.Substring(24, 10);
            string D2 = Mod10(CCCCCCCCCC2).ToString();

            Grupo2 = string.Format("{0}.{1}{2} ", CCCCCCCCCC2.Substring(0, 5), CCCCCCCCCC2.Substring(5, 5), D2);

            #endregion Campo 2

            #region Campo 3

            string Grupo3 = string.Empty;

            string CCCCCCCCCC3 = boleto.CodigoBarra.Codigo.Substring(34, 10);
            string D3 = Mod10(CCCCCCCCCC3).ToString();

            Grupo3 = string.Format("{0}.{1}{2} ", CCCCCCCCCC3.Substring(0, 5), CCCCCCCCCC3.Substring(5, 5), D3);


            #endregion Campo 3

            #region Campo 4

            string Grupo4 = string.Empty;

            string D4 = _dacBoleto.ToString();

            Grupo4 = string.Format("{0} ", D4);

            #endregion Campo 4

            #region Campo 5

            string Grupo5 = string.Empty;

            //string FFFF = boleto.CodigoBarra.Codigo.Substring(5, 4);//FatorVencimento(boleto).ToString() ;
            string FFFF = FatorVencimento(boleto).ToString();

            //if (boleto.Carteira == "06" && !Utils.DataValida(boleto.DataVencimento))
            //    FFFF = "0000";

            string VVVVVVVVVV = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            VVVVVVVVVV = Utils.FormatCode(VVVVVVVVVV, 10);

            //if (Utils.ToInt64(VVVVVVVVVV) == 0)
            //    VVVVVVVVVV = "000";

            Grupo5 = string.Format("{0}{1}", FFFF, VVVVVVVVVV);

            #endregion Campo 5

            boleto.CodigoBarra.LinhaDigitavel = Grupo1 + Grupo2 + Grupo3 + Grupo4 + Grupo5;

        }

        /// <summary>
        /// 
        ///   *******
        /// 
        ///	O código de barra para cobrança contém 44 posições dispostas da seguinte forma:
        ///    01 a 03 - 3 - Identificação  do  Banco
        ///    04 a 04 - 1 - Código da Moeda
        ///    05 a 05 – 1 - Dígito verificador do Código de Barras
        ///    06 a 09 - 4 - Fator de vencimento
        ///    10 a 19 - 10 - Valor
        ///    20 a 44 – 25 - Campo Livre
        /// 
        ///   *******
        /// 
        /// </summary>
        /// 
        public override void FormataCodigoBarra(Boleto boleto)
        {
            string valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valorBoleto = Utils.FormatCode(valorBoleto, 10);

            if (boleto.Carteira == "02" || boleto.Carteira == "03" || boleto.Carteira == "09" || boleto.Carteira == "19")
            {
                boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}", Codigo.ToString(), boleto.Moeda,
                FatorVencimento(boleto), valorBoleto, FormataCampoLivre(boleto));
            }
            else if (boleto.Carteira == "06")
            {
                if (boleto.ValorBoleto == 0)
                {
                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}0000{2}{3}", Codigo.ToString(), boleto.Moeda,
                        valorBoleto, FormataCampoLivre(boleto));
                }
                else
                {
                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}", Codigo.ToString(), boleto.Moeda,
                        FatorVencimento(boleto), valorBoleto, FormataCampoLivre(boleto));
                }

            }
            else
            {
                throw new NotImplementedException("Carteira ainda não implementada.");
            }


            _dacBoleto = Mod11(boleto.CodigoBarra.Codigo, 9);

            boleto.CodigoBarra.Codigo = Strings.Left(boleto.CodigoBarra.Codigo, 4) + _dacBoleto + Strings.Right(boleto.CodigoBarra.Codigo, 39);
        }


        ///<summary>
        /// Campo Livre
        ///    20 a 23 -  4 - Agência Cedente (Sem o digito verificador,completar com zeros a esquerda quandonecessário)
        ///    24 a 25 -  2 - Carteira
        ///    26 a 36 - 11 - Número do Nosso Número(Sem o digito verificador)
        ///    37 a 43 -  7 - Conta do Cedente (Sem o digito verificador,completar com zeros a esquerda quando necessário)
        ///    44 a 44	- 1 - Zero            
        ///</summary>
        public string FormataCampoLivre(Boleto boleto)
        {

            string FormataCampoLivre = string.Format("{0}{1}{2}{3}{4}", boleto.Cedente.ContaBancaria.Agencia, boleto.Carteira,
                                            boleto.NossoNumero, boleto.Cedente.ContaBancaria.Conta, "0");

            return FormataCampoLivre;
        }

        public override void FormataNumeroDocumento(Boleto boleto)
        {
            throw new NotImplementedException("Função ainda não implementada.");
        }

        public override void FormataNossoNumero(Boleto boleto)
        {
            boleto.NossoNumero = string.Format("{0}/{1}-{2}", boleto.Carteira, boleto.NossoNumero, boleto.DigitoNossoNumero);
        }

        public string CalculaDigitoVerificador(Boleto boleto)
        {
            return Mod11Bradesco(boleto.Carteira + boleto.NossoNumero, 7);
            //return boleto.DigitoNossoNumero;
        }

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public override void ValidaBoleto(Boleto boleto)
        {
            if (boleto.Carteira != "02" && boleto.Carteira != "03" && boleto.Carteira != "06" && boleto.Carteira != "09" && boleto.Carteira != "19")
                throw new NotImplementedException("Carteira não implementada. Carteiras implementadas 02, 03, 06, 09, 19.");

            //O valor é obrigatório para a carteira 03
            if (boleto.Carteira == "03")
            {
                if (boleto.ValorBoleto == 0)
                    throw new NotImplementedException("Para a carteira 03, o valor do boleto não pode ser igual a zero");
            }

            //O valor é obrigatório para a carteira 09
            if (boleto.Carteira == "09")
            {
                if (boleto.ValorBoleto == 0)
                    throw new NotImplementedException("Para a carteira 09, o valor do boleto não pode ser igual a zero");
            }
            //else if (boleto.Carteira == "06")
            //{
            //    boleto.ValorBoleto = 0;
            //}

            //Verifica se o nosso número é válido
            if (boleto.NossoNumero.Length > 11)
                throw new NotImplementedException("A quantidade de dígitos do nosso número, são 11 números.");
            else if (boleto.NossoNumero.Length < 11)
                boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 11);

            //Verificar se a Agencia esta correta
            if (boleto.Cedente.ContaBancaria.Agencia.Length > 4)
                throw new NotImplementedException("A quantidade de dígitos da Agência " + boleto.Cedente.ContaBancaria.Agencia + ", são de 4 números.");
            else if (boleto.Cedente.ContaBancaria.Agencia.Length < 4)
                boleto.Cedente.ContaBancaria.Agencia = Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4);

            //Verificar se a Conta esta correta
            if (boleto.Cedente.ContaBancaria.Conta.Length > 7)
                throw new NotImplementedException("A quantidade de dígitos da Conta " + boleto.Cedente.ContaBancaria.Conta + ", são de 07 números.");
            else if (boleto.Cedente.ContaBancaria.Conta.Length < 7)
                boleto.Cedente.ContaBancaria.Conta = Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 7);


            //Atribui o nome do banco ao local de pagamento
            boleto.LocalPagamento += Nome + "";


            //Verifica se data do processamento é valida
			//if (boleto.DataProcessamento.ToString("dd/MM/yyyy") == "01/01/0001")
			if (boleto.DataProcessamento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataProcessamento = DateTime.Now;


            //Verifica se data do documento é valida
			//if (boleto.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
			if (boleto.DataDocumento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataDocumento = DateTime.Now;

            boleto.QuantidadeMoeda = 0;

            //Atribui o nome do banco ao local de pagamento
            boleto.LocalPagamento = "PAGÁVEL PREFERENCIALMENTE NAS AGÊNCIAS DO BRADESCO";

            // Calcula o DAC do Nosso Número
            _dacNossoNumero = CalcularDigitoNossoNumero(boleto);
            boleto.DigitoNossoNumero = _dacNossoNumero;
            
            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);
            FormataNossoNumero(boleto);
        }
        #endregion IBanco Members

        /// <summary>
        /// Verifica o tipo de ocorrência para o arquivo remessa
        /// </summary>
        public string Ocorrencia(string codigo)
        {
            switch (codigo)
            {
                case "02":
                    return "02-Entrada Confirmada";
                case "03":
                    return "03-Entrada Rejeitada";
                case "06":
                    return "06-Liquidação normal";
                case "09":
                    return "09-Baixado Automaticamente via Arquivo";
                case "10":
                    return "10-Baixado conforme instruções da Agência";
                case "11":
                    return "11-Em Ser - Arquivo de Títulos pendentes";
                case "12":
                    return "12-Abatimento Concedido";
                case "13":
                    return "13-Abatimento Cancelado";
                case "14":
                    return "14-Vencimento Alterado";
                case "15":
                    return "15-Liquidação em Cartório";
                case "17":
                    return "17-Liquidação após baixa ou Título não registrado";
                case "18":
                    return "18-Acerto de Depositária";
                case "19":
                    return "19-Confirmação Recebimento Instrução de Protesto";
                case "20":
                    return "20-Confirmação Recebimento Instrução Sustação de Protesto";
                case "21":
                    return "21-Acerto do Controle do Participante";
                case "23":
                    return "23-Entrada do Título em Cartório";
                case "24":
                    return "24-Entrada rejeitada por CEP Irregular";
                case "27":
                    return "27-Baixa Rejeitada";
                case "28":
                    return "28-Débito de tarifas/custas";
                case "30":
                    return "30-Alteração de Outros Dados Rejeitados";
                case "32":
                    return "32-Instrução Rejeitada";
                case "33":
                    return "33-Confirmação Pedido Alteração Outros Dados";
                case "34":
                    return "34-Retirado de Cartório e Manutenção Carteira";
                case "35":
                    return "35-Desagendamento ) débito automático";
                case "68":
                    return "68-Acerto dos dados ) rateio de Crédito";
                case "69":
                    return "69-Cancelamento dos dados ) rateio";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Verifica o código do motivo da rejeição informada pelo banco
        /// </summary>
        public string MotivoRejeicao(string codigo)
        {
            switch (codigo)
            {
                case "02":
                    return "02-Código do registro detalhe inválido";
                case "03":
                    return "03-Código da ocorrência inválida";
                case "04":
                    return "04-Código de ocorrência não permitida para a carteira";
                case "05":
                    return "05-Código de ocorrência não numérico";
                case "07":
                    return "07-Agência/conta/Digito - Inválido";
                case "08":
                    return "08-Nosso número inválido";
                case "09":
                    return "09-Nosso número duplicado";
                case "10":
                    return "10-Carteira inválida";
                case "16":
                    return "16-Data de vencimento inválida";
                case "18":
                    return "18-Vencimento fora do prazo de operação";
                case "20":
                    return "20-Valor do Título inválido";
                case "21":
                    return "21-Espécie do Título inválida";
                case "22":
                    return "22-Espécie não permitida para a carteira";
                case "24":
                    return "24-Data de emissão inválida";
                case "38":
                    return "38-Prazo para protesto inválido";
                case "44":
                    return "44-Agência Cedente não prevista";
                case "50":
                    return "50-CEP irregular - Banco Correspondente";
                case "63":
                    return "63-Entrada para Título já cadastrado";
                case "68":
                    return "68-Débito não agendado - erro nos dados de remessa";
                case "69":
                    return "69-Débito não agendado - Sacado não consta no cadastro de autorizante";
                case "70":
                    return "70-Débito não agendado - Cedente não autorizado pelo Sacado";
                case "71":
                    return "71-Débito não agendado - Cedente não participa da modalidade de débito automático";
                case "72":
                    return "72-Débito não agendado - Código de moeda diferente de R$";
                case "73":
                    return "73-Débito não agendado - Data de vencimento inválida";
                case "74":
                    return "74-Débito não agendado - Conforme seu pedido, Título não registrado";
                case "75":
                    return "75-Débito não agendado - Tipo de número de inscrição do debitado inválido";
                default:
                    return "";
            }
        }

        private string Mod11Bradesco(string seq, int b)
        {
            #region Trecho do manual layout_cobranca_port.pdf do BRADESCO
            /* 
            Para o cálculo do dígito, será necessário acrescentar o número da carteira à esquerda antes do Nosso Número, 
            e aplicar o módulo 11, com base 7.
            Multiplicar cada algarismo que compõe o número pelo seu respectivo multiplicador (PESO).
            Os multiplicadores(PESOS) variam de 2 a 7.
            O primeiro dígito da direita para a esquerda deverá ser multiplicado por 2, o segundo por 3 e assim sucessivamente.
             
              Carteira   Nosso Numero
                ______   _________________________________________
                1    9   0   0   0   0   0   0   0   0   0   0   2
                x    x   x   x   x   x   x   x   x   x   x   x   x
                2    7   6   5   4   3   2   7   6   5   4   3   2
                =    =   =   =   =   =   =   =   =   =   =   =   =
                2 + 63 + 0 + 0 + 0 + 0 + 0 + 0 + 0 + 0 + 0 + 0 + 4 = 69

            O total da soma deverá ser dividido por 11: 69 / 11 = 6 tendo como resto = 3
            A diferença entre o divisor e o resto, será o dígito de autoconferência: 11 - 3 = 8 (dígito de auto-conferência)
            
            Se o resto da divisão for “1”, desprezar o cálculo de subtração e considerar o dígito como “P”. 
            Se o resto da divisão for “0”, desprezar o cálculo de subtração e considerar o dígito como “0”.
            */
            #endregion

            /* Variáveis
             * -------------
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            int s = 0, p = 2;

            for (int i = seq.Length; i > 0; i--)
            {
                s = s + (Convert.ToInt32(Microsoft.VisualBasic.Strings.Mid(seq, i, 1)) * p);
                if (p == b)
                    p = 2;
                else
                    p = p + 1;
            }

            int r = (s % 11);

            if (r == 0)
                return "0";
            else if (r == 1)
                return "P";
            else
                return (11 - r).ToString();
        }

        public override DetalheSegmentoTRetornoCNAB240 LerDetalheSegmentoTRetornoCNAB240(string registro)
        {
            try
            {
                if (!registro.Substring(13, 1).Equals(@"T"))
                {
                    throw new Exception("Registro inválida. O detalhe não possuí as características do segmento T.");
                }
                DetalheSegmentoTRetornoCNAB240 segmentoT = new DetalheSegmentoTRetornoCNAB240(registro);

                segmentoT.CodigoBanco = Convert.ToInt32(registro.Substring(0, 3));
                segmentoT.Agencia = Convert.ToInt32(registro.Substring(17, 5));
                segmentoT.DigitoAgencia = registro.Substring(22, 1);
                segmentoT.Conta = Convert.ToInt32(registro.Substring(23, 12));
                segmentoT.DigitoConta = registro.Substring(35, 1);
                segmentoT.idCodigoMovimento = Convert.ToInt32(registro.Substring(15, 2));
                segmentoT.CodigoMovimento = new CodigoMovimento(237, segmentoT.idCodigoMovimento);
                segmentoT.NossoNumero = registro.Substring(45, 12);
                segmentoT.CodigoCarteira = Convert.ToInt32(registro.Substring(57, 1));
                segmentoT.NumeroDocumento = registro.Substring(58, 15);

                int dataVencimento = Convert.ToInt32(registro.Substring(73, 8));
                if (dataVencimento > 0)
                    segmentoT.DataVencimento = Convert.ToDateTime(dataVencimento.ToString("##-##-####"));

                segmentoT.ValorTitulo = Convert.ToDecimal(registro.Substring(81, 15)) / 100;
                segmentoT.IdentificacaoTituloEmpresa = registro.Substring(105, 25);
                segmentoT.TipoInscricao = Convert.ToInt32(registro.Substring(132, 1));
                segmentoT.NumeroInscricao = registro.Substring(133, 15);
                segmentoT.NomeSacado = registro.Substring(148, 40);
                segmentoT.ValorTarifas = Convert.ToDecimal(registro.Substring(198, 15)) / 100;
                segmentoT.CodigoRejeicao = registro.Substring(213, 10);

                return segmentoT;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO T.", ex);
            }
        }

        public override DetalheRetorno LerDetalheRetornoCNAB400(string registro)
        {
            try
            {
                DetalheRetorno detalhe = new DetalheRetorno(registro);

                //Tipo de Inscrição Empresa
                detalhe.CodigoInscricao = Utils.ToInt32(registro.Substring(1, 2));
                //Nº Inscrição da Empresa
                detalhe.NumeroInscricao = registro.Substring(3, 14);

                //Identificação da Empresa Cedente no Banco
                detalhe.Agencia = Utils.ToInt32(registro.Substring(24, 6));
                detalhe.Conta = Utils.ToInt32(registro.Substring(30, 7));
                detalhe.DACConta = Utils.ToInt32(registro.Substring(36, 1));

                //Nº Controle do Participante
                detalhe.NumeroControle = registro.Substring(37, 25);
                //Identificação do Título no Banco
                detalhe.NossoNumeroComDV = registro.Substring(70, 12);
                detalhe.NossoNumero = registro.Substring(70, 11);//Sem o DV
                detalhe.DACNossoNumero = registro.Substring(81, 1); //DV
                //Carteira
                detalhe.Carteira = registro.Substring(107, 1);
                //Identificação de Ocorrência
                detalhe.CodigoOcorrencia = Utils.ToInt32(registro.Substring(108, 2));

                //Descrição da ocorrência
                detalhe.DescricaoOcorrencia = this.Ocorrencia(registro.Substring(108, 2));

                //Número do Documento
                detalhe.NumeroDocumento = registro.Substring(116, 10);
                //Identificação do Título no Banco
                detalhe.IdentificacaoTitulo = registro.Substring(126, 20);

                //Valor do Título
                decimal valorTitulo = Convert.ToInt64(registro.Substring(152, 13));
                detalhe.ValorTitulo = valorTitulo / 100;
                //Banco Cobrador
                detalhe.CodigoBanco = Utils.ToInt32(registro.Substring(165, 3));
                //Agência Cobradora
                detalhe.AgenciaCobradora = Utils.ToInt32(registro.Substring(168, 5));
                //Espécie do Título
                detalhe.Especie = Utils.ToInt32(registro.Substring(173, 2));
                //Despesas de cobrança para os Códigos de Ocorrência (Valor Despesa)
                decimal valorDespesa = Convert.ToUInt64(registro.Substring(175, 13));
                detalhe.ValorDespesa = valorDespesa / 100;
                //Outras despesas Custas de Protesto (Valor Outras Despesas)
                decimal valorOutrasDespesas = Convert.ToUInt64(registro.Substring(188, 13));
                detalhe.ValorOutrasDespesas = valorOutrasDespesas / 100;
                // IOF
                decimal iof = Convert.ToUInt64(registro.Substring(214, 13));
                detalhe.IOF = iof / 100;
                //Abatimento Concedido sobre o Título (Valor Abatimento Concedido)
                decimal valorAbatimento = Convert.ToUInt64(registro.Substring(227, 13));
                detalhe.ValorAbatimento = valorAbatimento / 100;
                //Desconto Concedido (Valor Desconto Concedido)
                decimal valorDesconto = Convert.ToUInt64(registro.Substring(240, 13));
                detalhe.Descontos = valorDesconto / 100;
                //Valor Pago
                decimal valorPago = Convert.ToUInt64(registro.Substring(253, 13));
                detalhe.ValorPago = valorPago / 100;
                //Juros Mora
                decimal jurosMora = Convert.ToUInt64(registro.Substring(266, 13));
                detalhe.JurosMora = jurosMora / 100;
                //Outros Créditos
                decimal outrosCreditos = Convert.ToUInt64(registro.Substring(279, 13));
                detalhe.OutrosCreditos = outrosCreditos / 100;
                //Motivo do Código de Ocorrência 19 (Confirmação de Instrução de Protesto)
                detalhe.MotivoCodigoOcorrencia = registro.Substring(294, 1);

                //Data Ocorrência no Banco
                int dataOcorrencia = Utils.ToInt32(registro.Substring(110, 6));
                detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));
                //Data Vencimento do Título
                int dataVencimento = Utils.ToInt32(registro.Substring(146, 6));
                detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));
                // Data do Crédito
                int dataCredito = Utils.ToInt32(registro.Substring(295, 6));
                detalhe.DataCredito = Utils.ToDateTime(dataCredito.ToString("##-##-##"));

                //Origem Pagamento
                detalhe.OrigemPagamento = registro.Substring(301, 3);

                //Motivos das Rejeições para os Códigos de Ocorrência
                detalhe.MotivosRejeicao = registro.Substring(318, 10);
                //Número do Cartório
                detalhe.NumeroCartorio = Utils.ToInt32(registro.Substring(365, 2));
                //Número do Protocolo
                detalhe.NumeroProtocolo = registro.Substring(365, 2);
                //Nome do Sacado
                detalhe.NomeSacado = "";

                detalhe.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        #region Seygi gerando remessa
        #region HEADER
        /// <summary>
        /// Gera o HEADER do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            try
            {
                string _header = " ";

                base.GerarHeaderRemessa(numeroConvenio, cedente, tipoArquivo, numeroArquivoRemessa);

                switch (tipoArquivo)
                {

                    case TipoArquivo.CNAB240:
                        _header = GerarHeaderRemessaCNAB240(numeroConvenio,cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.CNAB400:
                        _header = GerarHeaderRemessaCNAB400(int.Parse(numeroConvenio), cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _header;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do HEADER do arquivo de REMESSA.", ex);
            }
        }

        #region Remessa240 - Específicos
        public string GerarHeaderRemessaCNAB240(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            string _headerLote = "";
            
            try
            {
                _headerLote = this.Codigo.ToString() + "00000         "; //posições 12-13 são espaços e não zeros

                if (cedente.CPFCNPJ.Length <= 11)
                    _headerLote += "1";
                else
                    _headerLote += "2";

                _headerLote += Utils.FitStringLength(cedente.CPFCNPJ, 14, 14, '0', 0, true, true, true);

                string ConvenioFormatatado = Utils.FitStringLength(numeroConvenio, 20, 20, '0', 0, true, true, true);

                _headerLote += Utils.FormatCode(ConvenioFormatatado, "0", 20, true);

                _headerLote += Utils.FitStringLength(cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true);
                _headerLote += Utils.FitStringLength(cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, true);
                _headerLote += Utils.FitStringLength(cedente.ContaBancaria.Conta, 12, 12, '0', 0, true, true, true);
                _headerLote += Utils.FitStringLength(cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);
                  
                _headerLote += " ";
                _headerLote += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);
                _headerLote += Utils.FitStringLength("BRADESCO", 30, 30, ' ', 0, true, true, false); // nome do banco
                _headerLote += Utils.FormatCode("", " ", 10);

                _headerLote += "1"; // fixo - '1' = Remessa, '2' = Retorno
                _headerLote += DateTime.Now.ToString("ddMMyyyy");
                _headerLote += DateTime.Now.ToString("HHmmss");
                _headerLote += Utils.FitStringLength(numeroArquivoRemessa.ToString(), 6, 1, '0', 0, true, true, true);
                _headerLote += "080"; // layout do arquivo
                _headerLote += Utils.FormatCode("", " ", 10); //densidade da gravação
                _headerLote += Utils.FormatCode("", " ", 40);
                _headerLote += Utils.FormatCode("", " ", 29);

                _headerLote = Utils.SubstituiCaracteresEspeciais(_headerLote);

                return _headerLote;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB240.", ex);
            }
        }

        public string GerarHeaderLoteRemessaCNAB240(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            /* Arquivo LAY-OUT CNAB 240 FEBRABAN VERSÃO 8.0 DE 31/08/2004. */
            string _headerLote = "";
            string Lote = "0001";
            string TipoOperacao = "R"; //Tipo de Operacao 'R' - Arquivo Remessa 

            try
            {

                _headerLote = this.Codigo.ToString();       // Código do Banco na Compensação
                _headerLote += Lote;                        // Lote de Serviço
                _headerLote += "1";                         // Tipo de Registro
                _headerLote += TipoOperacao;                // Tipo de Operação
                _headerLote += "01";                        // Tipo de Serviço
                _headerLote += "  ";                        // posições 12-13 são espaços e não zeros
                _headerLote += "042";                       // Nº da Versão do Layout do Lote
                _headerLote += " ";                         // Uso Exclusivo Febraban

                if (cedente.CPFCNPJ.Length <= 11)           // Tipo de Inscrição da Empresa
                    _headerLote += "1";
                else
                    _headerLote += "2";

                _headerLote += Utils.FitStringLength(cedente.CPFCNPJ, 15, 15, '0', 0, true, true, true); // Nº de Inscrição da Empresa
                
                string ConvenioFormatatado = Utils.FitStringLength(numeroConvenio, 20, 20, '0', 0, true, true, true); // Código do Convênio no Banco ??????? Verificar se tá certo
                _headerLote += Utils.FormatCode(ConvenioFormatatado,"0",20,true); //

                _headerLote += Utils.FitStringLength(cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true);            // Agência Mantenedora da Conta
                _headerLote += Utils.FitStringLength(cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, true);      // Dígito Verificador da Conta
                _headerLote += Utils.FitStringLength(cedente.ContaBancaria.Conta, 12, 12, '0', 0, true, true, true);            // Número da Conta Corrente
                _headerLote += Utils.FitStringLength(cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);        // Dígito Verificador da Conta

                if (cedente.ContaBancaria.DigitoConta.Length > 1) //Dígito Verificador da Ag/Conta
                    _headerLote += cedente.ContaBancaria.DigitoConta.Substring(1,1); 
                else
                    _headerLote += " "; 

                _headerLote += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false); //Nome da Empresa
                _headerLote += Utils.FormatCode("", " ", 40); // Mensagem 1
                _headerLote += Utils.FormatCode("", " ", 40); // Mensagem 2 
                _headerLote += Utils.FitStringLength(numeroArquivoRemessa.ToString(), 8, 8, '0', 0, true, true, true); //Número Sequencial Remessa/Retorno
                _headerLote += DateTime.Now.ToString("ddMMyyyy"); //Data de Gravação Remessa/Retorno 
                _headerLote += Utils.FormatCode("", " ", 8); //Data do Crédito
                _headerLote += Utils.FormatCode("", " ", 33);
                _headerLote = Utils.SubstituiCaracteresEspeciais(_headerLote);

                return _headerLote;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do HEADER DE LOTE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio)
        {
            try
            {
                string _segmentoP;
                string _nossoNumero;

                _segmentoP = this.Codigo.ToString() + "00013";
                _segmentoP += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);
                _segmentoP += "P 01";
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true);
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, true);
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 12, 12, '0', 0, true, true, true);
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);
                _segmentoP += "0"; //Dígito Verificador da Ag/Conta
                _segmentoP += Utils.FitStringLength(boleto.Cedente.Carteira, 3, 3, '0', 0, true, true, true);

                boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 11); 
                _nossoNumero = boleto.NossoNumero;

                _segmentoP += Utils.FormatCode("", "0", 5); //Zeros

                // Importante: Nosso número, alinhar à esquerda com brancos à direita (conforme manual)
                _segmentoP += Utils.FitStringLength(_nossoNumero, 11, 11, ' ', 0, true, true, false);
                _segmentoP += CalcularDigitoNossoNumero(boleto);

                /* 
                '1' = Cobrança Simples
                '2' = Cobrança Vinculada
                '3' = Cobrança Caucionada
                '4' = Cobrança Descontada
                */
                _segmentoP += "1"; //Cobrança Simples
                //_segmentoP += boleto.Carteira;

                //Forma de cadastramento do título no banco. Pode ser branco/espaço, 0, 1=cobrança registrada, 2=sem registro.
                _segmentoP += boleto.TipoModalidade;
                //Tipo de documento. Pode ser branco, 0, 1=tradicional, 2=escritural.
                _segmentoP += "1"; // ???? Verificar daonde carregar

                // Campo não tratado. Identificação de emissão do boleto. Pode ser branco/espaço, 0, ou:
                /*
                       '1' = Banco Emite
                        '2' = Cliente Emite
                        '3' = Banco Pré-emite e Cliente Complementa (NÃO TRATADO PELO BANCO)
                        '4' = Banco Reemite (NÃO TRATADO PELO BANCO)
                        '5' = Banco Não Reemite (NÃO TRATADO PELO BANCO)
                        '7' = Banco Emitente – Aberta (NÃO TRATADO PELO BANCO)
                        '8' = Banco Emitente - Auto-envelopável (NÃO TRATADO PELO BANCO)
                        Os códigos '4' e '5' só serão aceitos para código de movimento para remessa '31' (NÃO TRATADO PELO BANCO)
                 */
                _segmentoP += "2";

                /*
                 *   '1' = Banco Distribui
                     '2' = Cliente Distribui
                     ‘3’ = Banco envia e-mail (NÃO TRATADO PELO BANCO)
                     ‘4’ = Banco envia SMS (NÃO TRATADO PELO BANCO)
                 */
                _segmentoP += Utils.FitStringLength(boleto.Remessa.TipoDistribuicao, 1, 1, '2', 0, true, true, false);
                _segmentoP += Utils.FitStringLength(boleto.NumeroDocumento, 15, 15, ' ', 0, true, true, false);
                _segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, ' ', 0, true, true, false);
                _segmentoP += Utils.FitStringLength(boleto.ValorBoleto.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);
                _segmentoP += "00000 ";
                _segmentoP += Utils.FitStringLength(boleto.EspecieDocumento.Codigo.ToString(), 2, 2, '0', 0, true, true, true);
                _segmentoP += "N";
                _segmentoP += Utils.FitStringLength(boleto.DataDocumento.ToString("ddMMyyyy"), 8, 8, ' ', 0, true, true, false);

                /* codigo Juros de Mora
                    '1' = Valor por Dia
                    '2' = Taxa Mensal
                    '3' = Isento
                */

                if (boleto.JurosMora > 0)
                {
                    _segmentoP += "1";
                    _segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);
                    _segmentoP += Utils.FitStringLength(boleto.JurosMora.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);
                }
                else if (boleto.JurosPermanente)
                {
                    _segmentoP += "1";
                    _segmentoP += Utils.FormatCode("", "0", 8);
                    _segmentoP += Utils.FormatCode("", "0", 15);
                }
                else
                {
                    _segmentoP += "3";
                    _segmentoP += Utils.FormatCode("", "0", 8);
                    _segmentoP += Utils.FormatCode("", "0", 15);
                }

                if (boleto.ValorDesconto > 0)
                {
                    _segmentoP += "1";
                    _segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);
                    _segmentoP += Utils.FitStringLength(boleto.ValorDesconto.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);
                }
                else
                    _segmentoP += Utils.FormatCode("", "0", 24);

                _segmentoP += Utils.FormatCode("", "0", 15);
                _segmentoP += Utils.FormatCode("", "0", 15);
                _segmentoP += Utils.FitStringLength(boleto.NumeroDocumento, 25, 25, ' ', 0, true, true, false);

                // trata somente os códigos '1' – Protestar dias corridos, '2' – Protestar dias úteis, e '3' – Não protestar.

                /*
                    '1' = Protestar Dias Corridos
                    '2' = Protestar Dias Úteis
                    '3' = Não Protestar
                    ‘4’ = Protestar Fim Falimentar - Dias Úteis
                    ‘5’ = Protestar Fim Falimentar - Dias Corridos
                    ‘8’ = Negativação sem Protesto (NÃO TRATADO PELO BANCO)
                    '9' = Cancelamento Protesto Automático
                    (somente válido p/ CódigoMovimento Remessa = '31' - Descrição C004)
                */
                string codigo_protesto = "3";
                string dias_protesto = "00";

                foreach (Instrucao_Bradesco instrucao in boleto.Instrucoes)
                {
                    switch ((EnumInstrucoes_Bradesco)instrucao.Codigo)
                    {
                        case EnumInstrucoes_Bradesco.ProtestarAposNDiasCorridos:
                            codigo_protesto = "1";
                            dias_protesto = Utils.FitStringLength(instrucao.QuantidadeDias.ToString(), 2, 2, '0', 0, true, true, true); //Para código '1' – é possível, de 6 a 29 dias
                            break;
                        case EnumInstrucoes_Bradesco.ProtestarAposNDiasUteis:
                            codigo_protesto = "2";
                            dias_protesto = Utils.FitStringLength(instrucao.QuantidadeDias.ToString(), 2, 2, '0', 0, true, true, true); //Para código '2' – é possível, 3º, 4º ou 5º dia útil
                            break;
                        case EnumInstrucoes_Bradesco.NaoProtestar:
                            codigo_protesto = "3";
                            dias_protesto = "00";
                            break;
                        default:
                            break;
                    }
                }

                _segmentoP += codigo_protesto;
                _segmentoP += dias_protesto;

                // '1' = Baixar / Devolver '2' = Não Baixar / Não Devolver (NÃO TRATADO PELO BANCO) '3' = Cancelar Prazo para Baixa / Devolução (somente válido p/ CódigoMovimento Remessa = '31' - Descrição C004)
                _segmentoP += "0";      // Código para Baixa/Devolução
                _segmentoP += "000";    // Número de Dias para Baixa/Devolução
                _segmentoP += "09";     // Código da Moeda
                _segmentoP += Utils.FormatCode("", "0", 10);
                _segmentoP += " ";
                return Utils.SubstituiCaracteresEspeciais(_segmentoP);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do SEGMENTO P DO DETALHE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _zeros16 = new string('0', 16);
                string _brancos28 = new string(' ', 28);
                string _brancos40 = new string(' ', 40);

                string _segmentoQ;

                _segmentoQ = this.Codigo.ToString() + "00013";
                _segmentoQ += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);
                _segmentoQ += "Q 01";

                if (boleto.Sacado.CPFCNPJ.Length <= 11)
                    _segmentoQ += "1";
                else
                    _segmentoQ += "2";

                _segmentoQ += Utils.FitStringLength(boleto.Sacado.CPFCNPJ, 15, 15, '0', 0, true, true, true);
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Nome.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.End.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.Bairro.TrimStart(' '), 15, 15, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.CEP, 8, 8, ' ', 0, true, true, false).ToUpper(); ;
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.Cidade.TrimStart(' '), 15, 15, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.UF, 2, 2, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += _zeros16;
                _segmentoQ += _brancos40;
                _segmentoQ += "000";
                _segmentoQ += _brancos28;

                _segmentoQ = Utils.SubstituiCaracteresEspeciais(_segmentoQ);

                return _segmentoQ;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do SEGMENTO Q DO DETALHE do arquivo de REMESSA.", ex);
            }
        }


        public override string GerarDetalheSegmentoRRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _brancos110 = new string(' ', 110);
                string _brancos9 = new string(' ', 9);

                string _segmentoR;

                _segmentoR = this.Codigo.ToString() + "00013";
                _segmentoR += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);
                _segmentoR += "R 01";
                // Desconto 2
                _segmentoR += "000000000000000000000000"; //24 zeros
                // Desconto 3
                _segmentoR += "000000000000000000000000"; //24 zeros

                if (boleto.PercMulta > 0)
                {
                    // Código da multa 2 - percentual
                    _segmentoR += "2";
                    _segmentoR += Utils.FitStringLength(boleto.DataMulta.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);
                    _segmentoR += Utils.FitStringLength(boleto.PercMulta.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);
                }
                else if (boleto.ValorMulta > 0)
                {
                    // Código da multa 1 - valor fixo
                    _segmentoR += "1";
                    _segmentoR += Utils.FitStringLength(boleto.DataMulta.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);
                    _segmentoR += Utils.FitStringLength(boleto.ValorMulta.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);
                }
                else
                {
                    // Código da multa 0 - sem multa
                    _segmentoR += "0";
                    _segmentoR += Utils.FitStringLength(boleto.DataMulta.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);
                    _segmentoR += Utils.FitStringLength(0.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);
                }

                _segmentoR += _brancos110;
                _segmentoR += "0000000000000000"; //16 zeros
                _segmentoR += " "; //1 branco
                _segmentoR += "000000000000"; //12 zeros
                _segmentoR += "  "; //2 brancos
                _segmentoR += "0"; //1 zero
                _segmentoR += _brancos9;

                _segmentoR = Utils.SubstituiCaracteresEspeciais(_segmentoR);

                return _segmentoR;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do SEGMENTO R DO DETALHE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo)
        {
            try
            {
                string header = " ";

                base.GerarHeaderLoteRemessa(numeroConvenio, cedente, numeroArquivoRemessa, tipoArquivo);

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB240:
                        header = GerarHeaderLoteRemessaCNAB240(numeroConvenio, cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.CNAB400:
                        header = "";
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return header;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do HEADER DO LOTE do arquivo de REMESSA.", ex);
            }
        }


        public string GerarDetalheRemessaCNAB240()
        {
            throw new NotImplementedException("Função não implementada.");
        }
        #endregion

        public string GerarHeaderRemessaCNAB400(int numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                string complemento = new string(' ', 277);
                string _header;

                _header = "01REMESSA01COBRANCA       ";
                _header += Utils.FitStringLength(cedente.Codigo.ToString(), 20, 20, '0', 0, true, true, true);
                _header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false).ToUpper();
                _header += this.Codigo.ToString();
                _header += "BRADESCO       ";
                _header += DateTime.Now.ToString("ddMMyy");
                _header += "        ";
                _header += "MX";
                _header += Utils.FitStringLength(numeroArquivoRemessa.ToString(), 7, 7, '0', 0, true, true, true);
                _header += complemento;
                _header += "000001";

                _header = Utils.SubstituiCaracteresEspeciais(_header);

                return _header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }
        #endregion

        # region DETALHE

      
        /// <summary>
        /// DETALHE do arquivo CNAB
        /// Gera o DETALHE do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _detalhe = " ";

                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB240:
                        _detalhe = GerarDetalheRemessaCNAB240();
                        break;
                    case TipoArquivo.CNAB400:
                        _detalhe = GerarDetalheRemessaCNAB400(boleto, numeroRegistro, tipoArquivo);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _detalhe;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do DETALHE arquivo de REMESSA.", ex);
            }
        }

        public string GerarDetalheRemessaCNAB400(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                // USO DO BANCO - Identificação da operação no Banco (posição 87 a 107)
                string identificaOperacaoBanco = new string(' ', 10);
                string nrDeControle = new string(' ', 25);
                string mensagem = new string(' ', 12);
                string mensagem2 = new string(' ', 60);

                string usoBanco = new string(' ', 10);
                string _detalhe;
                //detalhe                           (tamanho,tipo) A= Alfanumerico, N= Numerico
                _detalhe = "1"; //Identificação do Registro         (1, N)

                //Parte Não Necessaria - Parte de dados do Sacado
                _detalhe += "00000"; //Agencia de Debito            (5, N) Não Usado
                _detalhe += " "; //Dig da Agencia                   (1, A) Não Usado
                _detalhe += "00000"; //Razao da Conta Corrente      (5, N) Não Usado
                _detalhe += "0000000"; //Conta Corrente             (7, N) Não Usado
                _detalhe += " "; //Dig da Conta Corrente            (1, A) Não Usado

                //Identificação da Empresa Cedente no Banco (17, A)
                _detalhe += "0";
                _detalhe += Utils.FitStringLength(boleto.Carteira, 3, 3, '0', 0, true, true, true); // Codigo da carteira (3)
                _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true); //N da agencia(5)
                _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 7, 7, '0', 0, true, true, true); //Conta Corrente(7)
                _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);//D da conta(1)
                //Nº de Controle do Participante - uso livre da empresa (25, A)  //  brancos
                _detalhe += nrDeControle;
                //Código do Banco, só deve ser preenchido quando cliente cedente optar por "Débito Automático".
                _detalhe += "000";
                //0=sem multa, 2=com multa (1, N)
                if (boleto.PercMulta > 0)
                {
                    _detalhe += "2";
                    _detalhe += Utils.FitStringLength(boleto.PercMulta.ToString("0.00").Replace(",", ""), 4, 4, '0', 0, true, true, true); //Percentual Multa 9(2)V99 - (04)
                }
                else
                {
                    _detalhe += "0";
                    _detalhe += "0000";
                }

                //Identificação do Título no Banco (12, A)
                _detalhe += Utils.FitStringLength(boleto.NossoNumero, 11, 11, '0', 0, true, true, true); //Nosso Numero (11)

                // Força o NossoNumero a ter 11 dígitos. Alterado por Luiz Ponce 07/07/2012
                _detalhe += Mod11Bradesco(boleto.Carteira + Utils.FitStringLength(boleto.NossoNumero, 11, 11, '0', 0, true, true, true), 7); // Digito de Auto Conferencia do Nosso Número (01)
                //Desconto Bonificação por dia (10, N)
                _detalhe += "0000000000";

                // 1 = Banco emite e Processa o registro
                // 2 = Cliente emite e o Banco somente processa
                //Condição para Emissão da Papeleta de Cobrança(1, N)
                _detalhe += "2";
                //Ident. se emite papeleta para Débito Automático (1, A)
                _detalhe += "N";
                //Identificação da Operação do Banco (10, A) Em Branco
                _detalhe += identificaOperacaoBanco;

                //Indicador de Rateio de Crédito (1, A)
                //Somente deverá ser preenchido com a Letra “R”, se a Empresa participa da rotina 
                // de rateio de crédito, caso não participe, informar Branco.
                _detalhe += " ";

                //Endereçamento para Aviso do Débito Automático em Conta Corrente (1, N)
                //1 = emite aviso, e assume o endereço do Sacado constante do Arquivo-Remessa;
                //2 = não emite aviso;
                //diferente de 1 ou 2 = emite e assume o endereço do cliente debitado, constante do nosso cadastro.
                _detalhe += "2";

                _detalhe += "  "; //Branco (2, A)

                //Identificação ocorrência(2, N)
                /*
                01..Remessa
                02..Pedido de baixa
                04..Concessão de abatimento
                05..Cancelamento de abatimento concedido
                06..Alteração de vencimento
                07..Alteração do controle do participante
                08..Alteração de seu número
                09..Pedido de protesto
                18..Sustar protesto e baixar Título
                19..Sustar protesto e manter em carteira
                31..Alteração de outros dados
                35..Desagendamento do débito automático
                68..Acerto nos dados do rateio de Crédito
                69..Cancelamento do rateio de crédito.
                */
                _detalhe += Utils.FitStringLength(boleto.Remessa.CodigoOcorrencia, 2, 2, '0', 0, true, true, false); //Código de movimento remessa
                //_detalhe += "01";

                _detalhe += Utils.Right(boleto.NumeroDocumento, 10, '0', true); //Nº do Documento (10, A)
                _detalhe += boleto.DataVencimento.ToString("ddMMyy"); //Data do Vencimento do Título (10, N) DDMMAA

                //Valor do Título (13, N)
                _detalhe += Utils.FitStringLength(boleto.ValorBoleto.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true);

                _detalhe += "000"; //Banco Encarregado da Cobrança (3, N)
                _detalhe += "00000"; //Agência Depositária (5, N)

                /*Espécie de Título (2,N)
                * 01-Duplicata
                02-Nota Promissória
                03-Nota de Seguro
                04-Cobrança Seriada
                05-Recibo
                10-Letras de Câmbio
                11-Nota de Débito
                12-Duplicata de Serv.
                99-Outros
                */
                //_detalhe += "99";
                _detalhe += Utils.FitStringLength(boleto.EspecieDocumento.Codigo.ToString(), 2, 2, '0', 0, true, true, true);

                _detalhe += "N"; //Identificação (1, A) A – aceito; N - não aceito
                _detalhe += boleto.DataProcessamento.ToString("ddMMyy"); //Data da emissão do Título (6, N) DDMMAA
                
                //Valida se tem instrução no list de instruções, repassa ao arquivo de remessa
                string vInstrucao1 = "00"; //1ª instrução (2, N) Caso Queira colocar um cod de uma instrução. ver no Manual caso nao coloca 00
                string vInstrucao2 = "00"; //2ª instrução (2, N) Caso Queira colocar um cod de uma instrução. ver no Manual caso nao coloca 00
                
                foreach (Instrucao_Bradesco instrucao in boleto.Instrucoes)
                {
                    switch ((EnumInstrucoes_Bradesco)instrucao.Codigo)
                    {
                        case EnumInstrucoes_Bradesco.Protestar:
                            vInstrucao1 = "06"; //Indicar o código “06” - (Protesto)
                            vInstrucao2 = "00";
                            break;
                        case EnumInstrucoes_Bradesco.NaoProtestar:
                            vInstrucao1 = "00";
                            vInstrucao2 = "00";
                            break;
                        case EnumInstrucoes_Bradesco.ProtestoFinsFalimentares:
                            vInstrucao1 = "06"; //Indicar o código “06” - (Protesto)
                            vInstrucao2 = "00";
                            break;
                        case EnumInstrucoes_Bradesco.ProtestarAposNDiasCorridos:
                            vInstrucao1 = "06"; //Indicar o código “06” - (Protesto)
                            vInstrucao2 = Utils.FitStringLength(instrucao.QuantidadeDias.ToString(), 2, 2, '0', 0, true, true, true);
                            break;
                        case EnumInstrucoes_Bradesco.ProtestarAposNDiasUteis:
                            vInstrucao1 = "06"; //Indicar o código “06” - (Protesto)
                            vInstrucao2 = Utils.FitStringLength(instrucao.QuantidadeDias.ToString(), 2, 2, '0', 0, true, true, true);
                            break;
                        case EnumInstrucoes_Bradesco.NaoReceberAposNDias:
                            vInstrucao1 = "00";
                            vInstrucao2 = "00";
                            break;
                        case EnumInstrucoes_Bradesco.DevolverAposNDias:
                            vInstrucao1 = "00";
                            vInstrucao2 = "00";
                            break;
                    }
                }
                _detalhe += vInstrucao1; //posições: 157 a 158 do leiaute
                _detalhe += vInstrucao2; //posições: 159 a 160 do leiaute
                //

                // Valor a ser cobrado por Dia de Atraso (13, N)
                _detalhe += Utils.FitStringLength(boleto.JurosMora.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true);

                //Data Limite P/Concessão de Desconto (06, N)
				//if (boleto.DataDesconto.ToString("dd/MM/yyyy") == "01/01/0001")
				if (boleto.DataDesconto == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                {
                    _detalhe += "000000"; //Caso nao tenha data de vencimento
                }
                else
                {
                    _detalhe += boleto.DataDesconto.ToString("ddMMyy");
                }

                //Valor do Desconto (13, N)
                _detalhe += Utils.FitStringLength(boleto.ValorDesconto.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true);

                //Valor do IOF (13, N)
                _detalhe += Utils.FitStringLength(boleto.IOF.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true);

                //Valor do Abatimento a ser concedido ou cancelado (13, N)
                _detalhe += Utils.FitStringLength(boleto.Abatimento.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true);

                /*Identificação do Tipo de Inscrição do Sacado (02, N)
                *01-CPF
                02-CNPJ
                03-PIS/PASEP
                98-Não tem
                99-Outros 
                00-Outros 
                */
                if (boleto.Sacado.CPFCNPJ.Length <= 11)
                    _detalhe += "01";  // CPF
                else
                    _detalhe += "02"; // CNPJ

                //Nº Inscrição do Sacado (14, N)
                string cpf_Cnpj = boleto.Sacado.CPFCNPJ.Replace("/", "").Replace(".", "").Replace("-", "");
                _detalhe += Utils.FitStringLength(cpf_Cnpj, 14, 14, '0', 0, true, true, true);

                //Nome do Sacado (40, A)
                _detalhe += Utils.FitStringLength(boleto.Sacado.Nome.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();

                //Endereço Completo (40, A)
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.End.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();

                //1ª Mensagem (12, A)
                /*Campo livre para uso da Empresa. A mensagem enviada nesse campo será impressa
                somente no boleto e não será confirmada no Arquivo Retorno.
                */
                _detalhe += Utils.FitStringLength(mensagem, 12, 12, ' ', 0, true, true, false);

                //CEP (5, N) + Sufixo do CEP (3, N) Total (8, N)
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.CEP.Replace("-", ""), 8, 8, '0', 0, true, true, true);

                //Sacador|Avalista ou 2ª Mensagem (60, A)
                _detalhe += Utils.FitStringLength(mensagem2, 60, 60, ' ', 0, true, true, false);

                //Nº Seqüencial do Registro (06, N)
                _detalhe += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);

                _detalhe = Utils.SubstituiCaracteresEspeciais(_detalhe);

                return _detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }
        }

        # endregion DETALHE

        # region TRAILER

        /// <summary>
        /// TRAILER do arquivo CNAB
        /// Gera o TRAILER do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal)
        {
            try
            {
                string _trailer = " ";

                base.GerarTrailerRemessa(numeroRegistro, tipoArquivo, cedente, vltitulostotal);

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB240:
                        _trailer = GerarTrailerRemessa240();
                        break;
                    case TipoArquivo.CNAB400:
                        _trailer = GerarTrailerRemessa400(numeroRegistro);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _trailer;

            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        public override string GerarTrailerLoteRemessa(int numeroRegistro)
        {
            int numeroTitulos = 0;

            try
            {
                StringBuilder trailerLote = new StringBuilder();

                string LoteServico = "0001";
                trailerLote.Append(this.Codigo.ToString() + LoteServico + "5"); // fixo
                trailerLote.Append(Utils.FormatCode("", " ", 9));
                trailerLote.Append(Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true)); // numero de registros do arquivo

                /* Totalização da Cobrança Simples */
                trailerLote.Append(Utils.FitStringLength(numeroTitulos.ToString(), 6, 6, '0', 0, true, true, true)); // numero de registros do arquivo
                trailerLote.Append(Utils.FormatCode("", " ", 17)); //Somatória dos Valores 15 ,2 ???? verificar se deve ser passado

                /* Totalização da Cobrança Vinculada */
                trailerLote.Append(Utils.FormatCode("", " ", 6));  // fixo 
                trailerLote.Append(Utils.FormatCode("", " ", 17));  // fixo 

                /* Totalização da Cobrança Caucionada */
                trailerLote.Append(Utils.FormatCode("", " ", 6));  // fixo 
                trailerLote.Append(Utils.FormatCode("", " ", 17));  // fixo 

                /* Totalização da Cobrança Descontada */
                trailerLote.Append(Utils.FormatCode("", " ", 6));  // fixo 
                trailerLote.Append(Utils.FormatCode("", " ", 17));  // fixo 

                trailerLote.Append(Utils.FormatCode("", " ", 8));  //  N. do Aviso 
                trailerLote.Append(Utils.FormatCode("", " ", 117)); // CNAB 

                return Utils.SubstituiCaracteresEspeciais(trailerLote.ToString()); 
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        public override string GerarTrailerArquivoRemessa(int numeroRegistro)
        {
            try
            {
                StringBuilder trailerArquivo = new StringBuilder();

                trailerArquivo.Append(this.Codigo.ToString() + "99999");                     // fixo
                trailerArquivo.Append(Utils.FormatCode("", " ", 9));
                trailerArquivo.Append("000001");                       //lotes do arquivo
                trailerArquivo.Append(Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true)); // numero de registros do arquivo
                trailerArquivo.Append("000000");                       // fixo
                trailerArquivo.Append(Utils.FormatCode("", " ", 205)); // fixo

                return Utils.SubstituiCaracteresEspeciais(trailerArquivo.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        public string GerarTrailerRemessa240()
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public string GerarTrailerRemessa400(int numeroRegistro)
        {
            try
            {
                string complemento = new string(' ', 393);
                string _trailer;

                _trailer = "9";
                _trailer += complemento;
                _trailer += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true); // Número sequencial do registro no arquivo.

                _trailer = Utils.SubstituiCaracteresEspeciais(_trailer);

                return _trailer;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }

        # endregion
        #endregion Seygi Gerando Remessa

        # region Métodos de processamento do arquivo retorno CNAB400

        public HeaderDeArquivoCNAB400 LerHeaderArquivoRetornoCNAB400(string registro)
        {
            HeaderDeArquivoCNAB400 header = new HeaderDeArquivoCNAB400();

            int data = Convert.ToInt32(registro.Substring(94, 6));
            header.DataGeracao = Convert.ToDateTime(data.ToString("##-##-####"));

            return header;
        }

        #endregion

        # region Métodos de processamento do arquivo retorno CNAB240

        public HeaderDeArquivoCNAB240 LerHeaderArquivoRetornoCNAB240(string registro)
        {
            HeaderDeArquivoCNAB240 header = new HeaderDeArquivoCNAB240();

            int data = Convert.ToInt32(registro.Substring(143, 8));
            header.DataGeracao = Convert.ToDateTime(data.ToString("##-##-####"));

            return header;
        }

        #endregion

        /// <summary>
        /// Efetua as Validações dentro da classe Boleto, para garantir a geração da remessa
        /// </summary>
        public override bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            bool vRetorno = true;
            string vMsg = string.Empty;
            ////IMPLEMENTACAO PENDENTE...
            mensagem = vMsg;
            return vRetorno;
        }

    }
}
