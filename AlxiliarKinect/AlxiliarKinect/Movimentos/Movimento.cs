using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace AlxiliarKinect.Movimentos
{
    public abstract class Movimento
    {
        protected int ContadorQuadros { get; set; }
        public string Nome { get; set; }
        public abstract EstadoRastreamento Rastrear(
        Skeleton esqueletoUsuario);
        protected abstract bool PosicaoValida(
        Skeleton esqueletoUsuario);
    }
    
}
