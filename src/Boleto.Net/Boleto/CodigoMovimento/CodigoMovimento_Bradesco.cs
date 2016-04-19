using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumCodigoMovimento_Bradesco
    {
        EntradaConfirmada = 2,
        EntradaRejeitada = 3,
        TransferenciaCarteiraEntrada = 4,
        TransferenciaCarteiraBaixa = 5,
        Liquidacao = 6,
        Baixa = 9,
        TitulosCarteiraEmSer = 11,
        ConfirmacaoRecebimentoInstrucaoAbatimento = 12,
        ConfirmacaoRecebimentoInstrucaoCancelamentoAbatimento = 13,
        ConfirmacaoRecebimentoInstrucaoAlteracaoVencimento = 14,
        FrancoPagamento = 15,
        LiquidacaoAposBaixa = 17,
        ConfirmacaoRecebimentoInstrucaoProtesto = 19,
        ConfirmacaoRecebimentoInstrucaoSustacaoProtesto = 20,
        RemessaCartorio = 23,
        RetiradaCartorioManutencaoCarteira = 24,
        ProtestadoBaixado = 25,
        InstrucaoRejeitada = 26,
        ConfirmaçãoPedidoAlteracaoOutrosDados = 27,
        DebitoTarifas = 28,
        OcorrenciaSacado = 29,
        AlteracaoDadosRejeitada = 30,

    }

    #endregion 

    public class CodigoMovimento_Bradesco : AbstractCodigoMovimento, ICodigoMovimento
    {
        #region Construtores 

        public CodigoMovimento_Bradesco()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public CodigoMovimento_Bradesco(int codigo)
        {
            try
            {
                this.carregar(codigo);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        #endregion

        #region Metodos Privados

        private void carregar(int codigo)
        {
            try
            {
                this.Banco = new Banco_Bradesco();

                switch ((EnumCodigoMovimento_Bradesco)codigo)
                {
                    case EnumCodigoMovimento_Bradesco.EntradaConfirmada:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.EntradaConfirmada;
                        this.Descricao = "Entrada confirmada";
                        break;
                    case EnumCodigoMovimento_Bradesco.EntradaRejeitada:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.EntradaRejeitada;
                        this.Descricao = "Entrada rejeitada";
                        break;
                    case EnumCodigoMovimento_Bradesco.TransferenciaCarteiraEntrada:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.TransferenciaCarteiraEntrada;
                        this.Descricao = "Transferência de carteira/entrada";
                        break;
                    case EnumCodigoMovimento_Bradesco.TransferenciaCarteiraBaixa:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.TransferenciaCarteiraBaixa;
                        this.Descricao = "Transferência de carteira/baixa";
                        break;
                    case EnumCodigoMovimento_Bradesco.Liquidacao:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.Liquidacao;
                        this.Descricao = "Liquidação normal";
                        break;
                    case EnumCodigoMovimento_Bradesco.Baixa:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.Baixa;
                        this.Descricao = "Baixa";
                        break;
                    case EnumCodigoMovimento_Bradesco.TitulosCarteiraEmSer:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.TitulosCarteiraEmSer;
                        this.Descricao = "Títulos em carteira em ser";
                        break;
                    case EnumCodigoMovimento_Bradesco.ConfirmacaoRecebimentoInstrucaoAbatimento:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.ConfirmacaoRecebimentoInstrucaoAbatimento;
                        this.Descricao = "Confirmação recebimento instrução de abatimento";
                        break;
                    case EnumCodigoMovimento_Bradesco.ConfirmacaoRecebimentoInstrucaoCancelamentoAbatimento:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.ConfirmacaoRecebimentoInstrucaoCancelamentoAbatimento;
                        this.Descricao = "Confirmação recebimento instrução de cancelamento de abatimento";
                        break;
                    case EnumCodigoMovimento_Bradesco.ConfirmacaoRecebimentoInstrucaoAlteracaoVencimento:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.ConfirmacaoRecebimentoInstrucaoAlteracaoVencimento;
                        this.Descricao = "Confirmação recebimento instrução alteração de vencimento";
                        break;
                    case EnumCodigoMovimento_Bradesco.FrancoPagamento:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.FrancoPagamento;
                        this.Descricao = "Franco pagamento";
                        break;
                    case EnumCodigoMovimento_Bradesco.LiquidacaoAposBaixa:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.LiquidacaoAposBaixa;
                        this.Descricao = "Liquidação após baixa";
                        break;
                    case EnumCodigoMovimento_Bradesco.ConfirmacaoRecebimentoInstrucaoProtesto:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.ConfirmacaoRecebimentoInstrucaoProtesto;
                        this.Descricao = "Confirmação de recebimento de instrução de protesto";
                        break;
                    case EnumCodigoMovimento_Bradesco.ConfirmacaoRecebimentoInstrucaoSustacaoProtesto:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.ConfirmacaoRecebimentoInstrucaoSustacaoProtesto;
                        this.Descricao = "Confirmação de recebimento de instrução de sustação de protesto";
                        break;
                    case EnumCodigoMovimento_Bradesco.RemessaCartorio:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.RemessaCartorio;
                        this.Descricao = "Remessa a cartório/aponte em cartório";
                        break;
                    case EnumCodigoMovimento_Bradesco.RetiradaCartorioManutencaoCarteira:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.RetiradaCartorioManutencaoCarteira;
                        this.Descricao = "Retirada de cartório e manutenção em carteira";
                        break;
                    case EnumCodigoMovimento_Bradesco.ProtestadoBaixado:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.ProtestadoBaixado;
                        this.Descricao = "Protestado e baixado/baixa por ter sido protestado";
                        break;
                    case EnumCodigoMovimento_Bradesco.InstrucaoRejeitada:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.InstrucaoRejeitada;
                        this.Descricao = "Instrução rejeitada";
                        break;
                    case EnumCodigoMovimento_Bradesco.ConfirmaçãoPedidoAlteracaoOutrosDados:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.ConfirmaçãoPedidoAlteracaoOutrosDados;
                        this.Descricao = "Confirmação do pedido de alteração de outros dados";
                        break;
                    case EnumCodigoMovimento_Bradesco.DebitoTarifas:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.DebitoTarifas;
                        this.Descricao = "Debito de tarifas/custas";
                        break;
                    case EnumCodigoMovimento_Bradesco.OcorrenciaSacado:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.OcorrenciaSacado;
                        this.Descricao = "Ocorrencias do sacado";
                        break;
                    case EnumCodigoMovimento_Bradesco.AlteracaoDadosRejeitada:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.AlteracaoDadosRejeitada;
                        this.Descricao = "Alteração de dados rejeitada";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = "( Selecione )";
                        break;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        private void Ler(int codigo)
        {
            try
            {
                switch (codigo)
                {
                    case 2:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.EntradaConfirmada;
                        this.Descricao = "Entrada confirmada";
                        break;
                    case 3:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.EntradaRejeitada;
                        this.Descricao = "Entrada rejeitada";
                        break;
                    case 4:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.TransferenciaCarteiraEntrada;
                        this.Descricao = "Transferência de carteira/entrada";
                        break;
                    case 5:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.TransferenciaCarteiraBaixa;
                        this.Descricao = "Transferência de carteira/baixa";
                        break;
                    case 6:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.Liquidacao;
                        this.Descricao = "Liquidação";
                        break;
                    case 9:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.Baixa;
                        this.Descricao = "Baixa";
                        break;
                    case 11:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.TitulosCarteiraEmSer;
                        this.Descricao = "Títulos em carteira em ser";
                        break;
                    case 12:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.ConfirmacaoRecebimentoInstrucaoAbatimento;
                        this.Descricao = "Confirmação recebimento instrução de abatimento";
                        break;
                    case 13:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.ConfirmacaoRecebimentoInstrucaoCancelamentoAbatimento;
                        this.Descricao = "Confirmação recebimento instrução de cancelamento de abatimento";
                        break;
                    case 14:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.ConfirmacaoRecebimentoInstrucaoAlteracaoVencimento;
                        this.Descricao = "Confirmação recebimento instrução alteração de vencimento";
                        break;
                    case 15:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.FrancoPagamento;
                        this.Descricao = "Franco pagamento";
                        break;
                    case 17:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.LiquidacaoAposBaixa;
                        this.Descricao = "Liquidação após baixa";
                        break;
                    case 19:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.ConfirmacaoRecebimentoInstrucaoProtesto;
                        this.Descricao = "Confirmação de recebimento de instrução de protesto";
                        break;
                    case 20:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.ConfirmacaoRecebimentoInstrucaoSustacaoProtesto;
                        this.Descricao = "Confirmação de recebimento de instrução de sustação de protesto";
                        break;
                    case 23:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.RemessaCartorio;
                        this.Descricao = "Remessa a cartório/aponte em cartório";
                        break;
                    case 24:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.RetiradaCartorioManutencaoCarteira;
                        this.Descricao = "Retirada de cartório e manutenção em carteira";
                        break;
                    case 25:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.ProtestadoBaixado;
                        this.Descricao = "Protestado e baixado/baixa por ter sido protestado";
                        break;
                    case 26:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.InstrucaoRejeitada;
                        this.Descricao = "Instrução rejeitada";
                        break;
                    case 27:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.ConfirmaçãoPedidoAlteracaoOutrosDados;
                        this.Descricao = "Confirmação do pedido de alteração de outros dados";
                        break;
                    case 28:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.DebitoTarifas;
                        this.Descricao = "Debito de tarifas/custas";
                        break;
                    case 29:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.OcorrenciaSacado;
                        this.Descricao = "Ocorrencias do sacado";
                        break;
                    case 30:
                        this.Codigo = (int)EnumCodigoMovimento_Bradesco.AlteracaoDadosRejeitada;
                        this.Descricao = "Alteração de dados rejeitada";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = "( Selecione )";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }


        #endregion
    }
}
