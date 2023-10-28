using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect;
using AlxiliarKinect.FuncoesBasicas;
using static DecEsqueleto3.MainWindow;

namespace DecEsqueleto3
{

    public partial class MainWindow : Window
    {
        private KinectSensor kinect;

        public MainWindow()
        {
            InitializeComponent();
            InicializarSeletor();

        }

        private void InicializarKinect(KinectSensor kinectSensor)
        {
            kinect = kinectSensor; slider.Value = kinect.ElevationAngle;
            kinect.DepthStream.Enable();
            kinect.SkeletonStream.Enable();
            kinect.ColorStream.Enable();
            kinect.AllFramesReady += kinect_AllFramesReady;
        }
        private void kinect_AllFramesReady
(object sender, AllFramesReadyEventArgs e)
        {
            byte[] imagem = ObterImagemSensorRGB(e.OpenColorImageFrame());
            if (chkEscalaCinza.IsChecked.HasValue &&
            chkEscalaCinza.IsChecked.Value)
                ReconhecerDistancia(e.OpenDepthImageFrame(),
                imagem, 2000);
            if (imagem != null)
                canvasKinect.Background = new ImageBrush(
                BitmapSource
                .Create(kinect.ColorStream.FrameWidth,
                kinect.ColorStream.FrameHeight,
                96, 96, PixelFormats.Bgr32, null,
                imagem,
                kinect.ColorStream.FrameBytesPerPixel
                * kinect.ColorStream.FrameWidth
                )
                );
            canvasKinect.Children.Clear();
            if (chkEsqueleto.IsChecked.HasValue &&
            chkEsqueleto.IsChecked.Value)
        Extensao.DesenharEsqueletoUsuario(e.OpenSkeletonFrame(), kinect, canvasKinect);
        }

        private void InicializarSeletor()
        {
            InicializadorKinect inicializador = new InicializadorKinect();
            inicializador.MetodoInicializadorKinect = InicializarKinect;
            seletorSensorUI.KinectSensorChooser = inicializador.SeletorKinect;
        }

        private byte[] ObterImagemSensorRGB(ColorImageFrame quadro)
        {
            if (quadro == null) return null;
            using (quadro)
            {
                byte[] bytesImagem = new byte[quadro.PixelDataLength];
                quadro.CopyPixelDataTo(bytesImagem);
                return bytesImagem;
            }
        }

        private void slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            kinect.ElevationAngle = Convert.ToInt32(slider.Value);
        }

        private void ReconhecerDistancia(DepthImageFrame quadro, byte[] bytesImagem, int distanciaMaxima)
        {
            if (quadro == null || bytesImagem == null) return;
            using (quadro)
            {
                DepthImagePixel[] imagemProfundidade =
                new DepthImagePixel[quadro.PixelDataLength];
                quadro.CopyDepthImagePixelDataTo(imagemProfundidade);
                DepthImagePoint[] pontosImagemProfundidade =
                new DepthImagePoint[640 * 480];
                kinect.CoordinateMapper
                .MapColorFrameToDepthFrame(kinect.ColorStream.Format,
                kinect.DepthStream.Format, imagemProfundidade,
                pontosImagemProfundidade);
                for (int i = 0; i < pontosImagemProfundidade.Length; i++)
                {
                    var point = pontosImagemProfundidade[i];
                    if (point.Depth < distanciaMaxima &&
                    KinectSensor.IsKnownPoint(point))
                    {
                        var pixelDataIndex = i * 4;
                        byte maiorValorCor =
                        Math.Max(bytesImagem[pixelDataIndex],
                        Math.Max(bytesImagem[pixelDataIndex + 1],
                        bytesImagem[pixelDataIndex + 2]));
                        bytesImagem[pixelDataIndex] = maiorValorCor;
                        bytesImagem[pixelDataIndex + 1] = maiorValorCor;
                        bytesImagem[pixelDataIndex + 2] = maiorValorCor;
                    }
                }
            }
        }




        public class EsqueletoUsuarioAuxiliar
        {
            private KinectSensor kinect;



            public EsqueletoUsuarioAuxiliar(KinectSensor kinect)
            {
                this.kinect = kinect;
            }

            private ColorImagePoint ConverterCoordenadasArticulacao
(Joint articulacao, double larguraCanvas, double alturaCanvas)
            {
                ColorImagePoint posicaoArticulacao =
                kinect.CoordinateMapper.MapSkeletonPointToColorPoint
                (articulacao.Position, kinect.ColorStream.Format);
                posicaoArticulacao.X = (int)
                (posicaoArticulacao.X * larguraCanvas) /
                kinect.ColorStream.FrameWidth;
                posicaoArticulacao.Y = (int)
                (posicaoArticulacao.Y * alturaCanvas) /
                kinect.ColorStream.FrameHeight;
                return posicaoArticulacao;
            }
            private Ellipse CriarComponenteVisualArticulacao
(int diametroArticulacao, int larguraDesenho, Brush corDesenho)
            {
                Ellipse objetoArticulacao = new Ellipse();
                objetoArticulacao.Height = diametroArticulacao;
                objetoArticulacao.Width = diametroArticulacao;
                objetoArticulacao.StrokeThickness = larguraDesenho;
                objetoArticulacao.Stroke = corDesenho;
                return objetoArticulacao;
            }
            public void DesenharArticulacao(Joint articulacao, Canvas canvasParaDesenhar)
            {
                int diametroArticulacao = articulacao.JointType == JointType.Head ? 50 : 10;
                int larguraDesenho = 4;
                Brush corDesenho = Brushes.Red;

                Ellipse objetoArticulacao = CriarComponenteVisualArticulacao(diametroArticulacao, larguraDesenho, corDesenho);
                ColorImagePoint posicaoArticulacao = ConverterCoordenadasArticulacao(articulacao, canvasParaDesenhar.ActualWidth, canvasParaDesenhar.ActualHeight);

                double deslocamentoHorizontal = posicaoArticulacao.X - objetoArticulacao.Width / 2;
                double deslocamentoVertical = posicaoArticulacao.Y - objetoArticulacao.Height / 2;


                if (deslocamentoHorizontal >= 0 && deslocamentoVertical >= 0 &&
                    deslocamentoHorizontal + objetoArticulacao.Width <= canvasParaDesenhar.ActualWidth &&
                    deslocamentoVertical + objetoArticulacao.Height <= canvasParaDesenhar.ActualHeight)
                {
                    Canvas.SetLeft(objetoArticulacao, deslocamentoHorizontal);
                    Canvas.SetTop(objetoArticulacao, deslocamentoVertical);
                    Canvas.SetZIndex(objetoArticulacao, 100);
                    canvasParaDesenhar.Children.Add(objetoArticulacao);
                }
            }

            private Line CriarComponenteVisualOsso
(int larguraDesenho, Brush corDesenho,
double origemX, double origemY, double destinoX, double destinoY)
            {
                Line objetoOsso = new Line();
                objetoOsso.StrokeThickness = larguraDesenho;
                objetoOsso.Stroke = corDesenho;
                objetoOsso.X1 = origemX;
                objetoOsso.X2 = destinoX;
                objetoOsso.Y1 = origemY;
                objetoOsso.Y2 = destinoY;
                return objetoOsso;
            }

            public void DesenharOsso
(Joint articulacaoOrigem, Joint articulacaoDestino,
Canvas canvasParaDesenhar)
            {
                int larguraDesenho = 4;
                Brush corDesenho = Brushes.Green;
                ColorImagePoint posicaoArticulacaoOrigem =
                ConverterCoordenadasArticulacao(
                articulacaoOrigem,
                canvasParaDesenhar.ActualWidth,
                canvasParaDesenhar.ActualHeight);
                ColorImagePoint posicaoArticulacaoDestino =
                ConverterCoordenadasArticulacao(
                articulacaoDestino,
                canvasParaDesenhar.ActualWidth,
                canvasParaDesenhar.ActualHeight);
                Line objetoOsso =
                CriarComponenteVisualOsso(larguraDesenho, corDesenho,
                posicaoArticulacaoOrigem.X, posicaoArticulacaoOrigem.Y,
                posicaoArticulacaoDestino.X, posicaoArticulacaoDestino.Y);
                if (Math.Max(objetoOsso.X1, objetoOsso.X2) <
                canvasParaDesenhar.ActualWidth &&
                Math.Min(objetoOsso.X1, objetoOsso.X2) > 0 &&
                Math.Max(objetoOsso.Y1, objetoOsso.Y2) <
                canvasParaDesenhar.ActualHeight &&
                Math.Min(objetoOsso.Y1, objetoOsso.Y2) > 0)
                    canvasParaDesenhar.Children.Add(objetoOsso);
            }



        }
    }


    public static class Extensao
    {

        public static void DesenharEsqueletoUsuario
(this SkeletonFrame quadro, KinectSensor kinectSensor,
Canvas canvasParaDesenhar)
        {
            if (kinectSensor == null)
                throw new ArgumentNullException("kinectSensor");
            if (canvasParaDesenhar == null)
                throw new ArgumentNullException("canvasParaDesenhar");
            Skeleton esqueleto = ObterEsqueletoUsuario(quadro);
            if (esqueleto != null)
            {
                EsqueletoUsuarioAuxiliar esqueletoUsuarioAuxiliar =
                new EsqueletoUsuarioAuxiliar(kinectSensor);
                foreach (BoneOrientation osso in esqueleto.BoneOrientations)
                {
                    esqueletoUsuarioAuxiliar
                    .DesenharOsso(esqueleto.Joints[osso.StartJoint],
                    esqueleto.Joints[osso.EndJoint],
                    canvasParaDesenhar);
                    esqueletoUsuarioAuxiliar
                    .DesenharArticulacao
                    (esqueleto.Joints[osso.EndJoint], canvasParaDesenhar);
                }
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
}






