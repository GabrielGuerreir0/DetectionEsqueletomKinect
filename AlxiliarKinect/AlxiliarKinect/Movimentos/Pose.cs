using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace AlxiliarKinect.Movimentos
{
    public abstract class Pose : Movimento
    {
        protected int QuadroIdentificacao { get; set; }
        public override EstadoRastreamento Rastrear(Skeleton esqueletoUsuario)
        {
            if (PosicaoValida(esqueletoUsuario))
            {
                if (ContadorQuadros == QuadroIdentificacao)
                {
                    ContadorQuadros = 0;
                    return EstadoRastreamento.Identificado;
                }
                else
                {
                    ContadorQuadros++;
                    return EstadoRastreamento.EmExecucao;
                }
            }
            else
            {
                ContadorQuadros = 0;
                return EstadoRastreamento.NaoIdentificado;
            }
        }
        public int PercentualProgresso
        {
            get
            {
                return ContadorQuadros * 100 / QuadroIdentificacao;
            }
        }
    }

    }

