using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace AlxiliarKinect.Movimentos
{
    public interface IRastreador
    {
        void Rastrear(Skeleton esqueletoUsuario);
        EstadoRastreamento EstadoAtual { get; }
    }
}
