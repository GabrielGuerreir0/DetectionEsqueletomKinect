using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect;
using AlxiliarKinect;
using DocumentFormat.OpenXml.InkML;

namespace AlxiliarKinect.FuncoesBasicas
{
    static class Extensao
    {
        public static void DesenharEsqueletoUsuario
(this SkeletonFrame quadro,
KinectSensor kinectSensor,
Canvas canvasParaDesenhar)
        {
            if (kinectSensor == null)
                throw new ArgumentNullException("kinectSensor");
            if (canvasParaDesenhar == null)
                throw new ArgumentNullException("canvasParaDesenhar");
            Skeleton[] esqueletos =
            new Skeleton[quadro.SkeletonArrayLength];
            quadro.CopySkeletonDataTo(esqueletos);
            IEnumerable<Skeleton> esqueletosRastreados =
            esqueletos.Where(esqueleto =>
           esqueleto.TrackingState ==
           SkeletonTrackingState.Tracked);
            if (esqueletosRastreados.Count() > 0)
            {
                Skeleton esqueleto =
                esqueletosRastreados.First();
                EsqueletoUsuarioAuxiliar esqueletoUsuarioAuxiliar =
                new EsqueletoUsuarioAuxiliar(kinectSensor);
                foreach (Joint articulacao in esqueleto.Joints)
                    esqueletoUsuarioAuxiliar
                    .DesenharArticulacao(articulacao, canvasParaDesenhar);
            }
        }
        public static Skeleton ObterEsqueletoUsuario
(this SkeletonFrame quadro)
        {
            Skeleton esqueletoUsuario = null;
            Skeleton[] esqueletos =
            new Skeleton[quadro.SkeletonArrayLength];
            quadro.CopySkeletonDataTo(esqueletos);
            IEnumerable<Skeleton> esqueletosRastreados =
            esqueletos.Where(esqueleto =>
           esqueleto.TrackingState ==
           SkeletonTrackingState.Tracked);
            if (esqueletosRastreados.Count() > 0)
                esqueletoUsuario =
                esqueletosRastreados.First();
            return esqueletoUsuario;
        }
    }

    internal class EsqueletoUsuarioAuxiliar
    {
        private KinectSensor kinectSensor;

        public EsqueletoUsuarioAuxiliar(KinectSensor kinectSensor)
        {
            this.kinectSensor = kinectSensor;
        }

        internal void DesenharArticulacao(Joint articulacao, Canvas canvasParaDesenhar)
        {
            throw new NotImplementedException();
        }
    }
}
