using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect;
using AlxiliarKinect.Movimentos;
using static AlxiliarKinect.Movimentos.Poses.PoseT;

namespace AlxiliarKinect.Movimentos.Poses
{
    public class PosePause : Pose
    {
        public PosePause()
        {
            this.Nome = "PosePause";
            this.QuadroIdentificacao = 30;
        }
        protected override bool PosicaoValida
(Skeleton esqueletoUsuario)
        {
            Joint centroOmbros =
            esqueletoUsuario.Joints[JointType.ShoulderCenter];
            Joint maoDireita =
            esqueletoUsuario.Joints[JointType.HandRight];
            Joint cotoveloDireito =
            esqueletoUsuario.Joints[JointType.ElbowRight];
            Joint maoEsquerda =
            esqueletoUsuario.Joints[JointType.HandLeft];
            Joint cotoveloEsquerdo =
esqueletoUsuario.Joints[JointType.ElbowLeft];
            double margemErro = 0.30;
            bool maoDireitaAlturaCorreta =
            Util.CompararComMargemErro(margemErro,
            maoDireita.Position.Y, centroOmbros.Position.Y);
            bool maoDireitaDistanciaCorreta =
            Util.CompararComMargemErro(margemErro,
            maoDireita.Position.Z, centroOmbros.Position.Z);
            bool maoDireitaAposCotovelo =
            maoDireita.Position.X > cotoveloDireito.Position.X;
            bool cotoveloDireitoAlturaCorreta =
            Util.CompararComMargemErro(margemErro,
            cotoveloDireito.Position.Y, centroOmbros.Position.Y);
            bool cotoveloEsquerdoAlturaCorreta =
            Util.CompararComMargemErro(margemErro,
            cotoveloEsquerdo.Position.Y, centroOmbros.Position.Y);
            bool maoEsquerdaAlturaCorreta =
            Util.CompararComMargemErro(margemErro,
            maoEsquerda.Position.Y, centroOmbros.Position.Y);
            bool maoEsquerdaDistanciaCorreta =
            Util.CompararComMargemErro(margemErro,
            maoEsquerda.Position.Z, centroOmbros.Position.Z);
            bool maoEsquerdaAposCotovelo =
            maoEsquerda.Position.X < cotoveloEsquerdo.Position.X;
            return maoDireitaAlturaCorreta &&
            maoDireitaDistanciaCorreta &&
            maoDireitaAposCotovelo &&
            cotoveloDireitoAlturaCorreta &&
            maoEsquerdaAlturaCorreta &&
            maoEsquerdaDistanciaCorreta &&
            maoEsquerdaAposCotovelo &&
            cotoveloEsquerdoAlturaCorreta;
        }
        
        public override EstadoRastreamento Rastrear(Skeleton esqueletoUsuario)
        {
            EstadoRastreamento novoEstado;
            if (esqueletoUsuario != null && PosicaoValida(esqueletoUsuario))
            {
                if (QuadroIdentificacao == ContadorQuadros)
                    novoEstado = EstadoRastreamento.Identificado;
                else
                {
                    novoEstado = EstadoRastreamento.EmExecucao;
                    ContadorQuadros += 1;
                }
            }
            else
            {
                novoEstado = EstadoRastreamento.NaoIdentificado;
                ContadorQuadros = 0;
            }
            return novoEstado;
        }
    }
}
