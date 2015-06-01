using System;
using System.Collections;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_Santander
    {
        BaixarApos15Dias = 02,
        BaixarApos30Dias = 03,
        NaoBaixar = 04,
        Protestar = 06,
        NaoProtestar = 07,
        NaoCobrarJurosDeMora = 08,
        JurosDia = 09,
        Percentual_Multa = 10,
        DescontoporDia = 11,
        JurosdeMora =12
    }

    #endregion

    public class Instrucao_Santander : AbstractInstrucao, IInstrucao
    {

        #region Construtores
        public Instrucao_Santander()
        {
            try
            {
                this.Banco = new Banco(33);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public Instrucao_Santander(int codigo)
        {
            this.carregar(codigo, 0);
        }

        public Instrucao_Santander(int codigo, double nrDias)
        {
            this.carregar(codigo, nrDias);
        }
        #endregion

        #region Metodos Privados

        private void carregar(int idInstrucao, double valor)
        {
            try
            {
                this.Banco = new Banco_Santander();

                switch ((EnumInstrucoes_Santander)idInstrucao)
                {
                    case EnumInstrucoes_Santander.BaixarApos15Dias:
                        this.Codigo = (int)EnumInstrucoes_Santander.BaixarApos15Dias;
                        this.Descricao = "Baixar ap�s quinze dias do vencimento";
                        break;
                    case EnumInstrucoes_Santander.BaixarApos30Dias:
                        this.Codigo = (int)EnumInstrucoes_Santander.BaixarApos30Dias;
                        this.Descricao = "Baixar ap�s 30 dias do vencimento";
                        break;
                    case EnumInstrucoes_Santander.NaoBaixar:
                        this.Codigo = (int)EnumInstrucoes_Santander.NaoBaixar;
                        this.Descricao = "N�o baixar";
                        break;
                    case EnumInstrucoes_Santander.Protestar:
                        this.Codigo = (int)EnumInstrucoes_Santander.Protestar;
                        this.Descricao = "Protestar ap�s "+valor+" do vencimento";
                        this.QuantidadeDias = int.Parse(valor.ToString());
                        break;
                    case EnumInstrucoes_Santander.NaoProtestar:
                        this.Codigo = (int)EnumInstrucoes_Santander.NaoProtestar;
                        this.Descricao = "N�o protestar";
                        break;
                    case EnumInstrucoes_Santander.NaoCobrarJurosDeMora:
                        this.Codigo = (int)EnumInstrucoes_Santander.NaoCobrarJurosDeMora;
                        this.Descricao = "N�o cobrar juros de mora";
                        break;
                    case EnumInstrucoes_Santander.Percentual_Multa:
                        this.Codigo = (int)EnumInstrucoes_Santander.Percentual_Multa;
                        this.Descricao = "Ap�s vencimento cobrar multa de " + valor + " %";
                        break;
                    case EnumInstrucoes_Santander.JurosDia:
                        this.Codigo = (int)EnumInstrucoes_Santander.JurosDia;
                        this.Descricao = "Ap�s vencimento cobrar R$ " + valor + " por dia de atraso";
                        break;
                    case EnumInstrucoes_Santander.JurosdeMora:
                        this.Codigo = (int)EnumInstrucoes_Santander.JurosdeMora;
                        this.Descricao = "Juros de mora de "+ valor +"% ao m�s";
                        break;
                    case EnumInstrucoes_Santander.DescontoporDia:
                        this.Codigo = (int)EnumInstrucoes_Santander.DescontoporDia;
                        this.Descricao = "Conceder desconto de R$ " + valor + " por dia de antecipa��o"; // por dia de antecipa��o
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = "";
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
